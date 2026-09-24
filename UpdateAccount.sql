CREATE OR ALTER PROCEDURE UpdateAccount
    @Id UNIQUEIDENTIFIER,
    @Status TINYINT,
    @UpdatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Accounts
    SET
        Status = @Status,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS RowsAffected;
END;