CREATE OR REPLACE FUNCTION get_all_transactions()
RETURNS TABLE
(
    id UUID,
    event_id UUID,
    source_account_id UUID,
    destination_account_id UUID,
    transaction_type SMALLINT,
    amount NUMERIC(18,2),
    currency CHAR(3),
    reference_number VARCHAR(50),
    description VARCHAR(500),
    created_at TIMESTAMPTZ
)
LANGUAGE SQL
AS $
    SELECT
        id,
        event_id,
        source_account_id,
        destination_account_id,
        transaction_type,
        amount,
        currency,
        reference_number,
        description,
        created_at
    FROM transactions
    ORDER BY created_at DESC;
$;
