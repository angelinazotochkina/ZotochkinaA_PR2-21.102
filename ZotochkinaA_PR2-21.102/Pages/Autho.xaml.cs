using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using System.Windows.Threading;
using ZotochkinaA_PR2_21._102.Model;

namespace ZotochkinaA_PR2_21._102.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        private int countUnsuccessful = 0;
        private string captcha = string.Empty;
        private bool isButtonBlocked = false;
        private int countdownDuration = 10;
        private DispatcherTimer countdownTimer;
        private int z = 0;

        public Autho()
        {
            InitializeComponent();
            txtCaptcha.Visibility = Visibility.Hidden;
            textBlockCaptcha.Visibility = Visibility.Hidden;

            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = new TimeSpan(0, 0, 1);
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            int remainingSeconds = countdownDuration - z;
            if (remainingSeconds > 0)
            {
                seconds.Text = $"Вход заблокирован, попробуйте\nснова через: {remainingSeconds} секунд";
                z++;
            }
            else
            {
                countdownTimer.Stop();
                isButtonBlocked = false;
                btnEnter.IsEnabled = true;
                txtLogin.IsEnabled = true;
                txtPassword.IsEnabled = true;
                txtCaptcha.IsEnabled = true;
                z = 0;
                seconds.Text = null;
            }
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (isButtonBlocked)
            {
                MessageBox.Show("Кнопка входа заблокирована. Подождите, пока не истечет время блокировки.");
                return;
            }

            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            // Хэшируем введенный пароль
            string hashedPassword = hash.HashPassword(password);
            //  txtLogin.Text= hashedPassword.ToString();

            using (var Entities = new Entities())
            {
                Users user = Entities.Users.FirstOrDefault(p => p.Login == login && p.Password == hashedPassword);


                if (countUnsuccessful >= 3)
                {
                    BlockLoginButton();
                }
                if (user != null)
                {
                    
                    // Проверяем, включена ли у пользователя двухфакторная аутентификация
                    if (user.TwoFactorAuth == 1)
                    {
                        string userEmail = Entities.Users.FirstOrDefault(w => w.IDUser == user.IDUser)?.Login;
                        string confirmationCode = null;

                        if (userEmail != null)
                        {
                            if (userEmail.Contains("@mail.ru"))
                            {
                                // Отправляем код подтверждения на электронную почту
                                confirmationCode = MailRuMailSender.SendMailRu(userEmail);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Адрес электронной почты пользователя не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // Открываем окно подтверждения и обрабатываем результат
                        ConfirmWindow confirmWindow = new ConfirmWindow(confirmationCode);
                        bool? result = confirmWindow.ShowDialog();

                        if (result == true)
                        {
                             string fullName = GetFullName(user);
              

                    string welcomeMessage = $"Добро пожаловать, {fullName} ({user.RoleName})! ";
                    MessageBox.Show(welcomeMessage);
                    countUnsuccessful = 0;
                    LoadForm(user);
       
                        }
            
                        else
                        {
                            MessageBox.Show("Введенный код неверный. Пожалуйста, попробуйте еще раз!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    return;
                }    
                else
                {
                    countUnsuccessful++;
                    GenerateCaptcha();
                    MessageBox.Show("Вы ввели неверный логин или пароль! Введите капчу для продолжения.");
                }
            }
        }

      

        private void GenerateCaptcha()
        {
            textBlockCaptcha.Visibility = Visibility.Visible;
            txtCaptcha.Visibility = Visibility.Visible;

            Random random = new Random();
            int length = random.Next(5, 10);
            string captchaCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string generatedCaptcha = string.Empty;

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, captchaCharacters.Length);
                generatedCaptcha += captchaCharacters[randomIndex];
            }

            captcha = generatedCaptcha;

            textBlockCaptcha.Text = captcha;
        }
       
        public void CheckEmployeeAccess()
        {
            DateTime currentTime = DateTime.Now;
            if (currentTime.TimeOfDay < new TimeSpan(10, 0, 0) || currentTime.TimeOfDay > new TimeSpan(19, 0, 0))
            {
                // Блокируем доступ для сотрудников
                // Например, можно отключить кнопку авторизации и вывести сообщение об ошибке
                btnEnter.IsEnabled = false;
                lbAccessEnter.Content = "Доступ закрыт. Рабочее время еще не началось или закончилось.";
                lbAccessEnter.Foreground = Brushes.Red;
            }
        }
        private void BlockLoginButton()
        {
            txtLogin.IsEnabled = false;
            txtPassword.IsEnabled = false;
            txtCaptcha.IsEnabled = false;
            isButtonBlocked = true;
            btnEnter.IsEnabled = false;
            countdownTimer.Start();
        }

        private void LoadForm(Model.Users user)
        {
            switch (user.RoleName)
            {
                case "":
                case "Сотрудник":
                case "Администратор":

                    NavigationService.Navigate(new Admin1(user));
                    break;
                case "Клиент":
                    NavigationService.Navigate(new Client(user));
                    break;
            }
        }
      
        private string GetFullName(Users user)
        {
            string fullName = $"{user.UserSurname} {user.UserName}";

            if (!string.IsNullOrEmpty(user.UserPatronymic))
            {
                fullName += $" {user.UserPatronymic}";
            }

            return fullName;
        }
      


       
      
        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnforget_Click(object sender, RoutedEventArgs e)
        {
            PasswordRecovery passwordRecoveryWindow = new PasswordRecovery();
            passwordRecoveryWindow.Show();
        }

        private void txtCaptcha_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}