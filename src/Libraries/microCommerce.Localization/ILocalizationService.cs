using System.Collections.Generic;
using System.Threading.Tasks;

namespace microCommerce.Localization
{
    public interface ILocalizationService
    {
        Task InsertLocalizationResource(string name, string value, string languageCultureCode);
        Task UpdateLocalizationResource(string name, string value, string languageCultureCode);
        Task DeleteLocalizationResource(string name, string languageCultureCode);
        Task<LocalizationResource> GetLocalizationResourceByName(string name, string languageCultureCode);
        Task<IList<LocalizationResource>> GetAllResources(string languageCultureCode);
        Task<string> GetResourceValue(string name, string languageCultureCode, string defaultValue = null, bool setEmptyIfNotFound = false);
    }
}