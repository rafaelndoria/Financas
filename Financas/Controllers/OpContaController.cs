using Financas.Data;
using Financas.Enums;
using Financas.Helpers;
using Financas.Models;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpContaController : ControllerBase
    {
        private readonly IOpContaRepository _opContaRepository;
        private readonly IContaRepository _contaRepository;
        private readonly ITituloRepository _tituloRepository;
        private readonly IDbConnectionProvider _connection;

        public OpContaController(IOpContaRepository opContaRepository, IContaRepository contaRepository, IDbConnectionProvider connection, ITituloRepository tituloRepository)
        {
            _opContaRepository = opContaRepository;
            _contaRepository = contaRepository;
            _connection = connection;
            _tituloRepository = tituloRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var opsconta = _opContaRepository.Get();

            return Ok(opsconta);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var opsconta = _opContaRepository.GetById(id);

            return Ok(opsconta);
        }

        [HttpGet("OpsContaUsuarioLogado")]
        [Authorize]
        public IActionResult GetOpsContaUsuarioLogado()
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);

            var opsconta = _opContaRepository.GetOpContaUsuarioLogado(usuarioId);

            return Ok(opsconta);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(OpContaViewModel model)
        {
            var usuarioId = UsuarioLogadoHelper.ObterUsuarioId(HttpContext);
            using (var transaction = _connection.BeginTransaction())
            {
                _connection.CurrentTransaction = transaction;

                try
                {
                    if (model.ContaId == 0)
                    {
                        var contaId = _contaRepository.GetContaPrincipal(usuarioId);
                        if (contaId == 0)
                            throw new Exception("Nenhuma conta foi encontrada");

                        model.ContaId = contaId;
                    }

                    var conta = _contaRepository.GetById((int)model.ContaId);

                    var opConta = new OpConta(model.Descricao, model.Valor, model.DataOp, (int)model.ContaId, model.CategoriaOpId, model.TipoOpId);

                    if (!_opContaRepository.Create(opConta))
                        throw new Exception("Nao foi possivel criar a operacao da conta");

                    if(opConta.TipoOpId == (int)ETipoOp.Receita)
                    {
                        AtualizarValorConta(1, conta, model.Valor, opConta);
                    }
                    else if(opConta.TipoOpId == (int)ETipoOp.Despesa)
                    {
                        AtualizarValorConta(2, conta, model.Valor, opConta);
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

        private void AtualizarValorConta(int tipo, Conta conta, double valor, OpConta opConta)
        {
            if(tipo == 1)
            {
                conta.AdicionarReceita(valor);
            }
            else
            {
                if (conta.Balanco - valor < 0)
                    throw new Exception("Saldo insuficiente para operacao da conta");

                conta.DebitarConta(valor);
            }

            var contaViewModel = new ContaViewModel() { Balanco = conta.Balanco };

            if (!_contaRepository.Update(conta.ContaId, contaViewModel, conta.UsuarioId))
                throw new Exception("Nao foi possivel atualizar o valor da conta");

            if (!CriarTitulo(opConta.Descricao, "Titulo criado", 1, 1, (decimal)opConta.Valor, tipo,"Criado", opConta.DataOp, conta.ContaId))
                throw new Exception("Nao foi possivel criar o titulo");
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
