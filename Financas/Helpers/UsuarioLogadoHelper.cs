using Financas.Extensions;
using System.Security.Claims;

namespace Financas.Helpers
{
    public static class UsuarioLogadoHelper
    {
        public static int ObterUsuarioId(HttpContext httpContext)
        {
            ClaimsPrincipal usuarioLogado = httpContext.User;
            return usuarioLogado.Id();
        }
    }
}
