using System;
using System.Reflection;

namespace Dal
{
    internal static class Cloning
    {
        //third way - With Bonus // generic shallow copy, properties only

        internal static T Clone<T>(this T original) where T : new()
        {
            //T copyToObject = new T();
            var copyToObject = (T)Activator.CreateInstance(typeof(T));

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                propertyInfo.SetValue(copyToObject, propertyInfo.GetValue(original, null), null);
            }
            return copyToObject;
        }
    }
}
