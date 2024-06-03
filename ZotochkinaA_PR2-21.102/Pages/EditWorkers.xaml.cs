﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
using static ZotochkinaA_PR2_21._102.Model.Workers;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace ZotochkinaA_PR2_21._102.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditWorkers.xaml
    /// </summary>
    public partial class EditWorkers : Window
    {
        private int? IDRole;
        private string SelectedRoleName;
        public ObservableCollection<Workers> Workers { get; set; }
        private Entities context;
        private Workers Worker;

        public EditWorkers(Workers worker)
        {
            InitializeComponent();
            Worker = worker;
            context = new Entities();
            DataContext = this;
            LoadData(Worker.IDWorker); // Вызов метода LoadData() для загрузки данных выбранного сотрудника
            LoadRoles();

        }

        private void LoadRoles()
        {
            try
            {
                using (var context = new Entities())
                {
                    var roles = context.Roles.ToList();
                    cbRoles.ItemsSource = roles;
                    cbRoles.DisplayMemberPath = "RoleName";
                    cbRoles.SelectedValuePath = "IDRole";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке ролей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка данных для указанного сотрудника
        private void LoadData(int workerId)
        {
            using (var context = new Entities())
            {
                Workers worker = context.Workers.Find(workerId);
                try
                {
                    if (worker == null)
                    {
                        // Сотрудник не найден, можно обработать этот сценарий
                        // Например, можно вывести сообщение или очистить поля ввода
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("fail");
                }
            }
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtWorkerName.Text = string.Empty;
            txtWorkerSurname.Text = string.Empty;
            txtWorkerPatronymic.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtLogin.Text = string.Empty;
            txtPswd.Text = string.Empty;

            chbTwoFactorAuth.IsChecked = false;

            cbRoles.SelectedIndex = -1; // Очищаем выбранное значение
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание диалогового окна для выбора изображения.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|Все файлы (*.*)|*.*";

            // Отображение диалогового окна и установка выбранного изображения как источника для элемента Image.
            if (openFileDialog.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int twoFactorAuthValue = chbTwoFactorAuth.IsChecked == true ? 1 : 0; // Получаем значение для TwoFactorAuth: 1 если галочка установлена, иначе 0

            // Создание объекта editedWorker с новыми данными из полей ввода.
            Workers editedWorker = new Workers()
            {
                WorkerName = txtWorkerName.Text,
                WorkerSurname = txtWorkerSurname.Text,
                WorkerPatronymic = txtWorkerPatronymic.Text,
                PhoneNumber = txtPhoneNumber.Text,
                SeriePass = txtseriePass.Text,
                NumberPass = txtnumPass.Text,
             
                TwoFactorAuth = twoFactorAuthValue,
                
            };

            /*
            * Обработка ошибок валидации:
            *  - Если присутствуют ошибки валидации:
            *      - Объединяются все сообщения об ошибках в одну строку.
            *      - Отображается сообщение об ошибке с объединенными сообщениями.
            */
            var validationContext = new ValidationContext(editedWorker, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(editedWorker, validationContext, validationResults, validateAllProperties: true);

            // Проверка наличия ошибок валидации и вывод сообщения об ошибках при их обнаружении
            if (validationResults.Any())
            {
                string errorMessages = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                MessageBox.Show(errorMessages, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Поиск сотрудника в базе данных для редактирования
            Workers selectedWorker = context.Workers.FirstOrDefault(w => w.IDWorker == Worker.IDWorker);
            /*
            * Если работник найден:
            *  - Обновляются свойства работника значениями из нового объекта.
            *  - Если пароль был отредактирован:
            *      - Хешируется новый пароль с помощью объекта hash (предположительно, внедренная зависимость).
            *      - Обновляется пароль работника хешированным значением.
            */
            if (selectedWorker != null)
            {
                selectedWorker.WorkerName = editedWorker.WorkerName;
                selectedWorker.WorkerSurname = editedWorker.WorkerSurname;
                selectedWorker.WorkerPatronymic = editedWorker.WorkerPatronymic;
                selectedWorker.PhoneNumber = editedWorker.PhoneNumber;
                selectedWorker.SeriePass = editedWorker.SeriePass;
                selectedWorker.NumberPass = editedWorker.NumberPass;
               // selectedWorker.WorkerLogin = editedWorker.WorkerLogin;
                selectedWorker.TwoFactorAuth = editedWorker.TwoFactorAuth;
               // selectedWorker.IDRole = (int)cbRoles.SelectedValue;

                if (!string.IsNullOrWhiteSpace(txtPswd.Text))
                {
                    string hashedPassword = hash.HashPassword(txtPswd.Text);
                  //  selectedWorker.WorkerPassword = hashedPassword;
                }

                try
                {
                    context.SaveChanges();
                    MessageBox.Show("Данные сохранены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PrintList_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                FlowDocument flowdoc = Doc.Document as FlowDocument;
                IDocumentPaginatorSource idp = flowdoc;
                pd.PrintDocument(idp.DocumentPaginator, "Doc");
            }
        }

        private void cbRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту карточку?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                int workerId = Worker.IDWorker;

                if (workerId > 0)
                {
                    Workers selectedWorker = context.Workers.FirstOrDefault(w => w.IDWorker == workerId);

                    if (selectedWorker != null)
                    {
                        context.Workers.Remove(selectedWorker);
                        context.SaveChanges();

                        MessageBox.Show("Карточка удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
