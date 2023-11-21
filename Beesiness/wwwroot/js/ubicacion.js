function loadMapScenario() {
    var map = new Microsoft.Maps.Map('#myMap', {
        // Configuración inicial del mapa
        center: new Microsoft.Maps.Location(-35.31917337366901, -71.36038685586249),
        zoom: 250,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial
    });

    Microsoft.Maps.Events.addHandler(map, 'click', function (e) {
        var point = new Microsoft.Maps.Point(e.getX(), e.getY());
        var location = e.target.tryPixelToLocation(point);

        document.getElementById('Latitude').value = location.latitude.toFixed(6);
        document.getElementById('Longitude').value = location.longitude.toFixed(6);
        document.getElementById('UbicacionColmena').value = locationName;


        var pin = new Microsoft.Maps.Pushpin(location);
        map.entities.clear();
        map.entities.push(pin);

        // Realiza la geocodificación inversa
        reverseGeocode(location);
    });
}

function reverseGeocode(location) {
    var geocodeRequest = "https://dev.virtualearth.net/REST/v1/Locations/" + location.latitude + "," + location.longitude + "?o=json&key=AmrnF-1-dvaNYDUkUuogYW8SOvy2cut1Xdxs4EzsahlVv8HrT_MvGx0HPKP8dOlZ";
    fetch(geocodeRequest)
        .then(response => response.json())
        .then(data => {
            if (data && data.resourceSets && data.resourceSets.length > 0 &&
                data.resourceSets[0].resources && data.resourceSets[0].resources.length > 0) {

                // Asegúrate de que aquí se está extrayendo solo el texto:
                var locationName = data.resourceSets[0].resources[0].name;
                document.getElementById('locationName').innerText = "Ubicación escogida: " + locationName;
                document.getElementById('UbicacionColmena').value = locationName; // Esto debería ser una cadena de texto, no un objeto
            } else {
                document.getElementById('locationName').innerText = "Nombre de ubicación no encontrado";
            }
        });
}
