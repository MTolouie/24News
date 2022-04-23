using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Models.DTOs
{
    public class ApplicationUserDTO : IdentityUser
    {

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} Is Required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Avatar")]
        [MaxLength(100)]
        public string Avatar { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public List<NewsDTO> News { get; set; }
    }
}
