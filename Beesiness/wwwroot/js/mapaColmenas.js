let colmenasData = [];
let pins = [];
let map;
let contextMenuInfobox;
function loadMapScenario() {
    map = new Microsoft.Maps.Map('#myMap', {
        center: new Microsoft.Maps.Location(-35.31917337366901, -71.36038685586249),
        zoom: 250,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial
    });

    // Cargar los datos de las colmenas desde el servidor
    fetch('/Colmena/ObtenerDatosColmenas')
        .then(response => response.json())
        .then(data => {
            colmenasData = data;
            displayColmenas(map, colmenasData);
        })
        .catch(error => {
            console.error('Error al obtener los datos de la colmena: ', error);
        });
    Microsoft.Maps.Events.addHandler(map, 'rightclick', function (e) {
        var point = new Microsoft.Maps.Point(e.pageX, e.pageY);
        var loc = map.tryPixelToLocation(point);

        if (!contextMenuInfobox) {
            contextMenuInfobox = new Microsoft.Maps.Infobox(loc, {
                htmlContent: '<div class="context-menu"><ul><li onclick="redirectToCreateLocation()">Guardar Ubicación</li></ul></div>',
                visible: false
            });
            contextMenuInfobox.setMap(map);
        }

        contextMenuInfobox.setLocation(loc);
        contextMenuInfobox.setOptions({ visible: true });

        // Guardar los valores de latitud, longitud y nivel de zoom en los inputs ocultos
        document.getElementById('Latitude').value = loc.latitude;
        document.getElementById('Longitude').value = loc.longitude;
        document.getElementById('ZoomLevel').value = map.getZoom();
    });
}
function redirectToCreateLocation() {
    window.location.href = `/ubicacion/ubicacioncrear?lat=${document.getElementById('Latitude').value}&lng=${document.getElementById('Longitude').value}&zoom=${document.getElementById('ZoomLevel').value}`;
}

function displayColmenas(map, colmenas) {
    map.entities.clear();
    pins = [];

    colmenas.forEach(colmena => {
        var location = new Microsoft.Maps.Location(colmena.latitude, colmena.longitude);
        var pin = new Microsoft.Maps.Pushpin(location, {
            icon: '/css/bee-box.png',
            anchor: new Microsoft.Maps.Point(16, 16)
        });

        // Agregar evento de clic al pin para mostrar tu infobox personalizado
        Microsoft.Maps.Events.addHandler(pin, 'click', function () {
            var pixelLocation = map.tryLocationToPixel(location, Microsoft.Maps.PixelReference.control);
            var customInfobox = document.getElementById('customInfobox');
            customInfobox.innerHTML = `
                    <div class="infobox-content">
                        <div>Fecha de Ingreso: ${new Date(colmena.fechaIngreso).toLocaleDateString()}</div>
                        <div>Tipo de Colmena: ${colmena.tipoColmena}</div>
                        <div>Descripción: ${colmena.descripcion}</div>
                        <button onclick="location.href='/ruta_a_detalle_colmena/${colmena.Id}'">Más detalles</button>
                    </div>
                    <button class="infobox-close-btn" onclick="closeInfobox()">X</button>
                `;
            customInfobox.style.top = `${pixelLocation.y}px`;
            customInfobox.style.left = `${pixelLocation.x}px`;
            customInfobox.style.display = 'block';
        });
        map.entities.push(pin);
    });
}

function applyFilter(filter) {
    var filteredColmenas = colmenasData;

    if (filter !== 'Todas') {
        filteredColmenas = colmenasData.filter(c => c.tipoColmena === filter);
    }

    displayColmenas(map, filteredColmenas);
}

document.getElementById('filtroTipoColmena').addEventListener('change', function (e) {
    applyFilter(e.target.value);
});
function closeInfobox() {
    var customInfobox = document.getElementById('customInfobox');
    customInfobox.style.display = 'none';
}
