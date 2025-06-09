import os
import sys
import json
import threading
import time
from datetime import datetime, time as dt_time

import telebot
import requests

APP_DIR = os.path.dirname(sys.executable) if getattr(sys, 'frozen', False) else os.path.dirname(os.path.abspath(sys.argv[0]))
DATA_DIR = getattr(sys, '_MEIPASS', APP_DIR)
SETTINGS_FILE = os.path.join(APP_DIR, 'bot_settings.json')

DEFAULT_SETTINGS = {
    "notify_time": "08:30",
    "send_absent_only": True,
    "send_daily_updates": True
}

STATE_WAIT_NAME = 'wait_name'
STATE_WAIT_GROUP = 'wait_group'

API_BASE = os.getenv('SERVER_URL', 'http://127.0.0.1:5000')

user_states = {}
last_notification_date = None


def load_settings():
    if not os.path.exists(SETTINGS_FILE):
        with open(SETTINGS_FILE, 'w', encoding='utf-8') as f:
            json.dump(DEFAULT_SETTINGS, f, ensure_ascii=False, indent=2)
        return DEFAULT_SETTINGS
    with open(SETTINGS_FILE, 'r', encoding='utf-8') as f:
        return json.load(f)


def save_settings(settings):
    with open(SETTINGS_FILE, 'w', encoding='utf-8') as f:
        json.dump(settings, f, ensure_ascii=False, indent=2)


def api_post(path, data=None):
    try:
        r = requests.post(f"{API_BASE}{path}", json=data, timeout=5)
        if r.ok:
            return r.json()
    except Exception:
        pass
    return None


def api_get(path):
    try:
        r = requests.get(f"{API_BASE}{path}", timeout=5)
        if r.ok:
            return r.json()
    except Exception:
        pass
    return None


def find_employee(full_name):
    data = api_post('/api/find_employee', {'full_name': full_name})
    if not data or data.get('id') is None:
        return None
    return data['id'], data['role']


def get_groups_for_employee(emp_id):
    data = api_get(f'/api/groups_for_employee/{emp_id}')
    return [(g['id'], g['name']) for g in data] if data else []


def get_all_groups():
    data = api_get('/api/all_groups')
    return [(g['id'], g['name']) for g in data] if data else []


def add_subscriber(full_name, role, telegram_id, groups):
    api_post('/api/add_subscriber', {
        'full_name': full_name,
        'role': role,
        'telegram_id': telegram_id,
        'groups': groups
    })


BOT_TOKEN = os.getenv('BOT_TOKEN', '8099498307:AAFlYTIjCF_ChnuFx4rXTb_1pt_6VgoDmoY')

bot = telebot.TeleBot(BOT_TOKEN)


@bot.message_handler(commands=['start'])
def start_handler(message):
    bot.send_message(message.chat.id, 'Введите ФИО полностью:')
    user_states[message.chat.id] = {'state': STATE_WAIT_NAME}


@bot.message_handler(func=lambda m: user_states.get(m.chat.id, {}).get('state') == STATE_WAIT_NAME)
def handle_name(message):
    info = find_employee(message.text.strip())
    if not info:
        bot.send_message(message.chat.id, 'Сотрудник не найден')
        user_states.pop(message.chat.id, None)
        return
    emp_id, role = info
    state = user_states[message.chat.id]
    state.update({'emp_id': emp_id, 'role': role, 'full_name': message.text.strip()})
    groups = get_groups_for_employee(emp_id) if role == 'Куратор' else get_all_groups()
    if not groups:
        bot.send_message(message.chat.id, 'Группы не найдены')
        user_states.pop(message.chat.id, None)
        return
    markup = telebot.types.ReplyKeyboardMarkup(one_time_keyboard=True)
    for _, name in groups:
        markup.add(name)
    if role == 'Администратор':
        markup.add('Все')
    bot.send_message(message.chat.id, 'Выберите группу:', reply_markup=markup)
    state['state'] = STATE_WAIT_GROUP


@bot.message_handler(func=lambda m: user_states.get(m.chat.id, {}).get('state') == STATE_WAIT_GROUP)
def handle_group(message):
    state = user_states.get(message.chat.id)
    if not state:
        return
    selected = message.text.strip()
    role = state['role']
    if role == 'Администратор' and selected == 'Все':
        groups = [g for _, g in get_all_groups()]
    else:
        groups = [selected]
    add_subscriber(state['full_name'], role, message.chat.id, groups)
    bot.send_message(message.chat.id, 'Заявка отправлена на подтверждение')
    user_states.pop(message.chat.id, None)


def notify_visit(full_name, status, group, person_type):
    settings = load_settings()
    if not settings.get('send_daily_updates'):
        return

    if person_type != 'student' or not group:
        return

    subs = api_get('/api/confirmed_subscribers') or []
    for sub in subs:
        groups = sub.get('groups')
        if not groups:
            continue
        group_list = [g.strip() for g in groups.split(',') if g.strip()]
        if group in group_list:
            bot.send_message(sub['telegram_id'], f'{full_name}: {status}')



def send_notifications():
    settings = load_settings()
    notify_time = dt_time.fromisoformat(settings['notify_time'])
    now = datetime.now()
    if now.time() < notify_time:
        return
    global last_notification_date
    if last_notification_date == now.date():
        return
    if not settings.get('send_absent_only'):
        return
    subs = api_get('/api/confirmed_subscribers') or []
    for sub in subs:
        groups = [g.strip() for g in sub.get('groups', '').split(',') if g.strip()]
        data = api_post('/api/absent_students', {'groups': groups, 'date': now.date().isoformat()}) or {}
        lines = [f'Список отсутствующих после {notify_time.strftime("%H:%M")}' ]
        for name, students in data.items():
            if students:
                lines.append('\n' + name + ':')
                lines.extend(students)
        bot.send_message(sub['telegram_id'], '\n'.join(lines))
    last_notification_date = now.date()


def run_bot():
    def scheduler():
        while True:
            try:
                send_notifications()
            except Exception:
                pass
            time.sleep(60)

    threading.Thread(target=scheduler, daemon=True).start()
    bot.polling(none_stop=True)


if __name__ == '__main__':
    run_bot()

