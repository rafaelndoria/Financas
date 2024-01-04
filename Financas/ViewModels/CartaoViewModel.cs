namespace Financas.ViewModels
{
    public class CartaoViewModel
    {
        public string Nome { get; set; }
        public double LimiteCredito { get; set; }
        public int Principal { get; set; }
        public double? LimiteCreditoAtual { get; set; }
        public int? ContaId { get; set; }
        public int DiaVencimento { get; set; }

    }
}
