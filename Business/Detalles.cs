using Dominio;
using NegocioItem;
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
using System.Data.SqlClient;

namespace Negocio
{
    public partial class Detalles : Form
    {
        private Item item = null;
        public Detalles(Item item)
        {
            InitializeComponent();
            this.item = item;
        }

        private OpenFileDialog archivo = null;
        private void Detalles_Load(object sender, EventArgs e)
        { 
            lblNombre.Text = item.Nombre;
            txtCodigo.Text = item.Codigo.ToString();
            txtPrecio.Text = item.Precio.ToString();
            txtDescripcion.Text = item.Descripcion.ToString();
            txtMarca.Text = item.Marca.ToString();
            txtCategoria.Text = item.Categoria.ToString();
            try
            {
                pbxImagen.Load(item.ImagenUrl);
            }
            catch
            {
                pbxImagen.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
            
        }
    }   
            
}

