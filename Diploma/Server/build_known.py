import os, cv2, face_recognition, pickle, pathlib

faces_dir = os.path.join(os.path.dirname(__file__), 'Faces')
known_enc, known_ids = [], []

for img_path in pathlib.Path(faces_dir).rglob("*.jpg"):
    try:
        img = cv2.imread(str(img_path))
        rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

        boxes = face_recognition.face_locations(rgb)
        encs  = face_recognition.face_encodings(rgb, boxes)

        face_id = int(img_path.parent.name)

        for e in encs:
            known_enc.append(e)
            known_ids.append(face_id)
    except Exception:
        pass

with open("encodings.pkl", "wb") as f:
    pickle.dump({"encodings": known_enc, "ids": known_ids}, f)

print("[INFO] Сохранено:", len(known_enc))
