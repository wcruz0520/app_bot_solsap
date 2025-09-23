using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace botapp.Helpers
{
    class SupabaseDbHelper
    {
        private readonly string _supabaseUrl;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public SupabaseDbHelper(string supabaseUrl, string apiKey)
        {
            _supabaseUrl = supabaseUrl.TrimEnd('/');
            _apiKey = apiKey;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                // Consulta mínima: pedimos solo 1 registro de la tabla user_bot
                string url = string.Format("{0}/rest/v1/user_bot?select=user_code&limit=1", _supabaseUrl);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Hay conexión
                }

                return false; // No respondió con éxito
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en TestConnectionAsync: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Verifica si el usuario existe en la tabla user_bot con user_code y password.
        /// </summary>
        public async Task<bool> AuthenticateAsync(string userCode, string password)
        {
            try
            {
                // URL del endpoint REST de la tabla user_bot
                string url = string.Format(
                    "{0}/rest/v1/user_bot?user_code=eq.{1}&password=eq.{2}&select=*",
                    _supabaseUrl,
                    Uri.EscapeDataString(userCode),
                    Uri.EscapeDataString(password)
                );

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                string json = await response.Content.ReadAsStringAsync();

                // Si trae registros, autenticó correctamente
                return !string.IsNullOrWhiteSpace(json) && json != "[]";
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear con tu logger
                MessageBox.Show("Error en AuthenticateAsync: " + ex.Message);
                return false;
            }
        }

        public async Task<(string UserCode, string ProfileImageUrl)> GetUserDataAsync(string userCode)
        {
            try
            {
                string url = string.Format(
                    "{0}/rest/v1/user_bot?user_code=eq.{1}&select=user_code,profile_image_url",
                    _supabaseUrl,
                    Uri.EscapeDataString(userCode)
                );

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return (null, null);
                }

                string json = await response.Content.ReadAsStringAsync();

                // Ejemplo de respuesta: [{"user_code":"william","profile_image_url":"https://..."}]
                var data = Newtonsoft.Json.Linq.JArray.Parse(json);

                if (data.Count > 0)
                {
                    string userCodeResp = data[0]["user_code"]?.ToString();
                    string profileImageUrl = data[0]["profile_image_url"]?.ToString();
                    return (userCodeResp, profileImageUrl);
                }

                return (null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en GetUserDataAsync: " + ex.Message);
                return (null, null);
            }
        }

    }
}
