using System;
using System.Runtime.Serialization;

namespace microCommerce.Domain
{
    /// <summary>
    /// Base class with generic type identifier
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class BaseEntityTypeId<TId> : ValidatableObject
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual TId Id { get; set; }
    }
}