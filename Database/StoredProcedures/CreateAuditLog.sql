CREATE OR REPLACE PROCEDURE create_audit_log(
    p_id UUID,
    p_event_id UUID,
    p_user_id UUID,
    p_action VARCHAR(100),
    p_entity_type VARCHAR(100),
    p_entity_id UUID,
    p_old_values TEXT,
    p_new_values TEXT,
    p_ip_address TEXT,
    p_created_at TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO audit_logs (
        id,
        event_id,
        user_id,
        action,
        entity_type,
        entity_id,
        old_values,
        new_values,
        ip_address,
        created_at
    )
    VALUES (
        p_id,
        p_event_id,
        p_user_id,
        p_action,
        p_entity_type,
        p_entity_id,
        p_old_values::jsonb,
        p_new_values::jsonb,
        p_ip_address::inet,
        p_created_at
    );
END;
$$;