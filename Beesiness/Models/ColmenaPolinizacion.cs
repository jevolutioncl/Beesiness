using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class ColmenaPolinizacion
    {
        public int IdPolinizacion { get; set; }
        public Polinizacion Polinizacion { get; set; }

        public int IdColmena { get; set; }
        public Colmena Colmena { get; set; }
    }
}
