using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    // Esta clase es usada para que cada vez que se actualice IndexedDB, se refresque las sincronizaciones pendientes
    // Esta clase encapsula un evento al cual nuestro sincronizador se va a suscribir para que otros componentes puedan notificar el cambio y se ejecute un metodo de nuestro componente sincronizador
    public class AppState
    {
        public event Func<Task> OnActualizarSincronizacionesPendientes;
        public async Task NotificarActualizarSincronizacionesPendientes() => 
            await OnActualizarSincronizacionesPendientes?.Invoke();
    }
}
