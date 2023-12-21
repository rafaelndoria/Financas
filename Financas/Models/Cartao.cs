namespace Financas.Models
{
    public class Cartao
    {
        public Cartao()
        {
            
        }
        public Cartao(string nome, double limiteCredito, int principal, int contaId, Conta conta, double? limiteCreditoAtual = 0)
        {
            Nome = nome;
            LimiteCredito = limiteCredito;
            Principal = principal;
            ContaId = contaId;
            Conta = conta;
            LimiteCreditoAtual = limiteCreditoAtual;

            OpsCartao = new List<OpCartao>();
        }

        public int CartaoId { get; private set; }
        public string Nome { get; private set; }
        public double LimiteCredito { get; private set; }
        public double? LimiteCreditoAtual { get; private set; }
        public int Principal { get; private set; }

        public int ContaId { get; private set; }
        public Conta Conta { get; private set; }
        public List<OpCartao> OpsCartao { get; set; }

        public void RemoverLimite(double valor)
        {
            LimiteCreditoAtual -= valor;
        }
    }
}
