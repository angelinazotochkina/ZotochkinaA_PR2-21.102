
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Net;
using System.IO;
using Microsoft.Office.Interop.Word;

using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using Page = System.Windows.Controls.Page;
using Microsoft.Office.Interop.Excel;
using ZotochkinaA_PR2_21._102.Model;
using Range = Microsoft.Office.Interop.Word.Range;

namespace ZotochkinaA_PR2_21._102.Pages
{ 
    /// <summary>
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Page
    {
        private Entities context;
        private int? IDRole;
        private string SelectedRoleName;

        public bool DialogResult { get; set; }


        public AddWorker()
        {
            InitializeComponent();
            context = new Entities();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбрана ли роль
            //if (RoleName == null)
            //{
            //    MessageBox.Show("Пожалуйста, выберите роль перед сохранением данных.");
            //    return; // Прекращаем выполнение метода, так как роль не выбрана
            //}

            int twoFactorAuthValue = chbTwoFactorAuth.IsChecked == true ? 1 : 0; // Получаем значение для TwoFactorAuth: 1 если галочка установлена, иначе 0

            Workers newWorker = new Workers
            {
                WorkerName = txtWorkerName.Text,
                WorkerSurname = txtWorkerSurname.Text,
                WorkerPatronymic = txtWorkerPatronymic.Text,
                PhoneNumber = txtPhoneNumber.Text,
                SeriePass = txtseriePass.Text,
                NumberPass = txtnumPass.Text,
           
               


            }; 
            var selectedComboBoxItem = (ComboBoxItem)cb.SelectedItem;
            Users newUser = new Users
            {
               Login = txtLogin.Text,
               Password = hash.HashPassword(txtPswd.Text),
               TwoFactorAuth = twoFactorAuthValue,
               RoleName = selectedComboBoxItem.Content.ToString()


        };
           
            /*
            * Обработка ошибок валидации:
            *  - Если присутствуют ошибки валидации:
            *      - Объединяются все сообщения об ошибках в одну строку.
            *      - Отображается сообщение об ошибке с объединенными сообщениями.
            */
            var validationContext = new ValidationContext(newWorker, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(newWorker, validationContext, validationResults, validateAllProperties: true);

            if (validationResults.Any())
            {
                string errorMessages = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
                MessageBox.Show(errorMessages, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                context.Workers.Add(newWorker);
                context.Users.Add(newUser);
                context.SaveChanges();

                Contract(newWorker.SeriePass, newWorker.WorkerSurname, newWorker.WorkerName, newWorker.WorkerPatronymic, newWorker.NumberPass, SelectedRoleName);

                DialogResult = true;

                MessageBox.Show("Данные сохранены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                //NavigationService.GoBack(); // Navigate back
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Contract(string SeriePass, string WorkerSurname, string WorkerName, string WorkerPatronymic, string NumberPass, string roleName)
        {
            var items = new Dictionary<string, string>()
            {
                {"<city>", "Новосибирск"},
                {"<currentdate>", DateTime.Now.ToString("dd.MM.yyyy")},
                {"<olo>", "ООО"},
                {"<number>", "1"},
                {"<company>", "OOo Синар"},
                {"<dir>", "Краснова М.В"},
                {"<worker>", "Краснова М.В."},
                {"<address>", "Красина 14"},
                {"<kpp>", "123456789"},
                {"<zap>", "70000 рублей (70 тысяч рублей)" },
                {"<data>", "13"},
                {"<mvd>", "ГУ МВД России по Новосибирску"},
                {"<seriepass>", SeriePass},
                {"<name>", WorkerName},
                {"<surname>", WorkerSurname},
                {"<patronymic>", WorkerPatronymic},
                {"<numberpass>", NumberPass},
                {"<role>", roleName}
            };

            Microsoft.Office.Interop.Word.Application wordApp = null;
            Document wordDoc = null;
            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                object missing = System.Reflection.Missing.Value;
                string fileName = @"C:\Users\Angelina\OneDrive\Документы\3 курс\программные модули\ZotochkinaA_PR2-21.102lastedition\ZotochkinaA_PR2-21.102\Files\blank.docx";
                if (!File.Exists(fileName))
                {
                    MessageBox.Show("Файл не найден: " + fileName);
                    return;
                }
                wordDoc = wordApp.Documents.Open(fileName, ReadOnly: false, Visible: true);
                foreach (var item in items)
                {
                    object findText = item.Key;
                    object replaceText = item.Value;
                    Range myRange = wordDoc.Content;
                    myRange.Find.ClearFormatting();
                    myRange.Find.Execute(FindText: findText, ReplaceWith: replaceText, Replace: WdReplace.wdReplaceAll);
                }
                string newFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "contract.docx");
                wordDoc.SaveAs2(newFilePath);
                MessageBox.Show("Документ успешно сохранен: " + newFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
            finally
            {
                wordDoc?.Close();
                wordApp?.Quit();
            }
        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            txtWorkerName.Text = string.Empty;
            txtWorkerSurname.Text = string.Empty;
            txtWorkerPatronymic.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtseriePass.Text = string.Empty;
            txtnumPass.Text = string.Empty;
            txtLogin.Text = string.Empty;
            txtPswd.Text = string.Empty;

            chbTwoFactorAuth.IsChecked = false;

            cb.SelectedIndex = -1; // Очищаем выбранное значение
        }

        private void PrintList_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                FlowDocument flowDoc = Doc.Document as FlowDocument;
                IDocumentPaginatorSource idp = flowdoc;
                pd.PrintDocument(idp.DocumentPaginator, "Doc");
            }
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание диалогового окна для выбора изображения.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Установка изображения
                imgPhoto.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb.SelectedItem != null)
            {
                var selectedComboBoxItem = (ComboBoxItem)cb.SelectedItem;
                SelectedRoleName = selectedComboBoxItem.Content.ToString();
                int roleId;
                if (int.TryParse(selectedComboBoxItem.Tag.ToString(), out roleId))
                {
                  
                }
                else
                {
                    MessageBox.Show("Ошибка при получении ID роли.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

