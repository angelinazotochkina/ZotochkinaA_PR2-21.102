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
using ZotochkinaA_PR2_21._102.Pages;

namespace ZotochkinaA_PR2_21._102
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    { private void LoadLoginpage()
        {
          Autho loginPage = new Autho();

            // Устанавливаем его как содержимое фрейма
            FrmMain.NavigationService.Navigate(loginPage);
        }
        public MainWindow()
        {
            InitializeComponent();
            LoadLoginpage();
        }
       
        private void FrmMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.CanGoBack)
                btnback.Visibility = Visibility.Visible;
            else
                btnback.Visibility = Visibility.Hidden;
        }

        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                FrmMain.GoBack();
            }
        }

        private void btnback_Click_1(object sender, RoutedEventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                btnback.Visibility = Visibility.Visible;
            }
            else
            {
                btnback.Visibility = Visibility.Hidden;
            }
        }
    }
}
