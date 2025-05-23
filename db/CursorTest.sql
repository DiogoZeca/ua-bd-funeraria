USE p2g7;
GO



GO
CREATE PROCEDURE sp_CalculateFuneralCosts
AS
BEGIN
    DECLARE @funeral_id INT,
            @cemetery_price MONEY,
            @container_price MONEY,
            @flower_price MONEY,
            @total_price MONEY;

    DECLARE funeral_cursor CURSOR FOR
        SELECT id FROM Funeral;

    OPEN funeral_cursor;

    FETCH NEXT FROM funeral_cursor INTO @funeral_id;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Preço do cemitério
        SELECT @cemetery_price = c.price
        FROM Cemetery c
        JOIN Burial b ON b.cemetery = c.id
        WHERE b.funeral = @funeral_id;

        -- Preço do caixão ou urna
        SELECT @container_price = p.price
        FROM Product p
        JOIN Container c ON c.id = p.id
        WHERE c.id IN (
            SELECT container FROM Funeral WHERE id = @funeral_id
        );

        -- Preço das flores
        SELECT @flower_price = SUM(p.price * fp.quantity)
        FROM Funeral_Product fp
        JOIN Product p ON p.id = fp.product
        JOIN Flowers f ON f.id = p.id
        WHERE fp.funeral = @funeral_id;

        SET @total_price = ISNULL(@cemetery_price, 0) +
                           ISNULL(@container_price, 0) +
                           ISNULL(@flower_price, 0);

        PRINT 'Funeral ID: ' + CAST(@funeral_id AS VARCHAR) +
              ' | Total: ' + CAST(@total_price AS VARCHAR);

        FETCH NEXT FROM funeral_cursor INTO @funeral_id;
    END;

    CLOSE funeral_cursor;
    DEALLOCATE funeral_cursor;
END;
GO
