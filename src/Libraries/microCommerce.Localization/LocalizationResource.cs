using microCommerce.MongoDb;

namespace microCommerce.Localization
{
    public class LocalizationResource : MongoEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string LanguageCultureCode { get; set; }
    }
}