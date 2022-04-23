using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_DataLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "{0} Is Required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Avatar")]
        [MaxLength(100)]
        public string Avatar { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; } = true;

        #region Relations

        public List<News> News { get; set; }

        public List<PendingNews> PendingNews { get; set; }

        public List<Comment> Comments { get; set; }
        #endregion

    }
}
