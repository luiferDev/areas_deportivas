using System;
using System.Collections.Generic;
using areas_deportivas.Models.Enums;

namespace areas_deportivas.Models;

public partial class Reserva
{
    public Guid Id { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public Estado EstadoReserva { get; set; }

    public Guid IdUsuario { get; set; }

    public int IdAreaDeportiva { get; set; }

    public virtual AreaDeportiva IdAreaDeportivaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
