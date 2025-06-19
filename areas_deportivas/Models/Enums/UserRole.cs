using NpgsqlTypes;

namespace areas_deportivas.Models.Enums;

public enum UserRole
{
    [PgName("Admin")]
    Admin,
    [PgName("User")]
    User,
    [PgName("Instructor")]
    Instructor
}