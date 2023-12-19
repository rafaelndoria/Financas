namespace Financas.Models
{
    public class TipoOpCartao
    {
        public TipoOpCartao(string nome)
        {
            Nome = nome;
        }

        public int TipoOpCartaoId { get; private set; }
        public string Nome { get; private set; }
    }
}
