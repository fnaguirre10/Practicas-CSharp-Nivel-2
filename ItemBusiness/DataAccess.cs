using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBusiness
{
    public class DataAccess
    {
        private SqlConnection conection;
        private SqlCommand command;
        private SqlDataReader reader;
        public SqlDataReader Reader
        {
            get { return reader; }
        }

        public DataAccess()
        {
            conection = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true");
            command = new SqlCommand();
        }

        public void setConsult(string consult)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = consult;
        }

        public void executeReader()
        {
            command.Connection = conection;
            try
            {
                conection.Open();
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void executeAction()
        {
            command.Connection = conection;

            try
            {
                conection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void setParameter(string name, object value)
        {
            command.Parameters.AddWithValue(name, value);
        }

        public void closeConection()
        {
            if (reader != null)
            {
                reader.Close();
                conection.Close();
            }
        }
    }
}
