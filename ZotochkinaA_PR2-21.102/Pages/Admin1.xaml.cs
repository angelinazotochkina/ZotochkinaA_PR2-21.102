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
using System.Windows.Shapes;
using ZotochkinaA_PR2_21._102.Model;

namespace ZotochkinaA_PR2_21._102.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin1.xaml
    /// </summary>
    public partial class Admin1 : Page
    {
        private Model.Users user;
        public Admin1(Users users)
        {
            InitializeComponent();
            user = users;
            string greeting = greetUser.GetTimeOfDayGreeting();
            DateTime currentTime = DateTime.Now;
          //  lbGreetingAdmin.Content = $"{greeting},  {users.UserName} {users.UserSurname} {users.UserPatronymic},{currentTime}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new WorkersPage());
        }
    }
}
