using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Models.DTOs
{
    public class CategoryDTO
    {
        
        public int CatId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} Is Needed !")]
        [MaxLength(50)]
        public string CatTitle { get; set; }
    }
}
