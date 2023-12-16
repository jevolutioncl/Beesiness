using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beesiness.Models
{
    public class EstadoArduino
    {
        [Key]
        public int Id { get; set; }
        public bool ArduinoConectado { get; set; }
        public DateTime UltimaComunicacion { get; set; }

        [ForeignKey(nameof(Colmena))]
        public int ColmenaId { get; set; } // Usa ColmenaId en lugar de IdColmena

        public virtual Colmena Colmena { get; set; }
    }

}
