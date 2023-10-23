using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ZotochkinaA_PR2_21._102.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        
        public Autho()
        {
            InitializeComponent();
           
        }
      
        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            Client clientPage = new Client();
            NavigationService.Navigate(clientPage);
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
