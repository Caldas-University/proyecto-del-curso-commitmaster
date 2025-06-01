using System.Threading.Tasks;
using EventLogistics.Application.Services;

public class BadgeData
{
    public string ParticipantName { get; set; } = string.Empty;
    public string AccessType { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
    public string QrCode { get; set; } = string.Empty;
    // Puedes agregar más campos según lo que muestre la credencial
}

public class BadgeGenerator : IBadgeGenerator
{
    public Task<byte[]> GenerateAsync(BadgeData badgeData)
    {
        // Aquí iría la lógica real para crear el PDF o imagen.
        // Por ahora, devolvemos un array vacío solo para que compile y funcione.
        return Task.FromResult(new byte[0]);
    }
}
