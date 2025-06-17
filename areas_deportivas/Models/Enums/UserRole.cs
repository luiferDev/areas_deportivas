using NpgsqlTypes;

public enum UserRole
{
    [PgName("Admin")]
    Admin,
    [PgName("User")]
    User,
    [PgName("Instructor")]
    Instructor
}