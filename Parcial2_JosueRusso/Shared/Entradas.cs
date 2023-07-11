using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2_JosueRusso.Shared
{
    public class Entradas
    {
        [Key]
        public int EntradaId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required (ErrorMessage = "Debe tener un concepto")]
        public string? Concepto { get; set; }
        [Required (ErrorMessage = "Debe tener un Peso Total")]
        public int PesoTotal { get; set; }

        public int ProductoId { get; set; }
        [Required(ErrorMessage = "Debe tener una Cantidad Producida")]
        public int  CantidadProducida { get; set; }


        [ForeignKey(("EntradasDetalle"))]

        public ICollection<EntradasDetalle> EntradasDetalles { get; set; } = new List<EntradasDetalle>();

    }


    public class EntradasDetalle
    {
        [Key]
        public int DetalleId { get; set; }

        public int EntradaId { get; set; }

        public int ProductoId { get; set; }

        public double  CantidadUtilizada { get; set; }

    }

    
}
