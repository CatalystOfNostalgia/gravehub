from sqlalchemy import Column
from sqlalchemy import Integer
from sqlalchemy import String
from sqlalchemy import Date
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

class HeadquartersUpgrade(Base):
    __tablename__='headquarters_upgrade'

    level = Column(Integer, primary_key=True)
    resource_cost_fire = Column(Integer, nullable=False)
    resource_cost_water = Column(Integer, nullable=False)
    resource_cost_earth = Column(Integer, nullable=False)
    resource_cost_air = Column(Integer, nullable=False)
    poof_cap = Column(Integer, nullable=False)
    experience_gain = Column(Integer, nullable=False)

class DecorativeBuildingInfo(Base):
    __tablename__='decorative_building_info'

    building_info_id = Column(Integer, primary_key=True)
    name = Column(String(100), nullable=False)
    next_building_id = Column(Integer)
    resource_cost_fire = Column(Integer, nullable=False)
    resource_cost_water = Column(Integer, nullable=False)
    resource_cost_air = Column(Integer, nullable=False)
    resource_cost_earth = Column(Integer, nullable=False)
    poofs_generated = Column(Integer, nullable=False)
    experience_gain = Column(Integer, nullable=False)

class ResourceBuilding(Base):
    __tablename__='resource_building'

    building_id = Column(Integer, nullable=False, primary_key=True)
    experience_gain = Column(Integer, nullable=False)
    level = Column(Integer, nullable=False)
    next_building_id = Column(Integer)
    production_rate = Column(Integer, nullable=False)
    production_type = Column(Integer, nullable=False)
    resource_cost_fire = Column(Integer, nullable=False)
    resource_cost_water = Column(Integer, nullable=False)
    resource_cost_air = Column(Integer, nullable=False)
    resource_cost_earth = Column(Integer, nullable=False)
    experience_gain = Column(Integer, nullable=False)
