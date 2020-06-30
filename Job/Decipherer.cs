using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contract;

namespace Job
{
    class Decipherer
    {
  
        private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
        private List<string> listkey = new List<string>();
        private List<string> textsToDecipher = new List<string>();;
        
        public Decipherer(){}

        public Message Decipher(Message msg)
        {
            msg.info = "Demande de déchiffrage reçue";

            foreach(string str in msg.data)
            {
                textsToDecipher.Add(str);
            }

            
            CreateKey(0, 26);

            var options = new ParallelOptions() { MaxDegreeOfParallelism = 4 };

            Parallel.ForEach(listkey, options, str =>   // foreach (string str in listkey)
            {         
                {
                    //foreach (string strfile in textsToDecipher){Parallel.ForEach(listkey, options, str =>{textsDeciphered.Add(Dechiffrer(str.ToCharArray(), strfile));});}

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\FichiersDeciphered\" + str + ".txt", true))
                    {
                        foreach (string strfile in textsToDecipher)
                        {
                            //if(strfile.Contains()) .txt c'est que c'est le nom du fichier on y touche pas on l'ajoute aux texte final
                            file.WriteLine(Dechiffrer(str.ToCharArray(), strfile));
                        }

                        // Call methode to search for secrete info : send deciphered text, ke, and name of the file
                    }
                }
            });

            Console.ReadLine();
            return msg;
        }

        public void CreateKey(int start, int finish)
        {
            for (int i = start; i < finish; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            listkey.Add(alphabet[i].ToString() + alphabet[j].ToString() + alphabet[k].ToString() + alphabet[l].ToString());
                        }
                    }
                }
            }
        }


        public string Dechiffrer(char[] key, string fileCiphered)
        {
            int indexKey = 0;

            string fileDEciphered = "";
            foreach (char c in fileCiphered)
            {
                if (indexKey == 4)
                    indexKey = 0;

                // récupérer l'ascii de la lettre de la clé 
                byte byteKey = (byte)key[indexKey];

                // récupérer l'ascii du char de la string
                byte byteChar = (byte)c;

                // comparer les deux et stocker dans un int
                int byteXOR = byteKey ^ byteChar;

                fileDEciphered += Convert.ToChar(byteXOR);
                indexKey++;
            }
            return fileDEciphered;
        }
    }
}
