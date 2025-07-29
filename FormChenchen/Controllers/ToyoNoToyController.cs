using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using FormChenchen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.ComponentModel.DataAnnotations;
using System.Globalization; 
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace elchenchenvuelvecy.Controllers
{
    public class ToyoNoToyController : Controller
    {
        private readonly ToyoNoToyContext _context;
        private readonly ILogger<ToyoNoToyController> _logger;

        public ToyoNoToyController(ToyoNoToyContext context, ILogger<ToyoNoToyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult EnviarSolicitud()
        {
            _logger.LogInformation("EnviarSolicitud action called");
            _logger.LogInformation("TempData Codigo: {Codigo}", TempData["CodigoSolicitud"]);
            _logger.LogInformation("TempData Phone: {Phone}", TempData["NumeroWhatsapp"]);

            var codigoSolicitud = TempData["CodigoSolicitud"]?.ToString();
            var email = TempData["Email"]?.ToString();
            


            /*    if (!string.IsNullOrEmpty(codigoSolicitud) && !string.IsNullOrEmpty(email))
                {
                    SendConfirmationEmail(email, codigoSolicitud);
                }
            */
            ViewBag.Codigo = codigoSolicitud;
            ViewBag.phone = TempData["NumeroWhatsapp"];

            return View();
        }

        // GET: Formularios/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Create GET action called");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FormularioClass Formulario, string TipoFormulario, decimal MonthlySales)
        {

            string Cliente = Formulario.Contact.FullName;
            //string monto = Formulario.RequestDetail.QuantityToInvert.ToString();

            _logger.LogInformation("Create POST action called");
            //_logger.LogInformation("Formulario data received - Contact: {@MonthlySales}", MonthlySales);
            //_logger.LogInformation("validando DAtos para chen chen: {@monto}", monto);

                
            var existeCedula = _context.RequestInfos
                .Any(r => r.NumeroIdentificacion == Formulario.Contact.IdentificationNumber && r.TipoIdentificacion == Formulario.Contact.IdentificationType);

            var existeEmail = _context.RequestInfos
                .Any(r => r.NumeroIdentificacion == Formulario.Contact.Email && r.TipoIdentificacion == Formulario.Contact.Email);

            if (existeCedula)
            {
                TempData["CedulaDuplicada"] = true;
                TempData["MensajeError"] = "Ya existe un contacto con esta Identificación";
                ModelState.AddModelError("Contact.IdentificationNumber", "Identificación ya Existe");
                return View(Formulario);
            }
            else if (existeEmail) {
                TempData["CorreoDuplicado"] = true;
                TempData["MensajeError"] = "Ya existe un contacto con este correo electronico";
                ModelState.AddModelError("Contact.IdentificationNumber", "Email ya Existe");
                return View(Formulario);

            }

            try
            {
                _logger.LogInformation("********************************* Insertando datos **************************************");

                Formulario.Contact.Id = Guid.NewGuid();
                Formulario.Enterprise.Id = Guid.NewGuid();
                Formulario.RequestDetail.Id = Guid.NewGuid();

                //******** SE LE ESTA ASIGNANDO VALORES AL A TABLA REQUESTS ********//
                var newRequest = new Request
                {
                    Id = Guid.NewGuid(),
                    Code = $"{DateTime.Now:yyyyMMdd}-SCC{new Random().Next(10000000, 999999999)}",
                    CreationDate = DateTime.Now,
                    Suggestion = "",
                    Type = 0,
                };


                if (Formulario.RequestDetail.QuantityToInvert > 25000)
                {
                    newRequest.Type = 2;
                    newRequest.Suggestion = "Gestión Caja de Ahorros";
                }
                else
                {
                    newRequest.Type = 1;
                    newRequest.Suggestion = "Gestión directa de Ampyme";
                }


                _logger.LogInformation("New request created with Id: {Id}", newRequest.Id);
                _logger.LogInformation("New request created with Code: {Code}", newRequest.Code);
                _logger.LogInformation("New request created with CreationDate: {CreationDate}", newRequest.CreationDate);
                _logger.LogInformation("New request created with Suggestion: {Suggestion}", newRequest.Suggestion);
                _logger.LogInformation("New request created with Type: {Type}", newRequest.Type);


                if (string.IsNullOrEmpty(Formulario.Enterprise.Instagram))
                {
                    Formulario.Enterprise.Instagram = "N/T";
                }

                if (string.IsNullOrEmpty(Formulario.Enterprise.WebSite))
                {
                    Formulario.Enterprise.Instagram = "N/T";
                }
                //*****************************************************************//
                //-----------------------------------------------------------------//


                //SE INSERTAN LOS DATOS EN LA TABLA REQUEST_INFO PARA EL VISOR
                var NewRequestInfo = new RequestInfo
                {
                    CodId = Guid.NewGuid(),

                    CodigoDeSolicitud = newRequest.Code,

                    FechaDeCreacion = newRequest.CreationDate,

                    FechaActualizacion = newRequest.CreationDate,

                    Gestor = newRequest.Suggestion,

                    EtapaDelNegocio = TipoFormulario,

                    CorreoElectronico = Formulario.Contact.Email,

                    Nombre = Formulario.Contact.Nombre,

                    Apellido = Formulario.Contact.Apellido,

                    NumeroIdentificacion = Formulario.Contact.IdentificationNumber,

                    TipoIdentificacion = Formulario.Contact.IdentificationType,

                    Telefono = Formulario.Contact.Phone,

                    NombreNegocio = Formulario.Enterprise.BusinessName,

                    DescripcionNegocio = Formulario.Enterprise.BusinessDescription,

                    ActividadEconomica = Formulario.Enterprise.EconomicActivity,

                    Instagram = Formulario.Enterprise.Instagram,

                    Ruc = Formulario.Enterprise.Ruc,

                    WebSite = Formulario.Enterprise.WebSite,

                    Provincia = Formulario.Enterprise.Province,

                    Distrito = Formulario.Enterprise.District,

                    Corregimiento = Formulario.Enterprise.Corregimiento,

                    //ProyeccionVentasMensuales = Convert.ToInt32(Formulario.Enterprise.ProyectedSales).ToString(),
                    ProyeccionVentasMensuales = Formulario.Enterprise.ProyectedSales.ToString(),

                    //VentasMensuales = Convert.ToInt32(Formulario.Enterprise.MonthlySales).ToString(),
                    VentasMensuales = Formulario.Enterprise.MonthlySales.ToString(),

                    FechaInicioOperaciones = Convert.ToDateTime(Formulario.Enterprise.OperationsStartDate).ToString(),

                    //CuantoChenchenNecesitas = Convert.ToInt32(Formulario.RequestDetail.QuantityToInvert).ToString(),
                    CuantoChenchenNecesitas = Formulario.RequestDetail.QuantityToInvert.ToString(),

                    EnQueLoInvertiras = Formulario.RequestDetail.ReasonForMoney,

                    VerificacionCliente = Formulario.RequestDetail.VerifyClient,

                    GestionRealizada = "Por contactar",

                    Etapa = "Por Asignar",

                    UsuarioAsignado = "chenchen",

                    IdChen = newRequest.Id,

                    TipoRequest = newRequest.Type

                };


                //ESTO ES PARA INSERTAR LA DATA 
                _logger.LogInformation("Insertando Data para el Numero de solicitud");
                _context.Requests.Add(newRequest);
                _context.SaveChanges();

                _logger.LogInformation("Insertando los datos de las solicitud ");
                Formulario.Contact.RequestId = newRequest.Id;
                Formulario.Enterprise.RequestId = newRequest.Id;
                Formulario.RequestDetail.RequestId = newRequest.Id;

                _context.Contacts.Add(Formulario.Contact);
                _context.Enterprises.Add(Formulario.Enterprise);
                _context.RequestDetails.Add(Formulario.RequestDetail);
                _context.RequestInfos.Add(NewRequestInfo);

                //REALIZA EL COMMIT DE LA DATA
                _context.SaveChanges();
                _logger.LogInformation("Insert finalizada");

                TempData["CodigoSolicitud"] = newRequest.Code;
                TempData["NumeroWhatsapp"] = "+507" + Formulario.Contact.Phone;
                TempData["Email"] = Formulario.Contact.Email;

                return RedirectToAction("EnviarSolicitud", "ToyoNoToy");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR al Crear el Formulario");
                return View(Formulario);
            }
     

        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult SendConfirmationEmail(string email, string codigoSolicitud)
        {
            _logger.LogInformation("SendConfirmationEmail called for email: {Email}", email);
            try
            {
                var fromAddress = new MailAddress("elchenchenvuelve@outlook.com", "ElchenChenVuelve");
                var toAddress = new MailAddress(email);
                const string fromPassword = "Elchenchen507.";
                const string subject = "Código de Solicitud - ElchenChenVuelve";
                string body = $"Su código de solicitud es: {codigoSolicitud}\n\n" +
                             "Gracias por su interés en ElchenChenVuelve.\n" +
                             "Este código es necesario para dar seguimiento a su solicitud.";

                _logger.LogInformation("Attempting to send email to: {Email}", email);

                var smtp = new SmtpClient
                {
                    Host = "smtp-mail.outlook.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                _logger.LogInformation("Email sent successfully to: {Email}", email);
                return Json(new { success = true, message = "Correo enviado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to: {Email}", email);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
