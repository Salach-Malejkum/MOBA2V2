from mongoengine import Document
from mongoengine import StringField, ReferenceField, ListField
from flask import session
from app.models.trip import Trip

class User(Document):
    user_id = StringField(primary_key = True)
    username = StringField()
    password = StringField()
    trip_history = ListField(ReferenceField(Trip))
    favourite_trips = ListField(ReferenceField(Trip))

    meta = {'collection': 'mpk-users'}

    def start_session(self):
        session['logged_in'] = True
        session['user'] = self
        return 200
