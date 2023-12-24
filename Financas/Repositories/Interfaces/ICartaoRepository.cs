﻿using Financas.Models;
using Financas.ViewModels;

namespace Financas.Repositories.Interfaces
{
    public interface ICartaoRepository
    {
        bool PossuiCartaoPrincipal(int contaId);
        bool Insert(Cartao cartao);
        List<Cartao> Get();
        Cartao GetById(int id);
        List<Cartao> GetCartaoUsuarioLogado(int usuarioId);
        bool Delete(int cartaoId);
        bool Update(int cartaoId, CartaoViewModel cartao);
        bool RemoverCartaoPreferido();
    }
}
