namespace EventLogistics.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Role { get; private set; }
    public string Contact { get; private set; }
    public Dictionary<string, string> Preferences { get; private set; }

    private User()
    {
        Role = string.Empty;
        Contact = string.Empty;
        Preferences = new Dictionary<string, string>();
    }

    public User(string role, string contact)
    {
        Id = Guid.NewGuid();
        Role = role ?? throw new ArgumentNullException(nameof(role));
        Contact = contact ?? throw new ArgumentNullException(nameof(contact));
        Preferences = new Dictionary<string, string>();
    }
}
