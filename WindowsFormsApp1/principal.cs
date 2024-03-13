using System;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Drawing;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            //this.Text = "Servicio Atención Urgencia";
            



        }

        
       
        
        SqlConnection con = new SqlConnection(@"Data Source = 192.168.100.53; Initial Catalog = CLINIWIN; Persist Security Info=True;User ID = TURGENCIA; Password=184114518");

        
   
        public void cargarCantidades(object sender, EventArgs e)
        {
            con.Open();

            /// CANTIDAD ESPERA
            SqlCommand es = new SqlCommand(@"SELECT COUNT(INGRESO.PAC_NUMFICHA) AS ESPERA
                                            FROM INGRESO (nolock) 
                                            LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 

                                            WHERE FICHA.PAC_TIPO = 'U' 
                                            AND INGRESO.SER_CODIGO = 'URG'
                                            AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())
                                            AND INGRESO.ING_HORAALTA IS NULL 
                                            AND INGRESO.ING_FECCIER IS NULL 
                                            AND NOT  EXISTS ( SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL FROM CAMILLA_ASIGNADA  (nolock) 
				                                            WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA  AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL  AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL )
                                            AND NOT EXISTS ( SELECT TOP 1 HCU_ESI.PAC_NUMFICHA,HCU_ESI.ING_CORREL FROM HCU_ESI WHERE INGRESO.PAC_NUMFICHA = HCU_ESI.PAC_NUMFICHA  AND INGRESO.ING_CORREL = HCU_ESI.ING_CORREL  AND HCU_ESI.HE_ESI IS NOT NULL )
                                            ", con);
            

            es.ExecuteNonQuery();
            int ess = ((int)es.ExecuteScalar());
            espera.Text = ess.ToString();


            /// CANTIDAD EN BOX
         
            SqlCommand bx = new SqlCommand(@"SELECT COUNT(INGRESO.PAC_NUMFICHA)  AS BOX
                                            FROM INGRESO (nolock) 
                                            LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 
                                            WHERE FICHA.PAC_TIPO = 'U' 
                                            AND INGRESO.SER_CODIGO = 'URG' 
                                            AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())
                                            AND INGRESO.ING_HORAALTA IS NULL 
                                            AND INGRESO.ING_FECCIER IS NULL 
                                            AND EXISTS (SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL FROM CAMILLA_ASIGNADA (nolock) WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL AND CAMILLA_ASIGNADA.CMA_HORAHASTA IS NULL)
                                            ", con);
            
            bx.ExecuteNonQuery();
            int bxx = ((int)bx.ExecuteScalar());
            box.Text = bxx.ToString();


            /// CANTIDAD CATEGORIZADOS
            SqlCommand ct = new SqlCommand(@"SELECT count(INGRESO.PAC_NUMFICHA) AS CATEGORIZADOS
                                            FROM INGRESO (nolock) 
                                            LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 
                                            WHERE FICHA.PAC_TIPO = 'U'  
                                            AND INGRESO.SER_CODIGO = 'URG' 
                                            AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())
                                            AND INGRESO.ING_HORAALTA IS NULL 
                                            AND INGRESO.ING_FECCIER IS NULL 
                                            AND NOT  EXISTS ( SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL 	 FROM CAMILLA_ASIGNADA  (nolock) 	 WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL 	 AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL) 
                                            AND EXISTS ( SELECT TOP 1 HCU_ESI.PAC_NUMFICHA,HCU_ESI.ING_CORREL FROM HCU_ESI WHERE INGRESO.PAC_NUMFICHA = HCU_ESI.PAC_NUMFICHA  AND INGRESO.ING_CORREL = HCU_ESI.ING_CORREL  AND HCU_ESI.HE_ESI IS NOT NULL )
                                            ", con);
           

            ct.ExecuteNonQuery();
            int ctt = ((int)ct.ExecuteScalar());
            categoria.Text = ctt.ToString();

            ///CANTIDAD PEDIATRIA
            SqlCommand bxn = new SqlCommand(@"SELECT COUNT(INGRESO.PAC_NUMFICHA)  AS PEDIATRIA
                                                FROM INGRESO(nolock) 
                                                LEFT JOIN FICHA(nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 
                                                LEFT JOIN PACIENTE(nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL 
                                                WHERE FICHA.PAC_TIPO = 'U' 
                                                and datediff(MONTH, PAC_FECNAC, GETDATE()) < 179 
                                                AND INGRESO.SER_CODIGO = 'URG' 
                                                AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())
                                                AND INGRESO.ING_HORAALTA IS NULL 
                                                AND INGRESO.ING_FECCIER IS NULL 
                                                AND EXISTS(SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL  FROM CAMILLA_ASIGNADA(nolock) WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL AND CAMILLA_ASIGNADA.CMA_HORAHASTA IS NULL)          
                                                AND EXISTS ( SELECT TOP 1 HCU_ESI.PAC_NUMFICHA,HCU_ESI.ING_CORREL FROM HCU_ESI WHERE INGRESO.PAC_NUMFICHA = HCU_ESI.PAC_NUMFICHA  AND INGRESO.ING_CORREL = HCU_ESI.ING_CORREL  AND HCU_ESI.HE_ESI IS NOT NULL )
                                                ", con);
            

            bxn.ExecuteNonQuery();
            int bxnn = ((int)bxn.ExecuteScalar());
            pediatria.Text = bxnn.ToString();

            /// CANTIDAD ADULTOS 
            SqlCommand bxa = new SqlCommand(@"SELECT COUNT(INGRESO.PAC_NUMFICHA)  AS ADULTOS
                                                FROM INGRESO (nolock) 
                                                LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 
                                                LEFT JOIN PACIENTE  (nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL 
                                                WHERE FICHA.PAC_TIPO = 'U' 
                                                and datediff(MONTH,PAC_FECNAC,GETDATE()) >= 179 
                                                AND INGRESO.SER_CODIGO = 'URG' 
                                                AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())
                                                AND INGRESO.ING_HORAALTA IS NULL 
                                                AND INGRESO.ING_FECCIER IS NULL 
                                                AND EXISTS (SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL  FROM CAMILLA_ASIGNADA (nolock) WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL AND CAMILLA_ASIGNADA.CMA_HORAHASTA IS NULL)
                                                AND EXISTS ( SELECT TOP 1 HCU_ESI.PAC_NUMFICHA,HCU_ESI.ING_CORREL FROM HCU_ESI WHERE INGRESO.PAC_NUMFICHA = HCU_ESI.PAC_NUMFICHA  AND INGRESO.ING_CORREL = HCU_ESI.ING_CORREL  AND HCU_ESI.HE_ESI IS NOT NULL )

                                                ", con);
            

            bxa.ExecuteNonQuery();
            int bxaa = ((int)bxa.ExecuteScalar());
            adultos.Text = bxaa.ToString();



            con.Close();



        }

        // CARGA TABLA ESPERA
        public void cargarEspera(DataGridView dataEspera)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT CONVERT(varchar(10),getdate()-INGRESO.ING_horaING ,108) TIEMPO,
                                                                PACIENTE.PAC_NOMBRES +' '+ PACIENTE.PAC_PATERNO +', '+  CONVERT(varchar(100), (DATEDIFF(MONTH,PACIENTE.PAC_FECNAC,GETDATE()))/12)+' Años '+ CONVERT(varchar(100),(DATEDIFF(MONTH,PACIENTE.PAC_FECNAC,GETDATE())%12)) +' Meses' +', '+  PACIENTE.PAC_RUT as PACIENTE,
                                                                INGRESO.ING_DIAGING as DIAGNÓSTICO 
                                                                FROM INGRESO (nolock) 
                                                                LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 
                                                                LEFT JOIN PACIENTE  (nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL
                                                                LEFT JOIN PRESTADOR  (nolock) ON INGRESO.PTD_RUT_TRAT = PRESTADOR.PTD_RUT 
                                                                WHERE FICHA.PAC_TIPO = 'U' 
                                                                AND INGRESO.SER_CODIGO = 'URG'
                                                                AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())
                                                                AND INGRESO.ING_HORAALTA IS NULL 
                                                                AND INGRESO.ING_FECCIER IS NULL 
                                                                AND NOT  EXISTS ( SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL FROM CAMILLA_ASIGNADA  (nolock) WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA  AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL  AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL )
                                                                AND NOT EXISTS ( SELECT TOP 1 HCU_ESI.PAC_NUMFICHA,HCU_ESI.ING_CORREL FROM HCU_ESI WHERE INGRESO.PAC_NUMFICHA = HCU_ESI.PAC_NUMFICHA  AND INGRESO.ING_CORREL = HCU_ESI.ING_CORREL  AND HCU_ESI.HE_ESI IS NOT NULL )
                                                                ORDER BY TIEMPO DESC", con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                
                dataEspera.DataSource = table;
                foreach (DataGridViewColumn Col in dataEspera.Columns)
                {
                    Col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                foreach (DataGridViewRow row in dataEspera.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;

                }

                


            }
            catch
            {
               // MessageBox.Show("no se pudo consultar");

            }

        }

        // CARGA TABLA CATEGORIA
        public void cargarCategoria(DataGridView dataCateg)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT  
                                                                'En Espera: '+ CONVERT(varchar(10),getdate()-HCU_ESI.HE_FEC ,108)+', '+ 'Tiempo Total: ' + CONVERT(varchar(10),getdate()-INGRESO.ING_horaING ,108)+', '+  HCU_ESI.HE_ESI as TIEMPO,
                                                                PACIENTE.PAC_NOMBRES +' '+ PACIENTE.PAC_PATERNO +', '+ CONVERT(varchar(100), (DATEDIFF(MONTH,PACIENTE.PAC_FECNAC,GETDATE()))/12)+' Años '+ CONVERT(varchar(100),(DATEDIFF(MONTH,PACIENTE.PAC_FECNAC,GETDATE())%12)) +' Meses' +', '+  PACIENTE.PAC_RUT   as PACIENTE,
                                                                INGRESO.ING_DIAGING  as DIAGNOSTICO,
                                                                PRESTADOR.PTD_NOMBRE AS MÉDICO 
                                                                FROM INGRESO (nolock) 
                                                                LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 
                                                                LEFT JOIN PACIENTE  (nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL 
                                                                LEFT JOIN PRESTADOR  (nolock) ON INGRESO.PTD_RUT_TRAT = PRESTADOR.PTD_RUT 
                                                                LEFT JOIN HCU_ESI (nolock) ON INGRESO.PAC_NUMFICHA=HCU_ESI.PAC_NUMFICHA AND INGRESO.ING_CORREL=HCU_ESI.ING_CORREL
                                                                WHERE FICHA.PAC_TIPO = 'U'	
                                                                AND INGRESO.SER_CODIGO = 'URG' 
                                                                AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())    
                                                                AND INGRESO.ING_HORAALTA IS NULL  
                                                                AND INGRESO.ING_FECCIER IS NULL 
                                                                AND NOT  EXISTS ( SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL  FROM CAMILLA_ASIGNADA  (nolock)  WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA  AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL  AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL ) 
                                                                AND EXISTS ( SELECT TOP 1 HCU_ESI.PAC_NUMFICHA,HCU_ESI.ING_CORREL FROM HCU_ESI WHERE INGRESO.PAC_NUMFICHA = HCU_ESI.PAC_NUMFICHA  AND INGRESO.ING_CORREL = HCU_ESI.ING_CORREL  AND HCU_ESI.HE_ESI IS NOT NULL )
                                                                ORDER BY INGRESO.ING_CATEGORIZACION ASC,INGRESO.ING_horaING ASC", con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataCateg.DataSource = table;
                //int contador = table.Rows.Count();
                foreach (DataGridViewColumn Col in dataCateg.Columns)
                {
                    Col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                foreach (DataGridViewRow row in dataCateg.Rows)
                {

                    if ((Convert.ToString(row.Cells["tiempo"].Value)).Contains ("ESI4"))
                    {

                        row.DefaultCellStyle.BackColor = Color.Green;
                        row.DefaultCellStyle.ForeColor = Color.White;

                      

                    }



                        

                        if ((Convert.ToString(row.Cells["tiempo"].Value)).Contains("ESI3"))
                    {
                            row.DefaultCellStyle.BackColor = Color.Yellow;
                             row.DefaultCellStyle.ForeColor = Color.Black;


                    }

                        if ((Convert.ToString(row.Cells["tiempo"].Value)).Contains("ESI5"))
                    {

                            row.DefaultCellStyle.BackColor = Color.Blue;
                            row.DefaultCellStyle.ForeColor = Color.White;



                        }

                        if ((Convert.ToString(row.Cells["tiempo"].Value)).Contains("ESI2"))
                    {

                            row.DefaultCellStyle.BackColor = Color.DarkGoldenrod;
                            row.DefaultCellStyle.ForeColor = Color.White;



                        }

                        if ((Convert.ToString(row.Cells["tiempo"].Value)).Contains("ESI1"))
                    {

                            row.DefaultCellStyle.BackColor = Color.Red;
                            row.DefaultCellStyle.ForeColor = Color.White;



                        }

                    if ((Convert.ToString(row.Cells["tiempo"].Value)).Contains("NP"))
                    {

                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;



                    }




                } 

            }
            catch
            {
               // MessageBox.Show("no se pudo consultar");

            }

        }

        //CARGA TABLA BOX
        public void cargarBox(DataGridView dataBox)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT 'En Box: ' + CONVERT(varchar(10),getdate()-PRIMERAHORA.CMA_HORADESDE ,108) as TIEMPO ,
                                                            PACIENTE.PAC_NOMBRES+' '+ PACIENTE.PAC_PATERNO +', '+ CONVERT(varchar(100), (DATEDIFF(MONTH,PACIENTE.PAC_FECNAC,GETDATE()))/12)+' Años '+ CONVERT(varchar(100),(DATEDIFF(MONTH,PACIENTE.PAC_FECNAC,GETDATE())%12)) +' Meses' +', '+  PACIENTE.PAC_RUT  as PACIENTE,
                                                            INGRESO.ING_DIAGING AS DIAGNOSTICO,
                                                            ULTIMOBOX.CML_CODIGO + ', ' + HCU_ESI.HE_ESI AS BOX,

                                                            case when PRIMERAHORA.CMA_HORADESDE > URGENCIA_ANAMNESIS.UAN_HORA THEN  'Atendido, Box Cargado Tarde' else (case when URGENCIA_ANAMNESIS.UAN_HORA is NOT NULL then 'Atendido en ' +  CONVERT(varchar(10),URGENCIA_ANAMNESIS.UAN_HORA - PRIMERAHORA.CMA_HORADESDE,108)else CONVERT(varchar(10),getdate() - PRIMERAHORA.CMA_HORADESDE,108) + ' Sin Atencion' END)end as ANAMNESIS,

                                                            PRESTADOR.PTD_NOMBRE AS MÉDICO,

                                                            Case when HORA_ENFERMERA.RPE_HORA  < PRIMERAHORA.CMA_HORADESDE THEN 'Atendido, Box Cargado Tarde' else (case when HORA_ENFERMERA.RPE_HORA IS NULL THEN  CONVERT(varchar(10),GETDATE()-PRIMERAHORA.CMA_HORADESDE ,108) + ' Sin Atencion' else 'Atendido en ' + CONVERT(varchar(10),HORA_ENFERMERA.RPE_HORA-PRIMERAHORA.CMA_HORADESDE ,108)end) END as PROCEDIMIENTOS,
                                                            AM_USUARIO.USU_NOMBRES+' '+ AM_USUARIO.USU_PATERNO AS ENFERMERA

                                                            FROM INGRESO (nolock) 
                                                            LEFT JOIN URGENCIA_ANAMNESIS (NOLOCK) ON  URGENCIA_ANAMNESIS.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA AND URGENCIA_ANAMNESIS.ING_CORREL = INGRESO.ING_CORREL 
                                                            LEFT JOIN FICHA  (nolock) ON FICHA.PAC_NUMFICHA = INGRESO.PAC_NUMFICHA 
                                                            LEFT JOIN PACIENTE  (nolock) ON FICHA.PAC_CORREL = PACIENTE.PAC_CORREL 
                                                            LEFT JOIN PRESTADOR  (nolock) ON INGRESO.PTD_RUT_TRAT = PRESTADOR.PTD_RUT 
                                                            LEFT JOIN (SELECT * FROM REG_PROCED_ENFERMERIA where RPE_CORREL=2) AS HORA_ENFERMERA ON HORA_ENFERMERA.PAC_NUMFICHA=INGRESO.PAC_NUMFICHA AND HORA_ENFERMERA.ING_CORREL=INGRESO.ING_CORREL 
                                                            LEFT JOIN AM_USUARIO ON AM_USUARIO.USU_LOGIN=HORA_ENFERMERA.USU_LOGIN 
                                                            LEFT JOIN ( SELECT CMA_CORREL,PAC_NUMFICHA,ING_CORREL,CML_CODIGO FROM CAMILLA_ASIGNADA CA WHERE CMA_CORREL = ( SELECT TOP 1 CMA_CORREL FROM CAMILLA_ASIGNADA CAM WHERE CAM.PAC_NUMFICHA=CA.PAC_NUMFICHA AND CAM.ING_CORREL=CA.ING_CORREL ORDER BY CAM.CMA_CORREL DESC))AS ULTIMOBOX ON ULTIMOBOX.PAC_NUMFICHA=INGRESO.PAC_NUMFICHA AND ULTIMOBOX.ING_CORREL=INGRESO.ING_CORREL 	
                                                            LEFT JOIN ( SELECT CMA_CORREL,PAC_NUMFICHA,ING_CORREL,CMA_HORADESDE FROM CAMILLA_ASIGNADA CA WHERE CMA_CORREL = ( SELECT TOP 1 CMA_CORREL FROM CAMILLA_ASIGNADA CAM WHERE CAM.PAC_NUMFICHA=CA.PAC_NUMFICHA AND CAM.ING_CORREL=CA.ING_CORREL  ORDER BY CAM.CMA_CORREL ASC))AS PRIMERAHORA 	ON PRIMERAHORA.PAC_NUMFICHA=INGRESO.PAC_NUMFICHA AND PRIMERAHORA.ING_CORREL=INGRESO.ING_CORREL 	
                                                            LEFT JOIN HCU_ESI (nolock) ON INGRESO.PAC_NUMFICHA=HCU_ESI.PAC_NUMFICHA AND INGRESO.ING_CORREL=HCU_ESI.ING_CORREL

                                                            WHERE FICHA.PAC_TIPO = 'U' 
                                                            AND INGRESO.SER_CODIGO = 'URG' 
                                                            AND INGRESO.ING_HORAING > DATEADD(D, -1, GETDATE())
                                                            AND INGRESO.ING_HORAALTA IS NULL 
                                                            AND INGRESO.ING_FECCIER IS NULL 
                                                            AND EXISTS (SELECT TOP 1 CAMILLA_ASIGNADA.PAC_NUMFICHA,CAMILLA_ASIGNADA.ING_CORREL FROM CAMILLA_ASIGNADA (nolock) WHERE INGRESO.PAC_NUMFICHA = CAMILLA_ASIGNADA.PAC_NUMFICHA AND INGRESO.ING_CORREL = CAMILLA_ASIGNADA.ING_CORREL AND CAMILLA_ASIGNADA.CML_CODIGO IS NOT NULL AND CAMILLA_ASIGNADA.CMA_HORAHASTA IS NULL) 
                                                            AND EXISTS ( SELECT TOP 1 HCU_ESI.PAC_NUMFICHA,HCU_ESI.ING_CORREL FROM HCU_ESI WHERE INGRESO.PAC_NUMFICHA = HCU_ESI.PAC_NUMFICHA  AND INGRESO.ING_CORREL = HCU_ESI.ING_CORREL  AND HCU_ESI.HE_ESI IS NOT NULL )
                                                            ORDER BY INGRESO.ING_CATEGORIZACION ASC,PRIMERAHORA.CMA_HORADESDE ASC", con);
                //INGRESO.ING_CATEGORIZACION + ', ' + ULTIMOBOX.CML_CODIGO AS BOX
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataBox.DataSource = table;


                foreach (DataGridViewColumn Col in dataBox.Columns)
                {
                    Col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                foreach (DataGridViewRow row in dataBox.Rows)
                {

                    if ((Convert.ToString(row.Cells["box"].Value)).Contains("ESI4"))
                    {

                        row.DefaultCellStyle.BackColor = Color.Green;
                        row.DefaultCellStyle.ForeColor = Color.White;



                    }

                    if ((Convert.ToString(row.Cells["box"].Value)).Contains("ESI3"))
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;


                    }

                    if ((Convert.ToString(row.Cells["box"].Value)).Contains("ESI5"))
                    {

                        row.DefaultCellStyle.BackColor = Color.Blue;
                        row.DefaultCellStyle.ForeColor = Color.White;



                    }

                    if ((Convert.ToString(row.Cells["box"].Value)).Contains("ESI2"))
                    {

                        row.DefaultCellStyle.BackColor = Color.DarkGoldenrod;
                        row.DefaultCellStyle.ForeColor = Color.White;



                    }

                    if ((Convert.ToString(row.Cells["box"].Value)).Contains("ESI1"))
                    {

                        row.DefaultCellStyle.BackColor = Color.Red;
                        row.DefaultCellStyle.ForeColor = Color.White;



                    }
                }
            }

            catch
            {
               // MessageBox.Show("no se pudo consultar");

            }

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
            cargarCategoria(dataCateg);
        }
       


        private void dataBox_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {

            dataBox.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;


        }

        private void dataEspera_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dataEspera.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;

        }

        
        private void dataEspea_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataEspera.ClearSelection();
        }

        private void dataCategoria_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataCateg.ClearSelection();
        }

            private void timer1_Tick(object sender, EventArgs e)
        {
            cargarEspera(dataEspera);
            cargarBox(dataBox);
            cargarCategoria(dataCateg);
            cargarCantidades(null, null);

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.AutoScaleDimensions = new System.Drawing.SizeF(100F, 110F);
            this.PerformAutoScale();

            cargarEspera(dataEspera);
            cargarBox(dataBox);
            cargarCategoria(dataCateg);
            cargarCantidades(null, null);

        }

      

        private void dataCategoria_SelectionChanged(object sender, EventArgs e)
        {
            dataCateg.ClearSelection();
        }

        private void dataEspera_SelectionChanged(object sender, EventArgs e)
        {
            dataEspera.ClearSelection();
        }

        private void dataBox_SelectionChanged(object sender, EventArgs e)
        {
            dataBox.ClearSelection();
        }
        
    
        private void Form1_Resize(object sender, EventArgs e)
        {
            
            dataBox.Width = this.Width - 50;
            dataBox.Height = this.Height - 600;
            panel5.Width = this.Width - 10;

            dataCateg.Width = this.Width - 50;
            dataEspera.Width = this.Width - 800;
            
           // dataCategoria.Height = this.Height - 400;


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dataCateg_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {

           
                dataCateg.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
            
        }

        private void dataCateg_SelectionChanged(object sender, EventArgs e)
        {

            dataCateg.ClearSelection();
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            cargarEspera(dataEspera);
            cargarBox(dataBox);
            cargarCategoria(dataCateg);
            cargarCantidades(null, null);
        }

        private void dataBox_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataBox.ClearSelection();
        }

      
    }

   
 
}
