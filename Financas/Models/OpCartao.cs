namespace Financas.Models
{
    public class OpCartao
    {
        public OpCartao(string descricao, double valor, DateTime dataOp, int parcelado, int quantidade, int parcela, int cartaoId, Cartao cartao, int categoriaOpId, CategoriaOp categoriaOp, int tipoOpCartaoId, TipoOpCartao tipoOpCartao)
        {
            Descricao = descricao;
            Valor = valor;
            DataOp = dataOp;
            Parcelado = parcelado;
            Quantidade = quantidade;
            Parcela = parcela;
            ValorPorParcela = Parcelado == 1 ? (Valor / Quantidade) : Valor;
            CartaoId = cartaoId;
            Cartao = cartao;
            CategoriaOpId = categoriaOpId;
            CategoriaOp = categoriaOp;
            TipoOpCartaoId = tipoOpCartaoId;
            TipoOpCartao = tipoOpCartao;
        }

        public int OpCartaoId { get; private set; }
        public string Descricao { get; private set; }
        public double Valor { get; private set; }
        public DateTime DataOp { get; private set; }
        public int Parcelado { get; private set; }
        public int Quantidade { get; private set; }
        public int Parcela { get; private set; }
        public double? ValorPorParcela { get; private set; }

        public int CartaoId { get; private set; }
        public Cartao Cartao { get; private set; }
        public int CategoriaOpId { get; private set; }
        public CategoriaOp CategoriaOp { get; private set; }
        public int TipoOpCartaoId { get; private set; }
        public TipoOpCartao TipoOpCartao { get; private set; }
    }
}
