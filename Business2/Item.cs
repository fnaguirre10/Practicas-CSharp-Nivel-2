using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dominio
{
    public class Item
    {
        public int Id { get; set; }
        public string Codigo { get; set; }  
        public decimal precioDecimal { get; set; }
        public string dosDecimales { get; set; }
        public string Precio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public string ImagenUrl { get; set; }
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }
    }
}
