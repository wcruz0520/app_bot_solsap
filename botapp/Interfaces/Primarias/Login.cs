using botapp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace botapp.Interfaces.Primarias
{
    public partial class Login : UserControl
    {
        public event EventHandler LoginExitoso;
        public string supabaseUrl = "https://hgwbwaisngbyzaatwndb.supabase.co";
        public string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imhnd2J3YWlzbmdieXphYXR3bmRiIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTcwNDIwMDYsImV4cCI6MjA3MjYxODAwNn0.WgHmnqOwKCvzezBM1n82oSpAMYCT5kNCb8cLGRMIsbk";
        public Login()
        {
            InitializeComponent();
            btnVerClave.BackColor = Color.Transparent;
            btningresar.Enabled = InternetHelper.HayConexionInternet();

            string usuario_ = ConfigurationManager.AppSettings["usuario"] ?? "";
            string clave_ = ConfigurationManager.AppSettings["clave"] ?? "";
            string saveCreds = ConfigurationManager.AppSettings["savecredentials"] ?? "false";

            if (usuario_.Length > 0 && clave_.Length > 0 && saveCreds == "true")
            {
                txtusuario.Text = usuario_;
                txtclave.Text = clave_;
                chkgurdcontr.Checked = true;
                AutoLogin();
            }
        }

        private async void btningresar_Click(object sender, EventArgs e)
        {
            var helper = new SupabaseDbHelper(supabaseUrl, apiKey);

            bool validaconnsupa = await helper.TestConnectionAsync();

            if (!validaconnsupa)
            {
                MessageBox.Show("Sin conexión Supabase");
            }
            else
            {
                bool autenticado = await helper.AuthenticateAsync(txtusuario.Text, txtclave.Text);

                if (autenticado)
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    if (chkgurdcontr.Checked)
                    {
                        config.AppSettings.Settings["usuario"].Value = txtusuario.Text;
                        config.AppSettings.Settings["clave"].Value = txtclave.Text;
                        config.AppSettings.Settings["savecredentials"].Value = "true";
                    }
                    else
                    {
                        config.AppSettings.Settings["usuario"].Value = "";
                        config.AppSettings.Settings["clave"].Value = "";
                        config.AppSettings.Settings["savecredentials"].Value = "false";
                    }

                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    LoginExitoso?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("❌ Usuario o contraseña incorrectos");
                }
            }
        }

        private void btnVerClave_Click(object sender, EventArgs e)
        {
            txtclave.PasswordChar = (txtclave.PasswordChar == '*') ? '\0' : '*';
            if(txtclave.PasswordChar == '*')
            {
                btnVerClave.BackColor = Color.Transparent;
            }
            else
            {
                btnVerClave.BackColor = Color.LightBlue;
            }
        }

        private async void AutoLogin()
        {
            var helper = new SupabaseDbHelper(supabaseUrl, apiKey);

            bool validaconnsupa = await helper.TestConnectionAsync();
            if (!validaconnsupa)
            {
                MessageBox.Show("Sin conexión Supabase");
                return;
            }

            bool autenticado = await helper.AuthenticateAsync(txtusuario.Text, txtclave.Text);
            if (autenticado)
            {
                LoginExitoso?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
