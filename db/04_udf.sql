-- SQLBook: Code
DROP FUNCTION IF EXISTS dbo.findBiExists;
DROP FUNCTION IF EXISTS dbo.findNifExists;
DROP FUNCTION IF EXISTS dbo.findProcNumberExists;
GO

CREATE FUNCTION dbo.findBiExists(@bi VARCHAR(50))
RETURNS BIT
AS
BEGIN
    DECLARE @exists BIT;

    IF EXISTS (
        SELECT 1
        FROM Priest pr
        JOIN Representative r ON pr.representative_bi = r.person_bi
        JOIN Person p ON r.person_bi = p.bi
        WHERE p.bi = @bi
    )
        SET @exists = 1;
    ELSE
        SET @exists = 0;

    RETURN @exists;
END
GO

CREATE FUNCTION dbo.findNifExists(@nif INT)
RETURNS BIT
AS
BEGIN
    DECLARE @exists BIT;

    IF EXISTS (
        SELECT 1
        FROM Florist fl
        WHERE fl.nif = @nif
    )
        SET @exists = 1;
    ELSE
        SET @exists = 0;

    RETURN @exists;
END
GO

CREATE FUNCTION dbo.findProcNumberExists(@proc_number INT)
RETURNS BIT
AS 
BEGIN
    DECLARE @exists BIT;

    IF EXISTS (
        SELECT 1
        FROM Process pr
        WHERE pr.num_process = @proc_number
    )
        SET @exists = 1;
    ELSE
        SET @exists = 0;

    RETURN @exists;
END
GO