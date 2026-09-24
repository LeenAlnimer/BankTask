CREATE OR ALTER PROCEDURE GetAllAccounts
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
    WHERE Status <> 2;
END;