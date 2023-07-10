using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2_JosueRusso.Shared
{
    public class Entradas
    {
        [Key]
        public int EntradaId { get; set; }
        [Required (ErrorMessage = "Debe tener Una fecha")]
        public string? Fecha { get; set; }
        [Required (ErrorMessage = "Debe tener un concepto")]
        public string? Concepto { get; set; }
        [Required (ErrorMessage = "Debe tener un Peso Total")]
        public string? PesoTotal { get; set; }

        public int ProductoId { get; set; }
        [Required(ErrorMessage = "Debe tener una Cantidad Producida")]
        public string? CantidadProducida { get; set; }


        [ForeignKey(("EntradasDetalle"))]

        public ICollection<EntradasDetalle> EntradasDetalles { get; set; } = new List<EntradasDetalle>();

    }


    public class EntradasDetalle
    {
        [Key]
        public int DetalleId { get; set; }

        public int EntradaId { get; set; }

        public int ProductoId { get; set; }

        public float  CantidadUtilizada { get; set; }

    }

    
}
