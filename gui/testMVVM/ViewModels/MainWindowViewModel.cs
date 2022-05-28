using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testMVVM.Infrastructure.Commands;
using testMVVM.Models;
using testMVVM.Models.Test;
using testMVVM.ViewModels.Base;

namespace testMVVM.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Человекочитаемый отчет

        
        private string _HumanReport;
        public string HumanReport 
        {
            get => _HumanReport; set => Set(ref _HumanReport, value);
        }

        #endregion

        #region Человекочитаемый отчет

        
        private List<string> _MachineReport;
        public List<string> MachineReport 
        {
            get => _MachineReport; set => Set(ref _MachineReport, value);
        }

        #endregion

        #region Данные таблицы Ext2

        private List<Ext2> _Ext2Data;
        public List<Ext2> Ext2Data 
        {
            get => _Ext2Data; set => Set(ref _Ext2Data, value);
        }

        #endregion
        
        #region Список найденных аномалий

        private List<Anomaly> _AnomalyList;

        public List<Anomaly> AnomalyList
        {
            get => _AnomalyList; set => Set(ref _AnomalyList, value);
        }

        #endregion

        #region Выбранное уведомление в списке

        private Notification _SelectedNotification;
        public Notification SelectedNotification
        {
            get => _SelectedNotification; set => Set(ref _SelectedNotification, value);
        }
        #endregion

        #region Список уведомлений

        private List<Notification> _NotificationsList;

        public List<Notification> NotificationsList
        {
            get => _NotificationsList; set => Set(ref _NotificationsList, value);
        }

        #endregion

        public object[] CompositeCollection { get; }

        #region Путь к базе данных

        private object _DbPath;
        public object DbPath { get => _DbPath; set => Set(ref _DbPath, value); }

        #endregion

        #region Дата начала периода

        private DateTime _DateFrom = DateTime.Parse("2022-04-15");
        public DateTime DateFrom { get => _DateFrom; set => Set(ref _DateFrom, value); }

        #endregion

        #region Дата конца периода

        private DateTime _DateTo = DateTime.Parse("2022-04-20");
        public DateTime DateTo { get => _DateTo; set => Set(ref _DateTo, value); }

        #endregion

        #region Данные таблицы Ext

        private List<Ext> _ExtData;

        public List<Ext> ExtData
        {
            get => _ExtData;
            set => Set(ref _ExtData, value);
        }

        #endregion

        #region Выбранный непонятный элемент

        private object _SelectedCompositeValue;
        public object SelectedCompositeValue { get => _SelectedCompositeValue; set => Set(ref _SelectedCompositeValue, value); }

        #endregion

        #region Выбранный поиск аномалии

        private Anomaly _SelectedAnomaly;
        public Anomaly SelectedAnomalyTab { get => _SelectedAnomaly; set => Set(ref _SelectedAnomaly, value); }

        #endregion

        #region SelectedPageIndex

        private int _SelectedPageIndex = 0;
        public int SelectedPageIndex { get => _SelectedPageIndex; set => Set(ref _SelectedPageIndex, value); }

        #endregion

        #region TabsCount

        private int _TabsCount = 0;
        public int TabsCount { get => _TabsCount; set => Set(ref _TabsCount, value); }

        #endregion

        //График: если надо изменять точки, то возвращаем ObservableCollection, иначе перечисление

        #region ТестГрафика

        /// <summary> Тестовый набор данных для визуализации графиков </summary>

        private IEnumerable<DataPoint> _TestDataPoints;

        /// <summary> Тестовый набор данных для визуализации графиков </summary>

        public IEnumerable<DataPoint> TestDataPoints
        {
            get => _TestDataPoints;
            set => Set(ref _TestDataPoints, value);
        }

        #endregion

        #region Данные вылова

        /// <summary> Тестовый набор данных для визуализации графиков </summary>

        private IEnumerable<Catch> _CatchData;

        public IEnumerable<Catch> CatchData
        {
            get => _CatchData;
            set => Set(ref _CatchData, value);
        }

        #endregion

        #region Данные переработки

        private IEnumerable<Product> _ProductData;
        public IEnumerable<Product> ProductData
        {
            get => _ProductData;
            set => Set(ref _ProductData, value);
        }

        #endregion

        #region Заголовок окна 

        private string _Title = "Поиск аномалий";

        /// <summary>  Заголовок окна </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Справочник рыба
        private IEnumerable<Reference> _FishReference;
        public IEnumerable<Reference> FishReference
        {
            get => _FishReference;
            set => Set(ref _FishReference, value);
        }
        #endregion

        /*********************************************************************************************************************************************/

        #region Команды

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region ChangeSelectedIndexCommand

        public ICommand ChangeSelectedIndexCommand { get; }

        private bool CanChangeSelectedIndexCommandExecute(object p) => _SelectedPageIndex >= 0;

        public void OnChangeSelectedIndexCommandExecuted(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }

        #endregion

        #region ConfirmPathCommand
        public ICommand ImportConfirmCommand { get; }

        private bool CanImportConfirmCommandExecute(object p) => true;
        private void OnImportConfirmCommandExecuted(object p)
        {
            Import();
        }

        #endregion


        #region SearchAnomalyCommand
        public ICommand SearchAnomalyCommand { get; }

        private bool CanSearchAnomalyCommandExecute(object p) => true;
        private void OnSearchAnomalyCommandExecuted(object p)
        {
            try
            {
                string exepath = "data\\model.exe";
                //System.Diagnostics.Process.Start(exepath, $"\"{DbPath}\" \"{DateFrom}\" \"{DateTo}\"").WaitForExit();
                
                List<Notification> notify_list = new List<Notification>();

                string human_readable_report = File.ReadAllText("delay_report_anomaly.txt");
                HumanReport = human_readable_report;


                NotificationsList = notify_list;

            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message + "\nПопробуйте изменить входные данные", "Ошибка выполнения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        #endregion

        #endregion
        /*********************************************************************************************************************************************/
        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new RelatedCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ChangeSelectedIndexCommand = new RelatedCommand(OnChangeSelectedIndexCommandExecuted, CanChangeSelectedIndexCommandExecute);
            ImportConfirmCommand = new RelatedCommand(OnImportConfirmCommandExecuted, CanImportConfirmCommandExecute);
            SearchAnomalyCommand = new RelatedCommand(OnSearchAnomalyCommandExecuted, CanSearchAnomalyCommandExecute);


            #endregion

            AnomalyList = new List<Anomaly>
            {
                new Anomaly {Id = 1, Name = "Отсутствие или искажение обязательной информации в ССД", Description = "", Priority = Anomaly.Status.Dangerous},
                new Anomaly {Id = 2, Name = "Несоответствие данных привоза продукции и переработки", Description = "", Priority = Anomaly.Status.Minor},
                new Anomaly {Id = 3, Name = "Серьезное нарушение соответствия данных между ССД и данными системы “Меркурий”", Description = "Не очень", Priority = Anomaly.Status.Middle}
            };
            var data_points = new List<DataPoint>((int)(360 / 0.1));

            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }

            TestDataPoints = data_points;

            var data_list = new List<object>();

            data_list.Add("Hello World");
            data_list.Add(42);

            NotificationsList = new List<Notification>();

            //NotificationsList = new List<Notification>
            //{
            //    new Notification
            //    {
            //        Date = DateTime.Now,
            //        Anomaly = new Anomaly {Id = 1, Description = "Серьезная аномаль", Priority = Anomaly.Status.Dangerous},
            //    },

            //    new Notification
            //    {
            //        Date = DateTime.Today,
            //        Anomaly = new Anomaly {Id = 2, Description = "Не страшно", Priority = Anomaly.Status.Minor},
            //    },
            //};


            //using var watcher = new FileSystemWatcher(@"C:\");

            //watcher.NotifyFilter = NotifyFilters.Attributes
            //                     | NotifyFilters.CreationTime
            //                     | NotifyFilters.DirectoryName
            //                     | NotifyFilters.FileName
            //                     | NotifyFilters.LastAccess
            //                     | NotifyFilters.LastWrite
            //                     | NotifyFilters.Security
            //                     | NotifyFilters.Size;

        }
        private async Task<Dictionary<string,string>> GetReference(string path)
        {

            Dictionary<string, string> reference = new Dictionary<string, string>();
            string row_string = "";
            using (StreamReader reader = new StreamReader(path))
            {
                await reader.ReadLineAsync();
                while (!reader.EndOfStream)
                {
                    row_string = await reader.ReadLineAsync();
                    if (row_string != "")
                    {
                        string[] row = row_string.Split(';');
                        reference[row[0]] = row[1];
                    }
                }
            }
            
            return reference;
        }

        private async void Import()
        {
            #region Справочники

            try
            {
                var fish_task = await GetReference(DbPath + @"\ref\fish.csv");

                var fishref = await GetReference(DbPath + @"\ref\fish.csv");
                var prod_designate = await GetReference(DbPath + @"\ref\prod_designate.csv");
                var prod_type = await GetReference(DbPath + @"\ref\prod_type.csv");
                var region = await GetReference(DbPath + @"\ref\region.csv");
                var regime = await GetReference(DbPath + @"\ref\regime.csv");

                #endregion

                List<Catch> catch_report = new List<Catch>();

                using(StreamReader reader = new StreamReader(DbPath + @"\catch.csv"))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var catch_row = await reader.ReadLineAsync();
                        var catch_arr = catch_row.Split(',');

                        catch_report.Add(new Catch
                        {
                            Id_ves = int.Parse(catch_arr[0]),
                            Date = DateTime.Parse(catch_arr[1]),
                            Id_region = region[catch_arr[2]].Trim('"'),
                            Id_fish = fishref[catch_arr[3]].Trim('"'),
                            Catch_volume = decimal.Parse(catch_arr[4].Replace('.', ',')),
                            Id_regime = regime[catch_arr[5]].Trim('"'),
                            Permit = int.Parse(catch_arr[6]),
                            Id_own = int.Parse(catch_arr[7])
                        });
                    }
                }

               CatchData = catch_report;

                List<Product> product_report = new List<Product>();

                using(StreamReader reader = new StreamReader(DbPath + @"\product.csv"))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var row = await reader.ReadLineAsync();
                        var row_arr = row.Split(',');
                        var current_product = new Product();
                        current_product.Id_ves = int.Parse(row_arr[0]);
                        current_product.Date = DateTime.Parse(row_arr[1]);
                        current_product.Id_prod_designate = prod_designate[row_arr[2]].Trim('"');

                        if (prod_type.ContainsKey(row_arr[3])) current_product.Prod_type = prod_type[row_arr[3]].Trim('"');
                        current_product.Prod_volume = decimal.Parse(row_arr[4].Replace('.', ','));
                        current_product.Prod_board_volume = decimal.Parse(row_arr[5].Replace('.', ','));

                        product_report.Add(current_product);
                    }
                }

                ProductData = product_report;


                List<Ext> ext_report = new List<Ext>();

                using (StreamReader reader = new StreamReader(DbPath + @"db2\Ext.csv"))
                {
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var row = await reader.ReadLineAsync();
                        string[] row_arr = row.Split(',');
                        var current_product = new Ext();
                        int id_fishery, id_own, num_part, id_plat, id_vsd;
                        DateTime product_period, date_fishery;

                        int.TryParse(row_arr[0], out id_fishery);
                        int.TryParse(row_arr[1], out id_own);
                        DateTime.TryParse(row_arr[2], out date_fishery);
                        int.TryParse(row_arr[3], out num_part);
                        int.TryParse(row_arr[4], out id_plat);
                        int.TryParse(row_arr[5], out id_vsd);
                        DateTime.TryParse(row_arr[7], out product_period);

                        ext_report.Add(new Ext
                        {
                            Id_fishery = id_fishery,
                            Id_own = id_own,
                            Date_fishery = date_fishery,
                            Num_part = num_part,
                            Id_Plat = id_plat,
                            Id_vsd = id_vsd,
                            Name_plat = row_arr[6],
                            Product_period = product_period,
                            Region_plat = row_arr[8]
                        });
                        
                    }
                }

                ExtData = ext_report;

                List<Ext2> ext2_report = new List<Ext2>();

                using (StreamReader reader = new StreamReader(DbPath + @"db2\Ext2.csv"))
                {
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var row = await reader.ReadLineAsync();
                        string[] row_arr = row.Split(',');
                        var current_product = new Ext2();
                        int id_vsd, num_vsd, id_fish;
                        DateTime date_vsd;
                        decimal volume;
                        string fish, unit;


                        int.TryParse(row_arr[0], out id_vsd);
                        int.TryParse(row_arr[1], out num_vsd);
                        int.TryParse(row_arr[2], out id_fish);
                        fish = row_arr[3];
                        DateTime.TryParse(row_arr[4], out date_vsd);
                        decimal.TryParse(row_arr[5], out volume);
                        unit = row_arr[6];

                        ext2_report.Add(new Ext2
                        {
                            Id_vsd = id_vsd,
                            Num_vsd = num_vsd,
                            Id_fish = id_fish,
                            Date_vsd = date_vsd,
                            Volume = volume,
                            Unit = unit
                        });
                    }
                }

                Ext2Data = ext2_report;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Нет файла", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
