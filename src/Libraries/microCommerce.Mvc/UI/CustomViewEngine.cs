using Microsoft.AspNetCore.Mvc.Razor;

namespace microCommerce.Mvc.UI
{
    public abstract class CustomRazorPage<TModel> : RazorPage<TModel>
    {
        private Localizer _localizer;

        public virtual Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    _localizer = (format, args) =>
                    {
                        var resFormat = string.Empty; //TODO localization
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