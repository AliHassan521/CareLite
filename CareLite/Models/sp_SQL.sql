-- Get user by username
IF OBJECT_ID('sp_GetUserByUsername', 'P') IS NOT NULL DROP PROCEDURE sp_GetUserByUsername;
GO
CREATE PROCEDURE sp_GetUserByUsername
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT u.UserId, u.Username, u.PasswordHash, u.FullName, u.Email, u.Phone,
           u.RoleId, r.RoleName, u.IsActive
    FROM Users u
    INNER JOIN Roles r ON u.RoleId = r.RoleId
    WHERE u.Username = @Username;
END
GO

-- Insert audit log
IF OBJECT_ID('sp_InsertAuditLog', 'P') IS NOT NULL DROP PROCEDURE sp_InsertAuditLog;
GO
CREATE PROCEDURE sp_InsertAuditLog
    @CorrelationId UNIQUEIDENTIFIER,
    @UserId INT = NULL,
    @Action NVARCHAR(50),
    @Description NVARCHAR(300)
AS
BEGIN
    INSERT INTO AuditLogs (CorrelationId, UserId, Action, Description, CreatedAt)
    VALUES (@CorrelationId, @UserId, @Action, @Description, GETDATE());
END
GO

IF OBJECT_ID('sp_CreateUser', 'P') IS NOT NULL DROP PROCEDURE sp_CreateUser;
Go
CREATE PROCEDURE sp_CreateUser
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(255),
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @RoleId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users (Username, PasswordHash, FullName, Email, Phone, RoleId, IsActive, CreatedAt)
    VALUES (@Username, @PasswordHash, @FullName, @Email, @Phone, @RoleId, 1, GETUTCDATE());

    DECLARE @UserId INT = SCOPE_IDENTITY();

    SELECT 
        u.UserId, u.Username, u.PasswordHash, u.FullName, u.Email, u.Phone, u.RoleId, r.RoleName,
        u.IsActive, u.CreatedAt
    FROM Users u
    INNER JOIN Roles r ON u.RoleId = r.RoleId
    WHERE u.UserId = @UserId;
END

/*INSERT INTO Roles (RoleName) VALUES ('Admin');

INSERT INTO Users 
(Username, PasswordHash, FullName, Email, Phone, RoleId, IsActive, CreatedAt)
VALUES 
(
    'ali123', 
    '$2a$11$Kp0NsklVJfM0s5h7wOT6FepHEM0AXzVZyQpM5s0ENd5slfSkkk8kO', -- example BCrypt hash for "password123"
    'Ali Hassan', 
    'ali@example.com', 
    '03001234567', 
    1,
    1, 
    GETDATE()
);*/


Select * from users