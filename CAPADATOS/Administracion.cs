using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPADATOS
{
    public class Administracion
    {
        SqlConnection conn;

        string stringConexion = "Data Source=DESKTOP-3G9V4FR;Initial Catalog=ProyectoSitios; Integrated Security=True;";

        public SqlConnection Conn { get => conn; set => conn = value; }

        //CUC
        //string stringConexion = "Data Source=DESKTOP-303DRB9;Initial Catalog=ProyectoProgra; Integrated Security=True;"; 


        public void AbrirConexion()
        {
            Conn = new SqlConnection(stringConexion);
            Conn.Open();
        }

        public void CerrarConexion()
        {
            Conn = new SqlConnection(stringConexion);
            Conn.Close();

        }

    }
}
