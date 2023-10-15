using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Inversiones
{

    public class OpenAIClient
    {
        private static readonly HttpClient client = new HttpClient();
        private const string baseUrl = "https://api.openai.com/v1/chat/completions";
        private const string apiKey = "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";

        public async Task<string> GetGPT3Response(string prompt)
        {
            // Crear la estructura de datos para el cuerpo de la solicitud
            var data = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                new
                {
                    role = "user",
                    content = prompt
                }
            }
            };

            // Serializar los datos a formato JSON
            var json = JsonConvert.SerializeObject(data);

            // Crear el contenido de la solicitud
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Añadir el header de autorización
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Realizar la solicitud POST
            var response = await client.PostAsync(baseUrl, content);

            // Leer la respuesta y deserializarla
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

            // Extraer y devolver el contenido del mensaje
            return responseObject.choices[0].message.content;
        }
    }
}

