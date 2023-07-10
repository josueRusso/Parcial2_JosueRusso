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

        public string? Fecha { get; set; }

        public string? Concepto { get; set; }

        public string? PesoTotal { get; set; }

        public int ProductoId { get; set; }

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

        public string? CantidadUtilizada { get; set; }

    }
}
