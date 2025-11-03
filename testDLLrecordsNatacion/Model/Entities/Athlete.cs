using System;
using System.Collections.Generic;
using System.Reflection;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class Athlete: DbEntity
    {
        public int Id; //this property is not included in the .GetProperties()
        public string FullName { get; set; }
        public string Birthdate { get; set; } = "NO INFO";
        public string Gender { get; set; } = "X";
        public string Nation { get; set; } = null;
        public string License { get; set; } = null;
        public int ClubCode { get; set; } = -1;
        public string ClubName { get; set; } = null;
        public string ClubShortName { get; set; } = null;

        //public int IsHandicap { get; set; }

        /// <summary>
        /// Describes all of the Athlete's properties in a Dictionary with the desired output format.
        /// The key is the name of the property, the value is the value of the property 
        /// in a string formatted to how it has to be displayed in the frontend.
        /// </summary>
        /// <returns>Dictionary with the Athlete's properties described</returns>
        public Dictionary<string, string> DescribePropertiesFormattedStr() 
        { 
            Dictionary<string,string> attributes = new Dictionary<string,string>();

            PropertyInfo[] properties = this.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                string propertyType = property.PropertyType.Name;
                object propertyValue = property.GetValue(this);
                string formattedValue = propertyValue.ToString();

                //TODO: change formatting and dysplay options depending on datatype

                attributes.Add(propertyName, formattedValue);
            }

            return attributes;
        }

     
    }
}
