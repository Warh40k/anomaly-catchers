using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //#region CreateGroupCommand

        //public ICommand CreateGroupCommand { get; }

        //private bool CanCreateGroupCommandExecute(object p) => true;

        //private void OnCreateGroupCommandExecuted(object p)
        //{
        //    var group_max_index = Groups.Count + 1;
        //    var new_group = new Group
        //    {
        //        Name = $"Группа {group_max_index}",
        //        DataBase = new ObservableCollection<DataBase>()
        //    };

        //    Groups.Add(new_group);
        //}

        //#endregion

        //#region DeleteGroupCommand

        //public ICommand DeleteGroupCommand { get; }

        //private bool CanDeleteGroupCommandExecute(object p) => p is Group group && Groups.Contains(group);
        //private void OnDeleteGroupCommandExecuted(object p)
        //{
        //    if (!(p is Group group)) return;
        //    int group_index = Groups.IndexOf(group);
        //    Groups.Remove(group);
        //    if (group_index < Groups.Count)
        //        SelectedGroup = Groups[group_index];
        //}

        //#endregion

        #endregion
        /*********************************************************************************************************************************************/
        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new RelatedCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ChangeSelectedIndexCommand = new RelatedCommand(OnChangeSelectedIndexCommandExecuted, CanChangeSelectedIndexCommandExecute);
            ImportConfirmCommand = new RelatedCommand(OnImportConfirmCommandExecuted, CanImportConfirmCommandExecute);


            #endregion

            var data_points = new List<DataPoint>((int)(360 / 0.1));

            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }

            TestDataPoints = data_points;




            //List<Product> product_report = new List<Product>();

            //using(StreamReader reader = new StreamReader(@"C:\Users\user\Desktop\Rosrybolovstvo\Датасет\db1\product.csv"))
            //{
            //    reader.ReadLine();
            //    while (!reader.EndOfStream)
            //    {
            //        string[] row = reader.ReadLine().Split(',');
            //        var current_product = new Product();
            //        current_product.Id_ves = Convert.ToInt32(row[0]);
            //        current_product.Date = Convert.ToDateTime(row[1]);
            //        current_product.Id_prod_designate = prod_designate[row[2]].Trim('"');

            //        if (prod_type.ContainsKey(row[3])) current_product.Prod_type = prod_type[row[3]].Trim('"');
            //        current_product.Prod_volume = Convert.ToDecimal(row[4].Replace('.', ','));
            //        current_product.Prod_board_volume = Convert.ToDecimal(row[5].Replace('.', ','));

            //        product_report.Add(current_product);
            //    }
            //}

            //ProductData = product_report;





            //var groups = Enumerable.Range(1, 20).Select(i => new Group()
            //{
            //    Name = "Группа" + i.ToString(),
            //    DataBase = new ObservableCollection<DataBase>(students)
            //});

            // Groups = new ObservableCollection<Group>(groups);

            var data_list = new List<object>();

            data_list.Add("Hello World");
            data_list.Add(42);
            //     data_list.Add(Groups[1].DataBase[1]);
            //    data_list.Add(Groups[1]);



            CompositeCollection = data_list.ToArray();

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
        private Dictionary<string, string> GetReference(string path)
        {

            Dictionary<string, string> reference = new Dictionary<string, string>();
            string row_string = "";
            using (StreamReader reader = new StreamReader(path))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    row_string = reader.ReadLine();
                    if (row_string != "")
                    {
                        string[] row = row_string.Split(';');
                        reference[row[0]] = row[1];
                    }
                }
            }
            
            return reference;
        }

        private void Import()
        {
            #region Справочники

            try
            {
                var fish = GetReference(Db1Path + @"\ref\fish.csv");
                var prod_designate = GetReference(Db1Path + @"\ref\prod_designate.csv");
                var prod_type = GetReference(Db1Path + @"\ref\prod_type.csv");
                var region = GetReference(Db1Path + @"\ref\region.csv");
                var regime = GetReference(Db1Path + @"\ref\regime.csv");

                #endregion

                List<Catch> catch_report = new List<Catch>();

                using(StreamReader reader = new StreamReader(Db1Path + @"\catch.csv"))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string[] catch_row = reader.ReadLine().Split(',');
                        catch_report.Add(new Catch
                        {
                            Id_ves = int.Parse(catch_row[0]),
                            Date = DateTime.Parse(catch_row[1]),
                            Id_region = region[catch_row[2]].Trim('"'),
                            Id_fish = fish[catch_row[3]].Trim('"'),
                            Catch_volume = decimal.Parse(catch_row[4].Replace('.', ',')),
                            Id_regime = regime[catch_row[5]].Trim('"'),
                            Permit = int.Parse(catch_row[6]),
                            Id_own = int.Parse(catch_row[7])
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
                        string[] row = reader.ReadLine().Split(',');
                        var current_product = new Product();
                        current_product.Id_ves = int.Parse(row[0]);
                        current_product.Date = DateTime.Parse(row[1]);
                        current_product.Id_prod_designate = prod_designate[row[2]].Trim('"');

                        if (prod_type.ContainsKey(row[3])) current_product.Prod_type = prod_type[row[3]].Trim('"');
                        current_product.Prod_volume = decimal.Parse(row[4].Replace('.', ','));
                        current_product.Prod_board_volume = decimal.Parse(row[5].Replace('.', ','));

                        product_report.Add(current_product);
                    }
                }

                ProductData = product_report;

                
                List<Ext> ext_report = new List<Ext>();

                using(StreamReader reader = new StreamReader(Db2Path + @"\Ext.csv"))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string[] row = reader.ReadLine().Split(',');
                        var current_product = new Ext();
                        current_product.Id_fishery = Convert.ToInt32(row[0]);
                        current_product.Id_own= Convert.ToInt32(row[1]);
                        current_product.Date_fishery = Convert.ToDateTime(row[2].Trim('"'));

                        current_product.Num_part = Convert.ToInt32(row[3]); //проверить здесь
                        current_product.Id_Plat = Convert.ToInt32(row[4]);
                        current_product.Id_vsd = Convert.ToInt32(row[5]);
                        current_product.Name_plat = row[6];
                        current_product.Product_period = Convert.ToDateTime(row[7]);
                        current_product.Region_plat = row[8];

                        ext_report.Add(current_product);
                    }
                }

                ExtData = ext_report;
            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
