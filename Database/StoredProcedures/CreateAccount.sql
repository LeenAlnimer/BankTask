CREATE OR ALTER PROCEDURE CreateAccount
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @AccountNumber VARCHAR(30),
    @Balance DECIMAL(18,2),
    @Currency CHAR(3),
    @Status TINYINT,
    @AccountType TINYINT,
    @CreatedAt DATETIME2,
    @UpdatedAt DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Accounts
    (
        Id,
        UserId,
        AccountNumber,
        Balance,
        Currency,
        Status,
        AccountType,
        CreatedAt,
        UpdatedAt
    )
    VALUES
    (
        @Id,
        @UserId,
        @AccountNumber,
        @Balance,
        @Currency,
        @Status,
        @AccountType,
        @CreatedAt,
        @UpdatedAt
    );

    SELECT
        Id,
        UserId,
        AccountNumber,
        Balance,
        Currency,
        Status,
        AccountType,
        CreatedAt,
        UpdatedAt
    FROM Accounts
    WHERE Id = @Id;
END;