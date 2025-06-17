using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class AreaDeportiva
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public Tipo TipoArea { get; set; }

    public bool Disponibilidad { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public decimal Precio { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
