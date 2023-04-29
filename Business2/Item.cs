using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dominio
{
    public class Item
    {
        public int Id { get; set; }
        
        [DisplayName("Código")]
        public string Codigo { get; set; }  
        public decimal precioDecimal { get; set; }
        public string dosDecimales { get; set; }
        public string Precio { get; set; }
        public string Nombre { get; set; }
        
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public string ImagenUrl { get; set; }
        public Marca Marca { get; set; }
        [DisplayName("Categoría")]
        public Categoria Categoria { get; set; }
    }
}
