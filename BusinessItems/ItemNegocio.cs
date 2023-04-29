using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using Dominio;

namespace NegocioItem
{
    public class ItemNegocio
    {
        public List<Item> crearLista()
        {
            List<Item> lista = new List<Item>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Codigo, Nombre, Precio, A.Descripcion, ImagenUrl, C.Descripcion Categoria, M.Descripcion Marca, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Item aux = new Item();
                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    if (!(lector["Precio"] is DBNull)) {
                        aux.precioDecimal = (decimal)lector["Precio"];
                        aux.dosDecimales = aux.precioDecimal.ToString("0.00");
                        aux.Precio = "$" + aux.dosDecimales;
                    }
                    aux.Descripcion = (string)lector["Descripcion"];
                    if (!(aux.ImagenUrl == ""))
                    {
                        aux.ImagenUrl = (string)lector["ImagenUrl"];
                    }
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)lector["IdMarca"];
                    aux.Marca.Descripcion = (string)lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)lector["Categoria"];

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
              catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Item nuevo)
        {
            DatoAcceso datos = new DatoAcceso();

            try
            {
                  datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria, ImagenUrl) values ('" + nuevo.Codigo + "', '" + nuevo.Nombre + "','" + nuevo.Descripcion + "', '" + nuevo.Precio + "', @idMarca, @idCategoria, @imagenUrl)");
                  datos.setearParametro("@idMarca", nuevo.Marca.Id);
                  datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                  datos.setearParametro("@imagenUrl", nuevo.ImagenUrl);
                  datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                 throw ex;
            }
            finally
            {
                  datos.cerrarConexion();
            }
        }

        public void modificar(Item recent)
        {
             DatoAcceso datos = new DatoAcceso();
             try
             {
                    datos.setearConsulta("update ARTICULOS set Codigo = @cod, Precio = @precio, Nombre = @nombre, Descripcion = @desc, ImagenUrl = @img, IdMarca = @idMarca, IdCategoria = @IdCategoria where Id = @id");
                    datos.setearParametro("@cod", recent.Codigo);
                    datos.setearParametro("@precio", recent.Precio);
                    datos.setearParametro("@nombre", recent.Nombre);
                    datos.setearParametro("@desc", recent.Descripcion);
                    datos.setearParametro("@img", recent.ImagenUrl);
                    datos.setearParametro("@idMarca", recent.Marca.Id);
                    datos.setearParametro("@idCategoria", recent.Categoria.Id);
                    datos.setearParametro("@id", recent.Id);

                    datos.ejecutarAccion();
             }
             catch (Exception ex)
                {

                throw ex;
             }
             finally
             {
                datos.cerrarConexion();
             }
        }

        public List<Item> filtro(string campo, string criteria, string filtro)
        {
            List<Item> lista = new List<Item>();
            DatoAcceso datos = new DatoAcceso();
            try
            {
                string consulta = "Select Codigo, Nombre, A.Descripcion, ImagenUrl, M.Descripcion Marca, C.Descripcion Categoria, A.IdMarca, A.IdCategoria, A.Id, A.Precio from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca and C.Id = A.IdCategoria and ";
                if (campo == "Código")
                {
                    switch (criteria)
                    {
                        case "Comienza con":
                            consulta += "Codigo like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Codigo like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Codigo like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criteria)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else if(campo == "Descripción")
                {
                    switch (criteria)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criteria)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo =="Categoría")
                {
                    switch (criteria)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Precio")
                {
                    switch (criteria)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro;
                            break;
                        default:
                            consulta += "A.Precio = " + filtro;
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLector();
                while (datos.Lector.Read())
                {
                    Item aux = new Item();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.precioDecimal = (decimal)datos.Lector["Precio"];
                    aux.dosDecimales = aux.precioDecimal.ToString("0.00");
                    aux.Precio = "$" + aux.dosDecimales;
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void borrar(int id)
        {
            try
            {
                DatoAcceso datos = new DatoAcceso();
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParametro("id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void borrarLogico(int id)
        {
            try
            {
                DatoAcceso datos = new DatoAcceso();
                datos.setearConsulta("update ARTICULOS set Id = 0 where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
