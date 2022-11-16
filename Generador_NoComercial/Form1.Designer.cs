namespace Generador_NoComercial
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupTipoCarta = new System.Windows.Forms.GroupBox();
            this.imgLoad = new System.Windows.Forms.PictureBox();
            this.btnRecargarRedes = new System.Windows.Forms.Button();
            this.cmbSubred = new System.Windows.Forms.ComboBox();
            this.lblSubRed = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.dtpFechaGeneracion = new System.Windows.Forms.DateTimePicker();
            this.lblFechaGeneracion = new System.Windows.Forms.Label();
            this.lblNegocio = new System.Windows.Forms.Label();
            this.cmbNegocio = new System.Windows.Forms.ComboBox();
            this.txtSeleccionarSucursales = new System.Windows.Forms.TextBox();
            this.btnSeleccionarSucursales = new System.Windows.Forms.Button();
            this.lstSucursales = new System.Windows.Forms.ListBox();
            this.lstProcesar = new System.Windows.Forms.ListBox();
            this.txtBuscarSucursales = new System.Windows.Forms.TextBox();
            this.txtBuscarProcesar = new System.Windows.Forms.TextBox();
            this.btnSeleccionadas = new System.Windows.Forms.Button();
            this.btnSeleccionarTodas = new System.Windows.Forms.Button();
            this.btnQuitarTodas = new System.Windows.Forms.Button();
            this.lstResultado = new System.Windows.Forms.ListBox();
            this.lblProcesar = new System.Windows.Forms.Label();
            this.lblAvisoBottom = new System.Windows.Forms.Label();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.txtProblemas = new System.Windows.Forms.TextBox();
            this.groupTipoCarta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // groupTipoCarta
            // 
            this.groupTipoCarta.Controls.Add(this.imgLoad);
            this.groupTipoCarta.Controls.Add(this.btnRecargarRedes);
            this.groupTipoCarta.Controls.Add(this.cmbSubred);
            this.groupTipoCarta.Controls.Add(this.lblSubRed);
            this.groupTipoCarta.Controls.Add(this.lblVersion);
            this.groupTipoCarta.Controls.Add(this.dtpFechaGeneracion);
            this.groupTipoCarta.Controls.Add(this.lblFechaGeneracion);
            this.groupTipoCarta.Controls.Add(this.lblNegocio);
            this.groupTipoCarta.Controls.Add(this.cmbNegocio);
            this.groupTipoCarta.Location = new System.Drawing.Point(68, 14);
            this.groupTipoCarta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupTipoCarta.Name = "groupTipoCarta";
            this.groupTipoCarta.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupTipoCarta.Size = new System.Drawing.Size(1165, 110);
            this.groupTipoCarta.TabIndex = 0;
            this.groupTipoCarta.TabStop = false;
            this.groupTipoCarta.Text = "Selecciona el tipo de carta";
            // 
            // imgLoad
            // 
            this.imgLoad.Image = ((System.Drawing.Image)(resources.GetObject("imgLoad.Image")));
            this.imgLoad.Location = new System.Drawing.Point(985, 52);
            this.imgLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imgLoad.Name = "imgLoad";
            this.imgLoad.Size = new System.Drawing.Size(59, 52);
            this.imgLoad.TabIndex = 18;
            this.imgLoad.TabStop = false;
            this.imgLoad.Visible = false;
            // 
            // btnRecargarRedes
            // 
            this.btnRecargarRedes.Location = new System.Drawing.Point(7, 28);
            this.btnRecargarRedes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRecargarRedes.Name = "btnRecargarRedes";
            this.btnRecargarRedes.Size = new System.Drawing.Size(205, 42);
            this.btnRecargarRedes.TabIndex = 17;
            this.btnRecargarRedes.Text = "Recargar redes";
            this.btnRecargarRedes.UseVisualStyleBackColor = true;
            this.btnRecargarRedes.Click += new System.EventHandler(this.btnRecargarRedes_Click);
            // 
            // cmbSubred
            // 
            this.cmbSubred.Enabled = false;
            this.cmbSubred.FormattingEnabled = true;
            this.cmbSubred.Location = new System.Drawing.Point(363, 47);
            this.cmbSubred.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbSubred.Name = "cmbSubred";
            this.cmbSubred.Size = new System.Drawing.Size(476, 24);
            this.cmbSubred.TabIndex = 9;
            this.cmbSubred.SelectedIndexChanged += new System.EventHandler(this.cmbSubred_SelectedIndexChanged);
            this.cmbSubred.TextChanged += new System.EventHandler(this.cmbSubred_TextChanged);
            // 
            // lblSubRed
            // 
            this.lblSubRed.AutoSize = true;
            this.lblSubRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubRed.Location = new System.Drawing.Point(291, 48);
            this.lblSubRed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubRed.Name = "lblSubRed";
            this.lblSubRed.Size = new System.Drawing.Size(55, 18);
            this.lblSubRed.TabIndex = 8;
            this.lblSubRed.Text = "Layout";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(981, 27);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(53, 16);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Version";
            // 
            // dtpFechaGeneracion
            // 
            this.dtpFechaGeneracion.CustomFormat = "";
            this.dtpFechaGeneracion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpFechaGeneracion.Location = new System.Drawing.Point(361, 82);
            this.dtpFechaGeneracion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpFechaGeneracion.Name = "dtpFechaGeneracion";
            this.dtpFechaGeneracion.Size = new System.Drawing.Size(476, 22);
            this.dtpFechaGeneracion.TabIndex = 6;
            this.dtpFechaGeneracion.Value = new System.DateTime(2022, 11, 4, 0, 0, 0, 0);
            this.dtpFechaGeneracion.ValueChanged += new System.EventHandler(this.dtpFechaGeneracion_ValueChanged);
            // 
            // lblFechaGeneracion
            // 
            this.lblFechaGeneracion.AutoSize = true;
            this.lblFechaGeneracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaGeneracion.Location = new System.Drawing.Point(191, 82);
            this.lblFechaGeneracion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFechaGeneracion.Name = "lblFechaGeneracion";
            this.lblFechaGeneracion.Size = new System.Drawing.Size(146, 18);
            this.lblFechaGeneracion.TabIndex = 5;
            this.lblFechaGeneracion.Text = "Fecha de generación";
            // 
            // lblNegocio
            // 
            this.lblNegocio.AutoSize = true;
            this.lblNegocio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNegocio.Location = new System.Drawing.Point(308, 18);
            this.lblNegocio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNegocio.Name = "lblNegocio";
            this.lblNegocio.Size = new System.Drawing.Size(35, 18);
            this.lblNegocio.TabIndex = 1;
            this.lblNegocio.Text = "Red";
            // 
            // cmbNegocio
            // 
            this.cmbNegocio.Enabled = false;
            this.cmbNegocio.FormattingEnabled = true;
            this.cmbNegocio.Location = new System.Drawing.Point(361, 18);
            this.cmbNegocio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbNegocio.Name = "cmbNegocio";
            this.cmbNegocio.Size = new System.Drawing.Size(476, 24);
            this.cmbNegocio.TabIndex = 0;
            this.cmbNegocio.SelectedIndexChanged += new System.EventHandler(this.cmbNegocio_SelectedIndexChanged);
            this.cmbNegocio.TextChanged += new System.EventHandler(this.cmbNegocio_TextChanged);
            // 
            // txtSeleccionarSucursales
            // 
            this.txtSeleccionarSucursales.Location = new System.Drawing.Point(68, 133);
            this.txtSeleccionarSucursales.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSeleccionarSucursales.Multiline = true;
            this.txtSeleccionarSucursales.Name = "txtSeleccionarSucursales";
            this.txtSeleccionarSucursales.Size = new System.Drawing.Size(1165, 84);
            this.txtSeleccionarSucursales.TabIndex = 1;
            // 
            // btnSeleccionarSucursales
            // 
            this.btnSeleccionarSucursales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarSucursales.Location = new System.Drawing.Point(68, 226);
            this.btnSeleccionarSucursales.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSeleccionarSucursales.Name = "btnSeleccionarSucursales";
            this.btnSeleccionarSucursales.Size = new System.Drawing.Size(1165, 46);
            this.btnSeleccionarSucursales.TabIndex = 2;
            this.btnSeleccionarSucursales.Text = "Seleccionar sucursales";
            this.btnSeleccionarSucursales.UseVisualStyleBackColor = true;
            this.btnSeleccionarSucursales.Click += new System.EventHandler(this.btnSeleccionarSucursales_Click);
            // 
            // lstSucursales
            // 
            this.lstSucursales.FormattingEnabled = true;
            this.lstSucursales.HorizontalScrollbar = true;
            this.lstSucursales.ItemHeight = 16;
            this.lstSucursales.Location = new System.Drawing.Point(68, 338);
            this.lstSucursales.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstSucursales.Name = "lstSucursales";
            this.lstSucursales.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lstSucursales.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSucursales.Size = new System.Drawing.Size(452, 116);
            this.lstSucursales.TabIndex = 3;
            this.lstSucursales.DoubleClick += new System.EventHandler(this.lstSucursales_DoubleClick);
            // 
            // lstProcesar
            // 
            this.lstProcesar.FormattingEnabled = true;
            this.lstProcesar.HorizontalScrollbar = true;
            this.lstProcesar.ItemHeight = 16;
            this.lstProcesar.Location = new System.Drawing.Point(760, 338);
            this.lstProcesar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstProcesar.Name = "lstProcesar";
            this.lstProcesar.Size = new System.Drawing.Size(473, 116);
            this.lstProcesar.TabIndex = 4;
            // 
            // txtBuscarSucursales
            // 
            this.txtBuscarSucursales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscarSucursales.Location = new System.Drawing.Point(68, 304);
            this.txtBuscarSucursales.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBuscarSucursales.Name = "txtBuscarSucursales";
            this.txtBuscarSucursales.Size = new System.Drawing.Size(452, 24);
            this.txtBuscarSucursales.TabIndex = 5;
            this.txtBuscarSucursales.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBuscarSucursales_KeyUp);
            // 
            // txtBuscarProcesar
            // 
            this.txtBuscarProcesar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscarProcesar.Location = new System.Drawing.Point(760, 304);
            this.txtBuscarProcesar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBuscarProcesar.Name = "txtBuscarProcesar";
            this.txtBuscarProcesar.Size = new System.Drawing.Size(473, 24);
            this.txtBuscarProcesar.TabIndex = 6;
            this.txtBuscarProcesar.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBuscarProcesar_KeyUp);
            // 
            // btnSeleccionadas
            // 
            this.btnSeleccionadas.Location = new System.Drawing.Point(531, 314);
            this.btnSeleccionadas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSeleccionadas.Name = "btnSeleccionadas";
            this.btnSeleccionadas.Size = new System.Drawing.Size(221, 42);
            this.btnSeleccionadas.TabIndex = 7;
            this.btnSeleccionadas.Text = "Seleccionadas";
            this.btnSeleccionadas.UseVisualStyleBackColor = true;
            this.btnSeleccionadas.Click += new System.EventHandler(this.btnSeleccionadas_Click);
            // 
            // btnSeleccionarTodas
            // 
            this.btnSeleccionarTodas.Location = new System.Drawing.Point(531, 363);
            this.btnSeleccionarTodas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSeleccionarTodas.Name = "btnSeleccionarTodas";
            this.btnSeleccionarTodas.Size = new System.Drawing.Size(221, 42);
            this.btnSeleccionarTodas.TabIndex = 8;
            this.btnSeleccionarTodas.Text = "Seleccionar todas las sucursales";
            this.btnSeleccionarTodas.UseVisualStyleBackColor = true;
            this.btnSeleccionarTodas.Click += new System.EventHandler(this.btnSeleccionarTodas_Click);
            // 
            // btnQuitarTodas
            // 
            this.btnQuitarTodas.Location = new System.Drawing.Point(531, 412);
            this.btnQuitarTodas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnQuitarTodas.Name = "btnQuitarTodas";
            this.btnQuitarTodas.Size = new System.Drawing.Size(221, 42);
            this.btnQuitarTodas.TabIndex = 9;
            this.btnQuitarTodas.Text = "Quitar todas las sucursales";
            this.btnQuitarTodas.UseVisualStyleBackColor = true;
            this.btnQuitarTodas.Click += new System.EventHandler(this.btnQuitarTodas_Click);
            // 
            // lstResultado
            // 
            this.lstResultado.FormattingEnabled = true;
            this.lstResultado.HorizontalScrollbar = true;
            this.lstResultado.ItemHeight = 16;
            this.lstResultado.Location = new System.Drawing.Point(68, 505);
            this.lstResultado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstResultado.Name = "lstResultado";
            this.lstResultado.Size = new System.Drawing.Size(1165, 116);
            this.lstResultado.TabIndex = 10;
            // 
            // lblProcesar
            // 
            this.lblProcesar.AutoSize = true;
            this.lblProcesar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcesar.ForeColor = System.Drawing.Color.Red;
            this.lblProcesar.Location = new System.Drawing.Point(861, 482);
            this.lblProcesar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProcesar.Name = "lblProcesar";
            this.lblProcesar.Size = new System.Drawing.Size(174, 18);
            this.lblProcesar.TabIndex = 11;
            this.lblProcesar.Text = "Número de sucursales: 0";
            // 
            // lblAvisoBottom
            // 
            this.lblAvisoBottom.AutoSize = true;
            this.lblAvisoBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvisoBottom.ForeColor = System.Drawing.Color.Red;
            this.lblAvisoBottom.Location = new System.Drawing.Point(96, 626);
            this.lblAvisoBottom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAvisoBottom.Name = "lblAvisoBottom";
            this.lblAvisoBottom.Size = new System.Drawing.Size(294, 18);
            this.lblAvisoBottom.TabIndex = 12;
            this.lblAvisoBottom.Text = "Da un clic sobre el nombre para ver la carta";
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(356, 690);
            this.btnProcesar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(205, 42);
            this.btnProcesar.TabIndex = 13;
            this.btnProcesar.Text = "Procesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(571, 690);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(205, 42);
            this.btnCancelar.TabIndex = 14;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(787, 690);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(205, 42);
            this.btnCerrar.TabIndex = 15;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // txtProblemas
            // 
            this.txtProblemas.Location = new System.Drawing.Point(429, 766);
            this.txtProblemas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProblemas.Multiline = true;
            this.txtProblemas.Name = "txtProblemas";
            this.txtProblemas.Size = new System.Drawing.Size(476, 153);
            this.txtProblemas.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 982);
            this.Controls.Add(this.txtProblemas);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.lblAvisoBottom);
            this.Controls.Add(this.lblProcesar);
            this.Controls.Add(this.lstResultado);
            this.Controls.Add(this.btnQuitarTodas);
            this.Controls.Add(this.btnSeleccionarTodas);
            this.Controls.Add(this.btnSeleccionadas);
            this.Controls.Add(this.txtBuscarProcesar);
            this.Controls.Add(this.txtBuscarSucursales);
            this.Controls.Add(this.lstProcesar);
            this.Controls.Add(this.lstSucursales);
            this.Controls.Add(this.btnSeleccionarSucursales);
            this.Controls.Add(this.txtSeleccionarSucursales);
            this.Controls.Add(this.groupTipoCarta);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Modulo para generar cartas de tiempos";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.groupTipoCarta.ResumeLayout(false);
            this.groupTipoCarta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLoad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupTipoCarta;
        private System.Windows.Forms.DateTimePicker dtpFechaGeneracion;
        private System.Windows.Forms.Label lblFechaGeneracion;
        private System.Windows.Forms.Label lblNegocio;
        private System.Windows.Forms.ComboBox cmbNegocio;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TextBox txtSeleccionarSucursales;
        private System.Windows.Forms.Button btnSeleccionarSucursales;
        private System.Windows.Forms.ListBox lstSucursales;
        private System.Windows.Forms.ListBox lstProcesar;
        private System.Windows.Forms.TextBox txtBuscarSucursales;
        private System.Windows.Forms.TextBox txtBuscarProcesar;
        private System.Windows.Forms.Button btnSeleccionadas;
        private System.Windows.Forms.Button btnSeleccionarTodas;
        private System.Windows.Forms.Button btnQuitarTodas;
        private System.Windows.Forms.ListBox lstResultado;
        private System.Windows.Forms.Label lblProcesar;
        private System.Windows.Forms.Label lblAvisoBottom;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.TextBox txtProblemas;
        private System.Windows.Forms.ComboBox cmbSubred;
        private System.Windows.Forms.Label lblSubRed;
        private System.Windows.Forms.Button btnRecargarRedes;
        private System.Windows.Forms.PictureBox imgLoad;
    }
}

