using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Models.ViewModels
{
    public class NewsForIndexViewModel
    {
        public int NewsId { get; set; }

        public string Images { get; set; }

        public string NewsTitle { get; set; }

        public DateTime CreateDate  { get; set; }

        public string? Author { get; set; }

        public string? CatTitle{ get; set; }

        public string? ShortDesc { get; set; }

        public int? Visit { get; set; }
    }
}
