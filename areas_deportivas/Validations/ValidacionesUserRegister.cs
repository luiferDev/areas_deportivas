using areas_deportivas.Models.DTO;
using FluentValidation;

namespace areas_deportivas.Validations;

public class ValidacionesUserRegister : AbstractValidator<UserRegisterDto>
{
    public ValidacionesUserRegister()
    {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula")
            .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número")
            .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial");
    }
}