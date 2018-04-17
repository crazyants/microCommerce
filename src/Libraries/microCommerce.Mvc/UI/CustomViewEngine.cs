using microCommerce.Ioc;
using microCommerce.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace microCommerce.Mvc.UI
{
    public abstract class CustomRazorPage<TModel> : RazorPage<TModel>
    {
        private Localizer _localizer;
        private ILocalizationService _localizationService;

        public virtual Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    if (_localizationService == null)
                        _localizationService = EngineContext.Current.Resolve<ILocalizationService>();

                    _localizer = (format, args) =>
                    {
                        string resFormat = _localizationService.GetResourceValue(format).Result;
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }

                        return new LocalizedString((args == null || args.Length == 0)
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }

                return _localizer;
            }
        }
    }

    public abstract class CustomRazorPage : CustomRazorPage<dynamic>
    {
    }
}