CREATE OR ALTER PROCEDURE CreateUser
    @Id UNIQUEIDENTIFIER,
    @FullName NVARCHAR(150),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(500),
    @CreatedAt DATETIME2,
    @UpdatedAt DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users
    (
        Id,
        FullName,
        Email,
        PasswordHash,
        CreatedAt,
        UpdatedAt
    )
    VALUES
    (
        @Id,
        @FullName,
        @Email,
        @PasswordHash,
        @CreatedAt,
        @UpdatedAt
    );
END;