using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using Domain;


namespace ItemBusiness
{
    public class ItemsBusiness
    {
        public List<Item> listMaker()
        {
            List<Item> list = new List<Item>();
            SqlConnection conection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            try
            {
                conection.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Select Codigo, Nombre, A.Descripcion, ImagenUrl, C.Descripcion Categoria, M.Descripcion Marca, A.IdCategoria, A.IdMarca, A.Id, A.Precio from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca";
                command.Connection = conection;

                conection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Item aux = new Item();
                    aux.Id = (int)reader["Id"];
                    //aux.Code = reader.GetInt32(0);
                    aux.Name = (string)reader["Nombre"];
                    aux.Description = (string)reader["Descripcion"];
                    if (!(reader["ImagenUrl"] is DBNull))
                        aux.UrlPicture = (string)reader["ImagenUrl"];
                    //aux.Price = reader.GetInt32(0);
                    aux.Brand = new Category();
                    aux.Brand.Id = (int)reader["IdMarca"];
                    aux.Brand.Description = (string)reader["Marca"];
                    aux.Category = new Category();
                    aux.Category.Id = (int)reader["IdCategoria"];
                    aux.Category.Description = (string)reader["Categoria"];

                    list.Add(aux);
                }

                conection.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void add(Item recent)
        {
            DataAccess data = new DataAccess();

            try
            {
                  data.setConsult("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, UrlImagen)values (" + recent.Code + ", '" + recent.Name + "','" + recent.Description + "', @idBrand, @idCategory, @urlPicture)");
                  data.setParameter("@idBrand", recent.Brand.Id);
                  data.setParameter("@idCategory", recent.Category.Id);
                  data.setParameter("@urlPicture", recent.UrlPicture);
                  data.executeAction();
            }
            catch (Exception ex)
            {

                 throw ex;
            }
            finally
            {
                  data.closeConection();
            }
        }

        public void modify(Item recent)
        {
             DataAccess data = new DataAccess();
             try
             {
                    data.setConsult("update POKEMONS set Codigo = @code, Nombre = @name, Descripcion = @desc, UrlImagen = @pic, IdMarca = @idBrand, IdCategoria = @idCategory where Id = @id");
                    data.setParameter("@code", recent.Code);
                    data.setParameter("@name", recent.Name);
                    data.setParameter("@desc", recent.Description);
                    data.setParameter("@pic", recent.UrlPicture);
                    data.setParameter("@idBrand", recent.Brand.Id);
                    data.setParameter("@idCategory", recent.Category.Id);
                    data.setParameter("@id", recent.Id);

                    data.executeAction();
             }
             catch (Exception ex)
                {

                throw ex;
             }
             finally
             {
                data.closeConection();
             }
        }

        public List<Item> filter(string field, string criteria, string filter)
        {
            List<Item> list = new List<Item>();
            DataAccess data = new DataAccess();
            try
            {
                string consult = "Select Numero, Nombre, A.Descripcion, UrlImagen, M.Descripcion Marca, C.Descripcion Category, A.IdMarca, A.IdCategoria, A.Id from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = P.IdMarca and C.Id = A.IdCategoria and ";
                if (field == "Item Code")
                {
                    switch (criteria)
                    {
                        case "Greater tham":
                            consult += "Item Code > " + filter;
                            break;
                        case "Less than":
                            consult += "Item Code < " + filter;
                            break;
                        default:
                            consult += "Item Code = " + filter;
                            break;
                    }
                }
                else if (field == "Name")
                {
                    switch (criteria)
                    {
                        case "Start with":
                            consult += "Name like '" + filter + "%'";
                            break;
                        case "End with":
                            consult += "Name like '%" + filter + "'";
                            break;
                        default:
                            consult += "Name like '%" + filter + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criteria)
                    {
                        case "Start with":
                            consult += "A.Descripcion like '" + filter + "%'";
                            break;
                        case "Termina con":
                            consult += "A.Descripcion like '%" + filter + "'";
                            break;
                        default:
                            consult += "A.Descripcion like '%" + filter + "%'";
                            break;
                    }
                }
                data.setConsult(consult);
                data.executeReader();
                while (data.Reader.Read())
                {
                    Item aux = new Item();
                    aux.Id = (int)data["Id"];
                    aux.Code = data.GetInt32(0);
                    aux.Name = (string)data["Code"];
                    aux.Description = (string)data["Descripcion"];
                    if (!(data["ImagenUrl"] is DBNull))
                        aux.UrlPicture = (string)data["ImagenUrl"];
                    aux.Brand = new Category();
                    aux.Brand.Id = (int)data["IdBrand"];
                    aux.Brand.Description = (string)data["Marca"];
                    aux.Category = new Category();
                    aux.Category.Id = (int)data["IdCategoria"];
                    aux.Category.Description = (string)data["Categoria"];

                    list.Add(aux);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void delete(int id)
        {
            try
            {
                DataAccess datos = new DataAccess();
                datos.setConsult("delete from pokemons where id = @id");
                datos.setParameter("id", id);
                datos.executeAction();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void logicDelete(int id)
        {
            try
            {
                DataAccess data = new DataAccess();
                data.setConsult("update ARTICULOS set Id = 0 where id = @id");
                data.setParameter("@id", id);
                data.executeAction();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
