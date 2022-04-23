using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace News_DataLayer.Models
{
    public class PendingNews
    {
        [Key]
        public int PendingId { get; set; }

        [Display(Name = "News")]
        
        public int? NewsId { get; set; }

        [Display(Name = "User")]
        
        public string? UserId{ get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "{0} Is Required")]
        [AllowHtml]
        public string Message { get; set; }

        #region Relations
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("NewsId")]
        public News News{ get; set; }
        #endregion

    }
}
