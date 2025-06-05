import os
import json
from datetime import datetime, time as dt_time

import telebot
import requests

SETTINGS_FILE = os.path.join(os.path.dirname(__file__), 'bot_settings.json')

DEFAULT_SETTINGS = {
    "notify_time": "08:30",
    "send_absent_only": True,
    "send_daily_updates": True
}

STATE_WAIT_NAME = 'wait_name'
STATE_WAIT_GROUP = 'wait_group'

API_BASE = os.getenv('SERVER_URL', 'http://127.0.0.1:5000')

user_states = {}


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


def send_notifications():
    settings = load_settings()
    notify_time = dt_time.fromisoformat(settings['notify_time'])
    now = datetime.now().time()
    if now < notify_time:
        return
    # Placeholder: получение списка отсутствующих
    absent_list = 'Список отсутствующих после 8:30'
    subs = api_get('/api/confirmed_subscribers') or []
    for sub in subs:
        bot.send_message(sub['telegram_id'], absent_list)


def run_bot():
    """Initialize DB and start polling."""
    bot.polling(none_stop=True)


if __name__ == '__main__':
    run_bot()
