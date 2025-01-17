﻿namespace Beesiness.Models
{
    public class EnfermedadViewModel
    {
        public IEnumerable<Usuario> Usuarios { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        // Calcula si hay una página anterior basado en la página actual.
        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }
    }
}
