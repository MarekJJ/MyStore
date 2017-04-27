using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyStoreDomain.Entities
{
   
   public class MyItem // class with metadata to creata textboxes i view for admin 
    {
        [Key] // this is calling admin to login

        [HiddenInput(DisplayValue = false)]
        public int NumberID { get; set; } 

        [Required(ErrorMessage = "Proszę podać nazwę produktu.")]
        [Display(Name = "Nazwa")]
        public string Name1 { get; set; }

        [Required(ErrorMessage = "Proszę podać Autora")]
        [Display(Name = "Autor")] // metadane
        public string AutorName { get; set; }

        [DataType(DataType.MultilineText), Display(Name = "Opis")]
        [Required(ErrorMessage = "Proszę podać opis.")]
        public string Description{ get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Proszę podać dodatnią cenę.")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Proszę określić kategorię.")]
        [Display(Name = "Kategoria")]
        public string Category { get; set; }


        public byte[] ImageData { get; set; }    // for picture
        public string ImageMimeType { get; set; }

        [Required(ErrorMessage = "Proszę Podać ilość odstepnych produktów")]
        [Display(Name = "Ilość produktów")]
        public string amount { get; set; }
    }
}
