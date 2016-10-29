﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projeto_morada_da_paz.basicas;

namespace Morada_da_paz_Biblioteca.DadosAcesso
{
    interface CrudAdvertencia
    {
        void inserir(advertencia a);
        void atualizar(advertencia a);
        void deletar(advertencia a);
        advertencia consultar(advertencia a);
        List<advertencia> listar();
    }
}