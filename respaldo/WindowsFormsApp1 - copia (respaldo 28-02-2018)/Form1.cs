using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        int nColor = 0;
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.Text = "Servicio Atención Urgencia";

        }


        SqlConnection con = new SqlConnection(@"Data Source = 192.168.100.13; Initial Catalog = CLINIWIN; Persist Security Info=True;User ID = IVIDAL; Password=184114518");

     



        public void cargarEspera(DataGridView dataEspera)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT CONVERT(varchar(10),getdate()-INGRESO.ING_horaING ,108) TIEMPO_ESPERA,ING_CATEGORIZACION AS CATEGORIA,INGRESO.ING_DIAGING AS DIAGNOSTICO, PACIENTE.PAC_PATERNO +' '+PACIENTE.PAC_MATERNO+' '+ PACIENTE.PAC_NOMBRES as NOMBRE,INGRESO.PAC_NUMFICHA AS FICHA , INGRESO.ING_CORREL AS INGRESO FROM INGRESO (nolock) LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA LEFT JOIN PACIENTE  (nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL LEFT JOIN PRESTADOR  (nolock) ON INGRESO.PTD_RUT_TRAT = PRESTADOR.PTD_RUT WHERE FICHA.PAC_TIPO = 'U' AND ING_CATEGORIZACION IS NULL AND INGRESO.SER_CODIGO = 'URG'AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())AND INGRESO.ING_HORAALTA IS NULL AND INGRESO.ING_FECCIER IS NULL AND NOT  EXISTS ( SELECT * FROM CAMILLA_ASIGNADA  (nolock) WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA  AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL  AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL )ORDER BY TIEMPO_ESPERA DESC", con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                
                dataEspera.DataSource = table;

                
               
            }
            catch
            {
                MessageBox.Show("no se pudo consultar");

            }

        }

        public void cargarCategoria(DataGridView dataCategoria)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT CONVERT(varchar(10),getdate()-INGRESO.ING_HORA_CATEG ,108) TIEMPO_ESPERA,ING_CATEGORIZACION AS CATEGORIA,INGRESO.ING_DIAGING AS DIAGNOSTICO, PACIENTE.PAC_PATERNO +' '+PACIENTE.PAC_MATERNO+' '+ PACIENTE.PAC_NOMBRES as NOMBRE,INGRESO.PAC_NUMFICHA AS FICHA , INGRESO.ING_CORREL AS INGRESO FROM INGRESO (nolock) LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA LEFT JOIN PACIENTE  (nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL LEFT JOIN PRESTADOR  (nolock) ON INGRESO.PTD_RUT_TRAT = PRESTADOR.PTD_RUT WHERE FICHA.PAC_TIPO = 'U'	AND ING_CATEGORIZACION IS NOT NULL AND INGRESO.SER_CODIGO = 'URG' AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())    AND INGRESO.ING_HORAALTA IS NULL  AND INGRESO.ING_FECCIER IS NULL AND NOT  EXISTS ( SELECT *  FROM CAMILLA_ASIGNADA  (nolock)  WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA  AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL  AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL )ORDER BY TIEMPO_ESPERA DESC,INGRESO.ING_CATEGORIZACION ASC", con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataCategoria.DataSource = table;
            }
            catch
            {
                MessageBox.Show("no se pudo consultar");

            }

        }

        public void cargarBox(DataGridView dataBox)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT CONVERT(varchar(10),getdate()-CAMILLA_ASIGNADA.CMA_HORADESDE ,108) TIEMPO_ESPERA,ING_CATEGORIZACION AS CATEGORIA,INGRESO.ING_DIAGING AS DIAGNOSTICO, PACIENTE.PAC_PATERNO +' '+PACIENTE.PAC_MATERNO+' '+ PACIENTE.PAC_NOMBRES as NOMBRE,INGRESO.PAC_NUMFICHA AS FICHA , INGRESO.ING_CORREL AS INGRESO FROM INGRESO (nolock) JOIN CAMILLA_ASIGNADA (nolock) ON CAMILLA_ASIGNADA.PAC_NUMFICHA=INGRESO.PAC_NUMFICHA AND CAMILLA_ASIGNADA.ING_CORREL=INGRESO.ING_CORREL LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA LEFT JOIN PACIENTE  (nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL LEFT JOIN PRESTADOR  (nolock) ON INGRESO.PTD_RUT_TRAT = PRESTADOR.PTD_RUT WHERE FICHA.PAC_TIPO = 'U'AND INGRESO.SER_CODIGO = 'URG' AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE()) AND INGRESO.ING_HORAALTA IS NULL AND INGRESO.ING_FECCIER IS NULL AND  EXISTS ( SELECT * FROM CAMILLA_ASIGNADA  (nolock)  WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA  AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL )ORDER BY TIEMPO_ESPERA DESC,INGRESO.ING_CATEGORIZACION ASC", con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataBox.DataSource = table;
            }
            catch
            {
                MessageBox.Show("no se pudo consultar");

            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaxi_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
                } 

        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
       
        }
        
        private void btnRest_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
           
          
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]

        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarEspera(dataEspera);
            cargarBox(dataBox);
            cargarCategoria(dataCategoria);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataCategoria_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {
          
            dataCategoria.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataBox_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {
           
            dataBox.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
          

        }

        private void dataEspea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataEspera_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dataEspera.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
            
        }

        private void dataCategoria_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
