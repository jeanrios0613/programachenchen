using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FormChenchen.Models;

public partial class RequestInfo
{
    public string? CodigoDeSolicitud { get; set; }

    public DateTimeOffset? FechaDeCreacion { get; set; }

    public DateTimeOffset? FechaActualizacion { get; set; }

    public string? Gestor { get; set; }

    public string? EtapaDelNegocio { get; set; }

    public string? CorreoElectronico { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? NumeroIdentificacion { get; set; }

    public string? TipoIdentificacion { get; set; }

    public string? Telefono { get; set; }

    public string? NombreNegocio { get; set; }

    public string? DescripcionNegocio { get; set; }

    public string? ActividadEconomica { get; set; }

    public string? Instagram { get; set; }

    public string? Ruc { get; set; }

    public string? WebSite { get; set; }

    public string? Provincia { get; set; }

    public string? Distrito { get; set; }

    public string? Corregimiento { get; set; }

    public string? ProyeccionVentasMensuales { get; set; }

    public string? VentasMensuales { get; set; }

    public string? FechaInicioOperaciones { get; set; }

    public string? CuantoChenchenNecesitas { get; set; }

    public string? EnQueLoInvertiras { get; set; }

    public string? VerificacionCliente { get; set; }

    public string? GestionRealizada { get; set; }

    public string? TipoAtencion { get; set; }

    public string? PorqueNoContacto { get; set; }

    public string? Etapa { get; set; }

    public string? UsuarioAsignado { get; set; }

    public int TipoRequest { get; set; }

    [Key]
    public Guid CodId { get; set; }

    public Guid? IdChen { get; set; }
}
