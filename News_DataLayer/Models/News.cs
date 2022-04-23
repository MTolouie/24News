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
    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} Is Required")]
        public int CatId { get; set; }

        [Display(Name = "Journalist")]
        [Required(ErrorMessage = "{0} Is Required")]
        public string UserId { get; set; }

        [Display(Name = "News Title")]
        [MaxLength(100)]
        [Required(ErrorMessage = "{0} Is Required")]
        public string NewsTitle { get; set; }

        [Display(Name = "News Title")]
        [MaxLength(800)]
        [Required(ErrorMessage = "{0}  Is Required")]
        [DataType(DataType.MultilineText)]
        public string ShortDesc { get; set; }

        [Display(Name = "News Text")]
        [Required(ErrorMessage = "{0}  Is Required")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Text { get; set; }

        [Display(Name = "Image")]
        [MaxLength(100)]
        public string Images { get; set; }

        [Display(Name = "Tags")]
        [Required(ErrorMessage = "Please Enter {0}")]
        [MaxLength(50)]
        public string Tags { get; set; }


        public int Visit { get; set; }

        [Display(Name = "Is Archived")]
        public bool IsArchived { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; } = false;

        [Display(Name = "Is Pending")]
        public bool IsPending { get; set; } = false;

        [Display(Name = "Published Date")]
        public System.DateTime CreateDate { get; set; }

        #region Relations
        [ForeignKey("CatId")]
        public Category Category{ get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public List<PendingNews> PendingNews { get; set; }

        public List<Comment> Comments { get; set; }
        #endregion
    }
}
