using botapp.Interfaces.Primarias;
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

namespace botapp
{
    public partial class PrincipalForm : Form
    {
        private Login login;

        public PrincipalForm()
        {
            InitializeComponent();

            login = new Login();
            login.Dock = DockStyle.Fill;

            login.LoginExitoso += Login_LoginExitoso;

            pnlprincipal.Controls.Add(login);
        }

        private void Login_LoginExitoso(object sender, EventArgs e)
        {
            pnlprincipal.Controls.Clear();

            var modulesform = new ModulesInterface();
            modulesform.Dock = DockStyle.Fill;

            //pnlprincipal.Controls.Add(modulesform);
            MostrarModulesInterface();
        }

        private void MostrarModulesInterface()
        {
            var modules = new ModulesInterface();
            modules.Dock = DockStyle.Fill;
            modules.CerrarSesion += Modules_CerrarSesion;
            pnlprincipal.Controls.Clear();
            pnlprincipal.Controls.Add(modules);
        }

        private void Modules_CerrarSesion(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Seguro que desea cerrar sesión?",
                                 "Cerrar sesión",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            login = new Login();
            login.Dock = DockStyle.Fill;
            login.LoginExitoso += Login_LoginExitoso;
            pnlprincipal.Controls.Clear();

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["usuario"].Value = "";
            config.AppSettings.Settings["clave"].Value = "";
            config.AppSettings.Settings["savecredentials"].Value = "false";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            login.LimpiarCampos();

            pnlprincipal.Controls.Add(login);
        }
    }
}
