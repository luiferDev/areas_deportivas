using areas_deportivas.Models.DTO;
using FluentValidation;

namespace areas_deportivas.Validations;

public class ValidacionesCrearAreaDeportiva : AbstractValidator<CreateAreaDeportivaDto>
{
    public ValidacionesCrearAreaDeportiva()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio");
        RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es obligatoria");
        RuleFor(x => x.ImageUrl).NotNull().NotEmpty().WithMessage("La imagen es obligatoria");
        RuleFor(x => x.TipoArea)
            .IsInEnum().WithMessage("El tipo de área debe estar entre los valores permitidos");
        RuleFor(x => x.Precio).NotEmpty().WithMessage("El {PropertyName} es obligatorio");
    }
}