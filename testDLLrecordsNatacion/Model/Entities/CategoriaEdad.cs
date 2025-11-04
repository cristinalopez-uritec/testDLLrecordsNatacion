using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class CategoriaEdad : EntidadBD
    {
        public int IdCategoriaEdad;
        public string NombreCategoria { get; set; }
        public string Genero { get; set; } = null;
        public int EdadMinima { get; set; } = -1;
        public int EdadMaxima { get; set; } = -1;
    }
}
