﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Windows;
using Contract;
using System.Threading;

namespace ClientApplication
{
    public class Communicator
    {

        private EndpointAddress endpointAdress = new EndpointAddress("net.tcp://localhost:8018/Job/MessageService");
        private IMessageService messageService;
        public Message message;

        private string tokenApp = "tokenApp";
        public string tokenUser;

        public ChannelFactory<Contract.IMessageService> channelFactory = null;

        public Communicator() {
            message = new Message();
        }

        private void ConnectionToServeur()
        {
            channelFactory = new ChannelFactory<Contract.IMessageService>("MaConfigurationClient");//*/new NetTcpBinding(), endpointAdress);
            messageService = channelFactory.CreateChannel();

            
        }

        private void WaitAndClose()
        {
            Thread.Sleep(600000*2);
            CloseConnectionToServer();
            MessageBox.Show("Votre session a expiré. Veuillez vous reconnecter");
        }

        private void CloseConnectionToServer()
        {
            if (channelFactory != null || channelFactory.State != CommunicationState.Opened)
            {
                channelFactory.Close();
            }
            tokenUser = null;
        }


        public void Authentification()
        {
            //MessageBox.Show("Classe Communicator, méthode authentification");
            if (channelFactory == null || channelFactory.State == CommunicationState.Closed)
            {
                ConnectionToServeur();
            }
            message.tokenApp = tokenApp;
            message = messageService.Servicing(message);
            if(message.tokenUser != null)
            {
                tokenUser = message.tokenUser;
                //MessageBox.Show("Connection réussie. Client : token user = " + tokenUser);
                Thread threadStop = new Thread(() => { WaitAndClose(); });
                threadStop.Start();
            }
            else
            {
                MessageBox.Show("Connection refusée. Veuillez réessayer. ");
                CloseConnectionToServer();
            }
            
        }

        public void Decipher()
        {
            if(channelFactory == null || channelFactory.State == CommunicationState.Closed)
            {
                MessageBox.Show("Veuillez vous identifier avant de lancer une requête");
            }
            else
            {
                message.tokenApp = tokenApp;
                message.tokenUser = tokenUser;
                message = messageService.Servicing(message);
                MessageBox.Show("Demande de déchiffrement des fichiers effectuée");
            }
        }

        public Message RequestResponse()
        {
            if (channelFactory != null || channelFactory.State != CommunicationState.Closed)
            {
                message = messageService.Servicing(message);
            }
            return message;
        }
    }
}