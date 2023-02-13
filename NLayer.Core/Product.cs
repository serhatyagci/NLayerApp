using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core
{
    public class Product:BaseEntity
    {
        //Null olmayacak ise ctor yazılır ve nullexception hatası atar.
        /*public Product(string name)
        {
            name=Name ?? throw new ArgumentNullException(nameof(Name));
        }*/
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CadegoryId { get; set; } //[ForeignKey("CategoryID")]
        public Category Category { get; set; }  //Navigation propertyler
        public ProductFeature ProductFeature { get; set; }  //Navigation propertyler
    }
}
