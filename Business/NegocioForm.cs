using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;
using Dominio;
using NegocioItem;

namespace Negocio
{
    public partial class NegocioForm : Form
    {
        private List<Item> listaItems;
        public NegocioForm()
        {
            InitializeComponent();
        }

        private void NegocioForm_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Código");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");
        }

        private void dgvItems_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow != null)
            {
                Item seleccionado = (Item)dgvItems.CurrentRow.DataBoundItem;
                try
                {
                    pbItem.Load(seleccionado.ImagenUrl);
                }
                catch
                {
                    pbItem.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
                }
             
            }

        }

        private void cargar()
        {
            ItemNegocio negocio = new ItemNegocio();
            try
            {
                listaItems = negocio.crearLista();
                dgvItems.DataSource = listaItems;
                ocultarColumnas();
                pbItem.Load(listaItems[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvItems.Columns["Id"].Visible = false;
            dgvItems.Columns["ImagenUrl"].Visible = false;
            dgvItems.Columns["dosDecimales"].Visible=false;
            dgvItems.Columns["precioDecimal"].Visible = false;
            
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbItem.Load(imagen);
            }
            catch (Exception ex)
            {
                pbItem.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            NuevoItem alta = new NuevoItem();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Item seleccionado;
            seleccionado = (Item)dgvItems.CurrentRow.DataBoundItem;

            NuevoItem modificar = new NuevoItem(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            borrar(true);
        }

        private void borrar(bool logica = false)
        {
            ItemNegocio negocio = new ItemNegocio();
            Item seleccionado;
            try
            {
                DialogResult answer = MessageBox.Show("Quieres eliminar este elemento?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    seleccionado = (Item)dgvItems.CurrentRow.DataBoundItem;

                    //if (logica)
                        //negocio.borrarLogico(seleccionado.Id);
                    //else
                        negocio.borrar(seleccionado.Id);
                    //Para que actualice la grilla
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool filtroValidacion()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un campo, por favor.");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un criterio, por favor.");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    MessageBox.Show("Debes cargar un filtro que sea para numéricos.");
                    return true;
                }
                if (!(soloNumeros(txtFiltro.Text)))
                {
                    MessageBox.Show("Solo puedes insertar números en este filtro.");
                    return true;
                }
            }

            return false;
        }

        private bool soloNumeros(string strings)
        {
            foreach (char character in strings)
            {
                if (!(char.IsNumber(character)))
                    return false;
            }
            return true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ItemNegocio negocio = new ItemNegocio();
            try
            {
                if (filtroValidacion())
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criteria = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                dgvItems.DataSource = negocio.filtro(campo, criteria, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string option = cboCampo.SelectedItem.ToString();
            if (option == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnVerDetalles_Click(object sender, EventArgs e)
        {
            Item seleccionado;
            seleccionado = (Item)dgvItems.CurrentRow.DataBoundItem;

            Detalles detalles = new Detalles(seleccionado);
            detalles.ShowDialog();
            cargar();

        }
    }
}

