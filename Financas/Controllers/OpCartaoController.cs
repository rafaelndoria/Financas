using Financas.Data;
using Financas.Enums;
using Financas.Helpers;
using Financas.Models;
using Financas.Repositories;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpCartaoController : ControllerBase
    {
        private readonly ICartaoRepository _cartaoRepository;
        private readonly IOpCartaoRepository _opCartaoRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IFaturaRepository _faturaRepository;
        private readonly ITituloRepository _tituloRepository;
        private readonly IDbConnectionProvider _connectionProvider;


        public OpCartaoController(ICartaoRepository cartaoRepository, IOpCartaoRepository opCartaoRepository, IContaRepository contaRepository, IFaturaRepository faturaRepository, IDbConnectionProvider connectionProvider, ITituloRepository tituloRepository)
        {
            _cartaoRepository = cartaoRepository;
            _opCartaoRepository = opCartaoRepository;
            _contaRepository = contaRepository;
            _faturaRepository = faturaRepository;
            _tituloRepository = tituloRepository;
            _connectionProvider = connectionProvider;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(OpCartaoViewModel model)
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);
            Cartao cartao;

            using (var transaction = _connectionProvider.BeginTransaction())
            {
                _connectionProvider.CurrentTransaction = transaction;

                try
                {
                    if (model.CartaoId == 0)
                    {
                        var cartaoId = _cartaoRepository.GetCartaoPrincipal(usuarioId);
                        if (cartaoId == 0)
                            throw new Exception("Nenhum cartao foi encontrado");

                        model.CartaoId = cartaoId;
                    }

                    cartao = _cartaoRepository.GetById((int)model.CartaoId);

                    var opCartao = new OpCartao(model.Descricao, model.Valor, model.DataOp, model.Parcelado, model.Quantidade, cartao.CartaoId, model.CategoriaOpId, model.TipoOpCartaoId);

                    if (!_opCartaoRepository.Create(opCartao))
                        throw new Exception("Nao foi possivel criar a operacao do cartao");

                    if (model.TipoOpCartaoId == (int)ETipoOpCartao.Debito)
                    {
                        DebitarValorEmConta(cartao.ContaId, opCartao.Valor, opCartao.Descricao, opCartao.DataOp);
                    }
                    else if (model.TipoOpCartaoId == (int)ETipoOpCartao.Credito)
                    {
                        CreditarFaturaCartao(cartao.CartaoId, opCartao.Parcelado, opCartao.QuantidadeParcelas, opCartao.Valor, (double)opCartao.ValorPorParcela, opCartao.DataOp, cartao, opCartao.Descricao, opCartao.DataOp);
                    }

                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var opCartaoes = _opCartaoRepository.Get();
            return Ok(opCartaoes);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var opCartao = _opCartaoRepository.GetById(id);
            return Ok(opCartao);
        }

        [HttpGet("GetByUsuarioLogado")]
        [Authorize]
        public IActionResult GetByUsuarioLogado()
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);

            var opCartaoes = _opCartaoRepository.GetOpCartoesUser(usuarioId);
            return Ok(opCartaoes);
        }

        private void DebitarValorEmConta(int contaId, double valor, string descricao, DateTime dataOp)
        {
            var saldoAtual = _contaRepository.GetSaldoConta(contaId);

            if ((saldoAtual - valor) < 0)
                throw new Exception();

            var conta = _contaRepository.GetById(contaId);
            conta.DebitarConta(valor);

            var contaAtualizar = new ContaViewModel();
            contaAtualizar.Balanco = conta.Balanco;

            if (!_contaRepository.Update(contaId, contaAtualizar, 0))
                throw new Exception();

            if (!CriarTitulo(descricao, "Criado atraves da rotina de Debitar Valor em Conta", 1, 1, (decimal)valor, 1, "Criado", dataOp, contaId))
                throw new Exception("Nao foi possivel criar o titulo");
        }

        private void CreditarFaturaCartao(int cartaoId, int parcelado, int quantidade, double valor, double valorPorParcela, DateTime dataOperacao, Cartao cartao, string descricao, DateTime dataOp)
        {
            if(cartao.LimiteCreditoAtual < valor)
                throw new Exception("Nao tem limite suficiente para operacao");

            cartao.RemoverLimite(valor);
            var cartaoAtualizar = new CartaoViewModel() { LimiteCreditoAtual = cartao.LimiteCreditoAtual };
            if (!_cartaoRepository.Update(cartao.CartaoId, cartaoAtualizar))
                throw new Exception("Nao foi possivel atualizar o limite do cartao");

            var dataFatura = DataFaturaHelper.DataFatura(dataOperacao, _cartaoRepository.GetDataVencimento(cartaoId));
            var mes = dataFatura.Month;
            var ano = dataFatura.Year;

            if (parcelado == 0)
            {
                var fatura = _faturaRepository.GetOrInsert(cartaoId, mes, ano);
                fatura.AdicionarFatura(valor);

                if (!_faturaRepository.Update(fatura.Valor, fatura.FaturaId))
                    throw new Exception("Nao foi possivel adicionar na fatura");

                if (!CriarTitulo(descricao, "Titulo criado atraves da rotina de CreditarValorEmConta", 1, 1, (decimal)valor, 1, "Criado", dataOp, cartao.ContaId))
                    throw new Exception("Nao foi possivel criar o titulo");
            }
            else
            {
                for(var i = 1; i <= quantidade; i++)
                {
                    var fatura = _faturaRepository.GetOrInsert(cartaoId, mes, ano);
                    fatura.AdicionarFatura(valorPorParcela);

                    if (!_faturaRepository.Update(fatura.Valor, fatura.FaturaId))
                        throw new Exception("Nao foi possivel adicionar na fatura");

                    if (!CriarTitulo(descricao, "Título criado com as parcelas automaticamente atraves da rotina de Creditar Valor em Conta", i, quantidade, (decimal)valorPorParcela, 1, "Criado", dataOp, cartao.ContaId))
                        throw new Exception("Nao foi possivel criar o titulo");

                    if (mes == 12)
                    {
                        mes = 1;
                        ano += 1;
                    }
                    else
                    {
                        mes++;
                    }
                }
            }
        }

        public bool CriarTitulo(string descricao, string observacao, int parcela, int numeroParcelas, decimal valor, int tipoTitulo, string statusTitulo, DateTime dataTitulo, int contaId)
        {
            var titulo = new TituloViewModel()
            {
                Descricao = descricao,
                Observacao = observacao,
                Parcela = parcela,
                NumeroParcelas = numeroParcelas,
                Valor = valor,
                TipoTitulo = tipoTitulo,
                StatusTitulo = statusTitulo,
                DataTitulo = dataTitulo,
                ContaId = contaId
            };
            try
            {
                _tituloRepository.Create(titulo);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
