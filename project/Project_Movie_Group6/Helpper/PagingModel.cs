using Microsoft.AspNetCore.Mvc;

namespace Project_Movie_Group6.Helpper
{
    public class PagingModel : Controller
    {
        public int currenPage { get; set; }
        public int countPage { get; set; }

        public Func<int?, string> generateURl { get; set; }
    }
}
