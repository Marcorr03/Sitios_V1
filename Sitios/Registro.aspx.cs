using CAPADATOS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitios
{
    public partial class Registro : System.Web.UI.Page
    {
        Administracion Admi = new Administracion();
        public void VerRoles()
        {
            Admi.AbrirConexion();
            using (SqlCommand command = new SqlCommand("sp_Usuario", Admi.Conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@op", 2);

                SqlParameter sqlParameter = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(sqlParameter);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                Rol_.Items.Clear();

                // Agrega cada elemento al HtmlSelect
                while (reader.Read())
                {
                    Rol_.Items.Add(new ListItem(reader["NombreRol"].ToString()));
                }
                Admi.CerrarConexion();
            }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Paso"] == null || string.IsNullOrEmpty(Session["Paso"].ToString()))
            {
                VerRoles();
                Session["Paso"] = "Paso" ;
            }
        }

        protected void AGREGAR_Click(object sender, EventArgs e)
        {
            Admi.AbrirConexion();

            using (SqlCommand command = new SqlCommand("sp_Usuario", Admi.Conn))
            {
                string[] lista = { "op", "Nombre", "Apellido1", "Apellido2", "Identificacion", "Correo", "Telefono", "Usuario", "Contra", "Estado", "Rol" };
                string[] datos = { "1", nombre.Text, PriApellido.Text, SegApellido.Text, identificacion.Text, correo.Text, telefono.Text, usuario.Text, contra.Text, Estado.Value, Rol_.Value };
                command.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < datos.Length; i++) { 
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
                if (mensaje == "Tu solicitud se proceso correctamente")
                {
                    tipo = "success";
                } else
                {
                    tipo = "error";
                }
                Mensaje(mensaje,tipo);
                
                

                //Mensaje1 = sqlParameter.Value.ToString();

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