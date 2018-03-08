using System;

namespace microCommerce.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnIgnoreAttribute : Attribute
    {
    }
}