# Documentación del Proyecto FormChenchen

## Información General del Proyecto

**FormChenchen** es una aplicación web desarrollada en **ASP.NET Core 8.0** que funciona como un sistema de formularios para solicitudes de financiamiento del programa "El Chen Chen Vuelve". La aplicación permite a emprendedores y empresarios solicitar financiamiento a través de un formulario web interactivo.

## Tecnologías Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework principal
- **C#** - Lenguaje de programación
- **Entity Framework Core 9.0** - ORM para acceso a datos
- **SQL Server** - Base de datos
- **MVC Pattern** - Arquitectura de la aplicación

### Frontend
- **Razor Views** - Motor de vistas
- **Bootstrap 5.3** - Framework CSS
- **jQuery 3.7.1** - Biblioteca JavaScript
- **jQuery Validation** - Validación del lado del cliente
- **Font Awesome 6.0** - Iconografía

### Herramientas de Desarrollo
- **Microsoft Visual Studio Web CodeGeneration** - Generación de código
- **Entity Framework Tools** - Herramientas de migración
- **System.Data.SqlClient** - Cliente de base de datos

## Estructura del Proyecto

### 📁 **Controllers/**
Contiene los controladores que manejan la lógica de negocio y las peticiones HTTP.

#### `ToyoNoToyController.cs`
- **Función Principal**: Controlador principal que maneja el formulario de solicitudes
- **Métodos Importantes**:
  - `Create()` - Muestra el formulario de solicitud
  - `Create(FormularioClass, string, decimal)` - Procesa el envío del formulario
  - `EnviarSolicitud()` - Muestra la página de confirmación
  - `SendConfirmationEmail()` - Envía correo de confirmación

#### `UbicacionesController.cs`
- **Función Principal**: API para obtener datos geográficos
- **Endpoints**:
  - `GET /api/ubicaciones/provincias` - Obtiene lista de provincias
  - `GET /api/ubicaciones/distritos/{provinciaId}` - Obtiene distritos por provincia
  - `GET /api/ubicaciones/corregimientos/{distritoId}` - Obtiene corregimientos por distrito

#### `HomeController.cs`
- **Función Principal**: Controlador para páginas generales (Index, Privacy, Error)

### 📁 **Models/**
Contiene las clases que representan las entidades de la base de datos y modelos de vista.

#### Modelos de Entidad Principal:
- **`ToyoNoToyContext.cs`** - Contexto de Entity Framework que define la base de datos
- **`FormularioClass.cs`** - Modelo de vista que agrupa todos los datos del formulario
- **`Contact.cs`** - Datos de contacto del solicitante
- **`Enterprise.cs`** - Información de la empresa/negocio
- **`Request.cs`** - Solicitud principal
- **`RequestDetail.cs`** - Detalles específicos de la solicitud

#### Modelos de Ubicación:
- **`Provincias.cs`** - Provincias de Panamá
- **`Distritos.cs`** - Distritos por provincia
- **`Corregimientos.cs`** - Corregimientos por distrito

#### Modelos de Sistema:
- **`User.cs`**, **`Role.cs`**, **`UserRole.cs`** - Sistema de usuarios y roles
- **`RequestInfo.cs`** - Vista consolidada de solicitudes
- **`Comment.cs`** - Sistema de comentarios
- **`DocumentReference.cs`** - Referencias de documentos

### 📁 **Views/**
Contiene las vistas Razor que generan el HTML de la aplicación.

#### `ToyoNoToy/`
- **`Create.cshtml`** - Formulario principal de solicitud
- **`EnviarSolicitud.cshtml`** - Página de confirmación de envío

#### `Shared/`
- **`_Layout.cshtml`** - Layout principal de la aplicación
- **`_ValidationScriptsPartial.cshtml`** - Scripts de validación
- **`Error.cshtml`** - Página de error

#### `Home/`
- **`Index.cshtml`** - Página de inicio
- **`Privacy.cshtml`** - Página de privacidad

### 📁 **wwwroot/**
Contiene archivos estáticos (CSS, JavaScript, imágenes).

#### `css/`
- **`site.css`** - Estilos personalizados
- **`style.css`** - Estilos adicionales

#### `js/`
- **`site.js`** - JavaScript personalizado

#### `img/`
- Logos y imágenes del proyecto

#### `lib/`
- Librerías de terceros (Bootstrap, jQuery, etc.)

### 📁 **Properties/**
- **`launchSettings.json`** - Configuración de lanzamiento
- **`PublishProfiles/`** - Perfiles de publicación

## Funcionalidades Principales

### 1. **Sistema de Formularios**
- Formulario dinámico que se adapta según el tipo de negocio (Emprendimiento vs Negocio Existente)
- Validación tanto del lado del cliente como del servidor
- Campos obligatorios y opcionales según el contexto

### 2. **Gestión de Ubicaciones**
- Sistema de provincias, distritos y corregimientos de Panamá
- Carga dinámica de ubicaciones mediante AJAX
- API REST para consulta de datos geográficos

### 3. **Sistema de Solicitudes**
- Generación automática de códigos de solicitud únicos
- Clasificación automática según el monto solicitado:
  - **Tipo 1**: Montos ≤ $25,000 (Gestión directa AMPYME)
  - **Tipo 2**: Montos > $25,000 (Gestión Caja de Ahorros)

### 4. **Validaciones de Negocio**
- Verificación de duplicados por cédula y email
- Validación de formatos específicos (cédula panameña, teléfono, etc.)
- Validación de rangos de fechas y montos

### 5. **Sistema de Notificaciones**
- Envío de correos electrónicos de confirmación
- Notificaciones toast para errores
- Modales de confirmación

## Flujo de la Aplicación

### 1. **Acceso Inicial**
```
Usuario accede → ToyoNoToyController.Create() → Vista Create.cshtml
```

### 2. **Proceso de Solicitud**
```
1. Usuario llena formulario
2. Validación del lado del cliente (jQuery)
3. Envío POST → ToyoNoToyController.Create()
4. Validación del servidor
5. Verificación de duplicados
6. Inserción en base de datos
7. Generación de código de solicitud
8. Redirección a página de confirmación
```

### 3. **Estructura de Datos**
```
Request (Solicitud Principal)
├── Contact (Datos de Contacto)
├── Enterprise (Datos del Negocio)
├── RequestDetail (Detalles de la Solicitud)
└── RequestInfo (Vista Consolidada)
```

## Configuración de Base de Datos

### Cadena de Conexión
```json
"ConnectionStrings": {
    "conexion": "Server=10.1.1.228;Database=ToyNoToy;User Id=managerchenchen;Password=123456ChenChen;TrustServerCertificate=True;"
}
```

### Tablas Principales
- **Requests** - Solicitudes principales
- **Contacts** - Datos de contacto
- **Enterprises** - Información empresarial
- **RequestDetails** - Detalles de solicitud
- **Request_Info** - Vista consolidada
- **Provincias, Distritos, Corregimientos** - Datos geográficos

## Características Técnicas

### Validaciones Implementadas
- **Cédula Panameña**: Formato específico (8-123-456, E-12-345, etc.)
- **Teléfono**: Formato 6XXX-XXXX
- **Email**: Validación estándar de email
- **URLs**: Validación de sitios web e Instagram
- **Montos**: Validación de decimales y rangos

### Seguridad
- **Anti-forgery tokens** en formularios
- **Validación de entrada** en todos los campos
- **Sanitización de datos** antes de inserción
- **Logging** de operaciones importantes

### Internacionalización
- **Localización** configurada para español (es-ES)
- **Formato de números** estadounidense para decimales
- **Mensajes de error** en español

## Puntos de Entrada de la Aplicación

### Ruta Principal
```
{ToyoNoToyController}/{Create}/{id?}
```
La aplicación inicia directamente en el formulario de solicitud.

### API Endpoints
```
GET /api/ubicaciones/provincias
GET /api/ubicaciones/distritos/{provinciaId}
GET /api/ubicaciones/corregimientos/{distritoId}
POST /ToyoNoToy/SendConfirmationEmail
```

## Consideraciones de Desarrollo

### Dependencias Críticas
- **Entity Framework Core** para acceso a datos
- **jQuery** para interactividad del frontend
- **Bootstrap** para diseño responsivo
- **SMTP** para envío de correos

### Configuración de Servidor
- **Puerto**: 5112 (configurado en appsettings.json)
- **Protocolo**: HTTP
- **Base de datos**: SQL Server remoto

### Logging
- Implementado con **ILogger**
- Niveles: Information, Warning, Error
- Logs de operaciones críticas y errores

## Conclusión

FormChenchen es una aplicación web robusta diseñada específicamente para el programa "El Chen Chen Vuelve" de Panamá. Su arquitectura MVC, validaciones exhaustivas y sistema de gestión de ubicaciones la convierten en una herramienta eficiente para la recolección y procesamiento de solicitudes de financiamiento empresarial.

La aplicación está optimizada para el contexto panameño, con validaciones específicas para documentos de identidad, números telefónicos y estructura geográfica del país, asegurando la integridad y relevancia de los datos recopilados.
