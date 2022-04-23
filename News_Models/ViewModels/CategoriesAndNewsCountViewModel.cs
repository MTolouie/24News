using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Models.ViewModels
{
    public class CategoriesAndNewsCountViewModel
    {

        public List<CategoryDTO> Categories { get; set; }

        public List<int> NewsCatIds { get; set; }

    }
}
