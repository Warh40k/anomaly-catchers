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

            #endregion

            var data_points = new List<DataPoint>((int)(360 / 0.1));

            for(var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }

            TestDataPoints = data_points;

            List<Catch> catch_report = new List<Catch>();

            using(StreamReader reader = new StreamReader(@"C:\Users\user\Desktop\Rosrybolovstvo\Датасет\db1\catch.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] catch_row = reader.ReadLine().Split(',');
                    catch_report.Add(new Catch
                    {
                        Id_ves = Convert.ToInt32(catch_row[0]),
                        Date = Convert.ToDateTime(catch_row[1]),
                        Id_region = Convert.ToInt32(catch_row[2]),
                        Id_fish = Convert.ToInt32(catch_row[3]),
                        Catch_volume = Convert.ToDecimal(catch_row[4].Replace('.',',')),
                        Id_regime = Convert.ToInt32(catch_row[5]),
                        Permit = Convert.ToInt32(catch_row[6]),
                        Id_own = Convert.ToInt32(catch_row[7])
                    });
                }
            }

            CatchData = catch_report;


            List<Product> product_report = new List<Product>();

            using(StreamReader reader = new StreamReader(@"C:\Users\user\Desktop\Rosrybolovstvo\Датасет\db1\product.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] row = reader.ReadLine().Split(',');
                    product_report.Add(new Product
                    {
                        Id_ves = Convert.ToInt32(row[0]),
                        Date = Convert.ToDateTime(row[1]),
                        Id_prod_designate = Convert.ToInt32(row[2]),
                        Prod_type = Convert.ToInt32(row[3]),
                        Prod_volume = Convert.ToDecimal(row[4].Replace('.',',')),
                        Prod_board_volume = Convert.ToDecimal(row[5].Replace('.',',')),
                    });
                }
            }
            
            
            ProductData = product_report;
           
            


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
        private List<Reference> GetReference(string path)
        {
            List<Reference> reference = new List<Reference>();
            using(StreamReader reader = new StreamReader(path))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string[] row = reader.ReadLine().Split(',');
                    reference.Add(new Reference
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = row[1]
                    });
                }
            }
            return reference;
        }
    }
}
