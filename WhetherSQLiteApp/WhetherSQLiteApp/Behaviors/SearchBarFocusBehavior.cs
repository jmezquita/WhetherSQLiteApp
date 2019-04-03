using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WhetherSQLiteApp.Behaviors
{
    public class SearchBarFocusBehavior : Prism.Behaviors.BehaviorBase<SearchBar>
    {

        #region Variables
        SearchBar _searchBar;
        #endregion

        #region Properties
        public static readonly BindableProperty OnFocusProperty =
         BindableProperty.Create(nameof(OnFocus), typeof(bool?), typeof(SearchBarFocusBehavior), defaultValue: false,
             propertyChanged: OnFocusPropertyChanged);

        /// <summary>
        /// Prpiedad que indica el Focus en el objeto
        /// </summary>
        public bool? OnFocus
        {
            get => (bool?)GetValue(OnFocusProperty);
            set => SetValue(OnFocusProperty, value);
        }
        #endregion

        #region Functions and Void
        /// <summary>
        /// Metodo que actualiza el Focus
        /// </summary>
        private void UpdateFocused()
        {
            if (OnFocus == true)
                _searchBar.Focus();
            else
                _searchBar.Unfocus();
        }
        #endregion

        #region Override y Events
        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);
            _searchBar = bindable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        protected override void OnDetachingFrom(SearchBar bindable)
        {
            base.OnDetachingFrom(bindable);
            _searchBar = null;
        }

        /// <summary>
        /// Evento que indica cuando cambia la propiedad Focus
        /// </summary>
        /// <param name="sender">Objeto BindableObject</param>
        /// <param name="oldValue">Old Value</param>
        /// <param name="newValue">New value</param>
        private static void OnFocusPropertyChanged(BindableObject sender, object oldValue, object newValue)
        {
            ((SearchBarFocusBehavior)sender).UpdateFocused();
        }

        #endregion

    }
}
