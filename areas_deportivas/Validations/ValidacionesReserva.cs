using areas_deportivas.Models;
using areas_deportivas.Models.DTO;
using FluentValidation;

namespace areas_deportivas.Validations;

public class ValidacionesReserva : AbstractValidator<CrearReservaDto>
{
    public ValidacionesReserva()
    {
        RuleFor(x => x.Fecha)
            .Must(fecha => fecha > DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("La fecha de la reserva debe ser mayor a la fecha actual");
        RuleFor(x => x.HoraInicio).NotEmpty().WithMessage("La hora de inicio es obligatoria");
        RuleFor(x => x.HoraFin).Must((reserva, horaFin) =>
        {
            return horaFin > reserva.HoraInicio;
        }).WithMessage("La hora de la reserva debe ser mayor a la hora actual");
    }
}