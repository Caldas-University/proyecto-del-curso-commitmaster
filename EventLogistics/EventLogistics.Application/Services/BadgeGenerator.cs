using System.Threading.Tasks;
using EventLogistics.Application.Services;

public class BadgeGenerator : IBadgeGenerator
{
    public Task<byte[]> GenerateAsync(BadgeData badgeData)
    {
        // Aquí iría la lógica real para crear el PDF o imagen.
        // Por ahora, devolvemos un array vacío solo para que compile y funcione.
        return Task.FromResult(new byte[0]);
    }
}
