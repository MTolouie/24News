using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Models.ViewModels
{
    public class SingleNewsViewModel
    {
        public NewsDTO News{ get; set; }
        public ApplicationUserDTO Journalist{ get; set; }
    }
}
