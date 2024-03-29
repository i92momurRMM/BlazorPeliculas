﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorPeliculas.Shared.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "El campo {0} es requerido")]
        [Required(
            ErrorMessageResourceType = typeof(Resources.Resource), 
            ErrorMessageResourceName = nameof(Resources.Resource.required))]
        public string Nombre { get; set; }
        public List<GeneroPelicula> GeneroPeliculas { get; set; }
    }
}
