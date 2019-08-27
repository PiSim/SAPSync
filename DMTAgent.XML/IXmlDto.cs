using System.Linq;
using System.Reflection;

namespace DMTAgent.XML
{ 
    public interface IXmlDto<T> where T : class
    {
        #region Methods

        void SetValues(T entity);

        #endregion Methods
    }

    public class DtoBase<T> : IXmlDto<T> where T : class
    {
        #region Methods

        public virtual void SetValues(T entity)
        {
            PropertyInfo[] tProperties = typeof(T).GetProperties();
            PropertyInfo[] dtoProperties = GetType().GetProperties();

            foreach (PropertyInfo dtoproperty in dtoProperties)
                if (tProperties.Any(pro => pro.Name == dtoproperty.Name && pro.PropertyType == dtoproperty.PropertyType))
                    dtoproperty.SetValue(this, tProperties.First(p => p.Name == dtoproperty.Name).GetValue(entity));
        }

        #endregion Methods
    }

    public class DtoProperty
    {
        #region Properties

        public int ColumnIndex { get; set; }
        public PropertyInfo Property { get; set; }

        #endregion Properties
    }
}