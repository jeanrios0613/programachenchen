using System.Drawing.Printing;
using System.Security.Claims;
using BootstrapBlazor.Components;
using managerelchenchenvuelve.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using managerelchenchenvuelve.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http.HttpResults;

namespace managerelchenchenvuelve.Controllers
{
    //[Authorize]
    public class ProcessController : Controller
    {
        private readonly DatabaseConnection _db;
        private readonly ToyoNoToyContext _context;
        private readonly ILogger<ProcessController> _logger;

        public ProcessController(ToyoNoToyContext context, DatabaseConnection db, ILogger<ProcessController> logger)
        {
            _context = context;
            _logger = logger;
            _db = db;
        }

        // GET: ProcessController
        public ActionResult Index(string? id = null, int page = 1, int pageSize = 10, string? tarea = "P", string? search = null, string? business = null,string?  ListUser = null, string? sortOrder= "asc", string? Campo = "TiempoTranscurrido")
        {

            /******   Variable Siempre para colocar en el Inicio de Cada GET  ******/

            var username = HttpContext.Session.GetString("UserName");
            var Userss   = HttpContext.Session.GetString("Userss");
            var Roles    = HttpContext.Session.GetString("Roles");
            var CompaniaUser = HttpContext.Session.GetString("CompaniaUser");
            var Gestion = HttpContext.Session.GetString("Gestion");


            ViewBag.nombreUsuario = username;
            ViewBag.AdminUser = Roles;
            ViewBag.CompaniaUser = CompaniaUser;

            _logger.LogInformation("Accediendo a Process/Index"); 
            _logger.LogInformation("Usuario en sesión: {Username}", username);

            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("No se encontró usuario en la sesión");
                return RedirectToAction("Login", "Account");
            }


            /**********************************************************************/


            /******  QUERY PARA MOSTRAR DATA AL FRONT  ******/
            try
            { 

                List<DatosReca> Datos = new List<DatosReca>();
 
                string query = @"SELECT *
                                 FROM (SELECT  CONCAT( RI.codigo_de_solicitud, '  ', RI.NOMBRE,'  ',RI.APELLIDO,'  ', RI.NUMERO_IDENTIFICACION,'  ',RI.GESTOR) AS CompletaActividad, 
                                       FORMAT(SWITCHOFFSET(RI.Fecha_de_creacion, '-05:00'),'MMMM dd, yyyy hh:mm tt','es-es') AS FechaFormateada,  
                                       FORMAT(SWITCHOFFSET(RI.Fecha_Actualizacion, '-05:00'),'MMMM dd, yyyy hh:mm tt','es-es') AS FechaActualizacion,  
                                       CASE 
                                       WHEN DATEDIFF(MINUTE, RI.Fecha_de_creacion, SYSDATETIMEOFFSET()) < 60 
                                        THEN 'hace ' + CAST(DATEDIFF(MINUTE,RI.Fecha_de_creacion, SYSDATETIMEOFFSET()) AS VARCHAR) + ' minutos'
                                        WHEN DATEDIFF(HOUR, RI.Fecha_de_creacion, SYSDATETIMEOFFSET()) < 24 
                                        THEN 'hace ' + CAST(DATEDIFF(HOUR,RI.Fecha_de_creacion, SYSDATETIMEOFFSET()) AS VARCHAR) + ' horas'
                                        ELSE 
                                            'hace ' + CAST(DATEDIFF(DAY, RI.Fecha_de_creacion, SYSDATETIMEOFFSET()) AS VARCHAR) + ' días'
                                            END AS TiempoTranscurrido,
                                 
                                        TRY_CAST(CAST(DATEDIFF(DAY, RI.Fecha_de_creacion, SYSDATETIMEOFFSET()) AS VARCHAR) as int) AS Tiempo,
 
                                        RI.* 

                                      FROM  [dbo].[Request_info] as RI ) as REQS ";

                //Se utiliza este filtro para poder impletar el buscador 
                if (search != null )
                {
                     query += "WHERE CompletaActividad LIKE '%" + search + "%'";

                }
                else if (search == null)
                {
                       query += " WHERE TipoRequest = @Gestion ";
                         

                    

                  if (tarea != "A") { 

                    if (!string.IsNullOrEmpty(ListUser) )
                    {
                        query += " AND usuario_asignado in  ( "+ListUser+" )";
                    }
                    else
                    { 
                        query += " AND usuario_asignado = COALESCE(@username, usuario_asignado)";
                    }
                  }

                    if (tarea == "C")
                    {
                        query += " AND ETAPA = 'Completada'";

                    }
                    if (tarea == "P")
                    {

                        query += " AND ETAPA != 'Completada'";
                    }

                    if (tarea == "A")
                    {

                        query += " AND Etapa in ('Por Asignar', 'Solicitud de Cambio', 'Re-Abrir Solicitud') ";
                    }


                    if (!string.IsNullOrEmpty(business))
                    {
                        query += " AND Etapa_del_Negocio in (" + business + ")";
                    }
                }

                /////////////////////////////////////********** TOTAL PARA PAGINATION ************************//////////////////////////////////////////////
                SqlParameter[] paramet = new SqlParameter[]
                {
                    new SqlParameter("@username",Userss),
                    new SqlParameter("@Gestion",Gestion)
                };

                DataTable resultado = _db.ExecuteQuery(query, paramet);

                int totCount = resultado.Rows.Count;

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                query += " ORDER BY " + Campo + " " + sortOrder +
                             " OFFSET @Offset ROWS " +
                             " FETCH NEXT @PageSize ROWS ONLY";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@username",Userss),
                    new SqlParameter("@Gestion", Gestion), 
                    new SqlParameter("@Offset", (page - 1) * pageSize),
                    new SqlParameter("@PageSize", pageSize)
                };

                DataTable result = _db.ExecuteQuery(query, parameters);

                

                
                 
                _logger.LogInformation("************************* Cantidad2 : {totCount}", totCount);


                foreach (DataRow row in result.Rows)
                    {
                        Datos.Add(new DatosReca
                        {   Id = row["codigo_de_solicitud"].ToString(),
                            gestor = row["GESTOR"].ToString(),
                            Etapa = row["Etapa"].ToString(),
                            CompletaActividad = row["CompletaActividad"].ToString(),
                            FechaFormateada = row["FechaFormateada"].ToString(),
                            FechaActualizacion = row["FechaActualizacion"].ToString(),
                            TiempoTranscurrido = row["TiempoTranscurrido"].ToString(),
                            UserName = row["usuario_asignado"].ToString(),
                            CodigoDeSolicitud = row["codigo_de_solicitud"].ToString(),
                            Tiempo =Convert.ToInt32(row["Tiempo"].ToString())
                        });
                    }

                 
                    ViewBag.Cantidad     = Math.Min(page * pageSize, totCount);
                    ViewBag.TotalQuery   = totCount;
                    ViewBag.ViewQuery    = page * 10; 
                    ViewBag.TotalPages   = (int)Math.Ceiling((double)totCount / pageSize);
                    ViewBag.CurrentPage  = page;
                    ViewBag.DatosReca    = Datos;
                    ViewBag.Tarea        = tarea;
                    ViewBag.listUsers    = ObtenerUsuariosAmpyme();


                    _logger.LogInformation("Obteniendo lista de formularios");
                    return View(Datos);
                 
                 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en cargar pantalla principal");
                return RedirectToAction("ErrorSis", "Process");
            }
        }

        // GET: ProcessController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProcessController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProcessController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(); 
            }
        }

        // GET: ProcessController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProcessController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProcessController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProcessController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(); 
            }
        }


        public List<AsignacionClass> ObtenerUsuariosAmpyme() {

           var Gestion = HttpContext.Session.GetString("Gestion");

            List<AsignacionClass> Userlist = new List<AsignacionClass>();

            String Datas = @"select upper(SUBSTRING(us.names,1,1)) letters, us.username, (names+' '+Lastname) nombrecompleto 
                            from       [dbo].[Users]     as US
                            inner join [dbo].[UserRoles] as UR
                                    on us.id = UR.Userid
                            inner join [dbo].[Roles]     as RL 
                                    ON rl.id = ur.roleid
                            where  us.Status = 1
                            and    gestion   = @Gestion
                            order by 2"
            ;


            SqlParameter[] parametre = new SqlParameter[]
              {  
                  new SqlParameter("@Gestion",Gestion),
              };

            DataTable listUser = _db.ExecuteQuery(Datas, parametre);

            foreach (DataRow row in listUser.Rows)
            {
                Userlist.Add(new AsignacionClass
                {
                    Usuario = row["username"].ToString(),
                    NombreCompleto = row["nombrecompleto"].ToString(),
                    Letters = row["letters"].ToString()
                });
            }
            
            return Userlist;

        }



        [HttpPost]
        public IActionResult AsignarTareas([FromBody] AsignacionClass model)
        {
            try
            {
                if (model == null || model.Usuario == null || string.IsNullOrEmpty(model.Usuario) || model.Ids == null || !model.Ids.Any())
                {
                    return BadRequest("Datos incompletos");
                }

                foreach (var codigo in model.Ids)
                {
                    string updateQuery = @"UPDATE dbo.Request_Info 
                                          SET usuario_asignado = @usuario,
                                              Etapa = 'En Curso',
                                            Fecha_Actualizacion = @FechaActualizacion
                                          WHERE codigo_de_solicitud = @codigo";

                    _logger.LogInformation("hacer update ************************: {codigo}", codigo);
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@usuario", model.Usuario),
                        new SqlParameter("@FechaActualizacion", DateTime.Now),
                        new SqlParameter("@codigo", codigo)
                    };

                    _db.ExecuteNonQuery(updateQuery, parameters);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error al asignar tareas");
                return RedirectToAction("ErrorSis", "Process");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorSis()
        {
            /******   Variable Siempre para colocar en el Inicio de Cada GET  ******/

            var username = HttpContext.Session.GetString("UserName");
            var Userss = HttpContext.Session.GetString("Userss");
            var Roles = HttpContext.Session.GetString("Roles");

            ViewBag.nombreUsuario = username;
            ViewBag.AdminUser = Roles;
             
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("No se encontró usuario en la sesión");
                return RedirectToAction("Login", "Account");
            }


            /**********************************************************************/
            return View();
        }

       

    }
}
