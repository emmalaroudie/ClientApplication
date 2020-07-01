using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApplication
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Processor processor = new Processor();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = processor;
            
        }

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            Thread thread1 = new Thread(() => { processor.sendRequest("authentificate"); });
            thread1.Start();
        }

        private void Decipher_Click(object sender, RoutedEventArgs e)
        {
            Thread thread2 = new Thread(() => { processor.sendRequest("decipher"); });
            thread2.Start();
        }

    }
}
