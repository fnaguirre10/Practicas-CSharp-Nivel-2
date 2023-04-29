using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain
{
    public class Item
    {
        public int Id { get; set; }
        public int Code { get; set; }  
        public int Price { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string UrlPicture { get; set; }
        public Category Brand { get; set; }
        public Category Category { get; set; }
    }
}
