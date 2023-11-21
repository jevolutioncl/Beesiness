function loadMapScenario() {
    var map = new Microsoft.Maps.Map('#myMap', {
        center: new Microsoft.Maps.Location(-35.31917337366901, -71.36038685586249),
        zoom: 250,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial
    });

    // Cargar los datos de las colmenas desde el servidor
    fetch('/Colmena/ObtenerDatosColmenas')
        .then(response => response.json())
        .then(colmenas => {
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
        })
        .catch(error => {
            console.error('Error al cargar los datos de las colmenas:', error);
        });
}
function closeInfobox() {
    var customInfobox = document.getElementById('customInfobox');
    customInfobox.style.display = 'none';
}
