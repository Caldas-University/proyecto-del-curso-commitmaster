# Event Logistics - Caso de Uso CU-LO-01

## Descripción

Este proyecto implementa el caso de uso **CU-LO-01: Registrar asistencia de participantes y generar credenciales con cronograma personalizado**. Permite a los organizadores validar la llegada de los asistentes el día del evento y entregarles su escarapela identificativa junto con un cronograma de actividades personalizado.

## Funcionalidades principales

- **Registro de asistencia** mediante escaneo de código QR o ingreso manual de nombre/documento.
- **Generación automática de credenciales** (escarapela) con datos del participante y tipo de acceso.
- **Entrega de cronograma personalizado** con las actividades y sesiones inscritas, incluyendo ubicaciones y horarios.
- **Alerta y regularización** en tiempo real si el participante no ha completado su inscripción.

## Estructura del Proyecto

- **DTOs**: Objetos de transferencia de datos para exponer solo la información relevante en la API.
- **Mappers**: Clases estáticas para transformar entidades de dominio a DTOs.
- **Controllers**: Exponen los endpoints necesarios para registrar asistencia y consultar información de participantes.

## Entidades principales

- **Participant**: Datos del asistente (nombre, documento, tipo de acceso, etc).
- **Attendance**: Registro de asistencia de cada participante.
- **Credential**: Información de la escarapela generada.
- **Activity**: Actividades y sesiones del evento.
- **ParticipantActivity**: Relación N:N entre participantes y actividades.

## Ejemplo de flujo

1. El organizador escanea el QR o ingresa manualmente los datos del participante.
2. El sistema valida la inscripción y registra la asistencia.
3. Se genera la credencial (escarapela) y el cronograma personalizado.
4. Si la inscripción está incompleta, se alerta y permite regularizar el acceso.

## Uso de la API

- **Swagger**: Documentación y pruebas interactivas disponibles en `/swagger`.
- **Postman**: Todos los endpoints están diseñados para ser consumidos desde Postman.

## Ejemplo de endpoint

- `POST /api/attendance/register`  
  Registra la asistencia de un participante y retorna la credencial y cronograma.

## Consideraciones

- No se implementa interfaz visual, solo API REST.
- La estructura y nombres siguen las mejores prácticas para claridad y mantenibilidad.

---

**Autor:**  
Equipo EventLogistics  
Universidad - Software II - 2025



EventLogistics/
│
├── EventLogistics.Api/                       # Proyecto API (Web)
│   ├── Controllers/
│   │   ├── AttendanceController.cs           # Endpoints para registrar asistencia (QR/manual)
│   │   ├── CredentialController.cs           # Endpoints para credenciales y cronograma
│   ├── DTOs/
│   │   ├── Attendance/
│   │   │   ├── AttendanceRegisterDto.cs      # Datos para registrar asistencia
│   │   │   ├── AttendanceResponseDto.cs      # Respuesta tras registrar asistencia
│   │   ├── Credential/
│   │   │   ├── CredentialResponseDto.cs      # Datos de la credencial generada
│   │   │   ├── PersonalizedScheduleDto.cs    # Cronograma personalizado
│   │   │   ├── ActivityScheduleDto.cs        # Detalle de cada actividad
│   ├── Mappers/
│   │   ├── AttendanceMapper.cs               # Mapeo entidad <-> DTO asistencia
│   │   ├── CredentialMapper.cs               # Mapeo entidad <-> DTO credencial/cronograma
│   └── Program.cs
│
├── EventLogistics.Application/               # Lógica de aplicación (servicios)
│   ├── Services/
│   │   ├── AttendanceService.cs              # Lógica de registro de asistencia
│   │   ├── CredentialService.cs              # Lógica de credenciales y cronograma
│   ├── DTOs/
│   │   ├── PersonalizedScheduleDto.cs        # DTO para cronograma personalizado
│   │   ├── ActivityScheduleDto.cs            # DTO para detalle de actividades
│   ├── Interfaces/
│   │   ├── IAttendanceService.cs
│   │   ├── ICredentialService.cs
│
├── EventLogistics.Domain/                    # Entidades y contratos de dominio
│   ├── Entities/
│   │   ├── Participant.cs
│   │   ├── Activity.cs
│   │   ├── ParticipantActivity.cs            # Relación participante-actividad
│   │   ├── Attendance.cs
│   │   ├── Credential.cs
│   │   ├── Event.cs
│   ├── Repositories/
│   │   ├── IParticipantRepository.cs
│   │   ├── IActivityRepository.cs
│   │   ├── IAttendanceRepository.cs
│   │   ├── ICredentialRepository.cs
│
├── EventLogistics.Infrastructure/            # Implementación de acceso a datos
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   ├── Repositories/
│   │   ├── ParticipantRepository.cs
│   │   ├── ActivityRepository.cs
│   │   ├── AttendanceRepository.cs
│   │   ├── CredentialRepository.cs
│
└── EventLogistics.Tests/                     # Pruebas unitarias y de integración
    ├── Application/
    │   ├── CredentialServiceTests.cs
    │   ├── AttendanceServiceTests.cs
    ├── Api/
    │   ├── AttendanceControllerTests.cs
    │   ├── CredentialControllerTests.cs