CREATE OR ALTER PROCEDURE GetNextAccountNumber
AS
BEGIN
    SET NOCOUNT ON;

    SELECT NEXT VALUE FOR AccountNumberSequence AS AccountNumber;
END;