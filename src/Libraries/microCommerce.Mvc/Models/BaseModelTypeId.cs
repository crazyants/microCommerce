namespace microCommerce.Mvc.Models
{
    public class BaseModelTypeId<TId> : BaseModel
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual TId Id { get; set; }
    }
}