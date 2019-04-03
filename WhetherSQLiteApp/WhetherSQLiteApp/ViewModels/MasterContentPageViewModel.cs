using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WhetherSQLiteApp.Models;
using WhetherSQLiteApp.Services;

namespace WhetherSQLiteApp.ViewModels
{
    public class MasterContentPageViewModel : ViewModelBase
    {


        #region Variables
        private readonly INavigationService _navigationService;
        private WeatherDataSource _weatherDataSourceList;
        private ObservableCollection<RecentItems> _recentItemsList;
        private ObservableCollection<RecentItems> _OriginalrecentItemsList;
        private readonly IDataService _sourcesService;
        private readonly Realm _realm;
        private bool _isVisibleSearchBar = false;
        private RecentItems _itemSelected;
        private bool _isRefreshing = false;
        private bool _isVisibleWhether =false;
        private bool _isVisibleRecentList = false;
        #endregion

        private string _searchBarValue;


        public MasterContentPageViewModel(INavigationService navigationService, Realm realm, IDataService sourcesService, Prism.Events.IEventAggregator e) :
            base(navigationService)
        {
            Title = "Consulta el clima";
            _navigationService = navigationService;
            _weatherDataSourceList = new WeatherDataSource();
            _recentItemsList = new ObservableCollection<RecentItems>();
            _OriginalrecentItemsList = new ObservableCollection<RecentItems>();
            _sourcesService = sourcesService;
            _realm = realm;

            SearchButtonCommand = new DelegateCommand(ExecuteClick, CanExecuteClick);
            ExecuteItemTapCommand = new DelegateCommand(ExecuteItemTap);

            CmdExecuteDeleteItemRecentList = new DelegateCommand<RecentItems>(DeleteItemRecentList);


            SearchCommand = new  DelegateCommand(ExecuteSearchCommand);
            cmdRefreshCommand = new DelegateCommand(ExecuteRefreshCommand);

            cmdRefreshWhetherCommand = new DelegateCommand(ExecuteRefreshWhetherCommand);

            DeleteRecentItems();
            LoadRecentItemFromLocalDatabase();



        RecentItems itemfirstSelect = new RecentItems();
            itemfirstSelect = RecentItemsList.FirstOrDefault();
            if (itemfirstSelect!=null)
            GetWeatherDataSourceFromAPI(itemfirstSelect.Name);


        }


        #region Properties
        /// <summary>
        /// Lista de Fuente de datos
        /// </summary>
        public WeatherDataSource WeatherDataSourceList
        {
            get =>_weatherDataSourceList; 
            set { SetProperty(ref _weatherDataSourceList, value); }
        }

        /// <summary>
        /// Lista de las busquedas recientes
        /// </summary>
        public ObservableCollection<RecentItems> RecentItemsList
        {
            get => _recentItemsList;
            set { SetProperty(ref _recentItemsList, value); }
        }

        /// <summary>
        /// Propiedad que ejecutara la busqueda
        /// </summary>
        public string SearchBarValue
        {
            get => _searchBarValue;
            set
            {
                SetProperty(ref _searchBarValue, value);

                //RecentItemsList = new ObservableCollection<RecentItems>(from c in _OriginalrecentItemsList
                //                                                        where string.IsNullOrEmpty(_searchBarValue.Trim().ToString()) ||
                //                                                c.Name.IndexOf(_searchBarValue.Trim().ToString(), StringComparison.OrdinalIgnoreCase) > -1
                //                                                select c);




                //GetWeatherDataSourceFromAPI(_searchBarValue);
            }
        }


        /// <summary>
        /// propiedad que indica cuando sera visible la lista de lista reciente
        /// </summary>
        public bool IsVisibleRecentList
        {
            get => _isVisibleRecentList;
            set
            {
                SetProperty(ref _isVisibleRecentList, value);
            }
        }

        /// <summary>
        /// propiedad que indica cuando sera visible la seccion del clima
        /// </summary>
        public bool IsVisibleWhether
        {
            get => _isVisibleWhether;

            set
            {
                SetProperty(ref _isVisibleWhether, value);
            }
    }


        /// <summary>
        /// especifica si el objeto de busca sera visible
        /// </summary>
        public bool IsVisibleSearchBar
        {
            get=>_isVisibleSearchBar; 
            set
            {
                
                SetProperty(ref _isVisibleSearchBar, value);
                if (!value)
                {
                    SearchBarValue = "";
                }

            }
        }

        /// <summary>
        /// Iten seleccionado de la lista de busqueda recientes
        /// </summary>
        public RecentItems ItemSelected
        {
            get => _itemSelected;
            set => SetProperty(ref _itemSelected, value);
        }

 
        public bool IsRefreshing
        {
            get=>_isRefreshing; 
            set
            {

                SetProperty(ref _isRefreshing, value);
            }
        }



            #endregion
            /// <summary>
            /// Funcion que ejecutara la Busqueda directamente desde la api
            /// </summary>
            /// <param name="TextToSearch">Parametro Opcional si no se envia nada automaticamente tomara la ciudad de tu localizacion</param>
            async void GetWeatherDataSourceFromAPI(string SeachText)
        {
            try
            {

                if (String.IsNullOrEmpty(SeachText))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Debes Digitar la ciudad para buscar el clima", "OK");
                    return;
                }
                IsVisibleWhether = false;
                IsRunning = true;
                IsVisibleWaitIndicator = true;
                TextInformation = "Loading Dada...";
                IsVisibleWaitAbsoluteLayout = true;

                WeatherDataSourceList = await _sourcesService.GetWeatherDataSourceAsync(SeachText);

                IsRunning = false;
                if (WeatherDataSourceList.Location != null && WeatherDataSourceList.Current!=null)
                {
                    IsVisibleWaitAbsoluteLayout = false;
                    TextInformation = "";
                    AddRecent(SeachText, WeatherDataSourceList.Current.Condition.IconUrl);

                    //Title = SeachText;
                    IsVisibleSearchBar = false;
                    IsVisibleWhether = true;
                }
                else
                {
                    IsVisibleWhether = false;
                    //Title = "Consulta el clima";
                    IsVisibleWaitAbsoluteLayout = true;
                    IsVisibleWaitIndicator = false;
                    TextInformation = "There is no data to show";
                }
            }
            catch (Exception)
            {
                //Title = "Consulta el clima";
                IsVisibleWaitAbsoluteLayout = true;
                IsRunning = false;
                IsVisibleWaitIndicator = false;
                TextInformation = "Error getting the data";
            }
             ((DelegateCommand)SearchButtonCommand).RaiseCanExecuteChanged();

        }
        public ICommand SearchCommand
        {
            get;
        }

        private void ExecuteSearchCommand()
        {
            GetWeatherDataSourceFromAPI(SearchBarValue);
        }

        public ICommand cmdRefreshCommand
        {
            get;
        }
        public ICommand cmdRefreshWhetherCommand
        {
            get;
        }
        

        async void ExecuteRefreshCommand()
        {

            IsRefreshing = true;
            await LoadRecentItemFromLocalDatabase();
            IsRefreshing = false;

        }
        async void ExecuteRefreshWhetherCommand()
        {

            RecentItems itemfirstSelect = new RecentItems();
            itemfirstSelect = RecentItemsList.FirstOrDefault();
            if (itemfirstSelect != null)
                GetWeatherDataSourceFromAPI(itemfirstSelect.Name);
    
        }

        public ICommand SearchButtonCommand
        {
            get;
        }

        public ICommand ExecuteItemTapCommand
        {
            get;
        }
        public ICommand CmdExecuteDeleteItemRecentList
        {
            get;
        }

        private void ExecuteClick()
        {
            IsVisibleSearchBar = !IsVisibleSearchBar;
        }
        private bool CanExecuteClick()
        {
            return IsNotRunning;
        }
        async void ExecuteItemTap()
        {
            var navigationParams = new NavigationParameters
            {
                { "model", _itemSelected }
            };

            GetWeatherDataSourceFromAPI(_itemSelected.Name);
        }


     

        async void DeleteItemRecentList(RecentItems t)
        {
            if (t != null)
            {
                DeleteRecentItems(t);
            }

        }



        private void AddRecent(string SeachText, string pIconUrl)
        {
            try
            {

                var Reg = _realm.All<RecentItems>().Where(d => d.Name== SeachText.Trim().ToLower()).FirstOrDefault();
                if (Reg == null)
                {
                  
                    _realm.Write(() =>
                    {
                        _realm.Add(new RecentItems
                        {
                        Name = SeachText.Trim().ToLower(),
                        OpenedDate = DateTime.Now.ToString(),
                        IconUrl = pIconUrl
                    });

                    });

                }
                else
                {
                        using (var db = _realm.BeginWrite())
                        {
                            Reg.OpenedDate = DateTime.Now.ToString();
                            Reg.IconUrl = pIconUrl;
                        db.Commit();
                        }
                }

                ExecuteRefreshCommand();


            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

        }

        private bool DeleteRecentItems(RecentItems Item = null)
        {
            try
            {

                if (Item != null)
                {
                    using (var trans = _realm.BeginWrite())
                    {
                        _realm.Remove(Item);
                        trans.Commit();
                    }


                    RecentItemsList.Remove(Item);
                    RecentItemsList.OrderByDescending(O => O.OpenedDate);

                }
                else
                {
                    var allItems = _realm.All<RecentItems>().Where(b => Convert.ToDateTime(b.OpenedDate) < DateTime.Now.AddDays(-10));

                    if (allItems != null && allItems.Count() > 0)
                    {
                        foreach (var item in allItems)
                        {
                            using (var trans = _realm.BeginWrite())
                            {
                                _realm.Remove(item);
                                trans.Commit();
                            }
                        }
                    }
                }

                ExecuteRefreshCommand();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return false;
            }
        }
        


        async Task LoadRecentItemFromLocalDatabase()
        {
            try
            {
                var result = _realm.All<RecentItems>().OrderByDescending(O => O.OpenedDate);
                RecentItemsList = new ObservableCollection<RecentItems>(result);
                if (RecentItemsList != null && RecentItemsList.Count>0)
                {
                    IsVisibleRecentList = true;
                }
                else
                {
                    IsVisibleRecentList = false;
                }

            }
         catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }


    }
}
