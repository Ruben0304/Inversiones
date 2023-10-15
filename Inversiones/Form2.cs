using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inversiones
{
    public partial class Consejo : Form
    {
        public Consejo()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Consejo_Load(object sender, EventArgs e)
        {
           
        }

        private async void Enviar_Click(object sender, EventArgs e)
        {
            respuesta.Text = "Analizando tu pregunta ... ";
            var client = new KuCoinClient();
            var dbManager = new DatabaseManager("Finanzas.db");
            try
            {
                var verificarIp = true;
                // var verificarIp = await client.verificarIP();
                var balance = verificarIp ? (await client.GetAccountBalanceAsync()).ToString("0.00") + " usd" : "0.00 usd (tiene activada la vpn y no se puede obtener la cantidad exacta)";
                Usuario usuario = dbManager.ObtenerPrimerUsuario();
                var openai = new OpenAIClient();
                var prompt = "Imagina que eres un asesor de finanzas personales. Un interesado en consejos de finanzas te va a hacer una pregunta teniendo en cuenta que cuenta con el siguiente dinero ( 1 peso cubano es equivalente a alrededor de 190 usd en el mercado informal ) : En tarjeta en el banco tiene " + usuario.Tarjeta.ToString("0.00") + " pesos cubanos,en efectivo tiene " + usuario.Efectivo.ToString("0.00") + " pesos cubanos, tiene una cuenta para inversion en criptomonedas " + balance + ".Vive en Cuba un pais donde los ingresos son bastante bajos y los precios caros y existe inflacion, ademas no existen cosas como financiamientos o compras a plazo de productos ni tiendas como amazon ya que es un pais socialista y solo existen empresas privadas y estatales llamadas PYMES.Todos los productos que fabrican empresas internacionales como apple, microsoft etc en Cuba solo se encuentran revendidos ya que no exiten tiendas de esas empresas aqui.Casi todo se compra en revolico que es un sitio web informal donde las personas publican ofertas de sus productos, o en grupos de facebook. En la respuesta usa algun emoji pero no abuses de ellos. La pregunta es la siguiente : " + pregunta.Text;
                var response = await openai.GetGPT3Response(prompt);
                respuesta.Text = response;
            }
            catch (Exception)
            {
               
            }
            
        }
    }
}
