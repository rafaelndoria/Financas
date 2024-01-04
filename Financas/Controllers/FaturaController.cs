using Financas.Data;
using Financas.Helpers;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaturaController : ControllerBase
    {
        private readonly IFaturaRepository _faturaRepository;
        private readonly IPagamentoFaturaRepository _pagamentoFaturaRepository;
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IDbConnectionProvider _connectionProvider;

        public FaturaController(IFaturaRepository faturaRepository, IPagamentoFaturaRepository pagamentoFaturaRepository, ICartaoRepository cartaoRepository, IDbConnectionProvider connectionProvider)
        {
            _faturaRepository = faturaRepository;
            _pagamentoFaturaRepository = pagamentoFaturaRepository;
            _cartaoRepository = cartaoRepository;
            _connectionProvider = connectionProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var faturas = _faturaRepository.Get();
            return Ok(faturas);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var fatura = _faturaRepository.GetById(id);
            return Ok(fatura);
        }

        [HttpGet("{mes:int}/{ano:int}")]
        [Authorize]
        public IActionResult GetByMesAno(int mes, int ano)
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);

            var fatura = _faturaRepository.GetByMesAno(mes, ano, usuarioId);
            return Ok(fatura);
        }

        [HttpGet("FaturasUsuarioLogado")]
        [Authorize]
        public IActionResult GetFaturasUsuarioLogado()
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);

            var faturas = _faturaRepository.GetFaturaUsuarioLogado(usuarioId);
            return Ok(faturas);
        }

        [HttpPut("{id:int}")]
        public IActionResult PagarFatura(int id, PagamentoFaturaViewModel model)
        {
            using(var transaction = _connectionProvider.BeginTransaction())
            {
                _connectionProvider.CurrentTransaction = transaction;

                try
                {
                    var fatura = _faturaRepository.GetById(id);
                    model.FaturaId = fatura.FaturaId;

                    if ((fatura.Valor - model.ValorPago) < 0)
                        throw new Exception("Não pode pagar mais que o valor da fatura");

                    fatura.RemoverValorFatura(model.ValorPago);

                    if (!_pagamentoFaturaRepository.Create(model))
                        throw new Exception("Não foi possivel realizar o pagamento da fatura");

                    if (!_faturaRepository.Update(fatura.Valor, fatura.FaturaId))
                        throw new Exception("Não foi possivel atualizar o valor da fatura");

                    var cartao = _cartaoRepository.GetById(fatura.CartaoId);
                    cartao.AdicionarLimite(model.ValorPago);
                    var cartaoAtualizar = new CartaoViewModel() { LimiteCreditoAtual = cartao.LimiteCreditoAtual };

                    if (!_cartaoRepository.Update(cartao.CartaoId, cartaoAtualizar))
                        throw new Exception("Não foi possivel atualizar o limite do cartão");

                    transaction.Commit();

                    return Ok("Fatura paga com sucesso");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            
        }
    }
}
