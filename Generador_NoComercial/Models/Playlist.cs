using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_NoComercial.Models
{
    class Playlist
    {
        public string Id_Contenido { get; set; }
        public string Archivo { get; set; }
        public string Id_Campana { get; set; }
        public string Sku { get; set; }
        public string Medio { get; set; }
        public string Tipo_Contenido { get; set; }
        public string Nombre_Contenido { get; set; }
        public string Subcategoria { get; set; }
        public float Duracion { get; set; }
        public string Layout { get; set; }
        public string Fecha_Inicial { get; set; }
        public string Fecha_Final { get; set; }
        public int conteo_reproduccion { get; set; }
        public int prioridad { get; set; }
        public string categoria { get; set; }

    }
}
