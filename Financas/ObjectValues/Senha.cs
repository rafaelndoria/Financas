using System.Text.RegularExpressions;

namespace Financas.ObjectValues
{
    public class Senha
    {
        public Senha(string senha)
        {
            VerifyIsValid(senha);
            SenhaHash = senha; /* Implementar servico para codificar a senha */
        }

        public string SenhaHash { get; private set; }

        private void VerifyIsValid(string senha)
        {
            string pattern = @"^(?=.*[0-9])(?=.*[A-Z]).*$";
            if (senha == null)
                throw new Exception("A senha nao pode ser nula");
            if (senha.Length < 6)
                throw new Exception("A senha deve ser maior que 6 caracteres");
            if (senha.Length > 16)
                throw new Exception("A senha deve ser menor que 16 caracteres");
            if (!Regex.IsMatch(senha, pattern))
                throw new Exception("A senha deve conter pelo menos um número e uma letra maiúscula");
        }
    }
}
