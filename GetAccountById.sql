CREATE OR ALTER PROCEDURE GetAccountById
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

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