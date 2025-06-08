This folder contains the Python face recognition server used by the
application. To build a standalone executable use PyInstaller after
installing all dependencies from `requirements.txt`.

Recommended command:

```
pyinstaller --onefile server.py \
    --add-data "C:\\path\\to\\face_recognition_models\\models;face_recognition_models\\models" \
    --add-data "bot_settings.json;."
```

The `Faces` directory should be placed next to the executable so new
images can be saved. The settings file `bot_settings.json` is stored in
the same directory as the executable for persistence.
