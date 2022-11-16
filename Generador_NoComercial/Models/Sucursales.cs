using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generador_NoComercial.Models
{
    class Sucursales
    {
        public string Id_Sucursal { get; set; }
        public string Id_player { get; set; }
        public string Nombre_Sucursal { get; set; }
        public string Hora_Inicial { get; set; }
        public string Hora_Final { get; set; }

        public int id_grupo_layout { get; set; }
    }
}
