using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WhetherSQLiteApp.Models;
using WhetherSQLiteApp.Services;

namespace WhetherSQLiteApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Variables
        private readonly INavigationService _navigationService;
        private readonly IDataService _sourcesService;
        private ObservableCollection<WeatherDataSource> _weatherDataSourceList;
        private ObservableCollection<RecentItems> _recentItemsList;
        private bool _isVisibleSearchBar = false;
        private RecentItems _itemSelected;
        private readonly Realm _realm;
        //private ObservableCollection<Language> LanguageList;
        //private ObservableCollection<WeatherDataSource> _OriginalWeatherDataSourcetList;
        private string _searchBarValue;
        #endregion
        public MainPageViewModel(INavigationService navigationService, IDataService sourcesService, Prism.Events.IEventAggregator e) :
            base(navigationService)
        {
  
            _sourcesService = sourcesService;
            _navigationService = navigationService;
            Title = "Holaaaa";
            _weatherDataSourceList = new ObservableCollection<WeatherDataSource>();
            //_OriginalWeatherDataSourcet = new ObservableCollection<WeatherDataSource>();
            //_realm = realm;
            _recentItemsList = new ObservableCollection<RecentItems>();

            //LanguageList = new ObservableCollection<Language>();
            //SearchButtonCommand = new DelegateCommand(ExecuteClick, CanExecuteClick);
            ExecuteCommand = new DelegateCommand(Execute);
            GetWeatherDataSourceFromAPI();
        }


        #region Properties
        /// <summary>
        /// Lista de Fuente de datos
        /// </summary>
        public ObservableCollection<WeatherDataSource> WeatherDataSourceList
        {
            get { return _weatherDataSourceList; }
            set { SetProperty(ref _weatherDataSourceList, value); }
        }

        /// <summary>
        /// Lista de las busquedas recientes
        /// </summary>
        public ObservableCollection<RecentItems> RecentItemsList
        {
            get { return _recentItemsList; }
            set { SetProperty(ref _recentItemsList, value); }
        }

        /// <summary>
        /// Propiedad que ejecutara la busqueda
        /// </summary>
        public string SearchBarValue
        {
            get { return _searchBarValue; }
            set
            {
                SetProperty(ref _searchBarValue, value);
                //CountryList = new ObservableCollection<Country>(from c in _OriginalCountryList
                //                                                where string.IsNullOrEmpty(_searchBarValue) ||
                //                                                c.Name.IndexOf(_searchBarValue, StringComparison.OrdinalIgnoreCase) > -1
                //                                                select c);

                GetWeatherDataSourceFromAPI(_searchBarValue);
            }
        }

        /// <summary>
        /// especifica si el objeto de busca sera visible
        /// </summary>
        public bool IsVisibleSearchBar
        {
            get { return _isVisibleSearchBar; }
            set
            {

                SetProperty(ref _isVisibleSearchBar, value);
                if (!value)
                {
                    SearchBarValue = "";

                    //BeersList = new ObservableCollection<Beers>(from beer in _OriginalbeersList
                    //                                            select beer);
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


        #endregion

        /// <summary>
        /// Funcion que ejecutara la Busqueda directamente desde la api
        /// </summary>
        /// <param name="TextToSearch">Parametro Opcional si no se envia nada automaticamente tomara la ciudad de tu localizacion</param>
        async void GetWeatherDataSourceFromAPI(String TextToSearch = "Santo Domingo")
        {
            try
            {

                IsRunning = true;
                IsVisibleWaitIndicator = true;
                TextInformation = "Loading Dada...";
                IsVisibleWaitAbsoluteLayout = true;

                var result = await _sourcesService.GetWeatherDataSourceAsync(TextToSearch);
                IsRunning = false;
                //_weatherDataSourceList = new ObservableCollection<WeatherDataSource>(result);
                //CountryList = _OriginalCountryList;

                IsRunning = false;
                if (_weatherDataSourceList.Count > 0)
                {
                    IsVisibleWaitAbsoluteLayout = false;
                    TextInformation = "";

                }
                else
                {
                    IsVisibleWaitAbsoluteLayout = true;
                    IsVisibleWaitIndicator = false;
                    TextInformation = "There is no data to show";
                }
            }
            catch (Exception)
            {
                IsVisibleWaitAbsoluteLayout = true;
                IsRunning = false;
                IsVisibleWaitIndicator = false;
                TextInformation = "Error getting the data";
            }
             ((DelegateCommand)SearchButtonCommand).RaiseCanExecuteChanged();

        }

        public ICommand SearchButtonCommand
        {
            get;
        }

        public ICommand ExecuteCommand
        {
            get;
        }

        private void ExecuteClick()
        {
            IsVisibleSearchBar = !IsVisibleSearchBar;
          

        }

        //private bool CanExecuteClick()
        //{
        //    return IsNotRunning;
        //}
        async void Execute()
        {


            var navigationParams = new NavigationParameters
            {
                { "model", _itemSelected }
            };

            //StaticObject.MasterSelected = eMasterSelected.Country;
            await _navigationService.NavigateAsync("SourcesContentPage", navigationParams);
        }

    }
}
