using System;
using System.Collections.Generic;

namespace areas_deportivas.Models;

public partial class Reserva
{
    public Guid Id { get; set; }

    public DateOnly Fecha { get; set; }

    public DateTimeOffset HoraInicio { get; set; }

    public DateTimeOffset HoraFin { get; set; }

    public Estado EstadoReserva { get; set; }

	public Guid IdUsuario { get; set; }

    public int IdAreaDeportiva { get; set; }

    public required virtual AreaDeportiva IdAreaDeportivaNavigation { get; set; }

    public required virtual Usuario IdUsuarioNavigation { get; set; }
}
