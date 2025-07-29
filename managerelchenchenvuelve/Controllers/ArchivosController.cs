using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using managerelchenchenvuelve.Models;
using managerelchenchenvuelve.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Diagnostics;
using managerelchenchenvuelve.Controllers;



public class ArchivosController : Controller
{ 
    
    private readonly ToyoNoToyContext   _context;
    private readonly DatabaseConnection _db;
    private readonly string rutaServidor = @"..\Reportes\";
    private readonly ILogger<ArchivosController> _logger;


    public ArchivosController(ToyoNoToyContext context, DatabaseConnection db,ILogger<ArchivosController> logger)
    {
        _context = context;
        _logger = logger;
        _db      = db;
    }
 

    // GET: Archivos/SubirArchivo
    public IActionResult SubirArchivo(string? ProcessId)
    {
        var username = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(username))
        {
            _logger.LogWarning("No se encontró usuario en la sesión");
            return RedirectToAction("Login", "Account");
        }

        _logger.LogInformation("Buscando archivos para ProcessId: {ProcessId}", ProcessId);
        var archivos = _context.DocumentReferences.Where(a => a.ProcessInstanceId == ProcessId).ToList();
        _logger.LogInformation("Archivos encontrados: {Count}", archivos.Count);

        ViewBag.ProcessId = ProcessId;
        ViewBag.DataComenta = GetCommentsByProcessId(ProcessId);
        return View(archivos); 
    }


    // POST: Archivos/SubirArchivo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubirArchivo(IFormFile archivo, string ProcessId, string descripcion)
    {
        if (archivo != null && archivo.Length > 0 && archivo.Length <= 5 * 1024 * 1024)
        {
            // Crear la ruta de la carpeta usando el ProcessId
            string carpetaProcess = Path.Combine(rutaServidor, ProcessId);
            _logger.LogInformation($"Carpeta: {carpetaProcess}");
            // Crear la carpeta si no existe
            if (!Directory.Exists(carpetaProcess))
            {
                Directory.CreateDirectory(carpetaProcess);
            }

            // Guardar el archivo en la carpeta del ProcessId
            string rutaDocumento = Path.Combine(carpetaProcess, archivo.FileName);

            using (var stream = new FileStream(rutaDocumento, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var nuevoArchivo = new DocumentReference
            {
                Id = Guid.NewGuid(),
                ProcessInstanceId = ProcessId,
                DocumentTitle = archivo.FileName,
                StageName = rutaDocumento,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = HttpContext.Session.GetString("UserName")
            };

            _context.DocumentReferences.Add(nuevoArchivo);
            await _context.SaveChangesAsync();

            // Redirigir al usuario para que vea los archivos subidos
            return RedirectToAction(nameof(SubirArchivo), new { ProcessId = ProcessId });
        }

        ModelState.AddModelError("", "El archivo debe ser menor a 5 MB.");
        return View();
    }


     

    public List<Comment> GetCommentsByProcessId(string? ProcessId)
    {
        List<Comment> CommentList = new List<Comment>();

        if (string.IsNullOrEmpty(ProcessId))
        {
            return new List<Comment>();
        }
        _logger.LogInformation("Buscando Comentarios para ProcessId: {ProcessId}", ProcessId);
        CommentList = _context.Comments
            .Where(a => a.ProcessInstanceId == ProcessId)
            .Select(c => new Comment
            {
                Id = c.Id,
                ProcessInstanceId = c.ProcessInstanceId,
                Message = c.Message,
                CreatedBy = c.CreatedBy,
                CreatedAt = c.CreatedAt,
                StageName = c.StageName
            })
            .ToList();
        _logger.LogInformation("Archivos Comentarios encontrados: {Count}", CommentList.Count);

        return CommentList;
    }
     
}
