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

            // séparer le contenu des de leurs noms dans 2 listes différentes
            List<string> listNames = new List<string>();
            List<string> listFiles = new List<string>();
            for (int index = 0; index < messageCheckRequest.data.Length; index++)
            {
                if (index % 2 == 0)
                    listNames.Add(messageCheckRequest.data[index].ToString());
                else
                    listFiles.Add(messageCheckRequest.data[index].ToString());
            }

            DirectoryInfo di = Directory.CreateDirectory(path);

            Parallel.For(0, listFiles.Count, i =>
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + listNames.ElementAt(i) + ".txt", true))
                {
                    file.WriteLine(listFiles.ElementAt(i));
                }
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
