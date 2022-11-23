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
using System.Collections;

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
                                            spot.conteo_reproduccion = json[i].conteo_reproduccion;
                                            Diccionario_Comercial.Add(int.Parse(spot.id_campana), spot);
                                        }
                                    }


                                    /*
                                     * TO DO: puede existir un caso como en sendero que se ingresan contenidos de entretenimiento 
                                     * mayores a los que soporta la red pero a unas repeticiones mucho menores
                                     * esperamos que en el futuro esto no provoque un error ya que posiblemente
                                     * termine en loops sin entretenimiento.
                                     */
                                    #region DISTRIBUCION DE CONTENIDO EN LOOPS
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
                                    #endregion


                                    Console.WriteLine("Conteo de comercial despues de distribucion");
                                    foreach (var item in Diccionario_Comercial)
                                    {
                                        Console.WriteLine("campaña {0} repeticiones contratadas:{1}",item.Key,item.Value.conteo_reproduccion);
                                        var repeticionesObtenidas = 0;
                                        foreach (var item2 in Diccionario)
                                        {
                                            repeticionesObtenidas += item2.Value.Where(spot => spot.id_campana.Equals(item.Value.id_campana)).Count();
                                        }
                                        Console.WriteLine("campaña {0} repeticiones obtenidas:{1}", item.Key, repeticionesObtenidas);
                                    }

                                    #region AJUSTE DE TIEMPOS PARA LOOP Y MARCAR PARA ELIMINAR
                                    //Por loop vamos a ajustar tiempos antes de ordenar para que corresponda a los tiempos permitidos
                                    var TiempoComercialRestanteAnterior = 0;
                                    var TiempoEntretenimientoRestanteAnterior = 0;

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
                                         * 
                                         */
                                        /*
                                         * tambien tomar en cuenta que para el caso de que el loop le sobre un espacio de 10 o 20 segundos se añadira al siguiente loop
                                         * TENER EN CUENTA QUE ESTE FOR SOLO ES PARA SACAR EL CONTENIDO DE ENTRETENIMIENTO EXCEDENTE ES DECIR SOLO DEJAR LO NECESARIO PARA ORDENAR
                                         */
                                        /*
                                         * Sacaremos tambien la mayor prioridad que tienen los contenidos de entretenimiento del loop
                                         * para saber a cuales les tenemos que asegurar su repeticion y en caso de no poder asegurar la de todos, cual es la que tenemos 
                                         */
                                        var TotalTiempoComercialOcupado = 0;
                                        var TotalTiempoEntretenimientoOcuapdo = 0;//podemos estar sin esta
                                        var TiempoComercialRestante = 0;
                                        var TiempoEntretenimientoRestante = 0;
                                        var TotalComercialPermitido = 0;
                                        var TotalEntretenimientoPermitido = 0;
                                        var TotalTiempoLoop = 0;
                                        var TotalTiempoLoopRestante = 0;
                                        var PrioridadMaxima = 0;
                                        var sumaTotalContenidosEntretenimiento = 0;

                                        TotalTiempoComercialOcupado = (int)(Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL")).Sum(spot => spot.duracion));
                                        TotalComercialPermitido = red.tiempo_comercial;
                                        TotalEntretenimientoPermitido = red.tiempo_entretenimiento;
                                        TotalTiempoLoop = red.tiempo_comercial + red.tiempo_entretenimiento;
                                        TiempoComercialRestante = TotalComercialPermitido - TotalTiempoComercialOcupado;
                                        TotalTiempoLoopRestante = TotalTiempoLoop - TotalTiempoComercialOcupado;
                                        PrioridadMaxima = Diccionario[j].OrderByDescending(spot => spot.prioridad).FirstOrDefault().prioridad;
                                        Console.WriteLine("Prioridad maxima encontrada  en el loop {0}, {1}",j,PrioridadMaxima);
                                        sumaTotalContenidosEntretenimiento= (int)(Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO")).Sum(spot => spot.duracion));

                                        var prioridadsiguiente = PrioridadMaxima + 1;

                                        if (TiempoComercialRestante<0)
                                        {
                                            TiempoEntretenimientoRestante = TotalTiempoLoop - TotalTiempoComercialOcupado;
                                        }
                                        else
                                        {
                                            TiempoEntretenimientoRestante = TotalEntretenimientoPermitido;
                                        }

                                        if (TiempoComercialRestanteAnterior>0)
                                        {
                                            TiempoComercialRestante += TiempoComercialRestanteAnterior;
                                            TiempoComercialRestanteAnterior = 0;
                                            TotalTiempoLoopRestante += TiempoComercialRestanteAnterior;
                                        }
                                        if (TiempoEntretenimientoRestanteAnterior > 0)
                                        {
                                            TiempoEntretenimientoRestante += TiempoEntretenimientoRestanteAnterior;
                                            TiempoEntretenimientoRestanteAnterior = 0;
                                            TotalTiempoLoopRestante += TiempoEntretenimientoRestanteAnterior;
                                        }

                                        //Console.WriteLine("Datod de Loop {0} TotalTiempoComercialOcupado: {1}", j, TotalTiempoComercialOcupado);
                                        //Console.WriteLine("Datod de Loop {0} TotalTiempoEntretenimientoOcuapdo: {1}", j, TotalTiempoEntretenimientoOcuapdo);
                                        //Console.WriteLine("Datod de Loop {0} TiempoComercialRestante: {1}", j, TiempoComercialRestante);
                                        //Console.WriteLine("Datod de Loop {0} TiempoEntretenimientoRestante: {1}", j, TiempoEntretenimientoRestante);
                                        //Console.WriteLine("Datod de Loop {0} TotalComercialPermitido: {1}", j, TotalComercialPermitido);
                                        //Console.WriteLine("Datod de Loop {0} TotalEntretenimientoPermitido: {1}", j, TotalEntretenimientoPermitido);
                                        //Console.WriteLine("Datod de Loop {0} TotalTiempoLoop: {1}", j, TotalTiempoLoop);
                                        //Console.WriteLine("Datod de Loop {0} TotalTiempoLoopRestante: {1}", j, TotalTiempoLoopRestante);
                                        
                                        //Ya tenemos los tiempos ahora tenemos que trabajar el loop y vamos a discriminar por cada spot y decidir si se queda o no
                                        var posicion = 0;
                                        var vecesreiniciado = 0;
                                        var reducido = false;
                                        var ordenarPorPrioridad = 0;
                                        /*Antes tambien hay que revisar si la prioridad maxima es mayor a 0
                                         * porque la estrategia que emplearemos sera reemplazar la prioridad 0 por una mas a la prioridadmaxima
                                         * ya que la prioridad uno tiene mas peso sobre la dos y la dos sobre la tres y asi sucesivamente
                                         * pero la prioridad 0 siempre es la de menor peso en el contenido entretenimiento
                                         */
                                        //foreach (var ItemSpot in Diccionario[j])
                                        if (PrioridadMaxima>0)
                                        {
                                            ordenarPorPrioridad = 1;
                                            foreach (var itemSpot in Diccionario[j])
                                            {
                                                if (itemSpot.prioridad == 0)
                                                {
                                                    itemSpot.prioridad = prioridadsiguiente;
                                                }
                                            }
                                        }
                                        /*
                                         * Resulta que tenemos dos casos 
                                         * 1.- Cuando tu entretenimiento sobrepasa o es igual al tiempo comercial requerido 
                                         * 2.- cuando tu contenido de entretenimiento no alcanza a cubrir todo el espacio de entretenimiento
                                         *      digamos que solo tuvieras un contenido de entretenimiento
                                         */
                                        if (sumaTotalContenidosEntretenimiento>TotalEntretenimientoPermitido)
                                        {
                                            #region Tiempo Entretenimiento mayor a espacio de entretenimiento
                                            do
                                            {
                                                /*Tenemos que empezar revisando de acuerdo al tiempo de entretenimiento restante
                                                 * porque para el comercial restante se rellena con entretenimiento pero se agrega programatic
                                                 * para saber si esta correctamente reducido tenemos que tomar en cuenta lo siguiente
                                                 * 1.- ya no hay tiempo comercial restante
                                                 * 2.- ya no hay tiempo entretenimiento restante
                                                 * 3.- esta totalmente ocupado tiempo comercial y entretenimiento
                                                 * 4.- si hay tiempo sobrante añadirlo al siguiente loop
                                                 */

                                                /*Ya que estamos realizando un recorrido del mismo loop hasta tener todo ordenado o hasta que sean 3 veces si todo nuestro
                                                 * contenido de entretenimiento esta ocupado habra que reiniciar todo a desocupado
                                                 */
                                                if (Diccionario_Entretenimiento.Where(ItemSpot => ItemSpot.Value.ocupado == 0).Count() == 0)
                                                {
                                                    //Console.WriteLine("Se ocupo todo el contenido de entretenimiento al loop {0} y se reiniciara a 0", j);
                                                    ReiniciaDiccionarioSpotsUsado(ref Diccionario_Entretenimiento);
                                                }


                                                //Revisamos si hay que reducir de acuerdo a prioridad o hay que hacerlo sin prioridad en entretenimiento
                                                if (ordenarPorPrioridad == 1)
                                                {
                                                    #region Ordenado por priodidad
                                                    //Console.WriteLine("Loop {0} se ordenara con contenido de entretenimiento con valores de prioridad", j);
                                                    foreach (var itemSpot in Diccionario[j].OrderBy(spot => spot.prioridad))
                                                    {


                                                        if (itemSpot.tipo.ToUpper().Equals("ENTRETENIMIENTO"))
                                                        {


                                                            if ((TiempoEntretenimientoRestante - (int)itemSpot.duracion) >= 0 && itemSpot.ocupado == 0 && (Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado == 0 || itemSpot.prioridad != prioridadsiguiente) && TotalTiempoLoopRestante > 0)
                                                            {
                                                                /*si es entretenimiento 
                                                                 * y restando su duracion al tiempo entretenimiento restante es mayor a 0
                                                                 * y no esta ocupado
                                                                 * y el tiempo total del loop restante es mayor a 0
                                                                 * se deja el spot en la lista y se pone como ocupado
                                                                 * y se resta a los tiempos restantes
                                                                 */
                                                                Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado = 1;
                                                                TotalTiempoEntretenimientoOcuapdo += (int)itemSpot.duracion;
                                                                TiempoEntretenimientoRestante -= (int)itemSpot.duracion;
                                                                TotalTiempoLoopRestante -= (int)itemSpot.duracion;
                                                                itemSpot.eliminado = 0;
                                                                itemSpot.ocupado = 1;

                                                            }
                                                            else if (TiempoComercialRestante > 0 && itemSpot.ocupado == 0 && (TiempoComercialRestante - (int)itemSpot.duracion) >= 0 && Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado == 0 && TotalTiempoLoopRestante > 0)
                                                            {
                                                                /*Si no cumple la condicion anterior quiere decir que el espacio de entretenimiento se acabo
                                                                 * tenemos que revisar si quedo pendiente segundos para entonces rellenar con programatic
                                                                 * tambien vamos a ir reduciendo el tiempo loop restante y tiempo comercial restante
                                                                 * tambien aqui revisar si quedo entretenimiento restante ponerlo en la variable para sumarlo al siguiente loop en cada uno de sus tiempos
                                                                 * comercial
                                                                 * entretenimiento 
                                                                 * y totaltiempoloop
                                                                 */
                                                                Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado = 1;
                                                                TotalTiempoComercialOcupado += (int)itemSpot.duracion;
                                                                TiempoComercialRestante -= (int)itemSpot.duracion;
                                                                TotalTiempoLoopRestante -= (int)itemSpot.duracion;
                                                                itemSpot.programatic = 1;
                                                                itemSpot.eliminado = 0;
                                                                itemSpot.ocupado = 1;

                                                            }
                                                            else
                                                            {
                                                                /*En caso de que no entre en los casos anteriores quiere decir que
                                                                 * el tiempo del loop ya se termino y si queda algo restante tenemos que sumarlo al siguiente
                                                                 * en caso de que ya no haya mas espacio se retira 
                                                                 */
                                                                if (itemSpot.ocupado == 0 && itemSpot.tipo.ToUpper().Equals("ENTRETENIMIENTO"))
                                                                {
                                                                    itemSpot.eliminado = 1;
                                                                }
                                                            }
                                                        }

                                                        else if (vecesreiniciado >= 3)
                                                        {
                                                            reducido = true;
                                                        }
                                                        posicion++;
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Ordenado sin prioridad
                                                    //Console.WriteLine("Loop {0} se ordenara sin contenido de entretenimiento con valores de prioridad", j);
                                                    foreach (var itemSpot in Diccionario[j])
                                                    {


                                                        if (itemSpot.tipo.ToUpper().Equals("ENTRETENIMIENTO"))
                                                        {


                                                            if ((TiempoEntretenimientoRestante - (int)itemSpot.duracion) >= 0 && itemSpot.ocupado == 0 && Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado == 0 && TotalTiempoLoopRestante > 0)
                                                            {
                                                                /*si es entretenimiento 
                                                                 * y restando su duracion al tiempo entretenimiento restante es mayor a 0
                                                                 * y no esta ocupado
                                                                 * y el tiempo total del loop restante es mayor a 0
                                                                 * se deja el spot en la lista y se pone como ocupado
                                                                 * y se resta a los tiempos restantes
                                                                 */
                                                                Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado = 1;
                                                                TotalTiempoEntretenimientoOcuapdo += (int)itemSpot.duracion;
                                                                TiempoEntretenimientoRestante -= (int)itemSpot.duracion;
                                                                TotalTiempoLoopRestante -= (int)itemSpot.duracion;
                                                                itemSpot.ocupado = 1;
                                                                itemSpot.eliminado = 0;


                                                            }
                                                            else if (TiempoComercialRestante > 0 && itemSpot.ocupado == 0 && (TiempoComercialRestante - (int)itemSpot.duracion) >= 0 && Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado == 0 && TotalTiempoLoopRestante > 0)
                                                            {
                                                                /*Si no cumple la condicion anterior quiere decir que el espacio de entretenimiento se acabo
                                                                 * tenemos que revisar si quedo pendiente segundos para entonces rellenar con programatic
                                                                 * tambien vamos a ir reduciendo el tiempo loop restante y tiempo comercial restante
                                                                 * tambien aqui revisar si quedo entretenimiento restante ponerlo en la variable para sumarlo al siguiente loop en cada uno de sus tiempos
                                                                 * comercial
                                                                 * entretenimiento 
                                                                 * y totaltiempoloop
                                                                 */
                                                                Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado = 1;
                                                                TotalTiempoComercialOcupado += (int)itemSpot.duracion;
                                                                TiempoComercialRestante -= (int)itemSpot.duracion;
                                                                TotalTiempoLoopRestante -= (int)itemSpot.duracion;
                                                                itemSpot.programatic = 1;
                                                                itemSpot.eliminado = 0;
                                                                itemSpot.ocupado = 1;

                                                            }
                                                            else
                                                            {
                                                                /*En caso de que no entre en los casos anteriores quiere decir que
                                                                 * el tiempo del loop ya se termino y si queda algo restante tenemos que sumarlo al siguiente
                                                                 * en caso de que ya no haya mas espacio se retira 
                                                                 */
                                                                if (itemSpot.ocupado == 0 && itemSpot.tipo.ToUpper().Equals("ENTRETENIMIENTO"))
                                                                {
                                                                    itemSpot.eliminado = 1;
                                                                }

                                                            }
                                                        }
                                                        //if (posicion == (Diccionario[j].Count - 1) && TotalTiempoLoopRestante > 0 && vecesreiniciado < 3)
                                                        //{
                                                        //    ReiniciaDiccionarioSpotsUsado(ref Diccionario_Entretenimiento);
                                                        //    vecesreiniciado++;

                                                        //}
                                                        if (vecesreiniciado >= 3)
                                                        {
                                                            reducido = true;
                                                        }
                                                        posicion++;
                                                    }
                                                    #endregion
                                                }


                                                vecesreiniciado++;


                                            } while (!reducido);
                                            #endregion

                                        }
                                        else
                                        {
                                            #region Tiempo de entretenimiento menor a espacio de entretenimiento
                                            List<Spot> NuevaListaLoop=new List<Spot>();
                                            do
                                            {
                                              
                                                    #region Ordenado 
                                                    //Aqui no importa la prioridad porque hay muy poco contenido de entretenimiento lo que hara que se tenga que duplicar
                                                    foreach (var itemSpot in Diccionario[j].OrderBy(spot => spot.prioridad))
                                                    {
                                                        if (itemSpot.tipo.ToUpper().Equals("COMERCIAL")&&itemSpot.ocupado==0)
                                                        {
                                                            itemSpot.ocupado = 1;
                                                            NuevaListaLoop.Add(itemSpot);
                                                        }
                                                        
                                                        if (itemSpot.tipo.ToUpper().Equals("ENTRETENIMIENTO"))
                                                        {


                                                            if ((TiempoEntretenimientoRestante - (int)itemSpot.duracion) >= 0  && TotalTiempoLoopRestante > 0)
                                                            {
                                                                
                                                                Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado = 1;
                                                                TotalTiempoEntretenimientoOcuapdo += (int)itemSpot.duracion;
                                                                TiempoEntretenimientoRestante -= (int)itemSpot.duracion;
                                                                TotalTiempoLoopRestante -= (int)itemSpot.duracion;
                                                                itemSpot.eliminado = 0;
                                                                itemSpot.ocupado = 1;
                                                                Spot nuevoSpot=new Spot();
                                                                nuevoSpot.Id = itemSpot.Id;
                                                                nuevoSpot.id_campana = itemSpot.id_campana;
                                                                nuevoSpot.Nombre = itemSpot.Nombre;
                                                                nuevoSpot.duracion = itemSpot.duracion;
                                                                nuevoSpot.tipo = itemSpot.tipo;
                                                                nuevoSpot.categoria = itemSpot.categoria;
                                                                nuevoSpot.subcategoria = itemSpot.subcategoria;
                                                                nuevoSpot.prioridad = itemSpot.prioridad;
                                                                nuevoSpot.Layout = itemSpot.Layout;
                                                                nuevoSpot.conteo_reproduccion = itemSpot.conteo_reproduccion;
                                                                nuevoSpot.ocupado = itemSpot.ocupado;
                                                                nuevoSpot.medio = itemSpot.medio;
                                                                nuevoSpot.programatic = itemSpot.programatic;
                                                                nuevoSpot.ffin = itemSpot.ffin;
                                                                nuevoSpot.archivo = itemSpot.archivo;
                                                                nuevoSpot.repeticiones_usadas = itemSpot.repeticiones_usadas;
                                                                nuevoSpot.eliminado = itemSpot.eliminado;

                                                                NuevaListaLoop.Add(nuevoSpot);

                                                            }
                                                            else if (TiempoComercialRestante > 0  && (TiempoComercialRestante - (int)itemSpot.duracion) >= 0 && Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado == 0 && TotalTiempoLoopRestante > 0)
                                                            {
                                                                /*Si no cumple la condicion anterior quiere decir que el espacio de entretenimiento se acabo
                                                                 * tenemos que revisar si quedo pendiente segundos para entonces rellenar con programatic
                                                                 * tambien vamos a ir reduciendo el tiempo loop restante y tiempo comercial restante
                                                                 * tambien aqui revisar si quedo entretenimiento restante ponerlo en la variable para sumarlo al siguiente loop en cada uno de sus tiempos
                                                                 * comercial
                                                                 * entretenimiento 
                                                                 * y totaltiempoloop
                                                                 */
                                                                Diccionario_Entretenimiento[Int32.Parse(itemSpot.id_campana)].ocupado = 1;
                                                                TotalTiempoComercialOcupado += (int)itemSpot.duracion;
                                                                TiempoComercialRestante -= (int)itemSpot.duracion;
                                                                TotalTiempoLoopRestante -= (int)itemSpot.duracion;
                                                                //itemSpot.programatic = 1;
                                                                itemSpot.eliminado = 0;
                                                                itemSpot.ocupado = 1;
                                                                Spot nuevoSpot = new Spot();
                                                                nuevoSpot.Id = itemSpot.Id;
                                                                nuevoSpot.id_campana = itemSpot.id_campana;
                                                                nuevoSpot.Nombre = itemSpot.Nombre;
                                                                nuevoSpot.duracion = itemSpot.duracion;
                                                                nuevoSpot.tipo = itemSpot.tipo;
                                                                nuevoSpot.categoria = itemSpot.categoria;
                                                                nuevoSpot.subcategoria = itemSpot.subcategoria;
                                                                nuevoSpot.prioridad = itemSpot.prioridad;
                                                                nuevoSpot.Layout = itemSpot.Layout;
                                                                nuevoSpot.conteo_reproduccion = itemSpot.conteo_reproduccion;
                                                                nuevoSpot.ocupado = itemSpot.ocupado;
                                                                nuevoSpot.medio = itemSpot.medio;
                                                                nuevoSpot.programatic = 1;
                                                                nuevoSpot.ffin = itemSpot.ffin;
                                                                nuevoSpot.archivo = itemSpot.archivo;
                                                                nuevoSpot.repeticiones_usadas = itemSpot.repeticiones_usadas;
                                                                nuevoSpot.eliminado = itemSpot.eliminado;

                                                                NuevaListaLoop.Add(nuevoSpot);

                                                            }
                                                            else
                                                            {
                                                            /*Si ya no cae en ninguno de los anteriores casos que se rompa el ciclo
                                                             * ponemos que si se redujo
                                                             * y la lista creada la pasamos a la original
                                                             */
                                                                reducido = true;
                                                                Diccionario[j] = NuevaListaLoop;
                                                                break;
                                                            }
                                                        }

                                                       
                                                        
                                                    }
                                                    #endregion
                                                
                                               

                                            } while (!reducido);


                                            #endregion
                                        }

                                        //Console.WriteLine("Loop {0} reducido, quedando con {1} contenidos de entretenimiento y {2} comeriales"
                                        //    , j, Diccionario[j].Where(spot=>spot.tipo.ToUpper().Equals("ENTRETENIMIENTO")&&spot.eliminado==0).Count()
                                        //    , Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL") && spot.eliminado == 0).Count());
                                        //foreach (var item in Diccionario[j])
                                        //{
                                        //    Console.WriteLine("Loop {0} se quedo con idcampana:{1}", j, item.id_campana);
                                        //    Console.WriteLine("Loop {0} se quedo con Nombre:{1}", j, item.Nombre);
                                        //    Console.WriteLine("Loop {0} se quedo con ocupado:{1}", j, item.ocupado);
                                        //    Console.WriteLine("Loop {0} se quedo con eliminado:{1}", j, item.eliminado);
                                        //    Console.WriteLine("Loop {0} se quedo con tipo:{1}", j, item.tipo);
                                        //}
                                        //Console.WriteLine("Loop {0} se quedo con TiempoEntRestante:{1}", j, TiempoEntretenimientoRestante);
                                        //Console.WriteLine("Loop {0} se quedo con TiempoComRestante:{1}", j, TiempoComercialRestante);
                                        //Console.WriteLine("Loop {0} se quedo con TiempoLoopRestante:{1}", j, TotalTiempoLoopRestante);
                                        if (TiempoComercialRestante>0)
                                        {
                                            TiempoComercialRestanteAnterior = TiempoComercialRestante;
                                        }
                                        if (TiempoEntretenimientoRestante>0)
                                        {
                                            TiempoEntretenimientoRestanteAnterior = TiempoEntretenimientoRestante;
                                        }
                                        


                                    }
                                    #endregion

                                    Console.WriteLine("Conteo de comercial despues de ajuste de tiempos");
                                    foreach (var item in Diccionario_Comercial)
                                    {
                                        Console.WriteLine("campaña {0} repeticiones contratadas:{1}", item.Key, item.Value.conteo_reproduccion);
                                        var repeticionesObtenidas = 0;
                                        foreach (var item2 in Diccionario)
                                        {
                                            repeticionesObtenidas += item2.Value.Where(spot => spot.id_campana.Equals(item.Value.id_campana)).Count();
                                        }
                                        Console.WriteLine("campaña {0} repeticiones obtenidas:{1}", item.Key, repeticionesObtenidas);
                                    }
                                    #region ORDENAMIENTO
                                    //Vamos a iniciar con el ordenamiento por cada loop
                                    /*
                                     * Primero tendremos que identificar en que caso estamos
                                     * 1.- Contenidos entretenimiento son iguales o mayores a los contenidos comerciales
                                     * 2.- Contenidos de entretenimiento son menores a los comerciales
                                     * 
                                     */
                                    //reiniciaremos todo a ocupado = 0 para el diccionario que lleva todos nuestros contenidos ya que reutilizaremos la propiedad ocuapdo
                                    ReiniciaDiccionarioListasUsado(ref Diccionario);

                                    for (int j = 0; j < Diccionario.Count; j++)
                                    {
                                        var totalContenidosComerciales = 0;
                                        var totalContenidosEntretenimiento = 0;

                                        totalContenidosComerciales = Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL")).Count();
                                        totalContenidosEntretenimiento = Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO") && spot.eliminado == 0).Count();

                                        if (totalContenidosEntretenimiento >= totalContenidosComerciales && totalContenidosComerciales > 0)
                                        {
                                            #region  Ordenar caso 1 ent>=com
                                            //Creamos el diccionario con la cantidad de listas correspondientes a la cantidad de contenidos comerciales
                                            Dictionary<int, List<Spot>> Diccionariotemporal = new Dictionary<int, List<Spot>>();
                                            for (int i = 0; i < totalContenidosComerciales; i++)
                                            {
                                                List<Spot> NuevaLista = new List<Spot>();
                                                Diccionariotemporal.Add(i, NuevaLista);
                                            }

                                            //Acomodamos un comercial por cada una de las listas de nuestro nuevo diccionario
                                            var posicion = 0;
                                            foreach (var item in Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL")))
                                            {
                                                Diccionariotemporal[posicion].Add(item);
                                                posicion++;
                                            }

                                            /*
                                             * vamos a dividir los contenidos de entretenimiento entre los contenidos comerciales
                                             * y si nos sale con punto decimal vamos a tomar solo la parte entera y en la ultima lista
                                             * poner el resto de contenido de entretenimiento 
                                             * tambien revisar el contenido anterior su subcategoria para que no se repita
                                             */


                                            /*float cantidadATomar = (float)totalContenidosEntretenimiento / totalContenidosComerciales;
                                            int parteEntera = Int32.Parse(cantidadATomar.ToString().Split('.')[0]);
                                            */
                                            var posicionComercial = 0;

                                            foreach (var item in Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO") && spot.eliminado == 0))
                                            {
                                                Diccionariotemporal[posicionComercial].Add(item);
                                                if (posicionComercial == (totalContenidosComerciales - 1))
                                                {
                                                    posicionComercial = 0;
                                                }
                                                posicionComercial++;

                                            }



                                            /*
                                            for (int i = 0; i < Diccionariotemporal.Count; i++)
                                            {
                                                var tomados = 0;
                                                var vecesRecorridos = 0;
                                                var ordenado = false;
                                                var anteriorSubCategoria = "";


                                                do
                                                {

                                                    foreach (var item in Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO")&&spot.eliminado==0))
                                                    {
                                                        //vamos a separar el caso de cuando es la lista del ultimo contenido comercial aqui se mete el resto
                                                        if (i == (Diccionariotemporal.Count - 1))
                                                        {
                                                            if (tomados == 0)
                                                            {
                                                                Diccionariotemporal[i].Add(item);
                                                                anteriorSubCategoria = item.subcategoria;
                                                                item.ocupado = 1;
                                                                tomados++;
                                                            }
                                                            else if (vecesRecorridos < 1
                                                                && item.ocupado == 0
                                                                && !anteriorSubCategoria.Equals(item.subcategoria))
                                                            {
                                                                Diccionariotemporal[i].Add(item);
                                                                anteriorSubCategoria = item.subcategoria;
                                                                item.ocupado = 1;
                                                                tomados++;
                                                            }
                                                            else if (vecesRecorridos < 1
                                                                && item.ocupado == 0)
                                                            {
                                                                Diccionariotemporal[i].Add(item);
                                                                anteriorSubCategoria = item.subcategoria;
                                                                item.ocupado = 1;
                                                                tomados++;
                                                            }
                                                            if (Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO") && spot.eliminado == 0 &&spot.ocupado==0).Count()==0)
                                                            {
                                                                ordenado = true;
                                                                break;
                                                            }
                                                        }
                                                        else {
                                                            if (tomados == 0)
                                                            {
                                                                Diccionariotemporal[i].Add(item);
                                                                anteriorSubCategoria = item.subcategoria;
                                                                item.ocupado = 1;
                                                                tomados++;
                                                            }
                                                            else if (tomados < parteEntera
                                                                && vecesRecorridos < 1
                                                                && item.ocupado == 0
                                                                && !anteriorSubCategoria.Equals(item.subcategoria))
                                                            {
                                                                Diccionariotemporal[i].Add(item);
                                                                anteriorSubCategoria = item.subcategoria;
                                                                item.ocupado = 1;
                                                                tomados++;
                                                            }
                                                            else if (tomados < parteEntera
                                                                && vecesRecorridos < 1
                                                                && item.ocupado == 0)
                                                            {
                                                                Diccionariotemporal[i].Add(item);
                                                                anteriorSubCategoria = item.subcategoria;
                                                                item.ocupado = 1;
                                                                tomados++;
                                                            }
                                                            if (tomados == parteEntera)
                                                            {
                                                                ordenado = true;
                                                                break;

                                                            }

                                                        }


                                                        
                                                    }

                                                    vecesRecorridos++;
                                                    if (vecesRecorridos>=2)
                                                    {
                                                        ordenado = true;
                                                    }

                                                } while (!ordenado);
                                            }
                                            */
                                            //Hasta este punto ya tenemos acomodados el entretenimiento y el comercial 
                                            //ahora hay que juntarlo todo y pasarlo al Diccionario 1
                                            List<Spot> ListaOrdenada = new List<Spot>();

                                            foreach (var itemList in Diccionariotemporal)
                                            {
                                                foreach (var itemSpot in itemList.Value)
                                                {
                                                    ListaOrdenada.Add(itemSpot);
                                                }
                                            }
                                            Diccionario[j] = ListaOrdenada;

                                            #endregion
                                        }
                                        else if (totalContenidosEntretenimiento < totalContenidosComerciales)
                                        {
                                            #region  Ordenar caso 2 ent<com
                                            //Creamos el diccionario con la cantidad de listas correspondientes a la cantidad de contenidos entretenimiento
                                            Dictionary<int, List<Spot>> Diccionariotemporal = new Dictionary<int, List<Spot>>();
                                            for (int i = 0; i < totalContenidosEntretenimiento; i++)
                                            {
                                                List<Spot> NuevaLista = new List<Spot>();
                                                Diccionariotemporal.Add(i, NuevaLista);
                                            }

                                            //Acomodamos un entretenimiento por cada una de las listas de nuestro nuevo diccionario
                                            var posicion = 0;
                                            foreach (var item in Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO") && spot.eliminado == 0))
                                            {
                                                Diccionariotemporal[posicion].Add(item);
                                                posicion++;
                                            }
                                            /*
                                             * vamos a dividir los contenidos de COMERCIALES entre los contenidos ENTRETENIMIENTO
                                             * y si nos sale con punto decimal vamos a tomar solo la parte entera y en la ultima lista
                                             * poner el resto de contenido de COMERCIAL 
                                             *
                                             */
                                            /*float cantidadATomar =  totalContenidosComerciales/ totalContenidosEntretenimiento ;
                                            int parteEntera = Int32.Parse(cantidadATomar.ToString().Split('.')[0]);
                                            */
                                            var posicionEnt = 0;

                                            foreach (var item in Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL")))
                                            {
                                                Diccionariotemporal[posicionEnt].Add(item);
                                                if (posicionEnt == (totalContenidosEntretenimiento - 1))
                                                {
                                                    posicionEnt = 0;
                                                }
                                                posicionEnt++;

                                            }
                                            /*
                                            for (int i = 0; i < Diccionariotemporal.Count; i++)
                                            {
                                                var tomados = 0;
                                                var vecesRecorridos = 0;
                                                var ordenado = false;
                                                var anteriorSubCategoria = "";
                                                do
                                                {

                                                    foreach (var item in Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL")))
                                                    {
                                                        //vamos a separar el caso de cuando es la lista del ultimo contenido comercial aqui se mete el resto
                                                        if (i == (Diccionariotemporal.Count - 1))
                                                        {
                                                            Diccionariotemporal[i].Add(item);
                                                            item.ocupado = 1;
                                                            if (Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("COMERCIAL")  && spot.ocupado == 0).Count() == 0)
                                                            {
                                                                ordenado = true;
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (item.ocupado==0)
                                                            {
                                                                Diccionariotemporal[i].Add(item);
                                                                item.ocupado = 1;
                                                                tomados++;
                                                            }
                                                           
                                                            if (tomados==parteEntera)
                                                            {
                                                                ordenado = true;
                                                                break;
                                                            }

                                                        }



                                                    }

                                                    vecesRecorridos++;
                                                    if (vecesRecorridos >= 2)
                                                    {
                                                        ordenado = true;
                                                    }

                                                } while (!ordenado);
                                            }

                                            */
                                            //Hasta este punto ya tenemos acomodados el entretenimiento y el comercial 
                                            //ahora hay que juntarlo todo y pasarlo al Diccionario 1
                                            List<Spot> ListaOrdenada = new List<Spot>();

                                            foreach (var itemList in Diccionariotemporal)
                                            {
                                                foreach (var itemSpot in itemList.Value)
                                                {
                                                    ListaOrdenada.Add(itemSpot);
                                                }
                                            }
                                            Diccionario[j] = ListaOrdenada;

                                            #endregion
                                        }
                                        else if(totalContenidosComerciales==0) {
                                            //quiere decir que solo hay contenido de entretenimiento
                                            var totalContenidosEntretenimientoDiferentes = Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO") && spot.eliminado == 0).Select(spot=>spot.id_campana).Distinct().Count();
                                            Dictionary<int, List<Spot>> Diccionariotemporal = new Dictionary<int, List<Spot>>();
                                            for (int i = 0; i < totalContenidosEntretenimientoDiferentes; i++)
                                            {
                                                List<Spot> NuevaLista = new List<Spot>();
                                                Diccionariotemporal.Add(i, NuevaLista);
                                            }

                                            var posicionEnt = 0;

                                            foreach (var item in Diccionario[j].Where(spot => spot.tipo.ToUpper().Equals("ENTRETENIMIENTO") && spot.eliminado == 0))
                                            {
                                                Diccionariotemporal[posicionEnt].Add(item);
                                                if (posicionEnt == (totalContenidosEntretenimientoDiferentes - 1))
                                                {
                                                    posicionEnt = 0;
                                                }
                                                posicionEnt++;

                                            }

                                        }
                                    }

                                    #endregion

                                    Console.WriteLine("Conteo de comercial despues de ordenamiento");
                                   
                                    foreach (var item in Diccionario_Comercial)
                                    {
                                        Console.WriteLine("campaña {0} repeticiones contratadas:{1}", item.Key, item.Value.conteo_reproduccion);
                                        var repeticionesObtenidas = 0;
                                        foreach (var item2 in Diccionario)
                                        {
                                            repeticionesObtenidas += item2.Value.Where(spot => spot.id_campana.Equals(item.Value.id_campana)).Count();
                                        }
                                        Console.WriteLine("campaña {0} repeticiones obtenidas:{1}", item.Key, repeticionesObtenidas);
                                    }
                                    #region ESCRITURA EN ARCHIVO
                                    do
                                    {
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
                                                    //if (conteoReproduccion>RepeticionesUsadas)
                                                    //{
                                                        writer.WriteLine(string.Format("HORA&=&{0}&,&MEDIO&=&{1}&,&TIEMPO&=&{2}&,&" + SpotUrlArchivo + "&=&{3}&,&DURACION&=&{4}&,&CAMPANA&=&{5}" +
                                                            "&,&CONTENT&=&{6}" + ffin + programatic + lyout + subcat, hora, Diccionario[i][j].medio, Diccionario[i][j].tipo, Diccionario[i][j].archivo, Diccionario[i][j].duracion, Diccionario[i][j].id_campana,
                                                            Diccionario[i][j].Nombre));
                                                        hora = hora + TimeSpan.FromSeconds(Diccionario[i][j].duracion);
                                                        Diccionario_Comercial[Int32.Parse(Diccionario[i][j].id_campana)].repeticiones_usadas++;
                                                         
                                                    //}
                                                    
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
                                    #endregion

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




        private void ReiniciaDiccionarioListasUsado(ref Dictionary<int,List<Spot>> DiccionarioALimpiar)
        {
            foreach (var itemlist in DiccionarioALimpiar)
            {
                foreach (var itemSpot in itemlist.Value)
                {
                    itemSpot.ocupado = 0;
                }
            }
        }

        private void ReiniciaDiccionarioSpotsUsado(ref Dictionary<int, Spot> DiccionarioALimpiar)
        {
            foreach (var itemSpot in DiccionarioALimpiar)
            {
                
                    itemSpot.Value.ocupado = 0;
                
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
            float formula = (float)layout / conteo;
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
            if (item.Id_Campana.Equals("1836") )
            {
                Console.WriteLine("En este contenido da error");
            }
            try
            {
                string[] acomodoDecimal = dividirDecimales(acomodo);
                //revisar si tiene decimales ya que si tiene tendremos que tendremos que poner un spot cada cierto tiempo
                //en caso contrario podemos solo agregar la cantidad de veces el spot en el loop
             
                if (acomodoDecimal.Length == 1)
                {
                    for (int i = 0; i < Diccionario.Count; i++)
                    {
                        
                        for (int j = 0; j < Int32.Parse(acomodo.ToString()); j++)
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
                }
                else
                {
                    //Distribuimos en cada loop los spots de la parte entera
                    for (int i = 0; i < Diccionario.Count; i++)
                    {
                        
                        for (int j = 0; j < Int32.Parse(acomodoDecimal[0]); j++)
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
