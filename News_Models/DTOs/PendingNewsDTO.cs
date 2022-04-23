using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace News_Models.DTOs
{
    public class PendingNewsDTO
    {
        
        public int PendingId { get; set; }

        [Display(Name = "News")]
        [Required(ErrorMessage = "{0} Is Required")]
        public int NewsId { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "{0} Is Required")]
        [MaxLength(50)]
        public string UserId { get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "{0} Is Required")]
        [AllowHtml]
        public string Message { get; set; }
    }
}
