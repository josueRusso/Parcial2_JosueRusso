﻿using System;
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

        public string? Descripcion { get; set; }

        public int Tipo { get; set; }

        public float Existencia { get; set; }
    }
}
