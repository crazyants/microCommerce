using microCommerce.MongoDb;

namespace microCommerce.Setting
{
    public class Setting : MongoEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}