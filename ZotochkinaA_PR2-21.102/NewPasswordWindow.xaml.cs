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

namespace ZotochkinaA_PR2_21._102
{
    /// <summary>
    /// Логика взаимодействия для NewPasswordWindow.xaml
    /// </summary>
    public partial class NewPasswordWindow : Window
    {
        private string userEmail;
        public NewPasswordWindow(string userEmail)
        {
            InitializeComponent();
            this.userEmail = userEmail;
        }

        private void SaveNewPassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, попробуйте снова!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            ChangePassword(userEmail, newPassword);
        }
        public void ChangePassword(string userEmail, string newPassword)
        {
            using (Entities bd = new Entities())
            {
                /*
                 * Если пользователь найден:
                 * - Хешируется новый пароль.
                 * - Обновляется пароль пользователя в базе данных.
                 * - Отображается сообщение об успешном изменении пароля.
                 * Если пользователь не найден:
                 * - Отображается сообщение об ошибке.
                 */
                try
                {
                    Users user = bd.Users.FirstOrDefault(u => u.Login == userEmail);

                    if (user != null)
                    {
                       user.Password = hash.HashPassword(newPassword);
                        bd.SaveChanges();
                        MessageBox.Show("Пароль успешно изменен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с таким email не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при изменении пароля: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

