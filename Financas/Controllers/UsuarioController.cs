using Financas.Models;
using Financas.Repositories.Interfaces;
using Financas.Services;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegurancaJWT.Services;

namespace Financas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly JwtTokenService _jwt;
        private readonly CryptService _crypt;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(JwtTokenService jwt, IUsuarioRepository usuarioRepository, CryptService crypt)
        {
            _jwt = jwt;
            _usuarioRepository = usuarioRepository;
            _crypt = crypt;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UsuarioViewModel model)
        {
            try
            {
                var user = new Usuario(model.Nome, model.Email, model.Senha, DateTime.Parse(model.DataNascimento));
                var usuarioAdicionado = _usuarioRepository.Insert(user);

                if (!usuarioAdicionado)
                    return BadRequest("Nao foi possivel incluir o usuario");

                var userId = _usuarioRepository.GetId(user.Email);
                user.SetId(userId);

                var token = _jwt.Create(user);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                var user = _usuarioRepository.GetByEmail(model.Email);

                if (user == null)
                    return Unauthorized("Email não cadastrado no sistema");

                if (!_crypt.ComparePassword(model.Senha, user.Senha))
                    return Unauthorized("Senha ou email invalidos");

                var token = _jwt.Create(user);

                return Ok("Voce esta logado. Token: " + token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _usuarioRepository.Get();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _usuarioRepository.GetById(id);
            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _usuarioRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UsuarioViewModel model)
        {
            try
            {
                var atualizado = _usuarioRepository.Update(id, model);

                if (!atualizado)
                    return BadRequest("Nao foi possivel atualizar o usuario");

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
