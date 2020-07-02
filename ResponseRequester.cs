using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Contract;

namespace ClientApplication
{
    public class ResponseRequester : INotifyPropertyChanged
    {
        public Communicator communicator;
        private Message messageCheckRequest;
        public bool IsRequestFinished { get; set; }
        public string StateRequest { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;


        public ResponseRequester()
        {
            communicator = new Communicator();
            messageCheckRequest = new Message();

            IsRequestFinished = false;
            StateRequest = "none";
        }

        public void Download()
        {
            string path = "C:\\Desktop\\DecipheredFiles\\";
            MessageBox.Show("Starting Download : Your files will be available in : " + path);

            DirectoryInfo di = Directory.CreateDirectory(path);

            Parallel.For(0, messageCheckRequest.data.Length, index =>
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + messageCheckRequest.data[index] + ".txt", true))
                {
                    file.WriteLine("The content of the file has been deciphered using the followinf key : " + messageCheckRequest.data[index+2]);
                    file.WriteLine(messageCheckRequest.data[index+1]);
                }
                Interlocked.Add(ref index, 3);
            });

            MessageBox.Show("Download finished ");
        }

        public void RequestResult()
        {
            while (!IsRequestFinished)
            {

                if (communicator.tokenUser != null && communicator.channelFactory.State == System.ServiceModel.CommunicationState.Opened)
                {
                    communicator.message.operationName = "returnResult";
                    messageCheckRequest = communicator.RequestResponse();
                    StateRequest = messageCheckRequest.statusOp.ToString();
                    switch (StateRequest)
                    {
                        case "Waiting":
                            Console.WriteLine("Waiting");
                            Thread.Sleep(10000);
                            break;
                        case "Working":
                            Console.WriteLine("Working");
                            Thread.Sleep(5000);
                            break;
                        case "Finished":
                            Console.WriteLine("Finished");
                            IsRequestFinished = true;
                            break;
                    }

                }
                else
                {
                    StateRequest = "none";
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StateRequest"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRequestFinished"));
            }
        }
    }
}
