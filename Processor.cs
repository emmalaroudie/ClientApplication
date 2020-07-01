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
    public class Processor : INotifyPropertyChanged
    {
        public Communicator communicator;
        private Message messageRequest;
        private List<string> listData;
        private ResponseRequester requester;

        public string Path { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string StateConnection { get; set; }
        public bool ConnectionBtnIsEnabled { get; set; }
        public bool DecipherBtnIsEnabled { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public Processor()
        {

            messageRequest = new Message();
            communicator = new Communicator();
            listData = new List<string>();
            requester = new ResponseRequester();
            Thread t = new Thread(() => { CheckStateConnection(); });
            t.Start();
        }


        private void CheckStateConnection()
        {
            while (true)
            {
                if (communicator.channelFactory != null && communicator.tokenUser != null)
                {
                    StateConnection = communicator.channelFactory.State.ToString();
                    DecipherBtnIsEnabled = true;
                    ConnectionBtnIsEnabled = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StateConnection"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DecipherBtnIsEnabled"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ConnectionBtnIsEnabled"));
                }
                else
                {
                    StateConnection = "Closed";
                    DecipherBtnIsEnabled = false;
                    ConnectionBtnIsEnabled = true;
                }
                Thread.Sleep(1000);
            }
        }

        // Appel de la méthode correction et setting du nom de l'opération
        public void sendRequest(string opName)
        {
            messageRequest.operationName = opName;

            if (opName == "decipher")
            {
                if (Path != null){
                    GetFilesToDecipher();
                    communicator.message = messageRequest;
                    if (communicator.message.data != null)
                        communicator.Decipher();
                    else
                        MessageBox.Show("Une erreure s'est produite veuillez réessayer.");
                }
                else 
                    MessageBox.Show("Veuillez entrer un chemin valide.");
            }
            else if (opName == "authentificate")
            {

                GetLoginInfos();
                communicator.message = messageRequest;
                if (communicator.message.data != null)
                {
                    communicator.Authentification();
                    requester.communicator = communicator;

                    Thread threadRequestResult = new Thread(() => { requester.RequestResult(); });
                    threadRequestResult.Start();
                    
                }
            }
        }

        // Récupération du login et du mot de passe pour les ajouter à l'array de données du message
        private void GetLoginInfos()
        {
            if ((Login != null) && (Password != null))
            {
                listData.Add(Login);
                listData.Add(Password);
                messageRequest.data = listData.ToArray();
                listData.Clear();
            }
            else
            {
                MessageBox.Show("Veuillez entrer un identifiant et un mot de passe.");
                Thread.Sleep(10000);
                GetLoginInfos();
            }
        }


        // Récupération des fichiers à déchiffrer, stockage du texte des fichiers dans une liste de string,
        // conversion de la liste de string en array de string pour correspondre à l'array de Message object Data[]
        private void GetFilesToDecipher()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Path);              
                foreach (FileInfo file in dir.GetFiles("*.txt"))
                {
                    if (!file.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        //On ajoute le nom du fichier (indice % 2 = 0)
                        listData.Add(file.Name);
                        //On ajoute le contenu du fichier (indice % 2 = 1)
                        listData.Add(File.ReadAllText(file.FullName));
                    }
                }

                // on ajoute le contenu de la liste à l'object[] data du message
                messageRequest.data = listData.ToArray();
                listData.Clear();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file was not found:" + e);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The directory was not found:" + e);
            }
        }
    }
}
