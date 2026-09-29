CREATE OR REPLACE FUNCTION get_audit_logs(
    p_offset INTEGER,
    p_limit INTEGER
)
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
LANGUAGE SQL
AS $$
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
    FROM audit_logs AS al
    ORDER BY al.created_at DESC
    LIMIT p_limit
    OFFSET p_offset;
$$;