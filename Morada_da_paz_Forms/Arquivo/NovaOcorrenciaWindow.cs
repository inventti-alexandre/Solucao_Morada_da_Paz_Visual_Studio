﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Morada_da_paz_Biblioteca.basicas;
using Morada_da_paz_WebService;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Morada_da_paz_Forms.Arquivo
{
    public partial class NovaOcorrenciaWindow : Form
    {

        #region Atributos Sockets
        private NetworkStream networkStream;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;
        private TcpClient tcpClient;

        private Thread thread;
        #endregion

        public NovaOcorrenciaWindow()
        {
            InitializeComponent();
            if (PrincipalWindow.usuarioAtivo.Id_especializacao_usuario.Id == 2)
            {
                checkBox1.Visible = false;
            }
        }

        private void buttonEnviar_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            ocorrencia oco = new ocorrencia() { Numero_ocorrencia = "" + rnd.Next(1000), Situacao = "Em Aberto" };
            oco.Id_usuario.Id = PrincipalWindow.usuarioAtivo.Id;

            oco.Descricao = richTextBox1.Text;
            if (checkBox1.Checked == true)
                oco.TipoPublico = 1;
            else
                oco.TipoPublico = 0;


            ServiceMoradaDaPaz serviceinstance = new ServiceMoradaDaPaz();

            try
            {
                serviceinstance.inserirOcorrencia(oco);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Ocorrencia Cadastrada");

            #region comunicando com o lado servidor pelo socket

            try
            {
                if (PrincipalWindow.binaryWriter != null)
                {
                    PrincipalWindow.binaryWriter.Write("Uma nova ocorrência foi adcionada!\n\nAtualize a Lista!");
                }

            }
            catch (SocketException socketEx)
            {
                MessageBox.Show(socketEx.Message, "Erro");
            }
            #endregion

            //this.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }
        public void runCliente()
        {
            try
            {
                tcpClient = new TcpClient();
                //conectando ao servidor
                tcpClient.Connect("127.0.0.1", 2001);

                networkStream = tcpClient.GetStream();
                binaryWriter = new BinaryWriter(networkStream);
                binaryReader = new BinaryReader(networkStream);
                //binaryWriter.Write("Uma nova ocorrência foi adcionada!\n\nAtualize a Lista!");
                String message = "";

                #region laço para receber mensagem do servidor
                do
                {
                    try
                    {
                        message = binaryReader.ReadString();
                        Invoke(new MethodInvoker(
                          delegate { MessageBox.Show("lado cliente" + message); }
                          ));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erro");
                        message = "FIM";
                    }
                } while (message != "FIM");
                #endregion

                binaryWriter.Close();
                binaryReader.Close();
                networkStream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
        }
    }
}
