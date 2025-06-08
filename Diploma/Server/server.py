from flask import Flask, request, jsonify
import cv2
import face_recognition
import numpy as np
import base64
import os
import pickle
import sys
import time
from datetime import datetime
import threading
import pyodbc
import telegram_bot
import json
from build_known import build_known


APP_DIR = os.path.dirname(sys.executable) if getattr(sys, 'frozen', False) else os.path.dirname(__file__)
DATA_DIR = getattr(sys, '_MEIPASS', APP_DIR)
app = Flask(__name__)

known_faces = {}
last_seen = {}
COOLDOWN_SECONDS = 30


def get_db_connection():
    db_path = os.path.abspath(
        os.path.join(APP_DIR, '..', 'bin', 'Debug', 'EducationAccessSystem.mdf')
    )
    conn_str = (
        r"Driver={ODBC Driver 17 for SQL Server};"
        r"Server=(localdb)\MSSQLLocalDB;"
        r"Integrated Security=SSPI;"
        fr"AttachDbFilename={db_path};"
    )
    return pyodbc.connect(conn_str)


@app.route('/ping')
def ping():
    return jsonify({'status': 'ok'})



@app.route('/recognize', methods=['POST'])
def recognize():
    if 'frame' not in request.files:
        return jsonify({"id": "unknown", "face_image": ""})

    file = request.files['frame']
    npimg = np.frombuffer(file.read(), np.uint8)
    frame = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    face_locations = face_recognition.face_locations(rgb_frame)
    face_encodings = face_recognition.face_encodings(rgb_frame, face_locations)

    for (top, right, bottom, left), face_encoding in zip(face_locations, face_encodings):
        matches = face_recognition.compare_faces(known_faces["encodings"], face_encoding)
        if True in matches:
            matched_idx = matches.index(True)
            face_id = known_faces["ids"][matched_idx]

            now = time.time()
            if face_id in last_seen and (now - last_seen[face_id]) < COOLDOWN_SECONDS:
                return jsonify({"id": "unknown", "face_image": ""})

            last_seen[face_id] = now

            face_crop = frame[top:bottom, left:right]
            _, buffer = cv2.imencode('.jpg', face_crop)
            face_b64 = base64.b64encode(buffer).decode('utf-8')
            return jsonify({"id": face_id, "face_image": face_b64})


    return jsonify({"id": "unknown", "face_image": ""})


@app.route('/api/validate_photos', methods=['POST'])
def api_validate_photos():
    photos = request.files.getlist('photos')
    if not photos:
        return jsonify({'ok': False, 'error': 'no_photos'}), 400
    for f in photos:
        npimg = np.frombuffer(f.read(), np.uint8)
        frame = cv2.imdecode(npimg, cv2.IMREAD_COLOR)
        rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        boxes = face_recognition.face_locations(rgb)
        if len(boxes) == 0:
            return jsonify({'ok': False})
    return jsonify({'ok': True})

@app.route('/api/reload_encodings', methods=['POST'])
def api_reload_encodings():
    global known_faces
    try:
        known_faces = build_known(
            faces_dir=os.path.join(DATA_DIR, 'Faces'),
            output_file=os.path.join(APP_DIR, 'encodings.pkl'),
        )
        return jsonify({'status': 'ok'})
    except Exception as exc:
        return jsonify({'status': 'error', 'message': str(exc)}), 500

@app.route('/api/find_employee', methods=['POST'])
def api_find_employee():
    data = request.get_json()
    full_name = data.get('full_name') if data else None
    if not full_name:
        return jsonify({'error': 'missing full_name'}), 400
    query = '''SELECT s.id_сотрудника, st.Название
               FROM Сотрудники s
               JOIN Статус_должность st ON st.id_статуса=s.id_статуса
               WHERE RTRIM(CONCAT(s.Фамилия,' ',s.Имя,' ',COALESCE(s.Отчество,''))) = ?'''
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute(query, full_name)
        row = cur.fetchone()
    if row:
        return jsonify({'id': row[0], 'role': row[1]})
    return jsonify({'id': None})


@app.route('/api/groups_for_employee/<int:emp_id>')
def api_groups_for_employee(emp_id):
    query = '''SELECT g.id_группы, kc.Код + '-' + CAST(g.Год AS NVARCHAR(4))
               FROM Группа g
               JOIN Группа_код kc ON kc.id_код=g.id_код
               WHERE g.id_сотрудника=?'''
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute(query, emp_id)
        rows = cur.fetchall()
    return jsonify([{'id': r[0], 'name': r[1]} for r in rows])



@app.route('/api/all_groups')
def api_all_groups():
    query = '''SELECT DISTINCT g.id_группы,
                      kc.Код + '-' + CAST(g.Год AS NVARCHAR(4))
               FROM Группа g
               JOIN Группа_код kc ON kc.id_код=g.id_код'''

    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute(query)
        rows = cur.fetchall()
    return jsonify([{'id': r[0], 'name': r[1]} for r in rows])



@app.route('/api/add_subscriber', methods=['POST'])
def api_add_subscriber():
    data = request.get_json() or {}
    full_name = data.get('full_name')
    role = data.get('role')
    telegram_id = data.get('telegram_id')
    groups = data.get('groups', [])
    if not (full_name and role and telegram_id and groups):
        return jsonify({'error': 'invalid data'}), 400
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute('''INSERT INTO [ПодписчикиТелеграм]([ФИО], [Роль], [TelegramID], [Группы], [Статус])
                       VALUES(?, ?, ?, ?, 0)''', full_name, role, telegram_id, ','.join(groups))
        con.commit()
    return jsonify({'status': 'ok'})

def send_test_notification(tg_id: int):
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute('SELECT [Группы] FROM [ПодписчикиТелеграм] WHERE [TelegramID]=?', tg_id)
        row = cur.fetchone()
        if not row:
            return
        group_names = [g.strip() for g in row[0].split(',') if g.strip()]
        message_lines = ['Тестовое уведомление. Бот работает!']
        for name in group_names:
            cur.execute('''SELECT u.Фамилия + ' ' + u.Имя + ISNULL(' ' + u.Отчество, '')
                           FROM Учащиеся u
                           JOIN Группа g ON g.id_группы=u.id_группы
                           JOIN Группа_код kc ON kc.id_код=g.id_код
                           WHERE kc.Код + '-' + CAST(g.Год AS NVARCHAR(4))=?''', name)
            students = [r[0] for r in cur.fetchall()]
            if students:
                message_lines.append('\n' + name + ':')
                message_lines.extend(students)
        telegram_bot.bot.send_message(tg_id, '\n'.join(message_lines))


@app.route('/api/absent_students', methods=['POST'])
def api_absent_students():
    data = request.get_json() or {}
    groups = data.get('groups') or []
    if isinstance(groups, str):
        groups = [groups]
    date_str = data.get('date')
    try:
        qdate = datetime.fromisoformat(date_str).date() if date_str else datetime.now().date()
    except ValueError:
        return jsonify({'error': 'invalid date'}), 400

    results = {}
    with get_db_connection() as con:
        cur = con.cursor()
        for name in groups:
            cur.execute('''
                SELECT u.Фамилия + ' ' + u.Имя + ISNULL(' ' + u.Отчество, '')
                FROM Учащиеся u
                JOIN Группа g ON g.id_группы=u.id_группы
                JOIN Группа_код kc ON kc.id_код=g.id_код
                LEFT JOIN (
                    SELECT DISTINCT id_учащегося FROM Приход_уход
                    WHERE дата_прихода=? AND id_учащегося IS NOT NULL
                ) v ON v.id_учащегося=u.id_учащегося
                WHERE kc.Код + '-' + CAST(g.Год AS NVARCHAR(4))=? AND v.id_учащегося IS NULL
                ORDER BY u.Фамилия, u.Имя
            ''', qdate, name)
            results[name] = [r[0] for r in cur.fetchall()]
    return jsonify(results)


@app.route('/api/confirmed_subscribers')
def api_confirmed_subscribers():
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute('SELECT [TelegramID], [Группы] FROM [ПодписчикиТелеграм] WHERE [Статус]=1')
        rows = cur.fetchall()
    return jsonify([{'telegram_id': r[0], 'groups': r[1]} for r in rows])


@app.route('/api/subscribers')
def api_subscribers():
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute('SELECT [id_подписчика], [ФИО], [Роль], [TelegramID], [Группы], [Статус] FROM [ПодписчикиТелеграм]')
        rows = cur.fetchall()
    return jsonify([
        {
            'id': r[0],
            'full_name': r[1],
            'role': r[2],
            'telegram_id': r[3],
            'groups': r[4],
            'status': bool(r[5])
        }
        for r in rows
    ])


@app.route('/api/confirm_subscriber/<int:sub_id>', methods=['POST'])
def api_confirm_subscriber(sub_id):
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute('UPDATE [ПодписчикиТелеграм] SET [Статус]=1 WHERE [id_подписчика]=?', sub_id)
        con.commit()
    return jsonify({'status': 'ok'})


@app.route('/api/delete_subscriber/<int:sub_id>', methods=['POST'])
def api_delete_subscriber(sub_id):
    with get_db_connection() as con:
        cur = con.cursor()
        cur.execute('DELETE FROM [ПодписчикиТелеграм] WHERE [id_подписчика]=?', sub_id)
        con.commit()
    return jsonify({'status': 'ok'})


@app.route('/api/bot_settings', methods=['GET', 'POST'])
def api_bot_settings():
    if request.method == 'POST':
        data = request.get_json() or {}
        telegram_bot.save_settings(data)
        return jsonify({'status': 'ok'})
    return jsonify(telegram_bot.load_settings())


@app.route('/api/test_notify/<tg_id>')
def api_test_notify(tg_id):
    try:
        tg = int(tg_id)
    except ValueError:
        return jsonify({'error': 'invalid id'}), 400
    send_test_notification(tg)
    return jsonify({'status': 'ok'})


@app.route('/api/notify_visit', methods=['POST'])
def api_notify_visit():
    data = request.get_json() or {}
    full_name = data.get('full_name')
    status = data.get('status')
    if not full_name or not status:
        return jsonify({'error': 'invalid data'}), 400
    telegram_bot.notify_visit(full_name, status)
    return jsonify({'status': 'ok'})


if __name__ == '__main__':
    print("[INFO] Строим базу лиц...")
    try:
        known_faces = build_known(
            faces_dir=os.path.join(DATA_DIR, 'Faces'),
            output_file=os.path.join(APP_DIR, 'encodings.pkl'),
        )
    except Exception as exc:
        print(f"[ERROR] Ошибка при выполнении build_known.py: {exc}")
        exit(1)

    if not os.path.exists(os.path.join(APP_DIR, 'encodings.pkl')):
        print("[ERROR] Файл encodings.pkl не найден после сборки")
        exit(1)

    print("[INFO] Запускаем Telegram-бота...")
    bot_thread = threading.Thread(target=telegram_bot.run_bot, daemon=True)
    bot_thread.start()

    print("[INFO] Запускаем Flask сервер...")
    app.run(debug=False)
