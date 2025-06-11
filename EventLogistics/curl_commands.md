# Comandos cURL para EventLogistics API

## Base URL
BASE_URL="http://localhost:5158/api"

## 1. CREAR RECURSOS

### Proyector HD
curl -X POST "${BASE_URL}/Resource" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Proyector HD",
    "type": "Audiovisual",
    "capacity": 5,
    "availability": true,
    "fechaInicio": "2025-06-11T00:00:00Z",
    "fechaFin": "2025-12-31T23:59:59Z",
    "tags": ["audiovisual", "proyector", "hd"]
  }'

### Mesa de Conferencias
curl -X POST "${BASE_URL}/Resource" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Mesa de Conferencias",
    "type": "Mobiliario",
    "capacity": 10,
    "availability": true,
    "fechaInicio": "2025-06-11T00:00:00Z",
    "fechaFin": "2025-12-31T23:59:59Z",
    "tags": ["mobiliario", "mesa", "conferencia"]
  }'

### Sillas Ejecutivas
curl -X POST "${BASE_URL}/Resource" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Sillas Ejecutivas",
    "type": "Mobiliario",
    "capacity": 50,
    "availability": true,
    "fechaInicio": "2025-06-11T00:00:00Z",
    "fechaFin": "2025-12-31T23:59:59Z",
    "tags": ["mobiliario", "silla", "ejecutiva"]
  }'

### Micrófono Inalámbrico
curl -X POST "${BASE_URL}/Resource" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Microfono Inalambrico",
    "type": "Audiovisual",
    "capacity": 8,
    "availability": true,
    "fechaInicio": "2025-06-11T00:00:00Z",
    "fechaFin": "2025-12-31T23:59:59Z",
    "tags": ["audiovisual", "microfono", "inalambrico"]
  }'

### Laptop Dell
curl -X POST "${BASE_URL}/Resource" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop Dell",
    "type": "Tecnologia",
    "capacity": 3,
    "availability": true,
    "fechaInicio": "2025-06-11T00:00:00Z",
    "fechaFin": "2025-12-31T23:59:59Z",
    "tags": ["tecnologia", "laptop", "dell"]
  }'

### Servicio de Catering
curl -X POST "${BASE_URL}/Resource" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Servicio de Catering",
    "type": "Alimentacion",
    "capacity": 2,
    "availability": true,
    "fechaInicio": "2025-06-11T00:00:00Z",
    "fechaFin": "2025-12-31T23:59:59Z",
    "tags": ["alimentacion", "catering", "evento"]
  }'

## 2. CONSULTAR DATOS

### Ver todos los recursos
curl -X GET "${BASE_URL}/Resource" \
  -H "accept: application/json"

### Ver recursos disponibles
curl -X GET "${BASE_URL}/Resource/available" \
  -H "accept: application/json"

### Ver un recurso específico (reemplazar {id} con el ID real)
curl -X GET "${BASE_URL}/Resource/{id}" \
  -H "accept: application/json"

### Ver usuarios existentes
curl -X GET "${BASE_URL}/User" \
  -H "accept: application/json"

### Ver locaciones existentes
curl -X GET "${BASE_URL}/Location" \
  -H "accept: application/json"

### Ver eventos
curl -X GET "${BASE_URL}/Event" \
  -H "accept: application/json"

## 3. GENERAR REPORTES

### Reporte de recursos audiovisuales
curl -X GET "${BASE_URL}/Report?resourceType=Audiovisual" \
  -H "accept: application/json"

### Reporte de recursos de mobiliario
curl -X GET "${BASE_URL}/Report?resourceType=Mobiliario" \
  -H "accept: application/json"

### Recursos críticos
curl -X GET "${BASE_URL}/Report/critical" \
  -H "accept: application/json"

### Exportar reporte en PDF
curl -X GET "${BASE_URL}/Report/export/pdf" \
  -H "accept: application/pdf" \
  -o "reporte_recursos.pdf"

### Exportar reporte en Excel
curl -X GET "${BASE_URL}/Report/export/excel" \
  -H "accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  -o "reporte_recursos.xlsx"

## 4. ACTUALIZAR RECURSOS

### Actualizar estado de un recurso (reemplazar {id} con el ID real)
curl -X PUT "${BASE_URL}/Resource/{id}/status" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "no disponible"
  }'

### Asignar recurso a evento (reemplazar {resourceId} y {eventId} con IDs reales)
curl -X POST "${BASE_URL}/Resource/{resourceId}/assign" \
  -H "Content-Type: application/json" \
  -d '{
    "eventId": "{eventId}"
  }'

## 5. ELIMINAR RECURSOS

### Eliminar un recurso (reemplazar {id} con el ID real)
curl -X DELETE "${BASE_URL}/Resource/{id}" \
  -H "accept: application/json"

## NOTAS:
# - Reemplaza {id}, {resourceId}, {eventId} con IDs reales obtenidos de las respuestas
# - La aplicación debe estar ejecutándose en http://localhost:5158
# - Todos los comandos asumen que el servidor está disponible
