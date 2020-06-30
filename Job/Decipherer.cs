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
        static readonly object lockObject = new object();
        private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
        private char[] key;
        //private List<char[]> listkey = new List<char[]>();
        private List<string> listkey = new List<string>();

        private List<string> textsToDecipher;
        private List<string> textsDeciphered;
        
        public Decipherer()
        {
            textsToDecipher = new List<string>();
            textsDeciphered = new List<string>();
        }

        public Message Decipher(Message msg)
        {
            msg.info = "Demande de déchiffrage reçue";

            //foreach(string str in msg.data){textsToDecipher.Add(str);}
            textsToDecipher.Add(msg.data[2].ToString());
            
            CreateKey(0, 26);

            var options = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
            Parallel.ForEach(listkey, options, str =>
            {
                Dechiffrer(str.ToCharArray());
            });


            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\FichierDEciphered.txt"))
            {
                foreach (string str in textsDeciphered)
                {
                    file.WriteLine(str);
                }
            }



            //-----------------------------------------------------------------------------
            /*
                       //DateTime start1 = DateTime.Now;//.ToString("h:mm:ss tt");
                       Thread thread1 = new Thread(() => { CreateKey(0, 26); });
                       thread1.Start();
                       thread1.Join();
                       //DateTime end1 = DateTime.Now;
            */
            //-----------------------------------------------------------------------------

            //DateTime start2 = DateTime.Now;
            // Thread thread2 = new Thread(() => { CreateKey(0, 4); });
            // Thread thread3 = new Thread(() => { CreateKey(4, 7); });
            //Thread thread3 = new Thread(() => { CreateKey(7, 14); });
            //Thread thread4 = new Thread(() => { CreateKey(14, 20); });
            //Thread thread5 = new Thread(() => { CreateKey(20, 26); });
            //  thread2.Start();
            //  thread3.Start();
            //thread4.Start();
            //thread5.Start();
            //  thread2.Join();
            // thread3.Join();
            //thread4.Join();
            //thread5.Join();
            //  DateTime end2 = DateTime.Now;

            //-----------------------------------------------------------------------------



            /*
            DateTime start3 = DateTime.Now;
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 4 };
            Parallel.Invoke(
                parallelOptions,
                () => CreateKey(0, 3),
                () => CreateKey(3, 6),
                () => CreateKey(6, 9),
                () => CreateKey(9, 12),
                () => CreateKey(12, 15),
                () => CreateKey(15, 18),
                () => CreateKey(18, 21),
                () => CreateKey(21, 24),
                () => CreateKey(24, 26));
            DateTime end3 = DateTime.Now;
            */


            //-----------------------------------------------------------------------------

            /*Console.WriteLine("-----------------------using 1 thread-----------------------");
            Console.WriteLine(start1 + " - " + end1 + " = " + end1.Subtract(start1).ToString() + " secondes ");
            Console.WriteLine("-----------------------using 4 threads-----------------------");
            Console.WriteLine(start2 + " - " + end2 + " = " + end2.Subtract(start2).ToString() + " secondes ");
            Console.WriteLine("-------------using Parallel.Invoke with 4 threads-------------");
            Console.WriteLine(start3 + " - " + end3 + " = " + end3.Subtract(start3).TotalMilliseconds.ToString() + " secondes ");
            */
            Console.ReadLine();


            return msg;
        }
        public void CreateKey(int start, int finish)
        {

            List<string> test = new List<string>();
            for (int i = start; i < finish; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            /*
                            key = new char[4];
                            key[0] = alphabet[i];
                            key[1] = alphabet[j];
                            key[2] = alphabet[k];
                            key[3] = alphabet[l];
                            //listkey.Add(key);
                            */
                            //Console.WriteLine("key = " + key[0].ToString() + key[1].ToString() + key[2].ToString() + key[3].ToString());

                            // appel à déchiffrer avec la clé créée
                            listkey.Add(alphabet[i].ToString() + alphabet[j].ToString() + alphabet[k].ToString() + alphabet[l].ToString());

                            /*
                            string s = alphabet[i].ToString() + alphabet[j].ToString() + alphabet[k].ToString() + alphabet[l].ToString();
                            //Console.WriteLine(s);

                            if (!test.Contains(s))
                            {
                                test.Add(alphabet[i].ToString() + alphabet[j].ToString() + alphabet[k].ToString() + alphabet[l].ToString());

                            }
                            else{
                                Console.WriteLine("l'a voulu ajouter ça :");
                            }
                            */
                            //Dechiffrer(key);

                        }


                    }
                }
            }
            //Monitor.Enter(lockObject);
            //try
            //{
               /// listkey.AddRange(test);
                //foreach (string str in test){
                    //if (!listkeyContains(str)){
                       // Add(str);
                    //}
                //}
           // }
            //finally{  Monitor.Exit(lockObject);}




            //foreach(char[] key in listkey)
            //{
            //    string keystr = key[0].ToString()+ key[1].ToString()+key[2].ToString()+key[3].ToString();
            //    Console.WriteLine(keystr);
            //}
            //Console.ReadLine();
            
            //using(System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\keys.txt")){
             //   foreach(string str in test){
                //Console.WriteLine("\n"+str);
              //      file.WriteLine(str);
              //  }

            //}
         
        }

        public void Dechiffrer(char[] key)
        {
            //Console.WriteLine("\n key = " + key[0].ToString() + key[1].ToString() + key[2].ToString() + key[3].ToString());
            int indexKey = 0;
            foreach (string str in textsToDecipher)
            {
                //Console.WriteLine("string before = " + str);
                string strDeciphered = "";
                foreach (char c in str)
                {
                    if (indexKey == 4)
                        indexKey = 0;

                    // récupérer l'ascii de la lettre de la clé 
                    byte byteKey = (byte)key[indexKey];

                    // récupérer l'ascii du char de la string
                    byte byteChar = (byte)c;

                    // comparer les deux et stocker dans un int
                    int byteXOR = byteKey ^ byteChar;

                    // convertir en ascii/char et l'ajouter à la string le char
                    strDeciphered += Convert.ToChar(byteXOR);

                    indexKey++;

                }

                textsDeciphered.Add(strDeciphered);
            }
        }
    }
}
