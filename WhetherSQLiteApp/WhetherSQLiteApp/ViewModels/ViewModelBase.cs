using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhetherSQLiteApp.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
   
        #region Variables
        private string _title;
        private bool isRunning = false;
        private bool isNotRunning = true;
        private bool _isVisibleWaitIndicator;
        private bool _isNoVisibleWaitAbsoluteLayout;

        private bool _isVisibleWaitAbsoluteLayout;
        private string _textInformation;
        #endregion

        #region Properties
        /// <summary>
        /// Propiedad privada Contiene el servicio de Navegacion
        /// </summary>
        protected INavigationService NavigationService { get; private set; }


        /// <summary>
        /// Esta Propiedad Indica el titulo de la Vista
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// Esta Propiedad Indica si la vista esta ejecutando algun proceso
        /// </summary>
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                SetProperty(ref isRunning, value);
                SetProperty(ref isNotRunning, !value, nameof(IsNotRunning));

            }
        }

        /// <summary>
        /// Esta Propiedad Indica si la vista ya termino con el proceso
        /// /// </summary>
        public bool IsNotRunning
        {
            get => isNotRunning;
        }

        /// <summary>
        /// Especifica si sera visible o no el indicador de espera para algun proceso
        /// </summary>
        public bool IsVisibleWaitIndicator
        {
            get => _isVisibleWaitIndicator;
            set { SetProperty(ref _isVisibleWaitIndicator, value); }
        }

        /// <summary>
        /// Propiedad que especifica el texto a mostrar mientra se espera un proceso
        /// </summary>
        public string TextInformation
        {
            get => _textInformation;
            set { SetProperty(ref _textInformation, value); }
        }

        public bool IsVisibleWaitAbsoluteLayout
        {
            get => _isVisibleWaitAbsoluteLayout;
            set
            {

                SetProperty(ref _isVisibleWaitAbsoluteLayout, value);
                SetProperty(ref _isNoVisibleWaitAbsoluteLayout, !_isVisibleWaitAbsoluteLayout, nameof(IsNoVisibleWaitAbsoluteLayout));
            }
        }

        public bool IsNoVisibleWaitAbsoluteLayout
        {
            get;
        }

        #endregion


        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
