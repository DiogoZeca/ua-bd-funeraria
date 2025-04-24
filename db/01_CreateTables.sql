CREATE SCHEMA project
GO

IF OBJECT_ID('project.Users', 'U') IS NOT NULL
    DROP TABLE project.Users
IF OBJECT_ID('project.Client', 'U') IS NOT NULL
    DROP TABLE project.Client
IF OBJECT_ID('project.Representative', 'U') IS NOT NULL
    DROP TABLE project.Representative
IF OBJECT_ID('project.Person', 'U') IS NOT NULL
    DROP TABLE project.Person
IF OBJECT_ID('project.Deceased', 'U') IS NOT NULL
    DROP TABLE project.Deceased
IF OBJECT_ID('project.Process', 'U') IS NOT NULL
    DROP TABLE project.Process
IF OBJECT_ID('project.Funeral', 'U') IS NOT NULL
    DROP TABLE project.Funeral
IF OBJECT_ID('project.Cemetery', 'U') IS NOT NULL
    DROP TABLE project.Cemetery
IF OBJECT_ID('project.Crematory', 'U') IS NOT NULL
    DROP TABLE project.Crematory
IF OBJECT_ID('project.Ceremony', 'U') IS NOT NULL
    DROP TABLE project.Ceremony
IF OBJECT_ID('project.Church', 'U') IS NOT NULL
    DROP TABLE project.Church
IF OBJECT_ID('project.Priest', 'U') IS NOT NULL
    DROP TABLE project.Priest
IF OBJECT_ID('project.Container', 'U') IS NOT NULL
    DROP TABLE project.Container
IF OBJECT_ID('project.Products', 'U') IS NOT NULL
    DROP TABLE project.Products
IF OBJECT_ID('project.Coffin', 'U') IS NOT NULL
    DROP TABLE project.Coffin
IF OBJECT_ID('project.Urn', 'U') IS NOT NULL
    DROP TABLE project.Urn
IF OBJECT_ID('project.Flowers', 'U') IS NOT NULL
    DROP TABLE project.Flowers
IF OBJECT_ID('project.Florist', 'U') IS NOT NULL
    DROP TABLE project.Florist
IF OBJECT_ID('project.Funeral_Products', 'U') IS NOT NULL
    DROP TABLE project.Funeral_Products
IF OBJECT_ID('project.Burial', 'U') IS NOT NULL
    DROP TABLE project.Burial
IF OBJECT_ID('project.Cremation', 'U') IS NOT NULL
    DROP TABLE project.Cremation
IF OBJECT_ID('project.Florist_Flowers', 'U') IS NOT NULL
    DROP TABLE project.Florist_Flowers

CREATE TABLE project.Users (
    id INT PRIMARY KEY,
    name VARCHAR(255),
    username VARCHAR(255) UNIQUE,
    password VARCHAR(255),
    mail VARCHAR(255),
    num_func INT
);

CREATE TABLE project.Client (
    id INT PRIMARY KEY
);

CREATE TABLE project.Representative (
    id INT PRIMARY KEY,
    contact VARCHAR(255)
);

CREATE TABLE project.Person (
    id INT PRIMARY KEY,
    name VARCHAR(255),
    bi VARCHAR(50)
);

CREATE TABLE project.Deceased (
    id INT PRIMARY KEY REFERENCES project.Person(id),
    sex CHAR(1),
    birth_date DATE,
    marital_estate VARCHAR(50),
    residence VARCHAR(255),
    nationality VARCHAR(100)
);

CREATE TABLE project.Process (
    id INT PRIMARY KEY,
    num_process INT UNIQUE,
    start_date DATE,
    status VARCHAR(50),
    budget DECIMAL(10,2),
    description TEXT,
    type_of_payment VARCHAR(50),
    users_id INT REFERENCES project.Users(id),
	client_id INT REFERENCES project.Client(id),
    degree_kinship VARCHAR(255)
);

CREATE TABLE project.Funeral (
    num_process INT REFERENCES project.Process(id),
    funeral_date DATE,
    location VARCHAR(255),
    deceased_id INT REFERENCES project.Deceased(id),
	PRIMARY KEY (num_process)
);

CREATE TABLE project.Cemetery (
    id INT PRIMARY KEY,
    location VARCHAR(255),
    price DECIMAL(10,2)
);

CREATE TABLE project.Crematory (
    id INT PRIMARY KEY,
    location VARCHAR(255),
    price DECIMAL(10,2)
);

CREATE TABLE project.Priest (
    id INT PRIMARY KEY,
    price DECIMAL(10,2)
);

CREATE TABLE project.Church (
    id INT PRIMARY KEY,
    name VARCHAR(255),
	priest_id INT REFERENCES project.Priest(id),
    location VARCHAR(255)
);

CREATE TABLE project.Ceremony (
    funeral_id INT REFERENCES project.Funeral(num_process),
    church_id INT REFERENCES project.Church(id),
    PRIMARY KEY (funeral_id, church_id)
);

CREATE TABLE project.Products (
    id INT PRIMARY KEY,
    price DECIMAL(10,2),
    stock INT
);

CREATE TABLE project.Container (
    id INT PRIMARY KEY REFERENCES project.Products(id),
    supplier VARCHAR(255),
    size VARCHAR(50)
);

CREATE TABLE project.Coffin (
    id INT PRIMARY KEY REFERENCES project.Container(id),
    color VARCHAR(50),
    weight DECIMAL(10,2)
);

CREATE TABLE project.Urn (
    id INT PRIMARY KEY REFERENCES project.Container(id)
);

CREATE TABLE project.Flowers (
    id INT PRIMARY KEY REFERENCES project.Products(id),
    type VARCHAR(50),
    color VARCHAR(50)
);

CREATE TABLE project.Florist (
    id INT PRIMARY KEY,
    name VARCHAR(255),
    address VARCHAR(255),
    contact VARCHAR(50),
    nif VARCHAR(50)
);

CREATE TABLE project.Funeral_Products (
    funeral_id INT REFERENCES project.Funeral(num_process),
    product_id INT REFERENCES project.Products(id),
    quantity INT,
    PRIMARY KEY (funeral_id, product_id)
);

CREATE TABLE project.Burial (
    funeral_id INT REFERENCES project.Funeral(num_process),
    cemetery_id INT REFERENCES project.Cemetery(id),
    num_grave INT,
    PRIMARY KEY (funeral_id, cemetery_id)
);

CREATE TABLE project.Cremation (
    funeral_id INT REFERENCES project.Funeral(num_process),
    crematory_id INT REFERENCES project.Crematory(id),
    PRIMARY KEY (funeral_id, crematory_id)
);

CREATE TABLE project.Florist_Flowers (
    florist_id INT REFERENCES project.Florist(id),
    flower_id INT REFERENCES project.Flowers(id),
    PRIMARY KEY (florist_id, flower_id)
);

ALTER TABLE project.Client ADD representative_id INT REFERENCES project.Representative(id);
ALTER TABLE project.Funeral ADD client_id INT REFERENCES project.Client(id);