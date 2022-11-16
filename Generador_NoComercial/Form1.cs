using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;
using Generador_NoComercial.Models;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Net;

namespace Generador_NoComercial
{
    public partial class Form1 : Form
    {
        static readonly string URL_WS = ConfigurationManager.AppSettings["URL"].ToString();

        bool procesoCancelado = false;
        List<Negocio> catNegocios;
        List<Layout> lista;
        List<Sucursales> suc;
        int revisionEnPosicionLoop = 0;
        public Form1()
        {
            InitializeComponent();
            catNegocios = new List<Negocio>();
            lista = new List<Layout>();
            suc = new List<Sucursales>();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = Application.ProductVersion;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            cargarRedes();
        }

        #region interacciones
        private void cmbNegocio_TextChanged(object sender, EventArgs e)
        {
            Negocio red = catNegocios[cmbNegocio.SelectedIndex];
            Console.WriteLine(red.idNegocio);
            //limpiar elementos que dependen de la red;
            cmbSubred.Enabled = false;
            cmbSubred.Items.Clear();

            //if (red.LayoutList.Count > 0) {
            //    //cargar subredes al combo
            //   cmbSubred.Items.AddRange(red.LayoutList.ToArray());
            //}
            var strJson1 = ejecutaUrl(URL_WS, "funcion=cat_layouts&Seg=A76A623A87A65FFB93CB57F2C0DFB91079ADB600D5ED1A3A751461DB4EBA1F28&Id_negocio=" + red.idNegocio);
            var json1 = JsonConvert.DeserializeObject<List<Layout>>(strJson1);
            if (json1 != null && json1.Count > 0)
            {
                foreach (var red1 in json1)
                {

                    lista.Add(red1);
                    cmbSubred.Items.Add(red1.nombre);
                    cmbSubred.Enabled = true;

                }
            }

            cargarSucursales();
            cmbSubred.Enabled = true;


           



        }

        private void cmbSubred_TextChanged(object sender, EventArgs e)
        {
            Negocio red = catNegocios[cmbNegocio.SelectedIndex];
            Console.WriteLine(red.idNegocio);

            cmbSubred.Enabled = false;

            cargarSucursales();
            cmbSubred.Enabled = true;
        }
        private void lstSucursales_DoubleClick(object sender, EventArgs e)
        {
            var item = (ListBox)sender;
            if (string.IsNullOrEmpty(item.SelectedItem.ToString()))
                return;

            if (string.IsNullOrEmpty(cmbNegocio.Text))
            {
                MessageBox.Show("Selecciona un negocio");
                return;
            }
            if (lstProcesar.Items.Cast<string>().Any(x => x.Split('|')[1].Trim() == item.SelectedItem.ToString()))
                return;

            lstProcesar.Items.Add(dtpFechaGeneracion.Value.ToString("yyyyMMdd") + " | " + item.SelectedItem);
            txtBuscarSucursales.Text = "";
            lblProcesar.Text = "Número de sucursales: " + lstProcesar.Items.Count;
        }

        private void btnSeleccionadas_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbNegocio.Text))
            {
                MessageBox.Show("Selecciona un negocio");
                return;
            }

            foreach (var item in lstSucursales.SelectedItems)
            {
                if (!lstProcesar.Items.Cast<string>().Any(x => x.Split('|')[1].Trim() == item.ToString()))
                    lstProcesar.Items.Add(dtpFechaGeneracion.Value.ToString("yyyyMMdd") + " | " + item);
            }

            lblProcesar.Text = "Número de sucursales: " + lstProcesar.Items.Count;
        }

        private void btnSeleccionarTodas_Click(object sender, EventArgs e)
        {
            lstProcesar.Items.Clear();
            foreach (var item in lstSucursales.Items)
            {
                lstProcesar.Items.Add(string.Format("{0} | {1}", dtpFechaGeneracion.Value.ToString("yyyyMMdd"), item));
            }

            lblProcesar.Text = "Número de sucursales: " + lstProcesar.Items.Count;
        }

        private void btnQuitarTodas_Click(object sender, EventArgs e)
        {
            lstProcesar.Items.Clear();
            lblProcesar.Text = "Número de sucursales: " + lstProcesar.Items.Count;
        }

        private void txtBuscarSucursales_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(cmbNegocio.Text))
            {
                txtBuscarSucursales.Text = "";
                MessageBox.Show("Selecciona un negocio");
                return;
            }

            if (e.KeyValue == 13)
            {
                if (!lstProcesar.Items.Cast<string>().Any(x => x.Split('|')[1].Trim() == lstSucursales.SelectedItem.ToString()))
                    lstProcesar.Items.Add(dtpFechaGeneracion.Value.ToString("yyyyMMdd") + " | " + lstSucursales.SelectedItem);
                txtBuscarSucursales.Text = "";
                return;
            }

            lstSucursales.ClearSelected();

            var item = lstSucursales.Items.Cast<string>().Where(x => x.Split('-')[0].Trim() == txtBuscarSucursales.Text).FirstOrDefault();
            if (item != null)
                lstSucursales.SelectedItem = item;
        }

        private void txtBuscarProcesar_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(cmbNegocio.Text))
            {
                txtBuscarProcesar.Text = "";
                MessageBox.Show("Selecciona un negocio");
                return;
            }

            lstProcesar.ClearSelected();

            var item = lstProcesar.Items.Cast<string>().Where(x => x.Split('|')[1].Split('-')[0].Trim() == txtBuscarProcesar.Text).FirstOrDefault();
            if (item != null)
                lstProcesar.SelectedItem = item;
        }

        private void btnRecargarRedes_Click(object sender, EventArgs e)
        {
            cargarRedes();
        }
        #endregion
        #region Consultas del ws
        private void cargarRedes()
        {
            //cargar negocios cormerciales
            var strJson = ejecutaUrl(URL_WS, "funcion=cat_redes_comerciales&Seg=A76A623A87A65FFB93CB57F2C0DFB91079ADB600D5ED1A3A751461DB4EBA1F28");
            //strJson=strJson.Replace("null","0");
            var json = JsonConvert.DeserializeObject<List<Negocio>>(strJson);

            if (json != null && json.Count > 0)
            {
                foreach (var red in json)
                {
                    //if (red.idNegocio == 1)
                    //{

                    //    red.nombreNegocio = "EKT";
                    //    cmbNegocio.Items.Add("BAZ BANNER");
                    //    //cmbNegocio.Enabled = true;
                    //    //catNegocios.Add(red);
                    //    //red.nombreNegocio= "BAZ NORMAL";
                    //    cmbNegocio.Items.Add("BAZ NORMAL");
                    //    cmbNegocio.Enabled = true;
                    //    catNegocios.Add(red);

                    //}
                    //else
                    //{
                    //    catNegocios.Add(red);
                    //    cmbNegocio.Items.Add(red.nombreNegocio);
                    //    cmbNegocio.Enabled = true;
                    //}
                    catNegocios.Add(red);
                    cmbNegocio.Items.Add(red.nombreNegocio);
                    cmbNegocio.Enabled = true;
                }

                btnRecargarRedes.Enabled = false;
                btnRecargarRedes.Hide();
            }
            else
            {
                lstResultado.Items.Insert(0, "Error al consultar redes, usa el boton de \"Recargar redes\" para consultar de nuevo");
            }
        }

        private void cargarSubredes() { }

        private void cargarSucursales()
        {
            Negocio red = catNegocios[cmbNegocio.SelectedIndex];
            var subred = cmbSubred.Text;

            lstProcesar.Enabled = false;
            lstSucursales.Enabled = false;
            lstProcesar.Items.Clear();
            lstSucursales.Items.Clear();

            var strJson = ejecutaUrl(URL_WS,
                    "funcion=sucursales&Seg=A76A623A87A65FFB93CB57F2C0DFB91079ADB600D5ED1A3A751461DB4EBA1F28&Negocio=" + red.nombreNegocio+"");
            var json = JsonConvert.DeserializeObject<List<Sucursales>>(strJson);
            if (json == null)
            {
                lstResultado.Items.Insert(0, "Error al consultar las sucursales, por favor intenta nuevamente.");
            }
            else if (json.Count > 0)
            {
                suc = json;
                foreach (var item in json)
                {
                    //if (json[0].Id_player.)
                    //{
                    //    item.Id_Sucursal = item.Id_player;
                    //}
                    string horaInicial = string.Empty, horaFinal = string.Empty, nombreSucursal = string.Empty;
                    DateTime fechaInicial = new DateTime(), fechaFinal = new DateTime();

                    horaInicial = DateTime.TryParse(item.Hora_Inicial, out fechaInicial) ? fechaInicial.ToString("HH:mm:ss") : horaInicial;
                    horaFinal = DateTime.TryParse(item.Hora_Final, out fechaFinal) ? fechaFinal.ToString("HH:mm:ss") : horaFinal;
                    nombreSucursal = string.IsNullOrEmpty(item.Nombre_Sucursal) ? nombreSucursal : item.Nombre_Sucursal.Replace("-", "/");

                    
                        lstSucursales.Items.Add(string.Format("{0} - {1} - {2} - {3}", item.Id_Sucursal, nombreSucursal, horaInicial, horaFinal));
                    
                }
            }

            lstProcesar.Enabled = true;
            lstSucursales.Enabled = true;
        }
        #endregion
        private void btnSeleccionarSucursales_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbNegocio.Text))
            {
                MessageBox.Show("Selecciona un negocio");
                return;
            }

            txtSeleccionarSucursales.Text = txtSeleccionarSucursales.Text.Replace(Environment.NewLine, "").Replace(" ", "");
            var txtArray = txtSeleccionarSucursales.Text.Split(',');

            lstProcesar.Items.Clear();

            foreach (var item in txtArray)
            {
                var sucursal = lstSucursales.Items.Cast<string>().Where(x => x.Split('-')[0].Trim() == item).FirstOrDefault();
                if (sucursal != null)
                    lstProcesar.Items.Add(dtpFechaGeneracion.Value.ToString("yyyyMMdd") + " | " + sucursal);
            }

            lblProcesar.Text = "Número de sucursales: " + lstProcesar.Items.Count;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (btnProcesar.Enabled == false)
            {
                btnCancelar.Enabled = false;
                procesoCancelado = true;
            }
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                var horaInicioSucursal = new TimeSpan();
                var horaFinSucursal = new TimeSpan();

                btnProcesar.Enabled = false;
                txtProblemas.Text = string.Empty;
                lstResultado.Items.Clear();

                if (string.IsNullOrEmpty(cmbNegocio.Text))
                {
                    MessageBox.Show("Selecciona un negocio");
                    return;
                }

                //vamos a crear la playlist por cada sucursal seleccionada
                foreach (var itemProcesar in lstProcesar.Items)
                {
                    Negocio red = catNegocios[cmbNegocio.SelectedIndex];
                    var idSucursal = itemProcesar.ToString().Split('|')[1].Split('-').First().Trim();

                    var parsehoraInicioSucursal = TimeSpan.TryParse(itemProcesar.ToString().Split('-').Reverse().Skip(1).First().Trim(), out horaInicioSucursal);
                    var parsehoraFinSucursal = TimeSpan.TryParse(itemProcesar.ToString().Split('-').Last().Trim(), out horaFinSucursal);

                    
                    if (!parsehoraInicioSucursal || !parsehoraFinSucursal)
                    {
                        txtProblemas.Text = txtProblemas.Text.Length > 0 ? txtProblemas.Text + "," + idSucursal : idSucursal;
                        lstResultado.Items.Add(string.Format("Sucursal {0} no tiene un horario valido", idSucursal));
                    }
                    else
                    {
                        try
                        {
                            horaFinSucursal = TimeSpan.Parse(itemProcesar.ToString().Split('-').Last().Trim()); 
                            var strJson = ejecutaUrl(URL_WS,
                                "funcion=playlist_redes_comerciales&Seg=A76A623A87A65FFB93CB57F2C0DFB91079ADB600D5ED1A3A751461DB4EBA1F28&id_sucursal=" + idSucursal
                                + "&negocio=" + red.unidadNegocio + "&fecha=" + dtpFechaGeneracion.Value.ToString("yyyy-MM-dd"));
                            var json = JsonConvert.DeserializeObject<List<Playlist>>(strJson);


                            //Colocar a que funcion va a ir en capacitacion
                            var strJson1 = ejecutaUrl(URL_WS, 
                                "funcion=playlist_capacitacion&Seg=A76A623A87A65FFB93CB57F2C0DFB91079ADB600D5ED1A3A751461DB4EBA1F28&id_sucursal=" + idSucursal 
                                + "&negocio=" + red.unidadNegocio + "&fecha=" + dtpFechaGeneracion.Value.ToString("yyyy-MM-dd"));
                            var json1 = JsonConvert.DeserializeObject<List<Playlist>>(strJson1);
                            

                            if (json.Count > 0)
                            {
                                var hora = horaInicioSucursal;
                                var horafincapa = horaInicioSucursal;
                                if (red.segundos_capacitacion > 0)
                                   
                                {
                                    TimeSpan tiempocapacitacion = new TimeSpan(0, 0, red.segundos_capacitacion);
                                    
                                    horafincapa = horafincapa.Add(tiempocapacitacion);
                                }
                                var localPath = Path.GetDirectoryName(Application.ExecutablePath);
                                var fileName = string.Format("{0}_{1}.txt", dtpFechaGeneracion.Value.ToString("yyyyMMdd"), idSucursal);
                                var fullPath = Path.Combine(localPath, fileName);

                                if (File.Exists(fullPath))
                                    File.Delete(fullPath);
                                
                                    

                                using (var writer = new StreamWriter(fullPath, true))

                                {
                                    //Agregamos capacitacion
                                    if (red.segundos_capacitacion > 0)
                                    {
                                        while (hora < horafincapa)
                                        {

                                            foreach (var item in json1)
                                            {
                                                
                                                if (procesoCancelado)
                                                    break;

                                                var tipoContenido = item.Medio == "MI" ? "ARCHIVO" : "SPOT";
                                                var archivo = item.Medio == "MI" ? item.Archivo + "_" + idSucursal + ".sku" : item.Archivo;
                                                

                                                    if ((hora + TimeSpan.FromSeconds(item.Duracion)) >= horaFinSucursal)
                                                    {
                                                        writer.WriteLine(string.Format("HORA&=&{0}&,&MEDIO&=&{1}&,&TIEMPO&=&{2}&,&{3}&=&{4}&,&DURACION&=&{5}" +
                                                            "&,&CAMPANA&=&{6}&,&CONTENT&=&{7}&,&ALEATORIO&=&{8}", hora, item.Medio, item.Tipo_Contenido, tipoContenido, archivo, item.Duracion, item.Id_Campana,
                                                            item.Nombre_Contenido, generateAleatorio()));
                                                        hora = hora + TimeSpan.FromSeconds(item.Duracion);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        writer.WriteLine(string.Format("HORA&=&{0}&,&MEDIO&=&{1}&,&TIEMPO&=&{2}&,&{3}&=&{4}&,&DURACION&=&{5}" +
                                                            "&,&CAMPANA&=&{6}&,&CONTENT&=&{7}", hora, item.Medio, item.Tipo_Contenido, tipoContenido, archivo, item.Duracion, item.Id_Campana,
                                                            item.Nombre_Contenido));
                                                        hora = hora + TimeSpan.FromSeconds(item.Duracion);
                                                    }
                                                
                                            }
                                        }
                                       
                                        
                                    }

                                    //Ya no dejamos el while ya que la logica se cambio
                                    
                                        Layout id = lista[cmbSubred.SelectedIndex];

                                        Sucursales sucursales = new Sucursales();
                                        sucursales = suc.Select(x => x).Where(x => x.Id_Sucursal.Equals(idSucursal)).First();

                                        var strJson2 = ejecutaUrl(URL_WS,"funcion=tipo_layout&Seg=A76A623A87A65FFB93CB57F2C0DFB91079ADB600D5ED1A3A751461DB4EBA1F28&id_layout=" + id.id_layout + "&id_grupo_layout=" + sucursales.id_grupo_layout);
                                        var json2 = JsonConvert.DeserializeObject<List<Repeticiones>>(strJson2);
                                        //Se asigna de forma dinamica el layout
                                        var layout = json2[0].Repeticiones_base;

                                        float InventarioComercial = red.tiempo_comercial;
                                        float InventarioEntretenimiento = red.tiempo_entretenimiento;
                                        float Inventariototal = InventarioComercial + InventarioEntretenimiento;
                                        var totalcomerciales = 0;
                                        var totalentretenimiento = 0;

                                        Dictionary<int, List<Spot>> Diccionario = new Dictionary<int, List<Spot>>();

                                        var m = 1;
                                        var list = 1;
                                        for(int i = 0; i < layout; i++)
                                        {
                                            List<Spot> lista1 = new List<Spot>();
                                            Diccionario.Add(i, lista1);
                                        }

                                        Dictionary<int, List<Spot>> Diccionario2 = new Dictionary<int, List<Spot>>();

                                        for (int i = 0; i < layout; i++)
                                        {
                                            List<Spot> lista1 = new List<Spot>();
                                            Diccionario2.Add(i, lista1);
                                        }

                                        Dictionary<int, List<Spot>> Diccionario3 = new Dictionary<int, List<Spot>>();

                                        for (int i = 0; i < layout; i++)
                                        {
                                            List<Spot> lista1 = new List<Spot>();
                                            Diccionario3.Add(i, lista1);
                                        }
                                        /*Dictionary<int, List<Spot>> Diccionario4 = new Dictionary<int, List<Spot>>();

                                        for (int i = 0; i < layout; i++)
                                        {
                                            List<Spot> lista1 = new List<Spot>();
                                            Diccionario4.Add(i, lista1);
                                        }*/

                                    //Mapeamos todos los contenidos de entretenimiento
                                    Dictionary<int, Spot> Diccionario_Entretenimiento = new Dictionary<int, Spot>();
                                    List<Spot> ListaGeneralEntretenimiento = new List<Spot>();
                                        for (int i = 0; i < json.Count; i++)
                                        {
                                            if (json[i].Tipo_Contenido.ToUpper().Equals("ENTRETENIMIENTO"))
                                            {
                                                Spot spot = new Spot();
                                                
                                                spot.id_campana = json[i].Id_Campana;
                                                spot.Nombre = json[i].Nombre_Contenido;
                                                spot.duracion=json[i].Duracion;
                                                spot.tipo = json[i].Tipo_Contenido;
                                                spot.prioridad = json[i].prioridad;
                                                spot.subcategoria = json[i].Subcategoria;
                                                spot.medio = json[i].Medio;
                                                spot.ffin = json[i].Fecha_Final;
                                                spot.Layout = json[i].Layout;
                                                spot.archivo = json[i].Archivo;
                                                spot.ocupado = 0;
                                                Diccionario_Entretenimiento.Add(int.Parse(spot.id_campana),spot );
                                                ListaGeneralEntretenimiento.Add(spot);
                                            }
                                        }
                                       
                                    //Mapeamos todos los contenidos de comercial
                                    Dictionary<int, Spot> Diccionario_Comercial = new Dictionary<int, Spot>();

                                    for (int i = 0; i < json.Count; i++)
                                    {
                                        if (json[i].Tipo_Contenido.ToUpper().Equals("COMERCIAL"))
                                        {
                                            Spot spot = new Spot();

                                            spot.id_campana = json[i].Id_Campana;
                                            spot.Nombre = json[i].Nombre_Contenido;
                                            spot.duracion = json[i].Duracion;
                                            spot.tipo = json[i].Tipo_Contenido;
                                            spot.prioridad = json[i].prioridad;
                                            spot.subcategoria = json[i].Subcategoria;
                                            spot.medio = json[i].Medio;
                                            spot.ffin = json[i].Fecha_Final;
                                            spot.Layout = json[i].Layout;
                                            spot.archivo = json[i].Archivo;
                                            spot.ocupado = 0;
                                            spot.repeticiones_usadas = 0;
                                            Diccionario_Comercial.Add(int.Parse(spot.id_campana), spot);
                                        }
                                    }
                                    



                                    //var totalduracioncomercial = json.Where(x => x.Tipo_Contenido.ToString().Equals("COMERCIAL")).Sum(x=>x.Duracion);
                                    //Distribucion de contenido comercial y entretenimiento de la sucursal
                                    foreach (var item in json)
                                        {
                                            if (procesoCancelado)
                                                break;
                                            float acomodo = 0;

                                            //se determina reglas de acomodo dependiendo num de repeticiones contratadas
                                            //TODO: En las distribuciones de los loops revisar cuanto tiempo de contenido de entretenimiento llevo 

                                            if (item.conteo_reproduccion < layout)
                                            {
                                                /*RC repeticiones contratadas es decir las repeticiones del contenido
                                                 * RB repeticiones del loop 
                                                 * formula RC<RB
                                                 */
                                                acomodo = caso1_loop(layout, item.conteo_reproduccion);
                                                distribuir_contenidos_caso1(ref Diccionario,item,acomodo, idSucursal);
                                            }
                                            else if (item.conteo_reproduccion == layout)
                                            {
                                                /*RC repeticiones contratadas es decir las repeticiones del contenido
                                                 * RB repeticiones del loop 
                                                 * formula RC=RB
                                                 */
                                                acomodo = 1;
                                                distribuir_contenidos_caso2(ref Diccionario, item,acomodo, idSucursal);
                                            }
                                            else {
                                                /*RC repeticiones contratadas es decir las repeticiones del contenido
                                                 * RB repeticiones del loop 
                                                 * formula RC>RB
                                                 */
                                                acomodo = caso2_loop(layout, item.conteo_reproduccion);
                                                distribuir_contenidos_caso3(ref Diccionario, item,acomodo, idSucursal);
                                            }

                                            //A este punto ya estan distribuidos 

                                            ///TO DD:Idea para revisar que la distribucion comercial este correcta ya que sale una efectividad muy alta
                                            ///
                                            Console.WriteLine(Diccionario);
                                            var TOTALSPOTSENCARTA=0;
                                            
                                            



                                    }

                                    //Por loop vamos a ajustar tiempos antes de ordenar
                                    var TiempoRestanteAnterior = 0;

                                    for (int j = 0; j < Diccionario.Count; j++)
                                    {
                                        /*Recorremos loop por loop
                                         * Necesitamos saber lo siguiente para discriminar los contenidos de entretenimiento que necesitamos quitar
                                         * tiempo total de comercial ocupado en loop
                                         * tiempo comercial restante
                                         * tiempo de entretenimiento permitido
                                         * tiempo total de loop permitido
                                         * tiempo restante en loop
                                         * tiempo entretenimiento restante
                                         */
                                        /*
                                         * Escenarios
                                         * 1.-Comercial ocuapa totalmente o sobrepasa lo permitido: dar prioridad a lo comercial, luego a lo prioritario de entretenimiento
                                         * 2.-Comercial no ocupa todo lo permitido: rellenar con entretenimiento pero todo marcarlo como programatic
                                         * 3.-Entretenimiento sobrepasa lo permitido: dar prioridad a lo que tiene prioridad del 1 a mayor tomando el 1 como prioritario y luego completar el resto
                                         * 4.-Entretenimiento no completa lo permitido: recorrer lo actual para completar el espacio de entretenimiento
                                         */
                                        /*
                                         * tambien tomar en cuenta que para el caso de que el loop le sobre un espacio de 10 o 20 segundos se añadira al siguiente loop
                                         * TENER EN CUENTA QUE ESTE FOR SOLO ES PARA SACAR EL CONTENIDO DE ENTRETENIMIENTO EXCEDENTE ES DECIR SOLO DEJAR LO NECESARIO PARA ORDENAR
                                         */
                                        var TotalTiempoComercialOcupado = 0;
                                        //var TotalTiempoEntretenimientoOcuapdo = 0;//podemos estar sin esta
                                        var TiempoComercialRestante = 0;
                                        var TiempoEntretenimientoRestante = 0;
                                        var TotalComercialPermitido = 0;
                                        var TotalEntretenimientoPermitido = 0;
                                        var TotalTiempoLoop = 0;

                                        TotalTiempoComercialOcupado = (int)(Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL")).Sum(spot => spot.duracion));
                                        TotalComercialPermitido = red.tiempo_comercial;
                                        TotalEntretenimientoPermitido = red.tiempo_entretenimiento;
                                        TiempoComercialRestante = TotalComercialPermitido - TotalTiempoComercialOcupado;




                                        
                                    
                                    }

                                        //Vamos a iniciar con el ordenamiento por cada loop

                                        for (int j = 0; j < Diccionario.Count; j++)
                                        {
                                            
                                            revisionEnPosicionLoop=j;
                                            var numloop = j + 1;
                                            Console.WriteLine("Loop " + numloop);
                                            var total_contenidos = 0;
                                            var total_contenidos_comerciales = 0;
                                            var total_contenidos_entretenimiento = 0;
                                            var total_tiempo_comercial_usado = 0;
                                            var posicion1_contenido_entretenimiento = 0;
                                            var duracion_total_loop = 0;
                                            var tiempo_loop_restante = 0;
                                            var spots_restantes_para_llamar_fila = 0;
                                            var total_contenidos_comerciales_diferentes = 0;
                                            var LoopOrdenado = false;
                                            
                                            
                                            List<Spot> fila_espera_contenido_comercial=new List<Spot>();
                                            List<Spot> fila_espera_contenido_entretenimiento = new List<Spot>();
                                            Console.WriteLine(red.tiempo_entretenimiento);

                                            duracion_total_loop = red.tiempo_comercial + red.tiempo_entretenimiento;
                                            total_contenidos = Diccionario[j].Count;
                                            total_contenidos_comerciales=Diccionario[j].Where(x=>x.tipo.ToUpper().Equals("COMERCIAL")).Count();
                                            total_contenidos_comerciales_diferentes = Diccionario[j].Select(x=>x).Distinct().Where(x => x.tipo.ToUpper().Equals("COMERCIAL")).Count();
                                            total_contenidos_entretenimiento = Diccionario[j].Where(x => x.tipo.ToUpper().Equals("ENTRETENIMIENTO")).Count();
                                            total_tiempo_comercial_usado = Diccionario[j].Where(x => x.tipo.ToUpper().Equals("COMERCIAL")).Sum(x => Int32.Parse(x.duracion.ToString()));
                                            posicion1_contenido_entretenimiento = total_contenidos_comerciales;
                                            tiempo_loop_restante = duracion_total_loop-total_tiempo_comercial_usado;
                                            totalcomerciales = total_contenidos_comerciales;

                                            var programatic = InventarioComercial- total_tiempo_comercial_usado;

                                            List<Spot> ContenidoComercial=new List<Spot>();
                                            List<Spot> ContenidoEntretenimiento = new List<Spot>();
                                            List<Spot> ContenidoCombinado = new List<Spot>();
                                            var TodoComercialUsado = false;
                                            var TodoEntretenimientoUsado = false;
                                            var TodoTiemoEntretenimientoUsado=false;
                                            var TodoTiemopComercialUsado = false;
                                            var posicionComercial = 0;
                                            var posicionEntretenimiento = 0;
                                       
                                            //ya tenemos distribuido los contenidos den los loops ahora hay que espaciar los contenidos comerciales ya que posiblemente haya contenido comercial repetido
                                            //solo si hay contenido comercial repetido
                                            //Si tenemos contenidos comerciales repetidos en el loop es decir hay contenidos que van a mas de 48 repeticiones
                                            if (total_contenidos_comerciales > total_contenidos_comerciales_diferentes)
                                            {
                                                List<Spot> ContenidosRepetidosEnLoop = new List<Spot>();
                                                List<Spot> ContenidosComericalesENLoop = new List<Spot>();
                                                List<Spot> ContenidosNorerpetidos = new List<Spot>();
                                                List<Spot> ListaCombinada = new List<Spot>();

                                                Dictionary<string, int> conteoDeRepsXloop = new Dictionary<string, int>();

                                                for (int i = 0; i < total_contenidos_comerciales; i++)
                                                {
                                                    //Si el contenido aparece mas de una vez se agrega a la lista para distribuir
                                                    if (Diccionario[j].Where(x => x.id_campana.Equals(Diccionario[j][i].id_campana)).Count() > 1)
                                                    {
                                                        //Console.WriteLine(j + i);
                                                        ContenidosRepetidosEnLoop.Add(Diccionario[j][i]);
                                                        conteoDeRepsXloop.Add(Diccionario[j][i].id_campana, Diccionario[j].Where(x => x.id_campana.Equals(Diccionario[j][i].id_campana)).Count());
                                                    }
                                                    else
                                                    { //En caso de que aparezca solo una vez se agrega a la lista de no repetidos

                                                        ContenidosNorerpetidos.Add(Diccionario[j][i]);
                                                    }
                                                    ContenidosComericalesENLoop.Add(Diccionario[j][i]);
                                                }
                                                var a = 0;//servira para saber cual toca agregar
                                                var b = 0;//piciondose repetidos
                                                var c = 0;//posicion de no repetidos
                                                var d = 0;//terminado para repetidos  
                                                var ef = 0;// terminado para no repetidos
                                                for (int i = 0; i < ContenidosComericalesENLoop.Count; i++)
                                                {
                                                    if (a == 0)
                                                    {
                                                        if (ef == 0)
                                                            a = 1;
                                                        //agregamos repetidos
                                                        if (b < ContenidosRepetidosEnLoop.Count)
                                                        {
                                                            ListaCombinada.Add(ContenidosRepetidosEnLoop[b]);
                                                        }
                                                        b++;
                                                        if (b >= ContenidosRepetidosEnLoop.Count)
                                                            d = 1;



                                                    }
                                                    else
                                                    {
                                                        if (d == 0)
                                                            a = 0;
                                                        //agregamos no repetidos
                                                        if (c < ContenidosNorerpetidos.Count)
                                                            ListaCombinada.Add(ContenidosNorerpetidos[c]);
                                                        c++;
                                                        if (c >= ContenidosNorerpetidos.Count)
                                                            ef = 1;

                                                    }



                                                }
                                                Console.WriteLine(Diccionario[j]);
                                                for (int i = 0; i < ListaCombinada.Count; i++)
                                                {
                                                    Diccionario[j][i] = ListaCombinada[i];
                                                }


                                            }

                                            //Aqui se va a hacer el ordenamienco comercial y de entretenimiento
                                            /*REGLAS GENERALES DEL GENERADOR
                                                * 1.- los contenidos de entretenimiento no deben estar 2 subcategorias seguidas aun si se separa por un contenido comercial
                                                * 2.- El espacio de contenido comercial que no se ocupa debe ser programatico
                                                * 3.- Se deben dar solo las repeticiones contratadas para el comercial no mas y no menos
                                                * 4.- Asegurar todas las repeticiones de los contenidos contenidos de entretenimiento con prioridad
                                                * 
                                                * 6.- se quiere que si un comercial esta en el minuto 1 se añada tambien en el minuto 15 
                                                */
                                            /*Vamos a seguir ordenando el loop hasta saber que esta ordenado
                                             *
                                             *Sabremos que el loop esta ordenado bajo las siguientes condiciones
                                             *1.- El total de contenidos comerciales ya se uso
                                             *2.- El total de tiempo loop ya se uso
                                             *3.- Todo tiemop den comercial usado
                                             *4.- Todo tiempo entretenimiento usado
                                            */
                                            do
                                            {
                                                if (posicionComercial==0&&posicionEntretenimiento==0)
                                                {//quiere decir que estamos iniciando el ciclo de ordenamiento
                                                    Spot contenido_comercial = new Spot();
                                                    contenido_comercial.Id = m;
                                                    contenido_comercial.id_campana = Diccionario[j][p].id_campana;
                                                    contenido_comercial.Nombre = Diccionario[j][p].Nombre;
                                                    contenido_comercial.duracion = Diccionario[j][p].duracion;
                                                    contenido_comercial.tipo = Diccionario[j][p].tipo;
                                                    contenido_comercial.prioridad = Diccionario[j][p].prioridad;
                                                    contenido_comercial.Layout = Diccionario[j][p].Layout;
                                                    contenido_comercial.medio = Diccionario[j][p].medio;
                                                    contenido_comercial.ffin = Diccionario[j][p].ffin;
                                                    contenido_comercial.archivo = Diccionario[j][p].archivo;
                                                    contenido_comercial.conteo_reproduccion = Diccionario[j][p].conteo_reproduccion;
                                                    contenido_comercial.subcategoria = Diccionario[j][p].subcategoria;
                                                    Diccionario2[j].Add(contenido_comercial);
                                                }
                                           
                                            

                                                if (TodoComercialUsado && TodoTiemoEntretenimientoUsado && TodoTiemopComercialUsado && tiempo_loop_restante==0)
                                                {
                                                    LoopOrdenado = true;
                                                }

                                            } while (!LoopOrdenado);
                                    
                                        }

                                    do {
                                        //TO DO: Poner regla de que solo salga las repeticiones que se contrato para la campaña
                                        for (int i = 0; i < Diccionario.Count; i++)
                                        {
                                            for (int j = 0; j < Diccionario[i].Count; j++)
                                            {
                                                var SpotUrlArchivo = "SPOT";
                                                if (Diccionario[i][j].medio.ToUpper().Equals("DS"))
                                                    SpotUrlArchivo = "SPOT";
                                                else if(Diccionario[i][j].medio.ToUpper().Equals("MI"))
                                                    SpotUrlArchivo = "ARCHIVO";
                                                else
                                                    SpotUrlArchivo = "URL";

                                                var ffin = "";
                                                var programatic = "";
                                                var lyout = "";
                                                var subcat = "";

                                                if (Diccionario[i][j].ffin.Length>0)
                                                {
                                                    ffin = "&,&FFIN&=&" + Diccionario[i][j].ffin.Split(' ')[0];
                                                }
                                                if (Diccionario[i][j].programatic > 0)
                                                {
                                                    programatic = "&,&PROGRAMATIC&=&HIVESTACK";
                                                }
                                                if (Diccionario[i][j].Layout.Length > 0)
                                                {
                                                    lyout = "&,&LAYOUT&=&"+ Diccionario[i][j].Layout;
                                                }
                                                if (Diccionario[i][j].subcategoria != "0")
                                                { 
                                                    subcat= "&,&SUBCATEGORIA&=&" + Diccionario[i][j].subcategoria;
                                                }


                                                

                                                if (Diccionario[i][j].tipo.ToUpper().Equals("COMERCIAL"))
                                                {
                                                    var conteoReproduccion = Diccionario[i][j].conteo_reproduccion;
                                                    var RepeticionesUsadas = Diccionario_Comercial[Int32.Parse(Diccionario[i][j].id_campana)].repeticiones_usadas;
                                                    //Para hacer que solo ponga las repeticiones que se requieren de lo comercial
                                                    if (conteoReproduccion>RepeticionesUsadas)
                                                    {
                                                        writer.WriteLine(string.Format("HORA&=&{0}&,&MEDIO&=&{1}&,&TIEMPO&=&{2}&,&" + SpotUrlArchivo + "&=&{3}&,&DURACION&=&{4}&,&CAMPANA&=&{5}" +
                                                            "&,&CONTENT&=&{6}" + ffin + programatic + lyout + subcat, hora, Diccionario[i][j].medio, Diccionario[i][j].tipo, Diccionario[i][j].archivo, Diccionario[i][j].duracion, Diccionario[i][j].id_campana,
                                                            Diccionario[i][j].Nombre));
                                                        hora = hora + TimeSpan.FromSeconds(Diccionario[i][j].duracion);
                                                        Diccionario_Comercial[Int32.Parse(Diccionario[i][j].id_campana)].repeticiones_usadas++;
                                                         
                                                    }
                                                    
                                                }
                                                else
                                                {
                                                    writer.WriteLine(string.Format("HORA&=&{0}&,&MEDIO&=&{1}&,&TIEMPO&=&{2}&,&" + SpotUrlArchivo + "&=&{3}&,&DURACION&=&{4}&,&CAMPANA&=&{5}" +
                                                            "&,&CONTENT&=&{6}" + ffin + programatic + lyout + subcat, hora, Diccionario[i][j].medio, Diccionario[i][j].tipo, Diccionario[i][j].archivo, Diccionario[i][j].duracion, Diccionario[i][j].id_campana,
                                                            Diccionario[i][j].Nombre));
                                                    hora = hora + TimeSpan.FromSeconds(Diccionario[i][j].duracion);
                                                }

                                                
                                                if (hora > horaFinSucursal)
                                                {
                                                    break;
                                                }
                                            }

                                            
                                        }
                                        //Console.WriteLine("Hora Actual {0} vs hora permitida {1}",hora,horaFinSucursal);
                                        
                                    } while (hora < horaFinSucursal);
                                    //} while (hora < horaFinSucursal);

                                }
                                lstResultado.Items.Add(string.Format("Sucursal {0} Ok", idSucursal));
                            }
                            else
                            {
                                lstResultado.Items.Add(string.Format("Sucursal {0} no tiene contenidos", idSucursal));
                                txtProblemas.Text = txtProblemas.Text.Length > 0 ? txtProblemas.Text + "," + idSucursal : idSucursal;
                            }
                            lstResultado.Refresh();
                        }
                        catch (Exception ex)
                        {
                            lstResultado.Items.Add(string.Format("Falló sucursal {0}, error {1} en {2}", idSucursal, ex.Message, ex.StackTrace));
                            lstResultado.Items.Add(string.Format("Falló sucursal {0}, en loop {1}", idSucursal, revisionEnPosicionLoop));
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                            txtProblemas.Text = txtProblemas.Text.Length > 0 ? txtProblemas.Text + "," + idSucursal : idSucursal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lstResultado.Items.Add(string.Format("Error no controlado, {0} en {1}", ex.Message, ex.StackTrace));
                lstResultado.Items.Add(string.Format("en loop {0}", revisionEnPosicionLoop));
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            btnProcesar.Enabled = true;
        }




        private void ReiniciaDiccionarioUsado(ref Dictionary<int,List<Spot>> DiccionarioALimpiar)
        {
            foreach (var itemlist in DiccionarioALimpiar)
            {
                foreach (var itemSpot in itemlist.Value)
                {
                    itemSpot.ocupado = 0;
                }
            }
        }

        private string generateAleatorio()
        {
            try
            {

                Random obj = new Random();
                string sCadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                int longitud = sCadena.Length;
                char cletra;
                int nlongitud = obj.Next(100, 500);
                string sNuevacadena = string.Empty;
                for (int i = 0; i < nlongitud; i++)
                {
                    cletra = sCadena[obj.Next(62)];
                    sNuevacadena += cletra.ToString();
                }
                return sNuevacadena;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return string.Empty;
            }
        }

        private string ejecutaUrl(string Url, string Datos)
        {
            var responseString = "";
            Cursor.Current = Cursors.WaitCursor;
            imgLoad.Show();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Url);
                var data = Encoding.ASCII.GetBytes(Datos);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Cursor.Current = Cursors.Default;
                imgLoad.Hide();
                return responseString;
            }
            catch(Exception e)
            {
                lstResultado.Items.Insert(0, e.Message);
                Cursor.Current = Cursors.Default;
                imgLoad.Hide();
                return string.Empty;
            }
        }
        public float caso1_loop(int layout,int conteo)
        {
            //RC<RB
            float formula = layout / conteo;
            return formula;

        }
        public float caso2_loop(int layout, int conteo)
        {
            //RC>RB
            float formula = 0;
            try
            {
                var d = (float)layout/ conteo;
                formula = 1 / d;
            }
            catch (Exception e)
            {
                lstResultado.Items.Add(string.Format("error {0}", e.Message ));
            }
            
            return formula;
        }
        public string[] dividirDecimales(double numero)
        {
            string[] resultado = numero.ToString().Split('.');

            return resultado;
        
        }
        private void distribuir_contenidos_caso1(ref Dictionary<int, List<Spot>> Diccionario,Playlist item, double acomodo, string idSucursal) {

            try
            {
                string[] acomodoDecimal = dividirDecimales(acomodo);
                //revisar si tiene decimales ya que si tiene tendremos que tendremos que poner un spot cada cierto tiempo
                //en caso contrario podemos solo agregar la cantidad de veces el spot en el loop
                Random r = new Random();
                if (acomodoDecimal.Length==1)
                {
                    int parteEntera = Int32.Parse(acomodoDecimal[0]);
                    int contados = parteEntera;
                    
                    for (int i = r.Next(0, parteEntera); i < Diccionario.Count; i++)
                    {
                        Spot contenido = new Spot();

                        contenido.id_campana = item.Id_Campana;
                        contenido.Nombre = item.Nombre_Contenido;
                        contenido.duracion = item.Duracion;
                        contenido.tipo = item.Tipo_Contenido;
                        contenido.prioridad = item.prioridad;
                        contenido.Layout = item.Layout;
                        contenido.ffin = item.Fecha_Final;
                        contenido.medio = item.Medio;
                        contenido.archivo= item.Archivo;
                        contenido.subcategoria = item.Subcategoria;
                        contenido.conteo_reproduccion = item.conteo_reproduccion;
                        if (contados == parteEntera)
                        {
                            Diccionario[i].Add(contenido);
                            contados = 0;
                        }
                        contados++;

                    }
                }
                else
                {
                    int parteEntera = Int32.Parse(acomodoDecimal[0]);
                    int contados = parteEntera;
                    for (int i = r.Next(0, parteEntera); i < Diccionario.Count; i++)
                    {
                        Spot contenido = new Spot();

                        contenido.id_campana = item.Id_Campana;
                        contenido.Nombre = item.Nombre_Contenido;
                        contenido.duracion = item.Duracion;
                        contenido.tipo = item.Tipo_Contenido;
                        contenido.prioridad = item.prioridad;
                        contenido.Layout = item.Layout;
                        contenido.ffin = item.Fecha_Final;
                        contenido.medio = item.Medio;
                        contenido.archivo = item.Archivo;
                        contenido.subcategoria = item.Subcategoria;
                        contenido.conteo_reproduccion = item.conteo_reproduccion;
                        if (contados == parteEntera)
                        {
                            Diccionario[i].Add(contenido);
                            contados = 0;
                        }
                        contados++;

                    }
                    lstResultado.Items.Add(string.Format("Sucursal {0} revisar contenido {1} posibles repeticiones excedidas", idSucursal, item.Id_Campana));
                }

                }
            catch (Exception)
            {

                throw;
            }

        }
        private void distribuir_contenidos_caso2(ref Dictionary<int, List<Spot>> Diccionario, Playlist item, double acomodo, string idSucursal)
        {
            //Es el caso mas facil ya que solo tenemos que poner un spot en cada loop

            for (int i=0;i<Diccionario.Count;i++)
            {
                Spot contenido = new Spot();
               
                contenido.id_campana = item.Id_Campana;
                contenido.Nombre = item.Nombre_Contenido;
                contenido.duracion = item.Duracion;
                contenido.tipo = item.Tipo_Contenido;
                contenido.prioridad = item.prioridad;
                contenido.Layout = item.Layout;
                contenido.medio = item.Medio;
                contenido.ffin = item.Fecha_Final;
                contenido.archivo = item.Archivo;
                contenido.subcategoria = item.Subcategoria;
                contenido.conteo_reproduccion = item.conteo_reproduccion;
                Diccionario[i].Add(contenido);

            }

        }
        private void distribuir_contenidos_caso3(ref Dictionary<int, List<Spot>> Diccionario, Playlist item, double acomodo,string idSucursal)
        {
            try
            {
                string[] acomodoDecimal = dividirDecimales(acomodo);
                //revisar si tiene decimales ya que si tiene tendremos que tendremos que poner un spot cada cierto tiempo
                //en caso contrario podemos solo agregar la cantidad de veces el spot en el loop
             
                if (acomodoDecimal.Length == 1)
                {
                    for (int i = 0; i < Diccionario.Count; i++)
                    {
                        Spot contenido = new Spot();

                        contenido.id_campana = item.Id_Campana;
                        contenido.Nombre = item.Nombre_Contenido;
                        contenido.duracion = item.Duracion;
                        contenido.tipo = item.Tipo_Contenido;
                        contenido.prioridad = item.prioridad;
                        contenido.Layout = item.Layout;
                        contenido.medio = item.Medio;
                        contenido.ffin = item.Fecha_Final;
                        contenido.archivo = item.Archivo;
                        contenido.subcategoria = item.Subcategoria;
                        contenido.conteo_reproduccion = item.conteo_reproduccion;
                        for (int j = 0; j < Int32.Parse(acomodo.ToString()); j++)
                            Diccionario[i].Add(contenido);

                    }
                }
                else
                {
                    //Distribuimos en cada loop los spots de la parte entera
                    for (int i = 0; i < Diccionario.Count; i++)
                    {
                        Spot contenido = new Spot();

                        contenido.id_campana = item.Id_Campana;
                        contenido.Nombre = item.Nombre_Contenido;
                        contenido.duracion = item.Duracion;
                        contenido.tipo = item.Tipo_Contenido;
                        contenido.prioridad = item.prioridad;
                        contenido.Layout = item.Layout;
                        contenido.medio = item.Medio;
                        contenido.ffin = item.Fecha_Final;
                        contenido.archivo = item.Archivo;
                        contenido.subcategoria = item.Subcategoria;
                        contenido.conteo_reproduccion = item.conteo_reproduccion;
                        for (int j = 0; j < Int32.Parse(acomodoDecimal[0]); j++)
                            Diccionario[i].Add(contenido);

                    }
                    //Agregamos los de la parte decimal
                    double espacioEntreLoops = 1 / (acomodo - (Int32.Parse(acomodoDecimal[0])));
                    int contados = Int32.Parse(espacioEntreLoops.ToString());
                    for (int i = 0; i < Diccionario.Count; i++)
                    {
                        Spot contenido = new Spot();

                        contenido.id_campana = item.Id_Campana;
                        contenido.Nombre = item.Nombre_Contenido;
                        contenido.duracion = item.Duracion;
                        contenido.tipo = item.Tipo_Contenido;
                        contenido.prioridad = item.prioridad;
                        contenido.Layout = item.Layout;
                        contenido.medio = item.Medio;
                        contenido.archivo = item.Archivo;
                        contenido.ffin = item.Fecha_Final;
                        contenido.subcategoria = item.Subcategoria;
                        contenido.conteo_reproduccion = item.conteo_reproduccion;
                        if (contados == espacioEntreLoops)
                        {
                            Diccionario[i].Add(contenido);
                            contados = 0;
                        }
                        contados++;

                    }
                }
            }
            catch (Exception e)
            {

                lstResultado.Items.Add(string.Format("Sucursal {0} revisar contenido {1} si esta completo aviso {2}", idSucursal,item.Id_Campana,e.Message));
            }
        }
        private void cmbNegocio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpFechaGeneracion_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbSubred_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}
