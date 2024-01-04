namespace Financas.ViewModels
{
    public class TituloViewModel
    {
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public int Parcela { get; set; }
        public int NumeroParcelas { get; set; }
        public decimal Valor { get; set; }
        public int TipoTitulo { get; set; }
        public string StatusTitulo { get; set; }
        public DateTime DataTitulo { get; set; }
        public DateTime? DataPagamento { get; set; } = null;
        public int ContaId { get; set; }
    }
}
