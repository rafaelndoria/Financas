using Financas.ObjectValues;

namespace Financas.Models
{
    public class Usuario
    {
        public Usuario(string nome, Email email, Senha senha, DateTime dataNascimento)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            DataNascimento = dataNascimento;
            DataCadastro = DateTime.Now;

            Contas = new List<Conta>();
            Balancos = new List<Balanco>();
        }

        public int UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public Senha Senha { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public List<Conta> Contas { get; set; }
        public List<Balanco> Balancos { get; set; }   
    }
}
 