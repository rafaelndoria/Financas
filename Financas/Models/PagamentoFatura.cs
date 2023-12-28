namespace Financas.Models
{
    public class PagamentoFatura
    {
        public PagamentoFatura()
        {
            
        }
        public PagamentoFatura(double valorPago, int faturaId)
        {
            ValorPago = valorPago;
            FaturaId = faturaId;
        }

        public int PagamentoFaturaId { get; private set; }
        public double ValorPago { get; private set; }

        public int FaturaId { get; private set; }
        public Fatura Fatura { get; private set; }
    }
}
