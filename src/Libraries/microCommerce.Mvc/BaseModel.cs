namespace microCommerce.Mvc
{
    public abstract class BaseModel
    {
    }

    public abstract class BaseEntityModel : BaseModel
    {
        public virtual int Id { get; set; }
    }
}