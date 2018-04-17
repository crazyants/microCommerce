using microCommerce.Common;
using microCommerce.Domain.Customers;
using microCommerce.Domain.Globalization;
using microCommerce.Mvc.Themes;
using microCommerce.Setting;
using System;

namespace microCommerce.Mvc
{
    public class WebWorkContext : IWorkContext
    {
        #region Fields
        private ThemeInfo _cachedTheme;
        private Customer _cachedCustomer;
        private Language _cachedLanguage;
        private Currency _cachedCurrency;
        private Country _cachedCountry;

        private readonly IThemeProvider _themeProvider;
        private readonly StoreSettings _storeSettings;
        #endregion

        public WebWorkContext(IThemeProvider themeProvider,
            StoreSettings storeSettings)
        {
            _storeSettings = storeSettings;
            _themeProvider = themeProvider;
        }

        /// <summary>
        /// Gets the current store theme
        /// </summary>
        public virtual ThemeInfo CurrentTheme
        {
            get
            {
                if (_cachedTheme != null)
                    return _cachedTheme;

                string themeName = _storeSettings.DefaultTheme;
                if (!string.IsNullOrEmpty(themeName) && !_themeProvider.ThemeExists(themeName))
                    throw new Exception("Theme could not be found.");

                _cachedTheme = _themeProvider.LoadThemeInfo(themeName);

                return _cachedTheme;
            }
            set
            {
                _cachedTheme = null;
            }
        }

        /// <summary>
        /// Gets the current customer
        /// </summary>
        public virtual Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                _cachedCustomer = new Customer { FirstName = "Sefa", LastName = "Can", Email = "fsefacan@gmail.com", UserName = "sefacan" };

                return _cachedCustomer;
            }
            set
            {
                _cachedCustomer = null;
            }
        }

        /// <summary>
        /// Gets the current store language
        /// </summary>
        public virtual Language CurrentLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                _cachedLanguage = new Language { LanguageCulture = "en-US", UniqueSeoCode = "en" };

                return _cachedLanguage;
            }
            set
            {
                _cachedLanguage = null;
            }
        }

        /// <summary>
        /// Gets the current store currency
        /// </summary>
        public virtual Currency CurrentCurrency
        {
            get
            {
                if (_cachedCurrency != null)
                    return _cachedCurrency;

                _cachedCurrency = new Currency { CurrencyCode = "USD", Name = "Dolar" };

                return _cachedCurrency;
            }
            set
            {
                _cachedCurrency = null;
            }
        }

        /// <summary>
        /// Gets the current customer country
        /// </summary>
        public virtual Country CurrentCountry
        {
            get
            {
                if (_cachedCountry != null)
                    return _cachedCountry;

                _cachedCountry = new Country { TwoLetterIsoCode = "US", Name = "United States" };

                return _cachedCountry;
            }
            set
            {
                _cachedCountry = null;
            }
        }
    }
}