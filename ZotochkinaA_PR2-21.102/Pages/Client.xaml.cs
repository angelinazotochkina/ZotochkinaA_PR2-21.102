﻿using System;
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
using ZotochkinaA_PR2_21._102.Model;

namespace ZotochkinaA_PR2_21._102.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
       
        private Model.Users user;
        public Client(Users users)
        {
            InitializeComponent();
            user = users;
            //lbGreetingClient.Content = $"{users.UserName} {users.UserSurname} {users.UserPatronymic}";
           // var product = Entities.GetContext().Product.ToList();
         //   LViewProduct.ItemsSource= product;
        }

        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
    }
}
