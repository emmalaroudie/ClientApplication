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
        private Dictionary<string,string> listDataBis;

        //private ResponseRequester requester;
        private Dictionary<string, string> dico = new Dictionary<string, string>();
        public string Path { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public bool ConnectionBtnIsEnabled { get; set; }
        public bool DecipherBtnIsEnabled { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public string StateConnection
        {
            get; set;
        }
        public Processor()
        {

            messageRequest = new Message();
            communicator = new Communicator();
            listData = new List<string>();

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
                //if (Path != null){
                    Path = "E:\\FICHIERS";
                    GetFilesToDecipher();
                    communicator.message = messageRequest;
                    if (communicator.message.data != null)
                        communicator.Decipher();
                    else
                        MessageBox.Show("Une erreure s'est produite veuillez réessayer.");
                //}else MessageBox.Show("Veuillez entrer un chemin valide.");
            }
            else if (opName == "authentificate")
            {

                GetLoginInfos();
                communicator.message = messageRequest;
                if (communicator.message.data != null)
                {
                    communicator.Authentification();
                    //requester = new ResponseRequester(communicator);
                    //requester.CheckResult();
                }



            }
        }

        // Récupération du login et du mot de passe pour
        // les ajouter à l'array de données du message
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


        // Récupération des fichiers à déchiffrer,
        // stockage du texte des fichiers dans une liste de string
        // conversion de la liste de string en array de string 
        // pour correspondre à l'array de Message object Data[]
        private void GetFilesToDecipher()
        {
            try
            {
                listDataBis = new Dictionary<string, string>();

                DirectoryInfo dir = new DirectoryInfo(Path);
              
                foreach (FileInfo file in dir.GetFiles("*.txt"))
                {
                    if (!file.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        listDataBis.Add(file.Name, File.ReadAllText(file.FullName));
                        //listData.Add(file);
                        //listData.Add(file.Name + "\n" + File.ReadAllText(file.FullName));
                    }
                   
                }


                string[] test = new string[listDataBis.Count];

                listDataBis.Values.CopyTo(test, 0);

                messageRequest.data = test;

                // test du contenu de l'object data []
                // foreach(string str in messageRequest.data){MessageBox.Show(str);}
                //dico.Add(file.Name, File.ReadAllText(file.FullName));
                //count++;

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
