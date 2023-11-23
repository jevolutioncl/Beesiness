let colmenasData = [];
let pins = [];
let map;
let contextMenuInfobox;
let closeInfoboxTimer;
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
        var point = new Microsoft.Maps.Point(e.getX(), e.getY());
        var loc = map.tryPixelToLocation(point);

        if (!contextMenuInfobox) {
            contextMenuInfobox = new Microsoft.Maps.Infobox(loc, {
                htmlContent: `
            <div class="context-menu">
                <button class="context-menu-button" onclick="redirectToCreateLocation()">Guardar Ubicación</button>
                <button class="context-menu-button" onclick="redirectToCreateColmena()">Guardar Colmena</button>
                <button class="context-menu-close" onclick="closeContextMenu()">Cerrar</button>
            </div>`,
                visible: false
            });
            contextMenuInfobox.setMap(map);
        }

        contextMenuInfobox.setLocation(loc);
        contextMenuInfobox.setOptions({ visible: true });

        document.getElementById('Latitude').value = loc.latitude;
        document.getElementById('Longitude').value = loc.longitude;
        document.getElementById('ZoomLevel').value = map.getZoom();
    });

}
function closeContextMenu() {
    if (contextMenuInfobox) {
        contextMenuInfobox.setOptions({ visible: false });
    }
}

function redirectToCreateLocation() {
    // Guardar en el almacenamiento local o de sesión
    localStorage.setItem('LocationData', JSON.stringify({
        latitude: document.getElementById('Latitude').value,
        longitude: document.getElementById('Longitude').value,
        zoomLevel: document.getElementById('ZoomLevel').value
    }));

    // Redirigir a la nueva vista
    window.location.href = '/Mapa/UbicacionCrear';
}
function redirectToCreateColmena() {
    
    var lat = document.getElementById('Latitude').value;
    var lng = document.getElementById('Longitude').value;

    if (lat && lng) {
        // Guardar en el almacenamiento local
        localStorage.setItem('ColmenaLocation', JSON.stringify({
            latitude: lat,
            longitude: lng
        }));

        
        window.location.href = '/Colmena/ColmenaCrear';
    }
}
function displayColmenas(map, colmenas) {
    map.entities.clear();
    pins = [];
    const listaColmenas = document.getElementById('listaColmenas');
    listaColmenas.innerHTML = ''; // Limpia la lista actual

    colmenas.forEach(colmena => {
        var location = new Microsoft.Maps.Location(colmena.latitude, colmena.longitude);
        var pin = new Microsoft.Maps.Pushpin(location, {
            icon: '/css/bee-box.png',
            anchor: new Microsoft.Maps.Point(16, 16)
        });

        Microsoft.Maps.Events.addHandler(pin, 'mouseover', function () {
            showInfobox(pin, colmena);
        });

        Microsoft.Maps.Events.addHandler(pin, 'mouseout', function () {
            
            closeInfoboxTimer = setTimeout(closeInfobox, 500); 
        });

        var colmenaItem = document.createElement('div');
        colmenaItem.classList.add('colmena-item');
        colmenaItem.textContent = `Colmena ID: ${colmena.Id}`;
        colmenaItem.onclick = function () {
            map.setView({ center: new Microsoft.Maps.Location(colmena.latitude, colmena.longitude), zoom: 15 });
        };
        listaColmenas.appendChild(colmenaItem);
        map.entities.push(pin);
    });
}
function showInfobox(pin, colmena) {
    var pixelLocation = map.tryLocationToPixel(pin.getLocation(), Microsoft.Maps.PixelReference.control);
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
    clearTimeout(closeInfoboxTimer);
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
    if (customInfobox) {
        customInfobox.style.display = 'none';
    }
}

var customInfobox = document.getElementById('customInfobox');
customInfobox.addEventListener('mouseenter', function () {
    clearTimeout(closeInfoboxTimer); 
});
customInfobox.addEventListener('mouseleave', function () {
    closeInfobox(); 
});

function loadPredeterminedLocations() {
    fetch('/Mapa/ObtenerUbicacionesPredeterminadas')
        .then(response => response.json())
        .then(data => {
            let select = document.getElementById('ubicacionesPredeterminadas');
            data.forEach(location => {
                let option = document.createElement('option');
                option.value = JSON.stringify({
                    id: location.id,
                    lat: location.latitude,
                    lng: location.longitude,
                    zoom: location.zoomLevel
                });
                option.textContent = location.nombre;
                select.appendChild(option);
            });
        });
}
function toggleFiltros() {
    console.log('toggleFiltros llamado'); 
    var contenido = document.getElementById('contenidoFiltros');
    contenido.style.display = contenido.style.display === 'block' ? 'none' : 'block';
}

document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM completamente cargado y analizado");
    var botonFiltros = document.querySelector('.boton-filtros');
    botonFiltros.addEventListener('click', toggleFiltros);
});

document.getElementById('ubicacionesPredeterminadas').addEventListener('change', function (e) {
    let location = JSON.parse(e.target.value);
    map.setView({ center: new Microsoft.Maps.Location(location.lat, location.lng), zoom: location.zoom });
});

function eliminarUbicacionSeleccionada() {
    var select = document.getElementById('ubicacionesPredeterminadas');
    var selectedIndex = select.selectedIndex;

    if (selectedIndex > -1) {
        var selectedOption = select.options[selectedIndex];
        var ubicacion = JSON.parse(selectedOption.value);

        if (ubicacion.id && confirm('¿Estás seguro de que quieres eliminar esta ubicación?')) {
            fetch(`/Mapa/EliminarUbicacion?id=${ubicacion.id}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        console.log('Ubicación eliminada');
                        select.remove(selectedIndex); // Elimina la opción del DOM
                    } else {
                        console.error('Error al eliminar la ubicación');
                    }
                })
                .catch(error => console.error('Error:', error));
        }
    }
}



loadPredeterminedLocations();