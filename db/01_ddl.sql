-- SQLBook: Code
CREATE SCHEMA project
GO

IF OBJECT_ID('dbo.Cremation', 'U') IS NOT NULL
    DROP TABLE dbo.Cremation
IF OBJECT_ID('dbo.Burial', 'U') IS NOT NULL
    DROP TABLE dbo.Burial
IF OBJECT_ID('dbo.Urn', 'U') IS NOT NULL
    DROP TABLE dbo.Urn
IF OBJECT_ID('dbo.Coffin', 'U') IS NOT NULL
    DROP TABLE dbo.Coffin
IF OBJECT_ID('dbo.Container', 'U') IS NOT NULL
    DROP TABLE dbo.Container
IF OBJECT_ID('dbo.Flowers', 'U') IS NOT NULL
    DROP TABLE dbo.Flowers
IF OBJECT_ID('dbo.Florist', 'U') IS NOT NULL
    DROP TABLE dbo.Florist
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL
    DROP TABLE dbo.Products
IF OBJECT_ID('dbo.Crematory', 'U') IS NOT NULL
    DROP TABLE dbo.Crematory
IF OBJECT_ID('dbo.Cemetery', 'U') IS NOT NULL
    DROP TABLE dbo.Cemetery
IF OBJECT_ID('dbo.Have', 'U') IS NOT NULL
    DROP TABLE dbo.Have
IF OBJECT_ID('dbo.Church', 'U') IS NOT NULL
    DROP TABLE dbo.Church
IF OBJECT_ID('dbo.Priest', 'U') IS NOT NULL
    DROP TABLE dbo.Priest
IF OBJECT_ID('dbo.Funeral', 'U') IS NOT NULL
    DROP TABLE dbo.Funeral
IF OBJECT_ID('dbo.Process', 'U') IS NOT NULL
    DROP TABLE dbo.Process
IF OBJECT_ID('dbo.Deceased', 'U') IS NOT NULL
    DROP TABLE dbo.Deceased
IF OBJECT_ID('dbo.Client', 'U') IS NOT NULL
    DROP TABLE dbo.Client
IF OBJECT_ID('dbo.Representative', 'U') IS NOT NULL
    DROP TABLE dbo.Representative
IF OBJECT_ID('dbo.Person', 'U') IS NOT NULL
    DROP TABLE dbo.Person
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users

CREATE TABLE dbo.Users (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    name VARCHAR(255),
    username VARCHAR(255) UNIQUE,
    password VARCHAR(255),
    mail VARCHAR(255),
    ProfilePicture VARBINARY(MAX)
);

CREATE TABLE dbo.Person (
    bi VARCHAR(50) PRIMARY KEY NOT NULL,
    name VARCHAR(255)
);

CREATE TABLE dbo.Representative (
    person_bi VARCHAR(50) PRIMARY KEY REFERENCES dbo.Person(bi),
    contact VARCHAR(255)
);

CREATE TABLE dbo.Client (
    client_bi VARCHAR(50) PRIMARY KEY REFERENCES dbo.Representative(person_bi)
);

CREATE TABLE dbo.Deceased (
    person_bi VARCHAR(50) PRIMARY KEY REFERENCES dbo.Person(bi),
    sex CHAR(1),
    birth_date DATE,
    marital_estate VARCHAR(50),
    residence VARCHAR(255),
    nationality VARCHAR(100),
    picture VARBINARY(MAX)
);

CREATE TABLE dbo.Process (
    num_process INT PRIMARY KEY NOT NULL,
    start_date DATE,
    status VARCHAR(50),
    budget DECIMAL(10,2),
    description TEXT,
    type_of_payment VARCHAR(50),
    user_id INT REFERENCES dbo.Users(id),
	client_id VARCHAR(50) REFERENCES dbo.Client(client_bi),
    degree_kinship VARCHAR(255),
);

CREATE TABLE dbo.Funeral (
    num_process INT REFERENCES dbo.Process(num_process),
    funeral_date DATE,
    location VARCHAR(255),
    deceased_bi VARCHAR(50) REFERENCES dbo.Deceased(person_bi),
	PRIMARY KEY (num_process)
);

CREATE TABLE dbo.Priest (
    representative_bi VARCHAR(50) PRIMARY KEY REFERENCES dbo.Representative(person_bi),
    price DECIMAL(10,2)
);

CREATE TABLE dbo.Church (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    name VARCHAR(255),
    location VARCHAR(255)
);

CREATE TABLE dbo.Have (
    priest_bi VARCHAR(50) REFERENCES dbo.Priest(representative_bi),
    church_id INT REFERENCES dbo.Church(id)
);

CREATE TABLE dbo.Cemetery (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    location VARCHAR(255),
    price DECIMAL(10,2),
    contact INT NOT NULL
);

CREATE TABLE dbo.Crematory (
    id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    location VARCHAR(255),
    price DECIMAL(10,2),
    contact INT NOT NULL
);

CREATE TABLE dbo.Products (
    id INT PRIMARY KEY NOT NULL,
    price DECIMAL(10,2),
    stock INT
);

CREATE TABLE dbo.Florist (
    nif INT PRIMARY KEY,
    name VARCHAR(255),
    address VARCHAR(255),
    contact VARCHAR(50)
);

CREATE TABLE dbo.Flowers (
    id INT PRIMARY KEY REFERENCES dbo.Products(id),
    process_num INT REFERENCES dbo.Process(num_process),
    florist_nif INT REFERENCES dbo.Florist(nif),
    type VARCHAR(50),
    quantity INT,
    color VARCHAR(50)
);

CREATE TABLE dbo.Container (
    id INT PRIMARY KEY REFERENCES dbo.Products(id),
    supplier VARCHAR(255),
    size VARCHAR(50)
);

CREATE TABLE dbo.Coffin (
    id INT PRIMARY KEY REFERENCES dbo.Container(id),
    color VARCHAR(50),
    weight DECIMAL(10,2)
);

CREATE TABLE dbo.Urn (
    id INT PRIMARY KEY REFERENCES dbo.Container(id)
);

CREATE TABLE dbo.Cremation (
    funeral_id INT REFERENCES dbo.Funeral(num_process),
    crematory_id INT REFERENCES dbo.Crematory(id),
    coffin_id INT REFERENCES dbo.Coffin(id),
    urn_id INT REFERENCES dbo.Urn(id),
    PRIMARY KEY (funeral_id)
);

CREATE TABLE dbo.Burial (
    funeral_id INT REFERENCES dbo.Funeral(num_process),
    cemetery_id INT REFERENCES dbo.Cemetery(id),
    conffin_id INT REFERENCES dbo.Coffin(id),
    num_grave INT,
    PRIMARY KEY (funeral_id)
);

ALTER TABLE dbo.Priest
ADD title VARCHAR(20)
    CHECK (title IN ('Priest', 'Deacon', 'Pastor'));
