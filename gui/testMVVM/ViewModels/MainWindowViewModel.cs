using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        //public ObservableCollection<Group> Groups { get; }
        //static public IEnumerable<Anomaly> dict = new IEnumerable<Anomaly>
        //{
        //    //{"01", "Наличие значений, сильно выше или ниже средних значений по отрасли или для данного судна." +
        //    //    "\n\nПожалуйста, проверьте представленные данные и, если это необходимо, подвердите отправку официальному представителю РосРыболовства"},
        //    //{"04", "Ошибка в выборе единиц измерений\n\n" }
        //    new Anomaly{Id = "01", 
        //        Description = "Наличие значений, сильно выше или ниже средних значений по отрасли или для данного судна. Наличие значений, сильно выше или ниже средних значений по отрасли или для данного судна"},
        //    new Anomaly{Id = "02", 
        //        Description = "Незначительное нарушение.\n\nПожалуйста, проверьте полученную информацию. В случае обнаружения признаков незаконной деятельности подтвердите отправку данныхh"}

        //};
        #region Список искомых аномалий
        
        private List<Anomaly> _AnomalyList;

        public List<Anomaly> AnomalyList
        {
            get => _AnomalyList; set => Set(ref _AnomalyList, value);
        }

        #endregion

        #region Список уведомлений

        private List<Anomaly> _NotificationsList;

        public List<Anomaly> NotificationsList
        {
            get => _NotificationsList; set => Set(ref _NotificationsList, value);
        }

        #endregion

        public object[] CompositeCollection { get; }

        #region Путь к базе данных №1

        private object _Db1Path;
        public object Db1Path { get => _Db1Path; set => Set(ref _Db1Path, value); }

        #endregion

        #region

        private List<Ext> _ExtData;

        public List<Ext> ExtData
        {
            get => _ExtData;
            set => Set(ref _ExtData, value);
        }

        #endregion

        #region Путь к базе данных №2

        private object _Db2Path;
        public object Db2Path { get => _Db2Path; set => Set(ref _Db2Path, value); }

        #endregion

        #region Выбранный непонятный элемент

        private object _SelectedCompositeValue;
        public object SelectedCompositeValue { get => _SelectedCompositeValue; set => Set(ref _SelectedCompositeValue, value); }

        #endregion
        #region Выбранная группа
        /// <summary>
        /// Выбранная группа в списке
        /// </summary>

        // private Group _SelectedGroup;
        //public Group SelectedGroup { get => _SelectedGroup; set => Set(ref _SelectedGroup, value); } 

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

        /// <summary> Тестовый набор данных для визуализации графиков </summary>

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
            //set
            //{
            //    // 1) Можно так
            //    //if (Equals(_Title, value)) return;
            //    //_Title = value;
            //    //OnPropertyChanged();

            //    //2) Можно и так
            //    //Set(ref _Title, value);
            //}
            //3) Но самое жесткое
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
        #region Status: string - Статус программы

        ///<summary>Статус программы</summary>
        private string _Status = "Готово";

        public string Status { get => _Status; set => Set(ref _Status, value); }

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


        #region ConfirmPathCommand
        public ICommand SearchAnomalyCommand { get; }

        private bool CanSearchAnomalyCommandExecute(object p) => true;
        private void OnSearchAnomalyCommandExecuted(object p)
        {
            string exepath = "";
            System.Diagnostics.Process.Start(exepath, (string)Db1Path + " " + (string)Db2Path).WaitForExit();
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
            SearchAnomalyCommand = new RelatedCommand(OnImportConfirmCommandExecuted, CanImportConfirmCommandExecute);


            #endregion

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


            NotificationsList = new List<Anomaly>
            {
                new Anomaly
                {
                    Id = "01",
                    Description = "Все плохо"
                },

                new Anomaly
                {
                    Id = "02",
                    Description = "Ну почти плохо"
                }
            };            


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
                var fish_task = await GetReference(Db1Path + @"\ref\fish.csv");

                var fish = await GetReference(Db1Path + @"\ref\fish.csv");
                var prod_designate = await GetReference(Db1Path + @"\ref\prod_designate.csv");
                var prod_type = await GetReference(Db1Path + @"\ref\prod_type.csv");
                var region = await GetReference(Db1Path + @"\ref\region.csv");
                var regime = await GetReference(Db1Path + @"\ref\regime.csv");

                #endregion

                List<Catch> catch_report = new List<Catch>();

                using(StreamReader reader = new StreamReader(Db1Path + @"\catch.csv"))
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
                            Id_fish = fish[catch_arr[3]].Trim('"'),
                            Catch_volume = decimal.Parse(catch_arr[4].Replace('.', ',')),
                            Id_regime = regime[catch_arr[5]].Trim('"'),
                            Permit = int.Parse(catch_arr[6]),
                            Id_own = int.Parse(catch_arr[7])
                        });
                    }
                }

               CatchData = catch_report;

                List<Product> product_report = new List<Product>();

                using(StreamReader reader = new StreamReader(Db1Path + @"\product.csv"))
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

                using (StreamReader reader = new StreamReader(Db2Path + @"\Ext.csv"))
                {
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var row = await reader.ReadLineAsync();
                        string[] row_arr = row.Split(',');
                        var current_product = new Ext();
                        //current_product.Id_fishery = Convert.ToInt32(row[0]);
                        //current_product.Id_own = Convert.ToInt32(row[1]);
                        //current_product.Date_fishery = Convert.ToDateTime(row[2].Trim('"'));

                        //current_product.Num_part = Convert.ToInt32(row[3]); //проверить здесь
                        //current_product.Id_Plat = Convert.ToInt32(row[4]);
                        //current_product.Id_vsd = Convert.ToInt32(row[5]);
                        //current_product.Name_plat = row[6];
                        //current_product.Product_period = Convert.ToDateTime(row[7]);
                        //current_product.Region_plat = row[8];
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
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
