using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class Competicion : EntidadBD
    {
        public int IdCompeticion;
        public string NombreCompeticion { get; set; }
        public DateTime FechaCompeticion { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public int LongitudPiscina { get; set; } = 50;
        public int NumSesion { get; set; } = 0;
        public string NombreSesion { get; set; }
        public string CategoriaGenero { get; set; }
        public string RondaEvento { get; set; }
        public int CantidadRelevosNado { get; set; } = 0;
        //public string Handicap { get; set; }

        /// <summary>
        /// Describe todas las propiedades del objeto con el formato de salida deseado en un diccionario.
        /// La clave es el nombre de la propiedad, el valor es el valor de la propiedad recogido en 
        /// un string formateado tal y como se quiere mostrar el dato en el frontend.
        /// </summary>
        /// <returns>Un Diccionario con las propiedades del Atleta descritas y formateadas</returns>
        public Dictionary<string, string> DescribirPropiedadesFormateadasStr()
        {
            Dictionary<string, string> propiedades = new Dictionary<string, string>();

            PropertyInfo[] properties = this.ObtenerPropiedades();
            foreach (PropertyInfo propiedad in properties)
            {
                string nombrePropiedad = propiedad.Name;
                string tipoPropiedad = propiedad.PropertyType.Name;
                object valorPropiedad = propiedad.GetValue(this);
                string valorFormateado = valorPropiedad.ToString();

                //TODO: change formatting and dysplay options depending on datatype

                propiedades.Add(nombrePropiedad, valorFormateado);
            }

            return propiedades;
        }
    }
}
