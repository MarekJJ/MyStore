using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreDomain.Entities
{
   public class CVModel      // model with metadata for view CV
    {
        [Required(ErrorMessage = "Proszę podać swoje imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać swoje nazwisko")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Proszę podać email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Proszę podać prawidłowy email")]
        public string Email { get; set; }
    }
}
