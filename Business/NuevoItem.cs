using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using NegocioItem;
using System.Configuration;
using System.Xml.Linq;
using System.Drawing.Text;

namespace Negocio
{
    public partial class NuevoItem : Form
    {
        private Item item = null;
        private OpenFileDialog archivo = null;
        public NuevoItem()
        {
            InitializeComponent();
        }

        public NuevoItem(Item item)
        {
            InitializeComponent();
            this.item = item;
            Text = "Modificar Item";
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ItemNegocio negocio = new ItemNegocio();
            try
            {
                if (item == null)
                    item = new Item();

                item.Codigo = txtCodigo.Text;
                item.Nombre = txtNombre.Text;
                item.Descripcion = txtDescripcion.Text;
                item.ImagenUrl = txtImagenUrl.Text;
                item.Precio = txtPrecio.Text;
                item.Marca = (Marca)cboMarca.SelectedItem;
                item.Categoria = (Categoria)cboCategoria.SelectedItem;

                if (item.Id != 0)
                {
                    negocio.modificar(item);
                    MessageBox.Show("Modificado exitosamente");

                }
                else
                {
                    negocio.Agregar(item);
                    MessageBox.Show("Agregado exitosamente");
                }


                if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);
                }

                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void NuevoItem_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                cboMarca.DataSource = marcaNegocio.crearLista();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.crearLista();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (item != null)
                {
                    txtCodigo.Text = item.Codigo.ToString();
                    txtNombre.Text = item.Nombre;
                    txtDescripcion.Text = item.Descripcion;
                    txtImagenUrl.Text = item.ImagenUrl;
                    cargarImagen(item.ImagenUrl);
                    txtPrecio.Text = item.Precio;
                    cboMarca.SelectedValue = item.Marca.Id;
                    cboCategoria.SelectedValue = item.Categoria.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //foreach (char character in txtPrecio.Text)
            //{
            //    if (!(char.IsNumber(character)))
            //    {
            //        MessageBox.Show("Solo puedes insertar números en este campo.");
            //    }

            //}
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxItem.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxItem.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png|jpeg|*.jpeg";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            

        }

    }
}
