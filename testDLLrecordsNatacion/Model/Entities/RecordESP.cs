using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class RecordESP: EntidadBD
    {
        public int IdRecord;
        public DateTime FechaRecord { get; set; }
        public string TiempoNado { get; set; }
        public int Puntos { get; set; }

        public string RecorridoNado { get; set; } = null;
        public string DistanciaNado { get; set; } = null;
        public int? DistanciaSplit { get; set; } = null;
        public string EstiloNado { get; set; } = null;

        public int? EdadAtleta { get; set; } = null;
        public int? IdCategoriaEdad { get; set; } = null;
        public int IdAtleta { get; set; }
        public int? IdMarca { get; set; } = null;

        public Atleta Atleta = null;
        public Marca Marca = null;
        public CategoriaEdad CategoriaEdad = null;
    }
}
