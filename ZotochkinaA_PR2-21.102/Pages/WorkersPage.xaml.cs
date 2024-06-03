﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using Page = System.Windows.Controls.Page;
using Microsoft.Office.Interop.Excel;
using System.IO;
using ZotochkinaA_PR2_21._102.Model;

namespace ZotochkinaA_PR2_21._102.Pages
{
    /// <summary>
    /// Логика взаимодействия для WorkersPage.xaml
    /// </summary>
    public partial class WorkersPage : Page
    {
        public ObservableCollection<Workers> Workers { get; set; }
        public ObservableCollection<Roles> Roles { get; set; }
        public WorkersPage()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            using (var context = new Entities())
            {
                Workers = new ObservableCollection<Workers>(context.Workers.ToList());
            }
        }

        private void txtSearch_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            ICollectionView view = CollectionViewSource.GetDefaultView(LViewProduct.ItemsSource);

            if (view != null)
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    view.Filter = null; //Очистка фильтра, если поле поиска пустое
                }
                else
                {
                    //Устанавливается фильтр для поиска по нескольким полям объекта Workers.

                    view.Filter = item =>
                    {
                        Workers dataItem = item as Workers;

                        if (dataItem != null)
                        {
                            string itemPatronymic = "";
                            string itemName = dataItem.WorkerSurname.ToLower();
                            string itemSurname = dataItem.WorkerName.ToLower();
                            if (dataItem.WorkerPatronymic != null)
                            {
                                itemPatronymic = dataItem.WorkerPatronymic.ToLower();
                            }
                           // string itemWLogin = dataItem.WorkerLogin.ToLower();

                            /*
                             * Поиск по нескольким полям: фамилии, имени, отчеству и логину.
                             */
                            return itemName.Contains(searchText) ||
                                   itemSurname.Contains(searchText) ||
                                   itemPatronymic.Contains(searchText) ;
                                  // itemWLogin.Contains(searchText); 
                        }

                        return false;
                    };
                }
            }
        }

        private void ApplySorting()
        {
            /*
            * Применение сортировки к списку Workers по выбранному параметру и направлению.
            */
            ICollectionView view = CollectionViewSource.GetDefaultView(Workers);

            if (view != null)
            {
                ComboBoxItem selectedSortItem = cmbSorting.SelectedItem as ComboBoxItem;

                if (selectedSortItem != null && selectedSortItem.Tag != null)
                {
                    string[] sortParams = selectedSortItem.Tag.ToString().Split(',');
                    string sortProperty = sortParams[0];
                    ListSortDirection sortDirection = (ListSortDirection)Enum.Parse(typeof(ListSortDirection), sortParams[1]);

                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(sortProperty, sortDirection));
                }
            }
        }
        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySorting();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddWorker());
        }

        /*
         * Обработчик двойного щелчка по элементу списка Workers.
         * Открывает окно редактирования выбранного работника.
         */
        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (LViewProduct.SelectedItem != null)
            {
                using (Entities db = new Entities())
                {
                    int id = (LViewProduct.SelectedItem as Workers).IDWorker;
                    Workers worker = db.Workers.FirstOrDefault(w => w.IDWorker == id);
                    EditWorkers editWindow = new EditWorkers(worker);
                    editWindow.DataContext = LViewProduct.SelectedItem;
                    bool? result = editWindow.ShowDialog();

                    if (result == true)
                    {
                        LoadData();
                    }
                }
            }
        }
        private void UpdateList_Click(object sender, RoutedEventArgs e)
        {
            WorkersPage newWorkerPage = new WorkersPage();
            NavigationService.Navigate(newWorkerPage);
        }

        private void btnPrintList_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idp = doc;
                pd.PrintDocument(idp.DocumentPaginator, Title);
            }
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = excel.Workbooks.Add();
            Worksheet ws = (Worksheet)wb.ActiveSheet;
            ws.Cells[1, 1] = "Имя";
            ws.Cells[1, 2] = "Фамилия";
            ws.Cells[1, 3] = "Отчество";
            ws.Cells[1, 4] = "Номер телефона";
            ws.Cells[1, 5] = "Серия паспорта";
            ws.Cells[1, 6] = "Номер паспорта";
      
            int row = 2;
            foreach (var worker in Workers)
            {
                ws.Cells[row, 1] = worker.WorkerName;
                ws.Cells[row, 2] = worker.WorkerSurname;
                ws.Cells[row, 3] = worker.WorkerPatronymic;
                ws.Cells[row, 4] = worker.PhoneNumber;
                ws.Cells[row, 5] = worker.SeriePass;
                ws.Cells[row, 6] = worker.NumberPass;
                //ws.Cells[row, 7] = worker.IDRole;
                row++;
            }
            try
            {
                string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Workers.xlsx");
                wb.SaveAs(fileName);
                excel.Quit();
                MessageBox.Show("Данные экспортированы в Excel.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении файла Excel: " + ex.Message);
            }
        }
    }
}


