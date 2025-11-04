using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class EntidadBD
    {
        public PropertyInfo[] ObtenerPropiedades()
        {
            return this.GetType().GetProperties();
        }

        public override string ToString()
        {
            string str = this.GetType().ToString();
            PropertyInfo[] propiedades = this.ObtenerPropiedades();
            foreach (PropertyInfo propiedad in propiedades)
            {
                string nombrePropiedad = propiedad.Name;
                string tipoPropiedad = propiedad.PropertyType.Name;
                object valorPropiedad = propiedad.GetValue(this);
                string valorFormateado = valorPropiedad.ToString();

                str += $"\n\t{nombrePropiedad} [{tipoPropiedad}]: {valorPropiedad}"; 
            }
            return str;
        }

        /// <summary>
        /// Describe todas las propiedades de la entidad en el diccionario 
        /// con el fomato por defecto del .ToString() de su tipo de dato.
        /// </summary>
        /// <returns>Diccionario con las propiedades de la entidad descritas</returns>
        public Dictionary<string, string> DescribePropertiesStr()
        {
            Dictionary<string, string> atributos = new Dictionary<string, string>();

            PropertyInfo[] propiedades = this.ObtenerPropiedades();
            foreach (PropertyInfo propiedad in propiedades)
            {
                string nombrePropiedad = propiedad.Name;
                object valorPropiedad = propiedad.GetValue(this) ?? "NULL";

                atributos.Add(nombrePropiedad, valorPropiedad.ToString());
            }

            return atributos;
        }

    }
}
