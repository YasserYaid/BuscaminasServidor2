﻿using System.ComponentModel.DataAnnotations;

namespace Datos.BuscaminasModelos
{
    public class Sala
    {
        [Key]
        public int idSala { get; set; }
        public string codigo { get; set; }
        public int idJugadorSala { get; set; }
    }
}
