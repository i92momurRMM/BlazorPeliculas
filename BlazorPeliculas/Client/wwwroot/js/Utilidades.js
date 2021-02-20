// Creamos base de datos indexedDB
var db = new Dexie("mydb");
var dbVersion = 1;

//---------------------------------SINCRONIZAR POST Y DELETE -------------------------------------------
// Creamos las tablas crear y borrar con un campo id autonumerico, estas tablas contiene las llamadas a la api para POST y DELETE
db.version(dbVersion).stores({
    crear: 'id++',
    borrar: 'id++'
});

// Devuelve registros de las tablas
async function obtenerRegistrosPendientes() {
    return await {
        ObjetosACrear: await db.crear.toArray(),
        ObjetosABorrar: await db.borrar.toArray()
    };
}

async function obtenerCantidadRegistrosPendientes() {
    const pendientesCrear = await db.crear.count();
    const pendientesBorrar = await db.borrar.count();
    return pendientesCrear + pendientesBorrar;
}

async function guardarRegistroParaCrear(url, cuerpo) {
    await db.crear.put({ url, cuerpo: JSON.parse(cuerpo) });
}

async function guardarRegistroParaBorrar(url) {
    await db.borrar.put({ url });
}

async function borrarRegistro(tabla, id) {
    await db[tabla].where({ "id": id }).delete();
}

//----------------------------------------------------------------------------

async function obtenerEstatusPermisoNotificaciones() {
    const estatusPermiso = Notification.permission;
    if (estatusPermiso === 'denied') { return estatusPermiso; }

    const worker = await navigator.serviceWorker.getRegistration();
    const suscripcionExistente = await worker.pushManager.getSubscription();

    if (suscripcionExistente) { return "granted"; }
    else { return "default"; }
}

async function suscribirUsuario() {

    var permisoNotificacion = await Notification.requestPermission();

    if (permisoNotificacion !== 'granted') {
        return null;
    }

    const worker = await navigator.serviceWorker.getRegistration();
    const suscripcionExistente = await worker.pushManager.getSubscription();

    if (!suscripcionExistente) {
        const llavePublicaResponse = await fetch('/api/config/notificacionesllavepublica');
        const llavePublica = await llavePublicaResponse.text();
        console.log(llavePublica);

        const nuevaSuscripcion = await worker.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: llavePublica
        });

        return construirRespuestaSuscripcion(nuevaSuscripcion);
    } else {
        return construirRespuestaSuscripcion(suscripcionExistente);
    }
}

async function desuscribirUsuario() {
    const worker = await navigator.serviceWorker.getRegistration();
    const suscripcionExistente = await worker.pushManager.getSubscription();

    if (suscripcionExistente) {
        suscripcionExistente.unsubscribe();
        return construirRespuestaSuscripcion(suscripcionExistente);
    }
}

function construirRespuestaSuscripcion(suscripcion) {
    return {
        url: suscripcion.endpoint,
        p256dh: arrayBufferToBase64(suscripcion.getKey('p256dh')),
        auth: arrayBufferToBase64(suscripcion.getKey('auth'))
    }
}

function arrayBufferToBase64(buffer) {
    // https://stackoverflow.com/a/9458996
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}