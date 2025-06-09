import os
import cv2
import face_recognition
import pickle
import pathlib


def build_known(faces_dir=None, output_file="encodings.pkl"):

    faces_dir = faces_dir or os.path.join(os.path.dirname(__file__), "Faces")
    known_enc, known_ids = [], []

    for img_path in pathlib.Path(faces_dir).rglob("*.jpg"):
        try:
            img = cv2.imread(str(img_path))
            rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

            boxes = face_recognition.face_locations(rgb)
            encs = face_recognition.face_encodings(rgb, boxes)

            face_id = int(img_path.parent.name)

            for e in encs:
                known_enc.append(e)
                known_ids.append(face_id)
        except Exception:
            # Ignore files that cannot be processed
            pass

    data = {"encodings": known_enc, "ids": known_ids}
    with open(output_file, "wb") as f:
        pickle.dump(data, f)

    print("[INFO] Сохранено:", len(known_enc))
    return data


if __name__ == "__main__":
    build_known()
