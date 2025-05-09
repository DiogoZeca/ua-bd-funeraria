-- SQLBook: Code
DROP TRIGGER IF EXISTS trg_UpdateProductStock_Cremation;
DROP TRIGGER IF EXISTS trg_UpdateProductStock_Burial;
GO
DROP TRIGGER IF EXISTS trg_DeleteProcess;
GO

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

CREATE TRIGGER trg_DeleteProcess
ON dbo.Process
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get the process IDs that are being deleted
    DECLARE @deletedProcesses TABLE (process_id INT);
    INSERT INTO @deletedProcesses SELECT num_process FROM deleted;
    
    -- Delete from Flowers first (references Process directly)
    DELETE FROM dbo.Flowers 
    WHERE process_num IN (SELECT process_id FROM @deletedProcesses);
    
    -- Delete from Cremation (relies on Funeral)
    DELETE FROM dbo.Cremation 
    WHERE funeral_id IN (SELECT process_id FROM @deletedProcesses);
    
    -- Delete from Burial (relies on Funeral)
    DELETE FROM dbo.Burial 
    WHERE funeral_id IN (SELECT process_id FROM @deletedProcesses);
    
    -- Delete from Funeral
    DELETE FROM dbo.Funeral 
    WHERE num_process IN (SELECT process_id FROM @deletedProcesses);
    
    -- Finally delete from Process table
    DELETE FROM dbo.Process 
    WHERE num_process IN (SELECT process_id FROM @deletedProcesses);
END;
GO