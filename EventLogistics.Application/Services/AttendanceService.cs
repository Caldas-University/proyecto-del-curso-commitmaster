using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Mappers;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace EventLogistics.Application.Services;

/// <summary>
/// Servicio de aplicación para el registro de asistencia y generación de credenciales.
/// Permite registrar la asistencia de participantes (por QR o manual), generar la escarapela PDF
/// con los datos del participante y su cronograma personalizado, y obtener el contenido QR.
/// </summary>
public class AttendanceServiceApp : IAttendanceServiceApp
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IParticipantActivityRepository _participantActivityRepository;

    /// <summary>
    /// Inicializa el servicio de asistencia.
    /// </summary>
    public AttendanceServiceApp(
        IAttendanceRepository attendanceRepository,
        IParticipantRepository participantRepository,
        IParticipantActivityRepository participantActivityRepository)
    {
        _attendanceRepository = attendanceRepository;
        _participantRepository = participantRepository;
        _participantActivityRepository = participantActivityRepository;
    }

    /// <summary>
    /// Registra la asistencia de un participante escaneando un código QR.
    /// </summary>
    /// <param name="qrContent">Contenido del QR (formato: participantId|eventId|timestamp).</param>
    /// <returns>DTO de asistencia registrada.</returns>
    /// <exception cref="InvalidOperationException">Si el QR es inválido o el participante no cumple los requisitos.</exception>
    public async Task<AttendanceDto> RegisterAttendanceQrAsync(string qrContent)
    {
        var parts = qrContent.Split('|');
        if (parts.Length < 3 || !Guid.TryParse(parts[0], out var participantId) || !Guid.TryParse(parts[1], out var eventId))
            throw new InvalidOperationException("QR inválido.");

        return await RegisterAttendanceAsync(participantId, eventId, "QR");
    }

    /// <summary>
    /// Registra la asistencia de un participante ingresando manualmente su documento.
    /// </summary>
    /// <param name="document">Documento de identidad del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>DTO de asistencia registrada.</returns>
    /// <exception cref="InvalidOperationException">Si el participante no existe o no cumple los requisitos.</exception>
    public async Task<AttendanceDto> RegisterAttendanceManualAsync(string document, Guid eventId)
    {
        var participant = await _participantRepository.GetByDocumentAsync(document);
        if (participant == null)
            throw new InvalidOperationException("Participante no encontrado.");

        return await RegisterAttendanceAsync(participant.Id, eventId, "Manual");
    }

    /// <summary>
    /// Lógica central para registrar la asistencia de un participante.
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <param name="method">Método de registro ("QR" o "Manual").</param>
    /// <returns>DTO de asistencia registrada.</returns>
    /// <exception cref="InvalidOperationException">Si el participante no existe, no está inscrito o ya registró asistencia.</exception>
    public async Task<AttendanceDto> RegisterAttendanceAsync(Guid participantId, Guid eventId, string method)
    {
        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null)
            throw new InvalidOperationException("El participante no existe.");

        var inscrito = await _participantActivityRepository
            .AnyAsync(pa => pa.ParticipantId == participantId && pa.Activity != null && pa.Activity.EventId == eventId);

        if (!inscrito)
            throw new InvalidOperationException("El participante no está inscrito en ninguna actividad de este evento. Regularizar acceso.");

        if (await _attendanceRepository.ExistsAsync(participantId, eventId))
            throw new InvalidOperationException("La asistencia ya fue registrada.");

        var attendance = new Attendance(participantId, eventId, method);
        await _attendanceRepository.AddAsync(attendance);

        return AttendanceMapper.ToDto(attendance);
    }

    /// <summary>
    /// Genera la escarapela PDF y el cronograma personalizado para un participante tras registrar asistencia.
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Archivo PDF en bytes.</returns>
    /// <exception cref="InvalidOperationException">Si el participante no existe.</exception>
    public async Task<byte[]> GenerateCredentialAndSchedulePdfAsync(Guid participantId, Guid eventId)
    {
        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null)
            throw new InvalidOperationException("El participante no existe.");

        var eventName = "Nombre del Evento"; // Ajusta si tienes IEventRepository

        var participantActivities = await _participantActivityRepository.GetByParticipantAndEventAsync(participantId, eventId);
        var schedule = participantActivities.Select(pa => new ScheduleItemDto
        {
            ActivityName = pa.Activity?.Name ?? "Sin nombre",
            StartTime = pa.Activity?.StartTime ?? DateTime.MinValue,
            EndTime = pa.Activity?.EndTime ?? DateTime.MinValue,
            Place = pa.Activity?.Place ?? "Sin ubicación"
        }).ToList();

        var credencial = new CredentialDto
        {
            ParticipantName = participant.Name,
            AccessType = participant.AccessType,
            EventName = eventName,
            QRCode = $"{participant.Id}|{eventId}|{DateTime.UtcNow:yyyyMMddHHmmss}",
            Schedule = schedule
        };

        return GenerarEscarapelaConCronograma(credencial);
    }

    /// <summary>
    /// Obtiene el contenido QR para un participante y evento (útil para enviar o mostrar el QR antes del evento).
    /// </summary>
    /// <param name="participantId">ID del participante.</param>
    /// <param name="eventId">ID del evento.</param>
    /// <returns>Contenido del QR como string.</returns>
    /// <exception cref="InvalidOperationException">Si el participante no existe o no está inscrito.</exception>
    public async Task<string> GetQrContentAsync(Guid participantId, Guid eventId)
    {
        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null)
            throw new InvalidOperationException("El participante no existe.");

        var inscrito = await _participantActivityRepository
            .AnyAsync(pa => pa.ParticipantId == participantId && pa.Activity != null && pa.Activity.EventId == eventId);

        if (!inscrito)
            throw new InvalidOperationException("El participante no está inscrito en ninguna actividad de este evento.");

        var qrContent = $"{participantId}|{eventId}|{DateTime.UtcNow:yyyyMMddHHmmss}";
        return qrContent;
    }

    /// <summary>
    /// Genera el PDF de la escarapela y cronograma personalizado.
    /// </summary>
    /// <param name="credencial">Datos del participante y cronograma.</param>
    /// <returns>Archivo PDF en bytes.</returns>
    private byte[] GenerarEscarapelaConCronograma(CredentialDto credencial)
    {
        using var stream = new MemoryStream();

        var qrImageBytes = GenerateQrImage(credencial.QRCode);

        // Color según tipo de acceso
        var accessColor = credencial.AccessType switch
        {
            "VIP" => Colors.Yellow.Lighten2,
            "Ponente" => Colors.Green.Lighten3,
            "Asistente" => Colors.Blue.Lighten4,
            _ => Colors.Grey.Lighten3
        };

        var fechaEvento = DateTime.Now.ToString("dd/MM/yyyy"); // Ajusta si tienes la fecha real

        var actividadProxima = credencial.Schedule
            .OrderBy(s => s.StartTime)
            .FirstOrDefault(s => s.StartTime > DateTime.Now);

        var documento = Document.Create(container =>
        {
            // Página 1: Credencial
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A6.Landscape());
                page.PageColor(accessColor); // Color por tipo de acceso
                page.DefaultTextStyle(x => x.FontSize(13).FontFamily("Arial"));

                page.Header().Row(row =>
                {
                    row.RelativeItem().AlignMiddle().Text("CREDENCIAL DE PARTICIPANTE")
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Darken2).AlignLeft();
                    row.ConstantItem(40).Height(40).AlignRight().AlignTop()
                        .Image(qrImageBytes).FitArea();
                });

                page.Content().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("Nombre:").SemiBold().FontColor(Colors.Grey.Darken2).FontSize(13);
                        c.Item().Text(credencial.ParticipantName).Bold().FontSize(16).FontColor(Colors.Black);

                        c.Item().Text("Documento:").SemiBold().FontColor(Colors.Grey.Darken2).FontSize(12);
                        c.Item().Text(credencial.EventName ?? "N/A").FontColor(Colors.Black);

                        c.Item().Text("ID:").SemiBold().FontColor(Colors.Grey.Darken2).FontSize(12);
                        c.Item().Text(credencial.ParticipantName ?? "N/A").FontColor(Colors.Black);

                        c.Item().Text("Tipo de acceso:").SemiBold().FontColor(Colors.Grey.Darken2).FontSize(12);
                        c.Item().Text(credencial.AccessType).Bold().FontColor(Colors.Blue.Medium);

                        c.Item().Text("Evento:").SemiBold().FontColor(Colors.Grey.Darken2).FontSize(12);
                        c.Item().Text(credencial.EventName).Bold().FontColor(Colors.Black);

                        c.Item().Text("Fecha:").SemiBold().FontColor(Colors.Grey.Darken2).FontSize(12);
                        c.Item().Text(fechaEvento).FontColor(Colors.Black);

                        c.Item().PaddingTop(8).Text("Presente esta credencial en cada acceso.").Italic().FontColor(Colors.Grey.Darken2).FontSize(10);
                    });

                    row.ConstantItem(90)
                        .AlignCenter()
                        .AlignMiddle()
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .Padding(6)
                        .Column(qrCol =>
                        {
                            qrCol.Item().Text("QR").FontSize(10).FontColor(Colors.Blue.Medium).AlignCenter();
                            qrCol.Item().Image(qrImageBytes, ImageScaling.FitArea);
                        });
                });

                page.Footer().AlignCenter().Text("Conserve esta credencial durante todo el evento.")
                    .FontSize(9).FontColor(Colors.Grey.Darken2);
            });

            // Página 2: Cronograma
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A6.Landscape());
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(13).FontFamily("Arial"));

                page.Header().Column(col =>
                {
                    col.Item().AlignCenter().Height(40).Width(40).Image(qrImageBytes);
                    col.Item().AlignCenter().PaddingTop(8).Text("Cronograma de actividades")
                        .FontSize(15).SemiBold().FontColor(Colors.Blue.Darken2);
                });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // Actividad
                        columns.ConstantColumn(55); // Inicio
                        columns.ConstantColumn(55); // Fin
                        columns.RelativeColumn(2); // Lugar
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Background(Colors.Blue.Medium).Text("Actividad").SemiBold().FontColor(Colors.White);
                        header.Cell().Element(CellStyle).Background(Colors.Blue.Medium).Text("Inicio").SemiBold().FontColor(Colors.White);
                        header.Cell().Element(CellStyle).Background(Colors.Blue.Medium).Text("Fin").SemiBold().FontColor(Colors.White);
                        header.Cell().Element(CellStyle).Background(Colors.Blue.Medium).Text("Lugar").SemiBold().FontColor(Colors.White);
                    });

                    foreach (var s in credencial.Schedule)
                    {
                        var isProxima = actividadProxima != null && s.ActivityName == actividadProxima.ActivityName && s.StartTime == actividadProxima.StartTime;
                        table.Cell().Element(CellStyle).Background(isProxima ? Colors.Yellow.Lighten3 : Colors.White).Text(s.ActivityName);
                        table.Cell().Element(CellStyle).Background(isProxima ? Colors.Yellow.Lighten3 : Colors.White).Text(s.StartTime.ToString("HH:mm"));
                        table.Cell().Element(CellStyle).Background(isProxima ? Colors.Yellow.Lighten3 : Colors.White).Text(s.EndTime.ToString("HH:mm"));
                        table.Cell().Element(CellStyle).Background(isProxima ? Colors.Yellow.Lighten3 : Colors.White).Text(s.Place);
                    }
                });

                page.Footer().AlignCenter().Text("¡Disfrute el evento!").FontSize(10).FontColor(Colors.Grey.Darken2);
            });
        });

        documento.GeneratePdf(stream);
        return stream.ToArray();

        IContainer CellStyle(IContainer container) =>
            container.PaddingVertical(2).PaddingHorizontal(4);
    }

    /// <summary>
    /// (Opcional) Genera un string base64 simulado para el QR.
    /// </summary>
    /// <param name="content">Contenido a codificar.</param>
    /// <returns>String en base64.</returns>
    private string GenerateQrBase64(string content)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
    }

    private byte[] GenerateQrImage(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCoder.QRCode(qrData);
        using var bitmap = qrCode.GetGraphic(20);
        using var ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }

    private byte[] GenerateQrSvg(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var svgQrCode = new SvgQRCode(qrData);
        var svg = svgQrCode.GetGraphic(5);
        return System.Text.Encoding.UTF8.GetBytes(svg);
    }

    /// <summary>
    /// Lista todas las asistencias registradas.
    /// </summary>
    /// <returns>Lista de asistencias.</returns>
    public async Task<IEnumerable<AttendanceDto>> ListAllAsync()
    {
        var attendances = await _attendanceRepository.GetAllAsync();
        return attendances.Select(AttendanceMapper.ToDto);
    }

    /// <summary>
    /// Obtiene una asistencia por su ID.
    /// </summary>
    /// <param name="attendanceId">ID de la asistencia.</param>
    /// <returns>DTO de asistencia o null si no existe.</returns>
    public async Task<AttendanceDto?> GetByIdAsync(Guid attendanceId)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(attendanceId);
        return attendance != null ? AttendanceMapper.ToDto(attendance) : null;
    }

    // Listar asistencias por evento
    public async Task<IEnumerable<AttendanceDto>> ListByEventAsync(Guid eventId)
    {
        var attendances = await _attendanceRepository.GetAllByEventAsync(eventId);
        return attendances.Select(AttendanceMapper.ToDto);
    }

    // Listar asistencias por participante
    public async Task<IEnumerable<AttendanceDto>> ListByParticipantAsync(Guid participantId)
    {
        var attendances = await _attendanceRepository.GetByParticipantAsync(participantId);
        return attendances.Select(AttendanceMapper.ToDto);
    }

    // Obtener cronograma personalizado de un participante para un evento
    public async Task<IEnumerable<ActivityDto>> GetScheduleAsync(Guid participantId, Guid eventId)
    {
        var participantActivities = await _participantActivityRepository.GetByParticipantAndEventAsync(participantId, eventId);
        return participantActivities
            .Where(pa => pa.Activity != null)
            .Select(pa => new ActivityDto
            {
                Id = pa.Activity.Id,
                Name = pa.Activity.Name,
                Place = pa.Activity.Place,
                StartTime = pa.Activity.StartTime,
                EndTime = pa.Activity.EndTime,
                EventId = pa.Activity.EventId
            });
    }

    // Verificar inscripción de participante en evento
    public async Task<bool> VerifyInscriptionAsync(Guid participantId, Guid eventId)
    {
        return await _participantActivityRepository
            .AnyAsync(pa => pa.ParticipantId == participantId && pa.Activity != null && pa.Activity.EventId == eventId);
    }

    // Regularizar inscripción en tiempo real
    public async Task RegularizeInscriptionAsync(Guid participantId, Guid eventId)
    {
        // Lógica para inscribir al participante en el evento (puedes ajustar según tu modelo)
        // Ejemplo: inscribirlo en una actividad general del evento
        var actividades = await _participantActivityRepository.GetActivitiesByEventAsync(eventId);
        if (actividades.Any())
        {
            var actividad = actividades.First();
            var nuevaInscripcion = new ParticipantActivity(participantId, actividad.Id, actividad);
            await _participantActivityRepository.AddAsync(nuevaInscripcion);
        }
        // Si no hay actividades, puedes lanzar una excepción o manejarlo según tu lógica
    }

    // Obtener resumen de asistencia por evento
    public async Task<AttendanceSummaryDto> GetAttendanceSummaryAsync(Guid eventId)
    {
        var totalRegistered = await _participantActivityRepository.CountParticipantsByEventAsync(eventId);
        var totalAttended = await _attendanceRepository.GetAllByEventAsync(eventId);
        var eventName = "Nombre del Evento"; // Ajusta si tienes IEventRepository

        return new AttendanceSummaryDto
        {
            EventId = eventId,
            EventName = eventName,
            TotalRegistered = totalRegistered,
            TotalAttended = totalAttended.Count,
        };
    }

    public async Task<IEnumerable<ParticipantDto>> ListParticipantsByEventAsync(Guid eventId)
    {
        var participants = await _participantActivityRepository.GetParticipantsByEventAsync(eventId);
        return participants.Select(ParticipantMapper.ToDto);
    }
}