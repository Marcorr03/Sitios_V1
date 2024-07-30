using CAPADATOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitios
{
    public partial class iniciosec : System.Web.UI.Page
    {
        Administracion Admi =new Administracion();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        

        protected void INICIAR_Click(object sender, EventArgs e)
        {
            Admi.AbrirConexion();

            using (SqlCommand command = new SqlCommand("sp_Usuario", Admi.Conn))
            {
                string[] lista = { "op", "Usuario","Contra"};
                string[] datos = { "3", usuario.Text, contra.Text};
                command.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < datos.Length; i++)
                {
                    command.Parameters.AddWithValue("@" + lista[i], datos[i]);
                }

                SqlParameter sqlParameter = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };


                command.Parameters.Add(sqlParameter);
                command.ExecuteNonQuery();
                string mensaje = sqlParameter.Value.ToString();
                string tipo = "";
                if (mensaje.Split(',')[0] == "Bienvenido")
                {
                    tipo = "success";
                    Session["Usuario"]= mensaje.Split(',')[2];
                    Session["Rol"] = mensaje.Split(',')[1];
                }
                else
                {
                    tipo = "error";
                }
                Mensaje(mensaje.Split(',')[0], tipo);
            }
            Admi.CerrarConexion();
        }
        public void Mensaje(string texto, string icono)
        {
            string sweetAlertScript = "<script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>";
            ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert2", sweetAlertScript, false);

            string script = $@"
            setTimeout(function() {{
                Swal.fire({{
                    title: '{texto}',
                    text: 'Click para continuar',
                    icon: '{icono}'
                }});
            }}, 3000);";
            ScriptManager.RegisterStartupScript(this, GetType(), "SweetAlert", script, true);
        }
    }
}