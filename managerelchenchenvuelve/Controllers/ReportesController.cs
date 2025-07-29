using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using managerelchenchenvuelve.Models;

namespace managerelchenchenvuelve.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ToyoNoToyContext _context;

        public ReportesController(ToyoNoToyContext context)
        {
            _context = context;
        }
        public ActionResult Reporte()
        {
            return View();
        }
        public IActionResult Descargar_Reportes(string? Fecin, string? Fecfin)
        {
            var query = _context.RequestInfos
                     .Select(x => new {
                         x.CodigoDeSolicitud,
                         x.FechaDeCreacion,
                         x.FechaActualizacion,
                         x.Gestor,
                         x.EtapaDelNegocio,
                         x.CorreoElectronico,
                         x.Nombre,
                         x.Apellido,
                         x.NumeroIdentificacion,
                         x.TipoIdentificacion,
                         x.Telefono,
                         x.NombreNegocio,
                         x.DescripcionNegocio,
                         x.ActividadEconomica,
                         x.Instagram,
                         x.Ruc,
                         x.WebSite,
                         x.Provincia,
                         x.Distrito,
                         x.Corregimiento,
                         x.ProyeccionVentasMensuales,
                         x.VentasMensuales,
                         x.FechaInicioOperaciones,
                         x.CuantoChenchenNecesitas,
                         x.EnQueLoInvertiras,
                         x.VerificacionCliente,
                         x.GestionRealizada, 
                         x.PorqueNoContacto,
                         x.Etapa,
                         x.TipoAtencion,
                         x.UsuarioAsignado
                     })
                     .AsQueryable();

            if (!string.IsNullOrEmpty(Fecin))
            {
                if (DateTime.TryParse(Fecin, out DateTime fechaInicio))
                {
                    var inicioDate = new DateTimeOffset(fechaInicio);
                    query = query.Where(f => f.FechaDeCreacion >= inicioDate);
                }
            }

            if (!string.IsNullOrEmpty(Fecfin))
            {
                if (DateTime.TryParse(Fecfin, out DateTime fechaFin))
                {
                    var finDate = new DateTimeOffset(fechaFin);
                    query = query.Where(f => f.FechaDeCreacion <= finDate);
                }
            }

            var formularios = query.ToList();
            
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte");

                // Agregar encabezados
                worksheet.Cell(1, 1).Value = "Codigodesolicitud"; 
                worksheet.Cell(1, 2).Value = "FechaActualizacion";
                worksheet.Cell(1, 3).Value = "Gestor";
                worksheet.Cell(1, 4).Value = "EtapadelNegocio";
                worksheet.Cell(1, 5).Value = "CorreoElectronico";
                worksheet.Cell(1, 6).Value = "Nombre";
                worksheet.Cell(1, 7).Value = "Apellido";
                worksheet.Cell(1, 8).Value = "Numeroidentificacion";
                worksheet.Cell(1, 9).Value = "Tipoidentificacion";
                worksheet.Cell(1, 10).Value = "Telefono";
                worksheet.Cell(1, 11).Value = "NombreNegocio";
                worksheet.Cell(1, 12).Value = "Descripcionnegocio";
                worksheet.Cell(1, 13).Value = "Actividadeconomica";
                worksheet.Cell(1, 14).Value = "Instagram";
                worksheet.Cell(1, 15).Value = "RUC";
                worksheet.Cell(1, 16).Value = "WebSite";
                worksheet.Cell(1, 17).Value = "Provincia";
                worksheet.Cell(1, 18).Value = "Distrito";
                worksheet.Cell(1, 19).Value = "corregimiento";
                worksheet.Cell(1, 20).Value = "Proyeccionventasmensuales";
                worksheet.Cell(1, 21).Value = "Ventasmensuales";
                worksheet.Cell(1, 22).Value = "FechaInicioOperaciones";
                worksheet.Cell(1, 23).Value = "CuantoChenchennecesitas";
                worksheet.Cell(1, 24).Value = "Enqueloinvertiras";
                worksheet.Cell(1, 25).Value = "VerificacionCliente";
                worksheet.Cell(1, 26).Value = "GestionRealizada";
                worksheet.Cell(1, 27).Value = "Tipoatencion";
                worksheet.Cell(1, 28).Value = "Porquenocontacto";
                worksheet.Cell(1, 29).Value = "Etapa";
                worksheet.Cell(1, 30).Value = "UsuarioAsignado"; 

                // Agregar datos
                for (int i = 0; i < formularios.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = formularios[i].CodigoDeSolicitud; 
                    worksheet.Cell(i + 2, 2).Value = formularios[i].FechaActualizacion.ToString();
                    worksheet.Cell(i + 2, 3).Value = formularios[i].Gestor;
                    worksheet.Cell(i + 2, 4).Value = formularios[i].EtapaDelNegocio;
                    worksheet.Cell(i + 2, 5).Value = formularios[i].CorreoElectronico;
                    worksheet.Cell(i + 2, 6).Value = formularios[i].Nombre;
                    worksheet.Cell(i + 2, 7).Value = formularios[i].Apellido;
                    worksheet.Cell(i + 2, 8).Value = formularios[i].NumeroIdentificacion;
                    worksheet.Cell(i + 2, 9).Value = formularios[i].TipoIdentificacion;
                    worksheet.Cell(i + 2, 10).Value = formularios[i].Telefono;
                    worksheet.Cell(i + 2, 11).Value = formularios[i].NombreNegocio;
                    worksheet.Cell(i + 2, 12).Value = formularios[i].DescripcionNegocio;
                    worksheet.Cell(i + 2, 13).Value = formularios[i].ActividadEconomica;
                    worksheet.Cell(i + 2, 14).Value = formularios[i].Instagram;
                    worksheet.Cell(i + 2, 15).Value = formularios[i].Ruc;
                    worksheet.Cell(i + 2, 16).Value = formularios[i].WebSite;
                    worksheet.Cell(i + 2, 17).Value = formularios[i].Provincia;
                    worksheet.Cell(i + 2, 18).Value = formularios[i].Distrito;
                    worksheet.Cell(i + 2, 19).Value = formularios[i].Corregimiento;
                    worksheet.Cell(i + 2, 20).Value = formularios[i].ProyeccionVentasMensuales;
                    worksheet.Cell(i + 2, 21).Value = formularios[i].VentasMensuales;
                    worksheet.Cell(i + 2, 22).Value = formularios[i].FechaInicioOperaciones;
                    worksheet.Cell(i + 2, 23).Value = formularios[i].CuantoChenchenNecesitas;
                    worksheet.Cell(i + 2, 24).Value = formularios[i].EnQueLoInvertiras;
                    worksheet.Cell(i + 2, 25).Value = formularios[i].VerificacionCliente;
                    worksheet.Cell(i + 2, 26).Value = formularios[i].GestionRealizada;
                    worksheet.Cell(i + 2, 27).Value = formularios[i].TipoAtencion;
                    worksheet.Cell(i + 2, 28).Value = formularios[i].PorqueNoContacto;
                    worksheet.Cell(i + 2, 29).Value = formularios[i].Etapa;
                    worksheet.Cell(i + 2, 30).Value = formularios[i].UsuarioAsignado; 
                }

                using (var stream = new MemoryStream())
                {   workbook.SaveAs(stream);
                    stream.Position = 0;  
                    var fileName = "Reporte.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            } 
        }
    }
}
