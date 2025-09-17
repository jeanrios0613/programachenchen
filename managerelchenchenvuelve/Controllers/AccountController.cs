using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging; 
using managerelchenchenvuelve.Services;
using managerelchenchenvuelve.Recursos;
using System.Data;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using managerelchenchenvuelve.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;

namespace managerelchenchenvuelve.Controllers
{
    public class AccountController : Controller
    {
        private readonly ToyoNoToyContext _context;
        private readonly DatabaseConnection _db;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ToyoNoToyContext context, DatabaseConnection db, ILogger<AccountController> logger)
        {
            _context = context;
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Process");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["Mensaje"] = "Usuario y contraseña son requeridos.";
                return View();
            }

            List<VwUserRolesInfo> users = new List<VwUserRolesInfo>();


            string encryptedPassword = EncryptPass.Encriptar(password);

            string query =  " SELECT Id, DateUpdate, IndUpdate," +
                            "  UR.Username AS userss  , Nombre, RolName , status, UserCompania, Gestion, Email " +
                            " FROM   vw_UserRolesInfo  as UR " +
                            " WHERE [UserName] = @username   AND [PasswordHash] = @PasswordHash";

            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@username", username),
                new SqlParameter("@PasswordHash", encryptedPassword)
            };

            DataTable result;
            result = _db.ExecuteQuery(query, parameters);
            _logger.LogInformation("*********** mensaje de password: {encryptedPassword}", encryptedPassword);

            try
            {   
                

                
                foreach (DataRow row in result.Rows) 
                {
                    string? StatusUser = row["status"].ToString();

                    users.Add(new VwUserRolesInfo
                    {    id           = row["id"].ToString(),
                         Nombre       = row["Nombre"].ToString(),  
						 RolName      = row["RolName"].ToString(), 
                         Username     = row["userss"].ToString(),
                         Email        = row["Email"].ToString(),
                         Status       = Convert.ToBoolean(row["status"].ToString()),  
                         DateUpdate   = string.IsNullOrWhiteSpace(row["DateUpdate"]?.ToString())  ? DateTime.Now: Convert.ToDateTime(row["DateUpdate"]),
                         IndUpdate    = Convert.ToBoolean(row["IndUpdate"].ToString()),
                         UserCompania = row["UserCompania"].ToString(),
                         Gestion      = Convert.ToInt16(row["Gestion"].ToString()),

                    });
                }

                int cantidad = users.Count;

                _logger.LogInformation("*********** mensaje de cantidad : {cantidad}", cantidad);
                   
                if (users.Count > 0)
                {
                    HttpContext.Session.SetString("Userss", users[0].Username);
                    HttpContext.Session.SetString("Nombre", users[0].Nombre);
					HttpContext.Session.SetString("Roles" , users[0].RolName);
					HttpContext.Session.SetString("Email" , users[0].Email);
					HttpContext.Session.SetString("CompaniaUser" , users[0].UserCompania.ToString());
					HttpContext.Session.SetString("Gestion", users[0].Gestion.ToString());
                    HttpContext.Session.SetString("IdUser", users[0].id);

				}

                _logger.LogInformation("Resultado de login para {Username}: {Rows} filas", username, result.Rows.Count);

                if (users.Count == 0)
                {
                    ViewData["Mensaje"] = "Usuario o contraseña incorrecta.";
                    return View();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando usuario en base de datos.");
                ViewData["Mensaje"] = "Error al conectarse con la base de datos.";
                return View();
            }

            if (users[0].Status == false )
            {
                ViewData["Mensaje"] = "Usuario Deshabilitado.";
                return View();

            }

            if (result.Rows.Count == 0)
            {
                ViewData["Mensaje"] = "Usuario o contraseña incorrecta.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, users[0].Nombre),
                new Claim(ClaimTypes.Role, users[0].RolName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            try
            {
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                _logger.LogInformation("Usuario {Username} autenticado exitosamente.", username);

                HttpContext.Session.SetString("UserName", username);
               
                // Redirección segura
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (users != null && users.Count > 0)
                { 
                    if (users[0].DateUpdate == DateTime.Now || users[0].IndUpdate == true) {
                    return RedirectToAction("ChangePassword", "Account", new {id = users[0].id });
                    }
                }

                return RedirectToAction("Index", "Process");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el inicio de sesión del usuario: {Username}", username);
                ViewData["Mensaje"] = "Ocurrió un error durante el inicio de sesión.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }


        [HttpGet] 
        public IActionResult ChangePassword(string id)
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("No se encontró usuario en la sesión");
                return RedirectToAction("Login", "Account");
            }


            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();


            var roles = _context.Roles.ToList();
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id);

            ViewBag.Roles = roles;
            ViewBag.CurrentRoleId = userRole?.RoleId;
            TempData["UserPass"] = user.PasswordHash; 
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string NewPassword, string ConfirmPassword)
        {
            try
            {
                // Recupera la contraseña antigua de TempData de forma segura
                string? passAntigua = TempData["UserPass"] as string;

                // Validaciones de campos
                if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    ViewData["Mensaje"] = "Todos los campos son requeridos.";
                    return View();
                }

                if (NewPassword != ConfirmPassword)
                {
                    ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                    return View();
                }

                _logger.LogInformation("Contraseña antigua: {PassAntigua}", passAntigua);

                // Obtiene el nombre de usuario de la sesión
                string? username = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(username))
                {
                    ViewData["Mensaje"] = "Sesión expirada. Por favor, inicie sesión nuevamente.";
                    return RedirectToAction("Login", "Account");
                }

                // Encripta la nueva contraseña
                string encryptedPassword = EncryptPass.Encriptar(NewPassword);
                _logger.LogInformation("Contraseña nueva encriptada: {EncryptedPassword}", encryptedPassword);

                // Validación para evitar repetir la contraseña anterior
                if (passAntigua == encryptedPassword)
                {
                    ViewData["Mensaje"] = "La nueva contraseña no puede ser igual a la anterior.";
                    return View();
                }

                // Construye la consulta segura con parámetros
                string query = @"UPDATE Users 
                         SET PasswordHash = @NewPasswordHash,
                             IndUpdate = @ValorActualiza,
                             DateUpdate = @DateUpdate
                         WHERE UserName = @Username";

                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@NewPasswordHash", encryptedPassword),
                new SqlParameter("@ValorActualiza", false),
                new SqlParameter("@DateUpdate", DateTime.UtcNow.AddMonths(3)),
                new SqlParameter("@Username", username)
                };

                // Ejecuta la actualización
                _db.ExecuteNonQuery(query, parameters);

                ViewData["Mensaje"] = "Contraseña actualizada exitosamente.";
                _logger.LogInformation("Contraseña actualizada para el usuario: {Username}", username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar la contraseña");
                ViewData["Mensaje"] = "Error al cambiar la contraseña. Por favor, intente nuevamente.";
            }

            // Redirige al Index del proceso
            return RedirectToAction("Index", "Process");
        }



    }
}
