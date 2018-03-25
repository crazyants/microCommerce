namespace microCommerce.Domain
{
    public abstract class BaseEntityTypeId<TId> : ValidatableObject
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual TId Id { get; set; }
    }
}