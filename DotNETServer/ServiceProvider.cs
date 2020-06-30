using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contract;
using System.Threading;
using Job;
using System.Xml;

namespace DotNETServer
{
    public class ServiceProvider
    {
        private Uri baseAddress;
        private ServiceHost host;
        public void ServiceStart()
        {
            //baseAddress = new Uri("net.tcp://localhost:8018/Job/MessageService");
            //host = new ServiceHost(typeof(MessageService));
            //try
            //{
            //    host.AddServiceEndpoint(typeof(IMessageService), new NetTcpBinding(), baseAddress);
            //    host.Open();

            //    Console.WriteLine("Serveur started");
            //    Console.WriteLine("Press enter to stop the app");

            //    Console.ReadLine();
            //    host.Close();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Exception rencontrée : {ex.Message}");
            //}


            baseAddress = new Uri("net.tcp://localhost:8018/Job/MessageService");
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Message;
            binding.MaxReceivedMessageSize= 2147483647;

            XmlDictionaryReaderQuotas myReaderQuotas = new XmlDictionaryReaderQuotas();
            myReaderQuotas.MaxStringContentLength = 2147483647;
            binding.GetType().GetProperty("ReaderQuotas").SetValue(binding, myReaderQuotas, null);
            host = new ServiceHost(typeof(MessageService));
            try
            {
                host.AddServiceEndpoint(typeof(IMessageService), binding, baseAddress);
                host.Open();

                Console.WriteLine("Serveur started");
                Console.WriteLine("Type quit to stop the app");
                string command = "";
                while (command != "quit")
                {
                    command = Console.ReadLine();
                    if (command != "")
                    {
                        Console.WriteLine("Unknown command, type \"quit\" to stop the app");
                    }
                }
                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception rencontrée : {ex.Message}");
            }
        }
    }
}
