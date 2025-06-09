namespace EventLogistics.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Role { get; private set; }
    public string Contact { get; private set; }
    public Dictionary<string, string> Preferences { get; private set; }

<<<<<<< HEAD
    private User()
    {
        Role = string.Empty;
        Contact = string.Empty;
        Preferences = new Dictionary<string, string>();
=======
        public string Role { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Preferences { get; set; } // JSON for preferences
        public string PhoneNumber { get; set; }
        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
>>>>>>> sebas
    }

    public User(string role, string contact)
    {
        Id = Guid.NewGuid();
        Role = role ?? throw new ArgumentNullException(nameof(role));
        Contact = contact ?? throw new ArgumentNullException(nameof(contact));
        Preferences = new Dictionary<string, string>();
    }
}
