using botapp.Interfaces.Secundarias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace botapp.Interfaces.Primarias
{
    public partial class ModulesInterface : UserControl
    {
        public string supabaseUrl = "https://hgwbwaisngbyzaatwndb.supabase.co";
        public string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imhnd2J3YWlzbmdieXphYXR3bmRiIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTcwNDIwMDYsImV4cCI6MjA3MjYxODAwNn0.WgHmnqOwKCvzezBM1n82oSpAMYCT5kNCb8cLGRMIsbk";

        public event EventHandler CerrarSesion;

        public ConfigBotInterface configBotInterface;

        public ModulesInterface(string userName, string profileImageFileName)
        {
            InitializeComponent();

            lbluser.Text = userName;

            if (!string.IsNullOrEmpty(profileImageFileName))
            {
                _ = CargarImagenAsync(profileImageFileName); // llamamos async sin bloquear
            }
        }

        private async Task CargarImagenAsync(string fileName)
        {
            try
            {
                string url = $"{supabaseUrl}/storage/v1/object/public/imgs/{fileName}";

                using (var client = new HttpClient())
                {
                    // Para bucket privado
                    client.DefaultRequestHeaders.Add("apikey", apiKey);
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                    var bytes = await client.GetByteArrayAsync(url);

                    using (var ms = new MemoryStream(bytes))
                    {
                        pctBoxAvatar.Image = Image.FromStream(ms);
                    }
                }
            }
            catch(Exception ex)
            {
                // Opcional: asignar avatar por defecto
                // pctBoxAvatar.Image = Properties.Resources.default_avatar;
            }
        }

        private void btncerrarsesion_Click(object sender, EventArgs e)
        {
            CerrarSesion?.Invoke(this, EventArgs.Empty);
        }

        private void btnConfBot_Click(object sender, EventArgs e)
        {
            configBotInterface = new ConfigBotInterface();
            configBotInterface.Dock = DockStyle.Fill;
            pnlsecondary.Controls.Add(configBotInterface);
        }
    }
}
