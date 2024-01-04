using Financas.ViewModels;

namespace Financas.Repositories.Interfaces
{
    public interface ITituloRepository
    {
        bool Create(TituloViewModel model);
    }
}
