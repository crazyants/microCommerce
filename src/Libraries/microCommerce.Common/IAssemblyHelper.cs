using System;
using System.Collections.Generic;
using System.Reflection;

namespace microCommerce.Common
{
    public interface IAssemblyHelper
    {
        IEnumerable<Type> FindOfType<T>(bool onlyConcreteClasses = true);
        IEnumerable<Type> FindOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);
        IEnumerable<Type> FindOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
        IEnumerable<Type> FindOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
        IList<Assembly> GetAssemblies();
    }
}