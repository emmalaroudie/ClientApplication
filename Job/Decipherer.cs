using System;
using System.Collections.Generic;
using System.IO;
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
        private List<string> textsToDecipher = new List<string>();
        private List<string> namesOfFilesToDecipher = new List<string>();
        public Decipherer() { }

        public Message Decipher(Message msg)
        {
            msg.info = "Demande de déchiffrage reçue";

            for(int index = 0; index < msg.data.Length; index++)
            {
                if(index % 2 == 0)
                    namesOfFilesToDecipher.Add(msg.data[index].ToString());
                else
                    textsToDecipher.Add(msg.data[index].ToString());
            }


            CreateKey(0, 26);

            //var options = new ParallelOptions() { MaxDegreeOfParallelism = 4 };

            Parallel.ForEach(listkey, str => 
            {
                
                DirectoryInfo di = Directory.CreateDirectory(@"E:\FichiersD\" + str.ElementAt(0));
                DirectoryInfo di2 = Directory.CreateDirectory(@"E:\FichiersD\" + str.ElementAt(0) + "\\" + str.ElementAt(1));
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(di2.FullName + "\\" + str + ".txt", true))
                {
                    foreach (string strfile in textsToDecipher)
                    {
                        file.WriteLine(Dechiffrer(str.ToCharArray(), strfile));
                    }

                    // Call methode to search for secrete info : send deciphered text, key, and name of the file
                    // using the index of the string of textToDecipher in the nameOfFiles list
                }
            });
            return msg;
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
        }
    }
