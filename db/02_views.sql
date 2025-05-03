DROP VIEW IF EXISTS vw_AllProducts;
DROP VIEW IF EXISTS vw_PriestDetails;
DROP VIEW IF EXISTS vw_PriestWithPerson;
DROP VIEW IF EXISTS vw_ProcessesWithDeceased;
DROP VIEW IF EXISTS vw_ProcessesWithFuneral;
GO

CREATE VIEW vw_AllProducts AS
SELECT
    f.id,
    'Flower' AS Tipo,
    p.price,
    p.stock,
    NULL AS supplier,
    NULL AS size,
    f.process_num,
    f.florist_nif,
    f.color,
    f.type AS FlowerType,
    NULL AS peso
FROM Flowers f
JOIN Products p ON f.id = p.id

UNION ALL

SELECT
    c.id,
    'Coffin' AS Tipo,
    p.price,
    p.stock,
    ct.supplier,
    ct.size,
    NULL,
    NULL,
    c.color,
    NULL,
    c.weight
FROM Coffin c
JOIN Container ct ON c.id = ct.id
JOIN Products p ON c.id = p.id

UNION ALL

SELECT
    u.id,
    'Urn' AS Tipo,
    p.price,
    p.stock,
    ct.supplier,
    ct.size,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL
FROM Urn u
JOIN Container ct ON u.id = ct.id
JOIN Products p ON u.id = p.id;
GO

CREATE VIEW vw_PriestDetails AS
SELECT
    p.bi AS BI,
    p.name AS Name,
	pr.title AS Title,
    r.contact AS Contact,
    pr.price AS Price
FROM Priest pr
JOIN Representative r ON pr.representative_bi = r.person_bi
JOIN Person p ON r.person_bi = p.bi;
GO

CREATE VIEW vw_PriestWithPerson  AS
SELECT
	p.representative_bi,
	r.contact,
	per.name
FROM dbo.Priest p
INNER JOIN dbo.Representative r ON r.person_bi = p.representative_bi
INNER JOIN dbo.Person per ON per.bi = r.person_bi
GO

CREATE VIEW vw_ProcessesWithDeceased  AS
SELECT
	p.num_process,
	f.deceased_bi,
	d.sex,
	d.birth_date,
	d.residence,
	d.nationality,
	d.marital_estate,
	per.name
FROM dbo.Process p
INNER JOIN dbo.Funeral f ON f.num_process = p.num_process
INNER JOIN dbo.Deceased d ON d.person_bi = f.deceased_bi
INNER JOIN dbo.Person per ON per.bi = d.person_bi
GO

CREATE VIEW vw_ProcessesWithFuneral  AS
SELECT
	p.num_process,
	f.funeral_date,
	f.location
FROM dbo.Process p
INNER JOIN dbo.Funeral f ON f.num_process = p.num_process
GO
