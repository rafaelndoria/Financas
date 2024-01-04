namespace Financas.Models
{
    public class OpCartao
    {
        public OpCartao()
        {
            
        }
        public OpCartao(string descricao, double valor, DateTime dataOp, int parcelado, int quantidadeParcelas, int cartaoId, int categoriaOpId, int tipoOpCartaoId)
        {
            Descricao = descricao;
            Valor = valor;
            DataOp = dataOp;
            Parcelado = parcelado;
            QuantidadeParcelas = quantidadeParcelas;
            ValorPorParcela = Parcelado == 1 ? (Valor / QuantidadeParcelas) : Valor;
            CartaoId = cartaoId;
            CategoriaOpId = categoriaOpId;
            TipoOpCartaoId = tipoOpCartaoId;
        }

        public int OpCartaoId { get; private set; }
        public string Descricao { get; private set; }
        public double Valor { get; private set; }
        public DateTime DataOp { get; private set; }
        public int Parcelado { get; private set; }
        public int QuantidadeParcelas { get; private set; }
        public double? ValorPorParcela { get; private set; }

        public int CartaoId { get; private set; }
        public Cartao Cartao { get; private set; }
        public int CategoriaOpId { get; private set; }
        public CategoriaOp CategoriaOp { get; private set; }
        public int TipoOpCartaoId { get; private set; }
        public TipoOpCartao TipoOpCartao { get; private set; }
    }
}
