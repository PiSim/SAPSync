using System;
using System.Linq;
using System.Reflection;

namespace SAPSync.SyncElements
{
    public interface IXmlDto<T> where T :class
    {
        void SetValues(T entity);
    }

    public class DtoBase<T> : IXmlDto<T> where T : class
    {
        public virtual void SetValues(T entity)
        {
            PropertyInfo[] tProperties = typeof(T).GetProperties();
            PropertyInfo[] dtoProperties = GetType().GetProperties();

            foreach (PropertyInfo dtoproperty in dtoProperties)
                if (tProperties.Any(pro => pro.Name == dtoproperty.Name && pro.PropertyType == dtoproperty.PropertyType))
                    dtoproperty.SetValue(this, tProperties.First(p => p.Name == dtoproperty.Name).GetValue(entity));
        }
    }

    public class DtoProperty
    {
        #region Properties

        public int ColumnIndex { get; set; }
        public PropertyInfo Property { get; set; }

        #endregion Properties
    }

}