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
            this.label1 = new System.Windows.Forms.Label();
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
            this.groupTipoCarta.Location = new System.Drawing.Point(51, 11);
            this.groupTipoCarta.Name = "groupTipoCarta";
            this.groupTipoCarta.Size = new System.Drawing.Size(874, 89);
            this.groupTipoCarta.TabIndex = 0;
            this.groupTipoCarta.TabStop = false;
            this.groupTipoCarta.Text = "Selecciona el tipo de carta";
            // 
            // imgLoad
            // 
            this.imgLoad.Image = ((System.Drawing.Image)(resources.GetObject("imgLoad.Image")));
            this.imgLoad.Location = new System.Drawing.Point(739, 42);
            this.imgLoad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.imgLoad.Name = "imgLoad";
            this.imgLoad.Size = new System.Drawing.Size(44, 42);
            this.imgLoad.TabIndex = 18;
            this.imgLoad.TabStop = false;
            this.imgLoad.Visible = false;
            // 
            // btnRecargarRedes
            // 
            this.btnRecargarRedes.Location = new System.Drawing.Point(5, 23);
            this.btnRecargarRedes.Name = "btnRecargarRedes";
            this.btnRecargarRedes.Size = new System.Drawing.Size(154, 34);
            this.btnRecargarRedes.TabIndex = 17;
            this.btnRecargarRedes.Text = "Recargar redes";
            this.btnRecargarRedes.UseVisualStyleBackColor = true;
            this.btnRecargarRedes.Click += new System.EventHandler(this.btnRecargarRedes_Click);
            // 
            // cmbSubred
            // 
            this.cmbSubred.Enabled = false;
            this.cmbSubred.FormattingEnabled = true;
            this.cmbSubred.Location = new System.Drawing.Point(272, 38);
            this.cmbSubred.Name = "cmbSubred";
            this.cmbSubred.Size = new System.Drawing.Size(358, 21);
            this.cmbSubred.TabIndex = 9;
            this.cmbSubred.SelectedIndexChanged += new System.EventHandler(this.cmbSubred_SelectedIndexChanged);
            this.cmbSubred.TextChanged += new System.EventHandler(this.cmbSubred_TextChanged);
            // 
            // lblSubRed
            // 
            this.lblSubRed.AutoSize = true;
            this.lblSubRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubRed.Location = new System.Drawing.Point(218, 39);
            this.lblSubRed.Name = "lblSubRed";
            this.lblSubRed.Size = new System.Drawing.Size(43, 15);
            this.lblSubRed.TabIndex = 8;
            this.lblSubRed.Text = "Layout";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(736, 22);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Version";
            // 
            // dtpFechaGeneracion
            // 
            this.dtpFechaGeneracion.CustomFormat = "";
            this.dtpFechaGeneracion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtpFechaGeneracion.Location = new System.Drawing.Point(271, 67);
            this.dtpFechaGeneracion.Name = "dtpFechaGeneracion";
            this.dtpFechaGeneracion.Size = new System.Drawing.Size(358, 20);
            this.dtpFechaGeneracion.TabIndex = 6;
            this.dtpFechaGeneracion.Value = new System.DateTime(2022, 11, 1, 0, 0, 0, 0);
            this.dtpFechaGeneracion.ValueChanged += new System.EventHandler(this.dtpFechaGeneracion_ValueChanged);
            // 
            // lblFechaGeneracion
            // 
            this.lblFechaGeneracion.AutoSize = true;
            this.lblFechaGeneracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaGeneracion.Location = new System.Drawing.Point(143, 67);
            this.lblFechaGeneracion.Name = "lblFechaGeneracion";
            this.lblFechaGeneracion.Size = new System.Drawing.Size(123, 15);
            this.lblFechaGeneracion.TabIndex = 5;
            this.lblFechaGeneracion.Text = "Fecha de generación";
            // 
            // lblNegocio
            // 
            this.lblNegocio.AutoSize = true;
            this.lblNegocio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNegocio.Location = new System.Drawing.Point(231, 15);
            this.lblNegocio.Name = "lblNegocio";
            this.lblNegocio.Size = new System.Drawing.Size(30, 15);
            this.lblNegocio.TabIndex = 1;
            this.lblNegocio.Text = "Red";
            // 
            // cmbNegocio
            // 
            this.cmbNegocio.Enabled = false;
            this.cmbNegocio.FormattingEnabled = true;
            this.cmbNegocio.Location = new System.Drawing.Point(271, 15);
            this.cmbNegocio.Name = "cmbNegocio";
            this.cmbNegocio.Size = new System.Drawing.Size(358, 21);
            this.cmbNegocio.TabIndex = 0;
            this.cmbNegocio.SelectedIndexChanged += new System.EventHandler(this.cmbNegocio_SelectedIndexChanged);
            this.cmbNegocio.TextChanged += new System.EventHandler(this.cmbNegocio_TextChanged);
            // 
            // txtSeleccionarSucursales
            // 
            this.txtSeleccionarSucursales.Location = new System.Drawing.Point(51, 108);
            this.txtSeleccionarSucursales.Multiline = true;
            this.txtSeleccionarSucursales.Name = "txtSeleccionarSucursales";
            this.txtSeleccionarSucursales.Size = new System.Drawing.Size(875, 69);
            this.txtSeleccionarSucursales.TabIndex = 1;
            // 
            // btnSeleccionarSucursales
            // 
            this.btnSeleccionarSucursales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarSucursales.Location = new System.Drawing.Point(51, 184);
            this.btnSeleccionarSucursales.Name = "btnSeleccionarSucursales";
            this.btnSeleccionarSucursales.Size = new System.Drawing.Size(874, 37);
            this.btnSeleccionarSucursales.TabIndex = 2;
            this.btnSeleccionarSucursales.Text = "Seleccionar sucursales";
            this.btnSeleccionarSucursales.UseVisualStyleBackColor = true;
            this.btnSeleccionarSucursales.Click += new System.EventHandler(this.btnSeleccionarSucursales_Click);
            // 
            // lstSucursales
            // 
            this.lstSucursales.FormattingEnabled = true;
            this.lstSucursales.HorizontalScrollbar = true;
            this.lstSucursales.Location = new System.Drawing.Point(51, 275);
            this.lstSucursales.Name = "lstSucursales";
            this.lstSucursales.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lstSucursales.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSucursales.Size = new System.Drawing.Size(340, 95);
            this.lstSucursales.TabIndex = 3;
            this.lstSucursales.DoubleClick += new System.EventHandler(this.lstSucursales_DoubleClick);
            // 
            // lstProcesar
            // 
            this.lstProcesar.FormattingEnabled = true;
            this.lstProcesar.HorizontalScrollbar = true;
            this.lstProcesar.Location = new System.Drawing.Point(570, 275);
            this.lstProcesar.Name = "lstProcesar";
            this.lstProcesar.Size = new System.Drawing.Size(356, 95);
            this.lstProcesar.TabIndex = 4;
            // 
            // txtBuscarSucursales
            // 
            this.txtBuscarSucursales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscarSucursales.Location = new System.Drawing.Point(51, 247);
            this.txtBuscarSucursales.Name = "txtBuscarSucursales";
            this.txtBuscarSucursales.Size = new System.Drawing.Size(340, 21);
            this.txtBuscarSucursales.TabIndex = 5;
            this.txtBuscarSucursales.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBuscarSucursales_KeyUp);
            // 
            // txtBuscarProcesar
            // 
            this.txtBuscarProcesar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscarProcesar.Location = new System.Drawing.Point(570, 247);
            this.txtBuscarProcesar.Name = "txtBuscarProcesar";
            this.txtBuscarProcesar.Size = new System.Drawing.Size(356, 21);
            this.txtBuscarProcesar.TabIndex = 6;
            this.txtBuscarProcesar.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBuscarProcesar_KeyUp);
            // 
            // btnSeleccionadas
            // 
            this.btnSeleccionadas.Location = new System.Drawing.Point(398, 255);
            this.btnSeleccionadas.Name = "btnSeleccionadas";
            this.btnSeleccionadas.Size = new System.Drawing.Size(166, 34);
            this.btnSeleccionadas.TabIndex = 7;
            this.btnSeleccionadas.Text = "Seleccionadas";
            this.btnSeleccionadas.UseVisualStyleBackColor = true;
            this.btnSeleccionadas.Click += new System.EventHandler(this.btnSeleccionadas_Click);
            // 
            // btnSeleccionarTodas
            // 
            this.btnSeleccionarTodas.Location = new System.Drawing.Point(398, 295);
            this.btnSeleccionarTodas.Name = "btnSeleccionarTodas";
            this.btnSeleccionarTodas.Size = new System.Drawing.Size(166, 34);
            this.btnSeleccionarTodas.TabIndex = 8;
            this.btnSeleccionarTodas.Text = "Seleccionar todas las sucursales";
            this.btnSeleccionarTodas.UseVisualStyleBackColor = true;
            this.btnSeleccionarTodas.Click += new System.EventHandler(this.btnSeleccionarTodas_Click);
            // 
            // btnQuitarTodas
            // 
            this.btnQuitarTodas.Location = new System.Drawing.Point(398, 335);
            this.btnQuitarTodas.Name = "btnQuitarTodas";
            this.btnQuitarTodas.Size = new System.Drawing.Size(166, 34);
            this.btnQuitarTodas.TabIndex = 9;
            this.btnQuitarTodas.Text = "Quitar todas las sucursales";
            this.btnQuitarTodas.UseVisualStyleBackColor = true;
            this.btnQuitarTodas.Click += new System.EventHandler(this.btnQuitarTodas_Click);
            // 
            // lstResultado
            // 
            this.lstResultado.FormattingEnabled = true;
            this.lstResultado.HorizontalScrollbar = true;
            this.lstResultado.Location = new System.Drawing.Point(51, 410);
            this.lstResultado.Name = "lstResultado";
            this.lstResultado.Size = new System.Drawing.Size(875, 95);
            this.lstResultado.TabIndex = 10;
            // 
            // lblProcesar
            // 
            this.lblProcesar.AutoSize = true;
            this.lblProcesar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcesar.ForeColor = System.Drawing.Color.Red;
            this.lblProcesar.Location = new System.Drawing.Point(646, 392);
            this.lblProcesar.Name = "lblProcesar";
            this.lblProcesar.Size = new System.Drawing.Size(144, 15);
            this.lblProcesar.TabIndex = 11;
            this.lblProcesar.Text = "Número de sucursales: 0";
            // 
            // lblAvisoBottom
            // 
            this.lblAvisoBottom.AutoSize = true;
            this.lblAvisoBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvisoBottom.ForeColor = System.Drawing.SystemColors.Control;
            this.lblAvisoBottom.Location = new System.Drawing.Point(72, 509);
            this.lblAvisoBottom.Name = "lblAvisoBottom";
            this.lblAvisoBottom.Size = new System.Drawing.Size(244, 15);
            this.lblAvisoBottom.TabIndex = 12;
            this.lblAvisoBottom.Text = "Da un clic sobre el nombre para ver la carta";
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(267, 561);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(154, 34);
            this.btnProcesar.TabIndex = 13;
            this.btnProcesar.Text = "Procesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(428, 561);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(154, 34);
            this.btnCancelar.TabIndex = 14;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(590, 561);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(154, 34);
            this.btnCerrar.TabIndex = 15;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // txtProblemas
            // 
            this.txtProblemas.Location = new System.Drawing.Point(322, 622);
            this.txtProblemas.Multiline = true;
            this.txtProblemas.Name = "txtProblemas";
            this.txtProblemas.Size = new System.Drawing.Size(358, 125);
            this.txtProblemas.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(319, 509);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Cartas Generadas:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 798);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
    }
}

