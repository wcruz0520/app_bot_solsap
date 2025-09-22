using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace botapp.Interfaces.Primarias
{
    public partial class ModulesInterface : UserControl
    {
        public event EventHandler CerrarSesion;
        public ModulesInterface()
        {
            InitializeComponent();
        }

        private void btncerrarsesion_Click(object sender, EventArgs e)
        {
            CerrarSesion?.Invoke(this, EventArgs.Empty);
        }
    }
}
