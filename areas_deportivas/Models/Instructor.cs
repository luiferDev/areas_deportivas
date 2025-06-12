using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class Instructor
{
    public Guid Id { get; set; }

    public required string Especialidad { get; set; }

    public virtual Persona? Persona { get; set; }
}
