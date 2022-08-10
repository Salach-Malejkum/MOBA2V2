from app import create_app, register_bp
from app.blueprints.user_bp import user_bp

app = create_app()
if __name__ == '__main__':
    register_bp(app, user_bp)
    app.debug = True
    app.run(port=8080)