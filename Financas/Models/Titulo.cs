namespace Financas.Models
{
    public class Titulo
    {
        public Titulo()
        {
            
        }
        public Titulo(string descricao, string observacao, int parcela, int numeroParcelas, decimal valor, int tipoTitulo, int contaId)
        {
            Descricao = descricao;
            Observacao = observacao;
            Parcela = parcela;
            NumeroParcelas = numeroParcelas;
            Valor = valor;
            TipoTitulo = tipoTitulo;
            ContaId = contaId;
        }

        public int TituloId { get; private set; }
        public string Descricao { get; private set; }
        public string Observacao { get; private set; }
        public int Parcela { get; private set; }
        public int NumeroParcelas { get; private set; }
        public decimal Valor { get; private set; }
        public int TipoTitulo { get; private set; }
        public string StatusTitulo { get; private set; }
        public DateTime DataTitulo { get; private set; }
        public DateTime DataPagamento { get; private set; }

        public int ContaId { get; private set; }
        public Conta Conta { get; private set; }
    }
}
