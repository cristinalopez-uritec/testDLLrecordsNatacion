using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class DbEntity
    {
        public PropertyInfo[] GetProperties()
        {
            return this.GetType().GetProperties();
        }

        public override string ToString()
        {
            string str = this.GetType().ToString();
            PropertyInfo[] properties = this.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                string propertyType = property.PropertyType.Name;
                object propertyValue = property.GetValue(this);
                string formattedValue = propertyValue.ToString();

                str += $"\n\t{propertyName} [{propertyType}]: {propertyValue}"; 
            }
            return str;
        }

        /// <summary>
        /// Describes all of the Entity's properties in a Dictionary with the 
        /// default string format for the datatypes of its values.
        /// </summary>
        /// <returns>Dictionary with the Entity's properties described</returns>
        public Dictionary<string, string> DescribePropertiesStr()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            PropertyInfo[] properties = this.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(this) != null ? property.GetValue(this) : "NULL";

                attributes.Add(propertyName, propertyValue.ToString());
            }

            return attributes;
        }

    }
}
