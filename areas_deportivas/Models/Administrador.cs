using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class Administrador
{
    public Guid Id { get; set; }

    public virtual Persona? Persona { get; set; }
}
