using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_NoComercial.Models
{
    internal class Spot
    {
        public int Id { get; set; }
        public string id_campana { get; set; }
        public string Nombre { get; set; }
        public float duracion { get; set; }
        public string tipo { get; set; }
        public string categoria { get; set; }
        public string subcategoria { get; set; }
        public int prioridad { get; set; }
        public string Layout { get; set; }
        public int conteo_reproduccion { get; set; }
        public int ocupado { get; set; }
        public string medio { get; set; }
        public int programatic { get; set; }
        public string ffin { get; set; }
        public string archivo { get; set; }

        public int repeticiones_usadas { get; set; }
        public int eliminado { get; set; }
    }
}
