# EventLogistics API

Sistema de gestión logística para eventos desarrollado con arquitectura limpia (Clean Architecture) en .NET 8.

## 🏗️ Arquitectura del Proyecto

El proyecto está estructurado siguiendo los principios de Clean Architecture:

```
EventLogistics/
├── EventLogistics.Api/          # Capa de Presentación (Controllers, API)
├── EventLogistics.Application/  # Capa de Aplicación (Casos de Uso, DTOs, Interfaces)
├── EventLogistics.Domain/       # Capa de Dominio (Entidades, Reglas de Negocio)
└── EventLogistics.Infrastructure/ # Capa de Infraestructura (Repositorios, Base de Datos)
```

### Capas del Sistema

1. **Domain (Dominio)**: Entidades de negocio y reglas fundamentales
2. **Application (Aplicación)**: Casos de uso y lógica de aplicación
3. **Infrastructure (Infraestructura)**: Acceso a datos y servicios externos
4. **API (Presentación)**: Controladores REST y puntos de entrada

## 🚀 Características Principales

- ✅ **Gestión de Recursos**: CRUD completo de recursos con tipos, capacidades y disponibilidad
- ✅ **Reportes Avanzados**: Generación de reportes con filtros y recursos críticos
- ✅ **Gestión de Usuarios**: Sistema de usuarios con roles (Organizador, Asistente)
- ✅ **Locaciones**: Manejo de ubicaciones para eventos
- ✅ **Arquitectura Limpia**: Separación clara de responsabilidades
- ✅ **Base de Datos SQLite**: Almacenamiento persistente
- ✅ **Swagger UI**: Documentación interactiva de la API
- ✅ **Exportación de Reportes**: PDF y Excel (implementado)

## 📋 Prerrequisitos

- .NET 8.0 SDK
- Visual Studio 2022 o VS Code
- PowerShell (para scripts de población de datos)

## 🛠️ Instalación y Configuración

### 1. Clonar y Compilar

```powershell
# Navegar al directorio del proyecto
cd "c:\Users\User\Documents\CLASES\Ingenieria de Software 2\ProyectoCommitMasters\EventLogistics"

# Restaurar dependencias
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
cd EventLogistics.Api
dotnet run
```

### 2. Verificar Instalación

La aplicación se ejecutará en: `http://localhost:5158`

- **Swagger UI**: http://localhost:5158
- **API Base**: http://localhost:5158/api

## 🗄️ Poblado de Base de Datos

### Ejecutar Script Automático

```powershell
# Asegúrate de que la aplicación esté ejecutándose
cd "c:\Users\User\Documents\CLASES\Ingenieria de Software 2\ProyectoCommitMasters\EventLogistics"
.\populate_database_simple.ps1
```

### Comandos Manuales de Población

#### 1. Crear Recursos

```powershell
# Proyector HD
$resource1 = @{
    Name = "Proyector HD"
    Type = "Audiovisual"
    Capacity = 5
    Availability = $true
    Tags = @("audiovisual", "proyector", "hd")
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5158/api/Resource" -Method POST -Body $resource1 -ContentType "application/json"

# Mesa de Conferencias
$resource2 = @{
    Name = "Mesa de Conferencias"
    Type = "Mobiliario"
    Capacity = 10
    Availability = $true
    Tags = @("mobiliario", "mesa", "conferencia")
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5158/api/Resource" -Method POST -Body $resource2 -ContentType "application/json"

# Sillas Ejecutivas
$resource3 = @{
    Name = "Sillas Ejecutivas"
    Type = "Mobiliario"
    Capacity = 50
    Availability = $true
    Tags = @("mobiliario", "silla", "ejecutiva")
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5158/api/Resource" -Method POST -Body $resource3 -ContentType "application/json"

# Micrófono Inalámbrico
$resource4 = @{
    Name = "Microfono Inalambrico"
    Type = "Audiovisual"
    Capacity = 8
    Availability = $true
    Tags = @("audiovisual", "microfono", "inalambrico")
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5158/api/Resource" -Method POST -Body $resource4 -ContentType "application/json"

# Laptop Dell
$resource5 = @{
    Name = "Laptop Dell"
    Type = "Tecnologia"
    Capacity = 3
    Availability = $true
    Tags = @("tecnologia", "laptop", "dell")
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5158/api/Resource" -Method POST -Body $resource5 -ContentType "application/json"

# Servicio de Catering
$resource6 = @{
    Name = "Servicio de Catering"
    Type = "Alimentacion"
    Capacity = 2
    Availability = $true
    Tags = @("alimentacion", "catering", "evento")
} | ConvertTo-Json -Depth 3

Invoke-RestMethod -Uri "http://localhost:5158/api/Resource" -Method POST -Body $resource6 -ContentType "application/json"
```

## 🔗 Endpoints Principales

### 📦 Recursos (`/api/Resource`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Resource` | Obtener todos los recursos |
| GET | `/api/Resource/available` | Obtener recursos disponibles |
| GET | `/api/Resource/{id}` | Obtener recurso por ID |
| POST | `/api/Resource` | Crear nuevo recurso |
| PUT | `/api/Resource/{id}/status` | Actualizar estado del recurso |
| POST | `/api/Resource/{id}/assign` | Asignar recurso a evento |
| DELETE | `/api/Resource/{id}` | Eliminar recurso |
| GET | `/api/Resource/{id}/availability` | Verificar disponibilidad |

### 📊 Reportes (`/api/Report`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Report?resourceType={type}` | Generar reporte por tipo |
| GET | `/api/Report/critical` | Obtener recursos críticos |
| GET | `/api/Report/metrics?resourceType={type}` | Métricas de recursos |
| GET | `/api/Report/export/pdf` | Exportar reporte en PDF |
| GET | `/api/Report/export/excel` | Exportar reporte en Excel |

### 👤 Usuarios (`/api/User`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/User` | Obtener todos los usuarios |
| GET | `/api/User/{id}` | Obtener usuario por ID |
| POST | `/api/User` | Crear nuevo usuario |

### 🏢 Locaciones (`/api/Location`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Location` | Obtener todas las locaciones |
| GET | `/api/Location/{id}` | Obtener locación por ID |
| POST | `/api/Location` | Crear nueva locación |

### 📅 Eventos (`/api/Event`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/Event` | Obtener todos los eventos |
| GET | `/api/Event/{id}` | Obtener evento por ID |
| POST | `/api/Event` | Crear nuevo evento |

## 🧪 Comandos de Prueba

### Verificar Recursos Creados

```powershell
# Ver todos los recursos
Invoke-RestMethod -Uri 'http://localhost:5158/api/Resource' -Method GET

# Ver recursos disponibles
Invoke-RestMethod -Uri 'http://localhost:5158/api/Resource/available' -Method GET

# Ver recursos por tipo
Invoke-RestMethod -Uri 'http://localhost:5158/api/Report?resourceType=Audiovisual' -Method GET
```

### Generar Reportes

```powershell
# Reporte de recursos audiovisuales
Invoke-RestMethod -Uri 'http://localhost:5158/api/Report?resourceType=Audiovisual' -Method GET

# Recursos críticos (con poca disponibilidad)
Invoke-RestMethod -Uri 'http://localhost:5158/api/Report/critical' -Method GET

# Reporte de mobiliario
Invoke-RestMethod -Uri 'http://localhost:5158/api/Report?resourceType=Mobiliario' -Method GET
```

### Verificar Usuarios y Locaciones

```powershell
# Ver usuarios existentes
Invoke-RestMethod -Uri 'http://localhost:5158/api/User' -Method GET

# Ver locaciones existentes
Invoke-RestMethod -Uri 'http://localhost:5158/api/Location' -Method GET

# Ver eventos
Invoke-RestMethod -Uri 'http://localhost:5158/api/Event' -Method GET
```

## 📈 Ejemplos de Respuesta

### Recurso Individual

```json
{
  "id": "a39f5a0d-dca8-494f-9289-3c89ec49459c",
  "type": "Audiovisual",
  "availability": true,
  "capacity": 5,
  "assignments": []
}
```

### Reporte de Recursos

```json
[
  {
    "id": "a39f5a0d-dca8-494f-9289-3c89ec49459c",
    "nombre": "Proyector HD",
    "tipo": "Audiovisual",
    "cantidadTotal": 5,
    "cantidadUtilizada": 0,
    "cantidadDisponible": 5,
    "eventos": [],
    "actividades": [],
    "usoTotal": 0,
    "disponible": true
  }
]
```

## 🎯 Funcionalidades Destacadas

### 1. ResourceController Completo
- ✅ CRUD completo de recursos
- ✅ Validaciones de negocio
- ✅ Manejo de errores
- ✅ Asignación de recursos a eventos
- ✅ Verificación de disponibilidad

### 2. Sistema de Reportes
- ✅ Filtros por tipo de recurso
- ✅ Identificación de recursos críticos
- ✅ Métricas de uso
- ✅ Exportación a PDF y Excel

### 3. Arquitectura Limpia
- ✅ Separación de capas
- ✅ Inyección de dependencias
- ✅ Repositorios y servicios
- ✅ DTOs y mappers

## 🐛 Solución de Problemas

### La aplicación no inicia
```powershell
# Verificar que tienes .NET 8 instalado
dotnet --version

# Restaurar dependencias
dotnet restore

# Limpiar y compilar
dotnet clean
dotnet build
```

### Error de base de datos
```powershell
# La aplicación crea automáticamente la base de datos SQLite
# Si hay problemas, elimina el archivo eventlogistics.db y reinicia
```

### Problemas con PowerShell
```powershell
# Si tienes problemas con la ejecución de scripts
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

## 📝 Estado del Proyecto

### ✅ Completado
- ResourceController con todos los endpoints
- ReportController funcionando
- Sistema de usuarios y locaciones
- Base de datos SQLite configurada
- Swagger UI habilitado
- Scripts de población de datos

### 🔄 En Desarrollo
- EventController (problemas de validación)
- Sistema de participantes
- Asignaciones de recursos a eventos

### 📋 Por Implementar
- Sistema de notificaciones completo
- Reasignación automática de recursos
- Dashboard de métricas
- Autenticación y autorización

## 🤝 Contribución

Este proyecto sigue las mejores prácticas de Clean Architecture y está diseñado para ser mantenible y escalable.

## 📞 Soporte

Para problemas o preguntas sobre el proyecto, consulta la documentación de Swagger UI en http://localhost:5158 cuando la aplicación esté ejecutándose.

---

**Desarrollado con ❤️ usando .NET 8 y Clean Architecture**
