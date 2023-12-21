namespace Financas.Models
{
    public class Usuario
    {
        public Usuario()
        {
            
        }
        public Usuario(string nome, string email, string senha, DateTime dataNascimento)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            DataNascimento = dataNascimento.Date;
            DataCadastro = DateTime.Now;

            Contas = new List<Conta>();
            Balancos = new List<Balanco>();
        }

        public int UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public List<Conta> Contas { get; set; }
        public List<Balanco> Balancos { get; set; }   
    }
}
 