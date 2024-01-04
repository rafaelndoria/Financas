namespace Financas.ViewModels
{
    public class OpCartaoViewModel
    {
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public DateTime DataOp { get; set; }
        public int Parcelado { get; set; }
        public int Quantidade { get; set; }
        public int CategoriaOpId { get; set; }
        public int TipoOpCartaoId { get; set; }
        public int? CartaoId { get; set; }
    }
}
