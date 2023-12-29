namespace Beesiness.Models
{
    public class EnfermedadListViewModel
    {
        public IEnumerable<Enfermedad> Enfermedades { get; set; }
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
