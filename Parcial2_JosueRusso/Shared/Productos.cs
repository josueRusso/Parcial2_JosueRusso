using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2_JosueRusso.Shared
{
    public class Productos
    {
        [Key]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La descripcion es un campo obligatorio")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Es necesario especificar el precio con el que se compro el producto")]
        public double PrecioCompra { get; set; }

        [Required(ErrorMessage = "Es necesario especificar el precio con el que se vende el producto")]
        public double PrecioVenta { get; set; }

        [Required(ErrorMessage = "Es necesario especifcar la cantidad de productos que existen")]
        public int Existencia { get; set; }
    }
}