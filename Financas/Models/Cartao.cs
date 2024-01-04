namespace Financas.Models
{
    public class Cartao
    {
        public Cartao()
        {
            
        }
        public Cartao(string nome, double limiteCredito, int principal, int diaVencimento, int contaId, double? limiteCreditoAtual = 0)
        {
            Nome = nome;
            LimiteCredito = limiteCredito;
            Principal = principal;
            DiaVencimento = diaVencimento;
            ContaId = contaId;
            LimiteCreditoAtual = limiteCreditoAtual == 0 ? limiteCredito : limiteCreditoAtual;

            OpsCartao = new List<OpCartao>();
            Faturas = new List<Fatura>();
        }

        public int CartaoId { get; private set; }
        public string Nome { get; private set; }
        public double LimiteCredito { get; private set; }
        public double? LimiteCreditoAtual { get; private set; }
        public int Principal { get; private set; }
        public int DiaVencimento { get; private set; }

        public int ContaId { get; private set; }
        public Conta Conta { get; private set; }
        public List<OpCartao> OpsCartao { get; set; }
        public List<Fatura> Faturas { get; set; }

        public void RemoverLimite(double valor)
        {
            LimiteCreditoAtual -= valor;
        }

        public void AdicionarLimite(double valor)
        {
            LimiteCreditoAtual += valor;
        }
    }
}
