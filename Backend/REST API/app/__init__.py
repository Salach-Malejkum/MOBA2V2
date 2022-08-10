from flask import Flask
from flask_appbuilder import AppBuilder
from flask_appbuilder.security.mongoengine.manager import SecurityManager
from flask_mongoengine import MongoEngine
from config import Config
from flask_cors import CORS

def create_app():
    app = Flask(__name__)
    CORS(app)
    app.secret_key = b'\x02\x9d7\xc6\xff\xce{\x07U\xd9\xf8(\xef\x9f\xa8{'
    app.config['MONGODB_HOST'] = "mongodb://localhost"
    app.config['MONGODB_PORT'] = 5000
    app.config['MONGODB_DB'] = 'mpk'

    db = MongoEngine()

    db.init_app(app)
    
    appbuilder = AppBuilder(app, security_manager_class=SecurityManager)
    
    return app

def register_bp(app: Flask, blueprint):
    app.register_blueprint(blueprint)