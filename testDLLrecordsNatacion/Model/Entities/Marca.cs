using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class Marca: EntidadBD
    {
        public int IdMarca;
        public DateTime FechaMarca { get; set; }
        public string TiempoNado { get; set; }
        public int Puntos { get; set; } = 0;
        public string Comentario { get; set; } = null;

        public string RecorridoNado { get; set; } = null;
        public int DistanciaNado { get; set; } = 0;
        public int? DistanciaSplit { get; set; } = null;
        public string EstiloNado { get; set; } = null;

        public int? IdCategoriaEdad { get; set; } = null;
        public int IdAtleta { get; set; }
        public int? IdEvento { get; set; } = null;

        public Atleta Atleta = null;
        public Competicion Competicion = null;
        public CategoriaEdad CategoriaEdad = null;
    }
}
