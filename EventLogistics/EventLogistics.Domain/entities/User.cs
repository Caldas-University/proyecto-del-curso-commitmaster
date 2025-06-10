namespace EventLogistics.Domain.Entities;

public class User : BaseEntity
{
    public string Role { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string Preferences { get; set; } = string.Empty; // Serialized JSON
    
    // Propiedades adicionales que necesitan algunos servicios
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public User()
    {
        Role = string.Empty;
        Contact = string.Empty;
        Preferences = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
    }

    public User(string role, string contact, string email = "", string phoneNumber = "")
    {
        Role = role ?? throw new ArgumentNullException(nameof(role));
        Contact = contact ?? throw new ArgumentNullException(nameof(contact));
        Preferences = string.Empty;
        Email = email ?? string.Empty;
        PhoneNumber = phoneNumber ?? string.Empty;
    }
}
