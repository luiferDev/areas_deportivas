namespace areas_deportivas.Services;

public interface IUsuarioService
{
	Task ReservarArea();
	Task CancelarArea();
}