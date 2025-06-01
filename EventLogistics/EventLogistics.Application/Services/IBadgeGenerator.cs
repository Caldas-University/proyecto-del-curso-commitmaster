using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public interface IBadgeGenerator
    {
        Task<byte[]> GenerateAsync(BadgeData badgeData);
    }

    public class BadgeGenerator : IBadgeGenerator
    {
        public Task<byte[]> GenerateAsync(BadgeData badgeData)
        {
            // Implementación...
            return Task.FromResult(new byte[0]);
        }
    }
}