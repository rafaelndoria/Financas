namespace Financas.Models
{
    public class Fatura
    {
        public Fatura()
        {
            
        }
        public Fatura(double valor, int mes, int ano, int cartaoId)
        {
            Valor = valor;
            Mes = mes;
            Ano = ano;
            CartaoId = cartaoId;

            PagamentoFaturas = new List<PagamentoFatura>();
        }

        public int FaturaId { get; private set; }
        public double Valor { get; private set; }
        public int Mes { get; private set; }
        public int Ano { get; private set; }

        public int CartaoId { get; private set; }
        public Cartao Cartao { get; private set; }

        public List<PagamentoFatura> PagamentoFaturas { get; private set; }
    }
}
