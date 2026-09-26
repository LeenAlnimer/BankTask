CREATE OR REPLACE PROCEDURE create_transaction
(
    p_id UUID,
    p_event_id UUID,
    p_source_account_id UUID,
    p_destination_account_id UUID,
    p_transaction_type SMALLINT,
    p_amount NUMERIC(18,2),
    p_currency CHAR(3),
    p_reference_number VARCHAR(50),
    p_description VARCHAR(500),
    p_created_at TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO transactions
    (
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
    )
    VALUES
    (
        p_id,
        p_event_id,
        p_source_account_id,
        p_destination_account_id,
        p_transaction_type,
        p_amount,
        p_currency,
        p_reference_number,
        p_description,
        p_created_at
    );
END;
$$;