using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Models.DTOs
{
    public class CommentDTO
    {
        public int CommentId { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "{0} Is Required")]
        public string UserId { get; set; }

        [Display(Name = "News")]
        [Required(ErrorMessage = "{0} Is Required")]
        public int NewsId { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "{0} Is Required")]
        public string CommentText { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
