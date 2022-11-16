using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_NoComercial.Models
{
    class Negocio
    {
        [JsonProperty("Id_Negocio")]
        public int idNegocio { get; set; }
        [JsonProperty("Unidad_Negocio")]
        public string unidadNegocio { get; set; }
        [JsonProperty("Nombre_Negocio")]
        public string nombreNegocio { get; set; }
        [JsonProperty("Lst_Subred")]
        public List<string> subRedList { get; set; }
        [JsonProperty("segundos_capacitacion")]
        public int segundos_capacitacion { get; set; }
        [JsonProperty("Lst_layout")]
        public List<string> LayoutList { get; set; }
        [JsonProperty("Tiempo_Comercial")]
        public int  tiempo_comercial{ get; set; }
        [JsonProperty("Tiempo_Entretenimiento")]
        public int tiempo_entretenimiento { get; set; }
    }
}
