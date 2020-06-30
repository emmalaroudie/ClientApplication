﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Contract;

namespace ClientApplication
{
    public class ResponseRequester
    {
        public Communicator communicator;
        private Message messageCheckRequest;
        private bool isPositiveResponseReceived = false;
        //public Processor processor;

        public ResponseRequester(Communicator com)
        {
            communicator = new Communicator();
            communicator = com;

            messageCheckRequest = new Message();
            //communicator = new Communicator();
            //blbbla();


        }

        public async void CheckResult()
        {
            await Task.Run(() => RequestResult());
        }

        private async void RequestResult()
        //public void RequestResult()
        {
            while (!isPositiveResponseReceived)
            {
                if (communicator.tokenUser != null && communicator.channelFactory.State == System.ServiceModel.CommunicationState.Opened)
                {
                    communicator.message.operationName = "returnResult";
                    messageCheckRequest = communicator.RequestResponse();
                    switch (messageCheckRequest.statusOp.ToString())
                    {
                        case "Waiting" :
                            Console.WriteLine("Waiting");
                            Thread.Sleep(10000);
                            break;
                        case "Working":
                            Console.WriteLine("Working");
                            Thread.Sleep(5000);
                            break;
                        case "Finished":
                            Console.WriteLine("Finished");
                            isPositiveResponseReceived = true;
                            break;
                    }
                }
            }
            MessageBox.Show(messageCheckRequest.operationName + " & " + messageCheckRequest.statusOp);
        }
    }
}
