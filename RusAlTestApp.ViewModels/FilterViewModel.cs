using System.Collections.Generic;

namespace RusAlTestApp.ViewModels
{
    public class FilterViewModel
    {
        public int Take { get; set; }
        public int Skip { get; set; }

        public List<int> ColorIds { get; set; }
        public List<int> DrinkIds { get; set; }
    }
}
