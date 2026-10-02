CREATE   PROCEDURE UpdateUser
    @Id UNIQUEIDENTIFIER,
    @FullName NVARCHAR(150),
    @Email NVARCHAR(255),
    @UpdatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET
        FullName = @FullName,
        Email = @Email,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id;
END;