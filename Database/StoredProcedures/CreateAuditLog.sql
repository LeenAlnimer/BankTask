CREATE OR REPLACE FUNCTION get_audit_log_by_id(p_id UUID)
RETURNS TABLE
(
    id UUID,
    event_id UUID,
    user_id UUID,
    action VARCHAR(100),
    entity_type VARCHAR(100),
    entity_id UUID,
    old_values TEXT,
    new_values TEXT,
    ip_address TEXT,
    created_at TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        al.id,
        al.event_id,
        al.user_id,
        al.action,
        al.entity_type,
        al.entity_id,
        al.old_values::TEXT,
        al.new_values::TEXT,
        al.ip_address::TEXT,
        al.created_at
    FROM audit_logs al
    WHERE al.id = p_id;
END;
$$;