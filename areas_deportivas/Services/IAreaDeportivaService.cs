namespace areas_deportivas.Services;

public interface IAreaDeportivaService
{
	Task ActualizarDisponibilidadAsync(int areaId);
	Task ActualizarTodasLasDisponibilidadesAsync();
}