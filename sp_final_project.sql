CREATE PROCEDURE ValidateUserCredentials
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        UserId, Username, FullName, Email, Phone, RoleId, IsActive
    FROM Users
    WHERE Username = @Username
      AND PasswordHash = @PasswordHash
      AND IsActive = 1;
END

CREATE PROCEDURE InsertAuditLog
    @CorrelationId UNIQUEIDENTIFIER,
    @UserId INT,
    @Action NVARCHAR(50),
    @Description NVARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO AuditLogs (CorrelationId, UserId, Action, Description, CreatedAt)
    VALUES (@CorrelationId, @UserId, @Action, @Description, GETDATE());
END