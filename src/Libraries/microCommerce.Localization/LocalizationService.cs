using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.MongoDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace microCommerce.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IMongoRepository<LocalizationResource> _localizationResourceRepository;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

        public LocalizationService(IMongoRepository<LocalizationResource> localizationResourceRepository,
            IWorkContext workContext,
        ICacheManager cacheManager)
        {
            _localizationResourceRepository = localizationResourceRepository;
            _workContext = workContext;
            _cacheManager = cacheManager;
        }

        public virtual async Task InsertLocalizationResource(string name, string value, string languageCultureCode)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(languageCultureCode))
                return;

            var localizationResource = new LocalizationResource
            {
                Name = name.Trim().ToLowerInvariant(),
                Value = value,
                LanguageCultureCode = languageCultureCode
            };

            await _localizationResourceRepository.InsertAsync(localizationResource);
        }

        public virtual async Task UpdateLocalizationResource(string name, string value, string languageCultureCode)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(languageCultureCode))
                return;

            var localizationResource = _localizationResourceRepository.Find(lr => lr.Name == name && lr.LanguageCultureCode == languageCultureCode);
            if (localizationResource != null)
            {
                localizationResource.Value = value;
                await _localizationResourceRepository.UpdateAsync(localizationResource);
            }
        }

        public virtual async Task DeleteLocalizationResource(string name, string languageCultureCode)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(languageCultureCode))
                return;

            var localizationResource = _localizationResourceRepository.Find(lr => lr.Name == name && lr.LanguageCultureCode == languageCultureCode);
            if (localizationResource != null)
                await _localizationResourceRepository.DeleteAsync(localizationResource);
        }

        public virtual async Task<LocalizationResource> GetLocalizationResourceByName(string name, string languageCultureCode)
        {
            return await _localizationResourceRepository.FindAsync(lr => lr.Name == name && lr.LanguageCultureCode == languageCultureCode);
        }

        public virtual async Task<IList<LocalizationResource>> GetAllResources(string languageCultureCode)
        {
            return await _localizationResourceRepository.GetAsync(lr => lr.LanguageCultureCode == languageCultureCode);
        }

        public virtual async Task<string> GetResourceValue(string name, string defaultValue = "", bool setEmptyIfNotFound = false)
        {
            return await GetResourceValue(name, _workContext.CurrentLanguage.LanguageCulture, defaultValue, setEmptyIfNotFound);
        }

        public virtual async Task<string> GetResourceValue(string name, string languageCultureCode, string defaultValue = "", bool setEmptyIfNotFound = false)
        {
            var value = string.Empty;
            if (name == null)
                name = string.Empty;

            name = name.Trim().ToLowerInvariant();
            string cacheKey = string.Format("localization.resource.{0}.{1}", name, languageCultureCode);
            var localizationResource = _cacheManager.Get(cacheKey, () =>
            {
                return _localizationResourceRepository.Find(lr => lr.Name.Equals(name) &&
                lr.LanguageCultureCode.Equals(languageCultureCode));
            });

            if (localizationResource != null)
                value = localizationResource.Value;

            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(defaultValue))
                    value = defaultValue;
                else
                {
                    if (!setEmptyIfNotFound)
                        value = name;
                }
            }

            return await Task.FromResult(value);
        }
    }
}