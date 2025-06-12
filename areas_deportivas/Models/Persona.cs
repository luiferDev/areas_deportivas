using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class Persona
{
    public Guid Id { get; set; }

    public required string Nombre { get; set; }

    public required string Email { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public Role Role { get; set; }

    public Guid TipoPersonaId { get; set; }

    public required virtual Administrador TipoPersona { get; set; }

    public required virtual Usuario TipoPersona1 { get; set; }

    public required virtual Instructor TipoPersonaNavigation { get; set; }
}
