using System;

namespace microCommerce.Domain
{
    /// <summary>
    /// Column ignore attribute for database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnIgnoreAttribute : Attribute
    {
    }
}