namespace EventLogistics.Domain.Interfaces
{
    public interface IResourceRepository
    {
        Task<bool> CheckAvailabilityAsync(string resourceType, int quantity, DateTime date);
        Task<bool> ReserveResourceAsync(int resourceId, int quantity);
    }
}