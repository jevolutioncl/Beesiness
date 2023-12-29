using System.ComponentModel.DataAnnotations;

namespace Beesiness.Models
{
    public class HistorialColmena
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdColmena { get; set; } //agregados del difunto infoColmena
        [Required]
        public int numIdentificador { get; set; }
        [Required]
        public DateTime FechaIngreso { get; set; }
        [Required, MaxLength(50)]
        public string TipoColmena { get; set; }
        [MaxLength(250)]
        public string? Descripcion { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int? UbicacionMapaId { get; set; }
        [StringLength(100)]
        public string EstadoSalud { get; set; } //agregados del difunto infoColmena
        public DateTime FechaRespaldo { get; set; } //nuevo, muy necesario
        public string MotivoRespaldo { get; set; } //agregado por posible futura busqueda de colmenas eliminadas

        //public UbicacionMapa UbicacionMapa { get; set; } //no queremos que tenga mas conexion que colmena
        //public Colmena Colmena { get; set; }  //agregados del difunto infoColmena

        public HistorialColmena()
        {
            EstadoSalud = "Sana";
            MotivoRespaldo = string.Empty;
        }
    }
}