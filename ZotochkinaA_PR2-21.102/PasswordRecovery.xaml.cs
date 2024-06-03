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

namespace ZotochkinaA_PR2_21._102
{
    /// <summary>
    /// Логика взаимодействия для PasswordRecovery.xaml
    /// </summary>
    public partial class PasswordRecovery : Window
    {
        private string generatedCode;
        private string userEmail;
        public PasswordRecovery()
        {
            InitializeComponent();

        }

        private void SendConfirmationCode_Click(object sender, RoutedEventArgs e)
        {
            userEmail = txtUsernameOrEmail.Text;

            if (userEmail.Contains("@mail.ru"))
            {
                generatedCode = MailRuMailSender.SendMailRu(userEmail);
                MessageBox.Show("Код подтверждения отправлен на вашу почту.");
            }
            else
            {
                MessageBox.Show("Указанный почтовый сервис не поддерживается!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = txtConfirmationCode.Text;


            if (ValidateConfirmationCode(enteredCode, generatedCode))
            {

                OpenNewPasswordWindow();
            }
            else
            {
                MessageBox.Show("Введенный код неверный. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool ValidateConfirmationCode(string enteredCode, string generatedCode)
        {
            return enteredCode == generatedCode;
        }
        private void OpenNewPasswordWindow()
        {
            NewPasswordWindow newPasswordWindow = new NewPasswordWindow(userEmail);
            newPasswordWindow.Show();
            this.Close();
        }
    }
}
