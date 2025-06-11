namespace EventLogistics.Domain.Entities
{
    public class Organizator : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
        
        public Organizator()
        {
            Name = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Role = string.Empty;
        }

        public Organizator(string name, string email, string phone, string role)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}