using Financas.Models;
using Financas.ObjectValues;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegurancaJWT.Services;
using System.Security.Claims;
using Financas.Extensions;

namespace Financas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly JwtTokenService _jwt;

        public UsuarioController(JwtTokenService jwt)
        {
            _jwt = jwt;
        }

        [HttpPost]
        public IActionResult Register(UsuarioCadastroViewModel model)
        {
            var user = new Usuario(model.Nome, new Email(model.Email), new Senha(model.Senha), model.DataNascimento);
            // criar o usuario e depois buscar o id dele, desse formato nao esta pegando o id do usuario criado
            var token = _jwt.Create(user);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Teste()
        {
            return Ok("Voce tem permissao: ");
        }
    }
}
