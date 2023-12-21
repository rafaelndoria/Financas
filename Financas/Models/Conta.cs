namespace Financas.Models
{
    public class Conta
    {
        public Conta()
        {
            
        }
        public Conta(string nome, int principal, double balanco, int usuarioId, Usuario usuario)
        {
            Nome = nome;
            Principal = principal;
            Balanco = balanco;
            UsuarioId = usuarioId;
            Usuario = usuario;
            DataCadastro = DateTime.Now;

            Cartoes = new List<Cartao>();
            OpsConta = new List<OpConta>();
        }

        public int ContaId { get; private set; }
        public string Nome { get; private set; }
        public int Principal { get; private set; }
        public double Balanco { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; }

        public List<Cartao> Cartoes { get; set; }
        public List<OpConta> OpsConta { get; set; } 
    }
}
