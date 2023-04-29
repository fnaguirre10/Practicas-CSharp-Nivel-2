using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain;

namespace ItemBusiness
{
    public class CategoryBusiness
    {
        public List<Category> listMaker()
        {
            List<Category> lista = new List<Category>();
            DataAccess data = new DataAccess();
            try
            {
                data.setConsult("Select Id, Descripcion from CATEGORIAS");
                data.executeReader();

                while (data.Reader.Read())
                {
                    Category aux = new Category();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Description = (string)data.Reader["Descripcion"];

                    lista.Add(aux);
                }
                return lista;
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
    }
}
