namespace EventLogistics.Application.DTOs;

/// <summary>
/// DTO para los datos del participante.
/// </summary>
public class ParticipantDto
{
    /// <summary>
    /// Identificador único del participante.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre completo del participante.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Documento de identidad del participante.
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del participante.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de acceso (Asistente, Ponente, VIP, etc.).
    /// </summary>
    public string AccessType { get; set; } = string.Empty;
}