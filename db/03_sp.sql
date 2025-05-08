DROP PROCEDURE IF EXISTS AuthenticateUser;
DROP PROCEDURE IF EXISTS RegisterUser;
DROP PROCEDURE IF EXISTS sp_addCemetery;
DROP PROCEDURE IF EXISTS sp_addChurch;
DROP PROCEDURE IF EXISTS sp_addCrematory;
DROP PROCEDURE IF EXISTS sp_deleteChurch;
DROP PROCEDURE IF EXISTS sp_updateCemetery;
DROP PROCEDURE IF EXISTS sp_updateChurch;
DROP PROCEDURE IF EXISTS sp_updateCrematory;
DROP PROCEDURE IF EXISTS sp_updateUser;
GO

CREATE PROCEDURE AuthenticateUser
    @Username VARCHAR(100),
    @Password VARCHAR(256),
    @Status BIT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT,
	@UserID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @ID INT;
        DECLARE @User NVARCHAR(100);
        DECLARE @Pass NVARCHAR(256);

        SELECT @ID = id, @User=username, @Pass=password
		FROM dbo.Users
        WHERE username = @Username;

        IF @ID IS NULL
        BEGIN
            SET @Status = 0;
            SET @Message = 'Invalid Password or Username';
            RETURN;
        END

        IF @Password != @Pass
        BEGIN
            SET @Status = 0;
            SET @Message = 'Invalid Password or Username';
            RETURN;
        END

        SET @Status = 1;
        SET @Message = 'Login successful';
		SET @UserID = @ID
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE RegisterUser
    @Email VARCHAR(100),
    @Username NVARCHAR(100),
    @Password VARCHAR(256),
    @Name NVARCHAR(50),
    @ProfilePicture VARBINARY(MAX) = NULL,
	@Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF EXISTS(SELECT * FROM dbo.Users WHERE mail = @Email)
        BEGIN
            SET @Status = 0;
            SET @Message = 'Email already in use';
            RETURN;
        END

        IF EXISTS(SELECT * FROM dbo.Users WHERE username = @Username)
        BEGIN
            SET @Status = 0;
            SET @Message = 'Username already in use';
            RETURN;
        END

        INSERT INTO dbo.Users(name, mail, username, password, profilePicture)
        VALUES(@Name, @Email, @Username, @Password, @ProfilePicture);

        SET @Status = 1;
        SET @Message = 'User registered successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_addCemetery
    @Location NVARCHAR(255),
	@Contact INT,
	@Price DECIMAL(18, 2),
    @Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Cemetery(location, contact, price)
        VALUES(@Location, @Contact, @Price)

        SET @Status = 1;
        SET @Message = 'Cemetery add successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_addChurch
    @Location NVARCHAR(255),
	@Name INT,
    @Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Church(location, name)
        VALUES(@Location, @Name)

        SET @Status = 1;
        SET @Message = 'Church add successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_addCrematory
    @Location NVARCHAR(255),
	@Contact INT,
	@Price DECIMAL(18, 2),
    @Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.Crematory(location, contact, price)
        VALUES(@Location, @Contact, @Price)

        SET @Status = 1;
        SET @Message = 'Crematory add successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_deleteChurch
    @ChurchID INT,
	@Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM Have WHERE church_id = @ChurchID;

        DELETE FROM Church WHERE id = @ChurchID;

        COMMIT;
		SET @Status = 1;
        SET @Message = 'Church deleted successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_updateCemetery
	@CemeteryID INT,
    @Location NVARCHAR(4000) = NULL,
    @Contact INT,
    @Price DECIMAL(10, 2) = NULL,
    @Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN

    BEGIN TRY
        UPDATE Cemetery
        SET location = ISNULL(@Location, location),
            contact = ISNULL(@Contact, contact),
            price = ISNULL(@Price, price)
        WHERE id = @CemeteryID;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @Status = 0;
            SET @Message = 'Cemetery not found';
            RETURN;
        END

        SET @Status = 1;
        SET @Message = 'Cemetery details updated successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_updateChurch
	@ChurchID INT,
    @Location NVARCHAR(4000) = NULL,
    @Name  NVARCHAR(4000) = NULL,
    @Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN

    BEGIN TRY
        UPDATE Church
        SET location = ISNULL(@Location, location),
            name = ISNULL(@Name, name)
        WHERE id = @ChurchID;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @Status = 0;
            SET @Message = 'Church not found';
            RETURN;
        END

        SET @Status = 1;
        SET @Message = 'Church details updated successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_updateCrematory
	@CrematoryID INT,
    @Location NVARCHAR(4000) = NULL,
    @Contact INT,
    @Price DECIMAL(10, 2) = NULL,
    @Status INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN

    BEGIN TRY
        UPDATE Crematory
        SET location = ISNULL(@Location, location),
            contact = ISNULL(@Contact, contact),
            price = ISNULL(@Price, price)
        WHERE id = @CrematoryID;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @Status = 0;
            SET @Message = 'Crematory not found';
            RETURN;
        END

        SET @Status = 1;
        SET @Message = 'Crematory details updated successfully';
    END TRY
    BEGIN CATCH
        SET @Status = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE PROCEDURE sp_updateUser
	@UserID INT,
    @Name VARCHAR(100) = NULL,
    @Email VARCHAR(100) = NULL,
    @Username NVARCHAR(100) = NULL,
	@Password NVARCHAR(100) = NULL,
	@ProfilePicture VARBINARY(MAX) = NULL,
    @StatusOut INT OUTPUT,
    @Message NVARCHAR(4000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE Users
        SET name = ISNULL(@Name, name),
            username = ISNULL(@Username, username),
            mail = ISNULL(@Email, mail),
            password = ISNULL(@Password, password),
            ProfilePicture = ISNULL(@ProfilePicture, ProfilePicture)
        WHERE id = @UserID;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @StatusOut = 0;
            SET @Message = 'User not found';
            RETURN;
        END

        SET @StatusOut = 1;
        SET @Message = 'User details updated successfully';
    END TRY
    BEGIN CATCH
        SET @StatusOut = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO
