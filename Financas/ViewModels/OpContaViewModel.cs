namespace Financas.ViewModels
{
    public class OpContaViewModel
    {
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public DateTime DataOp { get; set; }
        public int CategoriaOpId { get; set; }
        public int TipoOpId { get; set; }
        public int? ContaId { get; set; } = 0;
    }
}
