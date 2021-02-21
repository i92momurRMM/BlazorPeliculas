using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorPeliculas.Shared.Entidades
{
    public class Notificacion
    {
        public int Id { get; set; }
        // Los siguientes 3 campos los proporciona el navegador
        public string URL { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
