using NpgsqlTypes;

public enum Tipo
{
	[PgName("FOOTBALL")]
	FOOTBALL,
	[PgName("TENIS")]
	TENIS,
	[PgName("BASKETBALL")]
	BASKETBALL,
	[PgName("GIMNASIO")]
	GIMNASIO,
	[PgName("BILLAR")]
	BILLAR,
	[PgName("PISCINA")]
	PISCINA
}