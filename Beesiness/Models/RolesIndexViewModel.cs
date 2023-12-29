namespace Beesiness.Models
{
    public class RolesIndexViewModel
    {
        public IEnumerable<Rol> Roles { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

    }
}
