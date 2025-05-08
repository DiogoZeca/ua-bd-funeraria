-- SQLBook: Code
CREATE TRIGGER trg_UpdateProductStock_Cremation
ON dbo.Cremation
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @coffinId INT, @urnId INT, @coffinStock INT, @urnStock INT;

    SELECT @coffinId = coffin_id, @urnId = urn_id FROM inserted;

    SELECT @coffinStock = stock FROM dbo.Products WHERE id = @coffinId;
    IF @coffinStock IS NULL OR @coffinStock < 1
    BEGIN
        RAISERROR('Stock insuficiente para o caixão (cremação).', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    SELECT @urnStock = stock FROM dbo.Products WHERE id = @urnId;
    IF @urnStock IS NULL OR @urnStock < 1
    BEGIN
        RAISERROR('Stock insuficiente para a urna.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    UPDATE dbo.Products SET stock = stock - 1 WHERE id = @coffinId;
    UPDATE dbo.Products SET stock = stock - 1 WHERE id = @urnId;
END;
GO

CREATE TRIGGER trg_UpdateProductStock_Burial
ON dbo.Burial
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @coffinId INT, @coffinStock INT;

    SELECT @coffinId = coffin_id FROM inserted;

    SELECT @coffinStock = stock FROM dbo.Products WHERE id = @coffinId;
    IF @coffinStock IS NULL OR @coffinStock < 1
    BEGIN
        RAISERROR('Stock insuficiente para o caixão (enterramento).', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    UPDATE dbo.Products SET stock = stock - 1 WHERE id = @coffinId;
END;
GO