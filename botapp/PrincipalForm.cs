using botapp.Interfaces.Primarias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            pnlprincipal.Controls.Add(modulesform);
        }

    }
}
