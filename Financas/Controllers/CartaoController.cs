using Financas.Extensions;
using Financas.Helpers;
using Financas.Models;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartaoController : ControllerBase
    {
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IContaRepository _contaRepository;

        public CartaoController(ICartaoRepository cartaoRepository, IContaRepository contaRepository)
        {
            _cartaoRepository = cartaoRepository;
            _contaRepository = contaRepository;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CartaoViewModel model)
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);

            try
            {
                if (!_contaRepository.VerificarExisteConta(usuarioId, (int)model.ContaId))
                    return Unauthorized("Nao foi possivel criar o cartão");

                if(model.Principal == 1)
                {
                    if (_cartaoRepository.PossuiCartaoPrincipal((int)model.ContaId))
                        return BadRequest("Pode conter apenas um cartão como principal");
                }

                var cartao = new Cartao(model.Nome, model.LimiteCredito, model.Principal, model.DiaVencimento, (int)model.ContaId, model.LimiteCreditoAtual);

                _cartaoRepository.Insert(cartao);

                return Ok("Cartão criada com sucesso");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var cartoes = _cartaoRepository.Get();
            return Ok(cartoes);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var cartao = _cartaoRepository.GetById(id);
            return Ok(cartao);
        }

        [HttpGet("GetCartaoUsuarioLogado")]
        [Authorize]
        public IActionResult GetCartaoUsuarioLogado()
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);
            var cartoes = _cartaoRepository.GetCartaoUsuarioLogado(usuarioId);
            return Ok(cartoes);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!_cartaoRepository.Delete(id))
                    return BadRequest("Nao foi possivel deletar o cartão");

                return Ok("Cartão deletado com sucesso");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public IActionResult Update(CartaoViewModel model, int id)
        {
            try
            {
                if (!_cartaoRepository.Update(id, model))
                    return BadRequest("Não foi possivel atualizar o cartão");
                return Ok("cartãoa atualizado");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
