from flask import Flask, request, jsonify
import cv2
import face_recognition
import numpy as np
import base64
import os
import pickle
import subprocess
import sys
import time

app = Flask(__name__)

known_faces = {}
last_seen = {}  # Кеш: id -> время последнего распознавания
COOLDOWN_SECONDS = 30  # 2 минуты

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

            # КД: если меньше 2 минут — игнорируем
            now = time.time()
            if face_id in last_seen and (now - last_seen[face_id]) < COOLDOWN_SECONDS:
                return jsonify({"id": "unknown", "face_image": ""})

            last_seen[face_id] = now

            face_crop = frame[top:bottom, left:right]
            _, buffer = cv2.imencode('.jpg', face_crop)
            face_b64 = base64.b64encode(buffer).decode('utf-8')
            return jsonify({"id": face_id, "face_image": face_b64})

    return jsonify({"id": "unknown", "face_image": ""})


if __name__ == '__main__':
    print("[INFO] Строим базу лиц...")
    try:
        subprocess.run([sys.executable, "build_known.py"], check=True)
    except subprocess.CalledProcessError:
        print("[ERROR] Ошибка при выполнении build_known.py")
        exit(1)

    if not os.path.exists("encodings.pkl"):
        print("[ERROR] Файл encodings.pkl не найден после сборки")
        exit(1)

    with open("encodings.pkl", "rb") as f:
        known_faces = pickle.load(f)

    print("[INFO] Запускаем Flask сервер...")
    app.run(debug=False)
