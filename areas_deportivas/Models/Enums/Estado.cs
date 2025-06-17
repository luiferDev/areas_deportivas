using NpgsqlTypes;

public enum Estado
{
	[PgName("CONFIRMADA")]
	CONFIRMADA,
	[PgName("NO CONFIRMADA")]
	NO_CONFIRMADA,
	[PgName("PENDIENTE")]
	PENDIENTE,
	[PgName("CANCELADA")]
	CANCELADA
}