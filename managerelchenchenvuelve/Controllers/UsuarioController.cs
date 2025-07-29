using System.Diagnostics;
using managerelchenchenvuelve.Models;
using managerelchenchenvuelve.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using managerelchenchenvuelve.Recursos;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Office2010.Excel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace managerelchenchenvuelve.Controllers
{   
    public class UsuarioController : Controller
    {
        private readonly ToyoNoToyContext _context;
        private readonly DatabaseConnection _db; 
        private readonly ILogger<UsuarioController> _logger;


        public UsuarioController(ToyoNoToyContext context, DatabaseConnection db, ILogger<UsuarioController> logger)
        {
            _context = context;
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(string? search = null)
        {
            try
            {
                var username = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(username))
                {
                    _logger.LogWarning("No se encontró usuario en la sesión");
                    return RedirectToAction("Login", "Account");
                }
                if (string.IsNullOrEmpty(search))
                {
                    var usuarios = _context.Users
                                  .OrderBy(u => u.UserName)
                                  .ToList();
                                  ;

                    _logger.LogInformation("Usuarios encontrados: {Count}", usuarios.Count);
                    return View(usuarios);
                }
                else
                {
                    var usuarios = _context.Users
                    .Where(u => u.UserName.Contains(search) || u.Names.Contains(search) || u.Lastname.Contains(search))
                    .OrderBy(u => u.UserName)
                    .ToList();
                    _logger.LogInformation("Usuarios encontrados: {Count}", usuarios.Count);
                    return View(usuarios);
                }            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al obtener los usuarios");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        // GET: Usuario/Create
        public IActionResult Create()   
        {
            var roles = _context.Roles.ToList();
            var Compania = _context.Companies.ToList();

            ViewBag.Compania = Compania;
            ViewBag.Roles = roles;
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Useres user, string? RoleId)
        {
            _logger.LogInformation("Iniciando creación de usuario: {RoleId}",  RoleId);      
            _logger.LogInformation("Datos recibidos - UserName: {UserName}, Email: {Email}, Names: {Names}, Lastname: {Lastname} , gestion : {gestion}",
                         user.UserName, user.Email, user.Names, user.Lastname, user.Gestion);

            if (user == null)
            {
                _logger.LogWarning("El modelo de usuario recibido es null");
                ModelState.AddModelError("", "No se recibieron datos del formulario");
                return View();
            }


            var emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            var usernameExists = await _context.Users.AnyAsync(u => u.UserName == user.UserName);

            if (emailExists || usernameExists)
            {
                if (emailExists)
                    ModelState.AddModelError("", "Ya existe un usuario con ese correo.");
                   ViewBag.Error = "Ya existe un usuario con ese correo.";
                ViewBag.Mensaje = "1";

                if (usernameExists)
                    ModelState.AddModelError("", "Ya existe un usuario con ese nombre de usuario.");

                ViewBag.Roles = await _context.Roles.ToListAsync();
                ViewBag.Compania = await _context.Companies.ToListAsync();
 
                return View(user);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Id = Guid.NewGuid().ToString();
                    user.Created = DateTime.Now;
                    user.PasswordHash = EncryptPass.Encriptar(user.PasswordHash);
                    user.Status = true;

                    var Roles = new UserRole
                    {   UserId = user.Id,
                        RoleId = RoleId
                    };

                    _context.Users.Add(user);
                    _context.UserRoles.Add(Roles);

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Usuario creado exitosamente: {UserName}", user.UserName);
                    TempData["SuccessMessage"] = "Usuario fue Creado. ";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear usuario: {Message}", ex.Message);
                    ModelState.AddModelError("", $"Error al crear el usuario: {ex.Message}"); 
                    return RedirectToAction("ErrorSis", "Process");
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Error de validación: {ErrorMessage}", error.ErrorMessage);
                    return RedirectToAction("ErrorSis", "Process");
                }
            }

            var roles = _context.Roles.ToList();
            ViewBag.Roles = roles;
            return View(user);
        }

        // GET: Usuario/UpdateUser/{id}
        public IActionResult UpdateUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            var roles = _context.Roles.ToList();
            var compania = _context.Companies.ToList();
            ViewBag.Roles = roles;
            ViewBag.Compania = compania;

            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id); 
            var userCompania = _context.Companies.FirstOrDefault(ur => ur.Codigo == user.Gestion); 

            ViewBag.CurrentRoleId = userRole?.RoleId;
            ViewBag.userCompania = userCompania?.Codigo;
            return View(user);
        }

        // POST: Usuario/UpdateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(Useres user, string RoleId)
        {
            _logger.LogInformation("Actualizar Usuario..... usuario: {Names}, Roles: {RoleId}, actualiza: {IndUpdate}, Compañia: {compañia}", user.Id,RoleId,user.IndUpdate, user.Gestion);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return NotFound();

              var userDb = _context.Users.FirstOrDefault(u => u.Id == user.Id);
               if (userDb == null)
                   return NotFound();

             
                userDb.Email = user.Email;
                userDb.IndUpdate = user.IndUpdate;
                userDb.Gestion = user.Gestion;

                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    userDb.PasswordHash = EncryptPass.Encriptar(user.PasswordHash);
                }
                
                var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                
                if (userRole != null && userRole.RoleId != RoleId)
                {
                    _context.UserRoles.Remove(userRole);
                    _context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = RoleId });
                }
                else if (userRole == null && !string.IsNullOrEmpty(RoleId))
                {
                    _context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = RoleId });
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuario fue correctamente actualizado " + user.Names;
                return RedirectToAction("Index");
             
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            user.Status = false;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Usuario fue correctamente actualizado " + user.Names;
            _logger.LogInformation("Usuario {UserId} deshabilitado exitosamente", id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            user.Status = !user.Status;
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Usuario fue correctamente actualizado " + user.Names;
            _logger.LogInformation("Usuario {UserId} cambió de estado a {Status}", id, user.Status ? "Activo" : "Inactivo");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUser(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Eliminación física:
            _context.Users.Remove(user);
            _context.SaveChanges();

            // Si prefieres borrado lógico, usa:
            // user.Status = false;
            // _context.SaveChanges();

            TempData["SuccessMessage"] = "Usuario eliminado correctamente " + user.Names;
            return RedirectToAction("Index");
        }

    }
}
