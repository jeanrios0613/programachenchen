using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace managerelchenchenvuelve.Filters
{
    public class AuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Verificar si la acción tiene el atributo [AllowAnonymous]
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            
            // Si la acción permite acceso anónimo, no hacer nada
            if (allowAnonymous)
            {
                return;
            }

            // Si el usuario no está autenticado, redirigir al login
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
} 