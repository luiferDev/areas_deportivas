using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class Persona
{
    public Guid Id { get; set; }

    public string? Nombre { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public Guid TipoPersonaId { get; set; }

    public virtual Administrador TipoPersona { get; set; } = null!;

    public virtual Usuario TipoPersona1 { get; set; } = null!;

    public virtual Instructor TipoPersonaNavigation { get; set; } = null!;
}
