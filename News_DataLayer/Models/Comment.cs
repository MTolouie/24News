using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_DataLayer.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Display(Name = "User")]
        public string? UserId { get; set; }

        [Display(Name = "News")]
        public int? NewsId { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = "{0} Is Required")]
        public string CommentText { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        #region Relations
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("NewsId")]
        public News News { get; set; }
        #endregion
    }

}
