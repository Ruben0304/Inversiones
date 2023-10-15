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
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            

            var client = new KuCoinClient();
            
            var dbManager = new DatabaseManager("Finanzas.db");
            Usuario usuario = dbManager.ObtenerPrimerUsuario();
            label_efectivo.Text = usuario.Efectivo.ToString("0.00") + " cup";
            label_tarjeta.Text = usuario.Tarjeta.ToString("0.00") + " cup";
           
            try 
            {
                // var verificarIp = await client.verificarIP();
                var verificarIp = true;
                var balance = verificarIp ? (await client.GetAccountBalanceAsync() * 1000000).ToString("0.00")+" usd" : "La VPN Mamaculo";
                label_digital.Text = balance;
            }
            catch(Exception) 
            {
                label_digital.Text = "Conectate ";
            }
            

            
               

                
            
            

             
            
            
        }
        private void aceptar_Click_1(object sender, EventArgs e)
        {
            float balance;
            int tipo = 0;
            var dbManager = new DatabaseManager("Finanzas.db");
            bool success = float.TryParse(monto.Text, out balance);
            if (ingreso.Checked == true)
            {
                tipo = 1;
            }
            else if (gasto.Checked== true)
            {
                tipo = 2;
            }
            if (success)
            {
                switch (sitio.Text)
                {
                    case "Efectivo":
                        dbManager.AgregarSaldo("Efectivo", balance,tipo);

                        break;
                    case "Tarjeta":
                        dbManager.AgregarSaldo("Tarjeta", balance,tipo);

                        break;
                    default:
                        label_tarjeta.Text = " default";
                        break;
                }
                Usuario usuario = dbManager.ObtenerPrimerUsuario();
                label_efectivo.Text = usuario.Efectivo.ToString("0.00") + " cup";
                label_tarjeta.Text = usuario.Tarjeta.ToString("0.00") + " cup";
            }
            else
            {
                label_tarjeta.Text = " else";
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ingreso.Checked)
                gasto.Checked = false;
            else
                gasto.Checked = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            

            if (gasto.Checked)
                ingreso.Checked = false;
            else
                ingreso.Checked = true;



        }
        private void aceptar_Click(object sender, EventArgs e)
        {
            
        }

        private void label6_Click(object sender, EventArgs e)
        {
            var consejo = new Consejo();
            consejo.ShowDialog();
        }
    }
}
