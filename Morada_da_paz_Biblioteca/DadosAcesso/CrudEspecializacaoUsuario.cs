﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Morada_da_paz_Biblioteca.basicas;

namespace Morada_da_paz_Biblioteca.DadosAcesso
{
    public interface CrudEspecializacaoUsuario
    {
        void inserir(especializacao_usuario eu);
        void atualizar(especializacao_usuario eu);
        void deletar(especializacao_usuario eu);
        especializacao_usuario consultar(especializacao_usuario eu);
        List<especializacao_usuario> listar();
    }
}
