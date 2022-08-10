from mongoengine import Document
from mongoengine import StringField, ReferenceField, ListField
from flask import session
from app.models.trip import Trip

class User(Document):
    user_id = StringField(primary_key = True)
    username = StringField()
    password = StringField()

    meta = {'collection': 'dm-users'}
