using System;
using System.Collections.Generic;
using System.Reflection;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class Atleta: EntidadBD
    {
        public int IdAtleta; //this property is not included in the .GetProperties()
        public string NombreCompleto { get; set; }
        public DateTime? FechaNacimiento { get; set; } = null;
        public string Genero { get; set; } = null;
        public string Pais { get; set; } = null;
        public string Licencia { get; set; } = null;
        public int? CodigoClub { get; set; } = null;
        public string NombreCompletoClub { get; set; } = null;
        public string NombreCortoClub { get; set; } = null;

        //public int IsHandicap { get; set; }

        /// <summary>
        /// Describe todas las propiedades del objeto con el formato de salida deseado en un diccionario.
        /// La clave es el nombre de la propiedad, el valor es el valor de la propiedad recogido en 
        /// un string formateado tal y como se quiere mostrar el dato en el frontend.
        /// </summary>
        /// <returns>Un Diccionario con las propiedades del Atleta descritas y formateadas</returns>
        public Dictionary<string, string> DescribirPropiedadesFormateadasStr() 
        { 
            Dictionary<string,string> propiedades = new Dictionary<string,string>();

            PropertyInfo[] properties = this.ObtenerPropiedades();
            foreach (PropertyInfo propiedad in properties)
            {
                string nombrePropiedad = propiedad.Name;
                string tipoPropiedad = propiedad.PropertyType.Name;
                object valorPropiedad = propiedad.GetValue(this);
                string valorFormateado = valorPropiedad != null ? valorPropiedad.ToString() : null;

                //TODO: change formatting and dysplay options depending on datatype

                propiedades.Add(nombrePropiedad, valorFormateado);
            }

            return propiedades;
        }

     
    }
}
