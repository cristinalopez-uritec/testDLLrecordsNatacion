using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class Event : DbEntity
    {
        public int Id;
        public string MeetName { get; set; }
        public DateTime MeetDate { get; set; }
        public string Nation { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public int PoolLength { get; set; }
        public int SessionNum { get; set; }
        public string SessionName { get; set; }
        public string GenderCategory { get; set; }
        public string EventRound { get; set; }
        public string EventCourse { get; set; }
        //public DateTime EventStartTime { get; set; }
        public int SwimDistance { get; set; }
        public string SwimStroke { get; set; }
        public int SwimRelayCount { get; set; }
        //public string Handicap { get; set; }

        /// <summary>
        /// Describes all of the Event's properties in a Dictionary with the desired output format.
        /// The key is the name of the property, the value is the value of the property 
        /// in a string formatted to how it has to be displayed in the frontend.
        /// </summary>
        /// <returns>Dictionary with the Event's properties described</returns>
        public Dictionary<string, string> DescribePropertiesFormattedStr()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            PropertyInfo[] properties = this.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                string propertyType = property.PropertyType.Name;
                object propertyValue = property.GetValue(this);
                string formattedValue = propertyValue != null ? propertyValue.ToString() : null;

                //TODO: change formatting and dysplay options depending on datatype

                attributes.Add(propertyName, formattedValue);
            }

            return attributes;
        }
    }
}
