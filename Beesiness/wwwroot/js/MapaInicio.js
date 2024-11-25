let map;
let customInfobox;
let closeInfoboxTimer;
let colmenas = [
    { id: 1, latitude: -35.4, longitude: -71.6, tipo: "Langstroth", identificador: "C-001", descripcion: "En revisión" },
    { id: 2, latitude: -35.5, longitude: -71.4, tipo: "Dadant", identificador: "C-002", descripcion: "En buen estado" },
    { id: 3, latitude: -35.6, longitude: -71.5, tipo: "Warré", identificador: "C-003", descripcion: "Requiere mantenimiento" },
    { id: 4, latitude: -35.7, longitude: -71.6, tipo: "Langstroth", identificador: "C-004", descripcion: "Requiere mantenimiento" }
];
let colmenaPins = [];

function loadMapScenario() {
    map = new Microsoft.Maps.Map('#myMap', {
        center: new Microsoft.Maps.Location(-35.5, -71.5),
        zoom: 8,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial
    });

    // Crear los pines de colmenas y agregarlos al mapa
    renderColmenasOnMap(colmenas);

    // Configurar el filtro
    document.getElementById("filterTipo").addEventListener("change", applyFilter);
}

function renderColmenasOnMap(colmenasToRender) {
    // Limpiar pines existentes
    map.entities.clear();
    colmenaPins = [];

    colmenasToRender.forEach(colmena => {
        const location = new Microsoft.Maps.Location(colmena.latitude, colmena.longitude);

        const pin = new Microsoft.Maps.Pushpin(location, {
            icon: '/css/bee-box.png',
            anchor: new Microsoft.Maps.Point(16, 16)
        });

        pin.metadata = {
            title: `Colmena ${colmena.identificador}`,
            description: `
                <div><strong>Tipo:</strong> ${colmena.tipo}</div>
                <div><strong>Descripción:</strong> ${colmena.descripcion}</div>
            `
        };

        Microsoft.Maps.Events.addHandler(pin, 'click', e => {
            showCustomInfobox(e.target, location);
        });

        map.entities.push(pin);
        colmenaPins.push(pin);
    });

    renderColmenaList(colmenasToRender);
}

function renderColmenaList(colmenasToRender) {
    const colmenaList = document.getElementById("colmenaList");
    colmenaList.innerHTML = ""; // Limpiar la lista actual

    colmenasToRender.forEach(colmena => {
        const colmenaItem = document.createElement("div");
        colmenaItem.className = "p-2 mb-1 border border-warning rounded bg-dark text-white cursor-pointer";
        colmenaItem.textContent = `Colmena ${colmena.identificador} - ${colmena.tipo}`;
        colmenaItem.onclick = () => {
            const location = new Microsoft.Maps.Location(colmena.latitude, colmena.longitude);
            map.setView({ center: location, zoom: 14 });
            showCustomInfobox({ metadata: colmenaItem.metadata }, location);
        };
        colmenaList.appendChild(colmenaItem);
    });
}

function applyFilter() {
    const filterValue = document.getElementById("filterTipo").value;
    const filteredColmenas = filterValue === "Todos" ? colmenas : colmenas.filter(colmena => colmena.tipo === filterValue);
    renderColmenasOnMap(filteredColmenas);
}

function showCustomInfobox(pin, location) {
    const pixelLocation = map.tryLocationToPixel(location, Microsoft.Maps.PixelReference.control);
    const customInfobox = document.getElementById('customInfobox');

    customInfobox.innerHTML = `
        <div class="card text-white bg-dark border-warning" style="max-width: 300px;">
            <div class="card-header d-flex justify-content-between align-items-center bg-warning text-dark">
                <h5 class="mb-0">${pin.metadata.title}</h5>
                <button class="btn-close btn-close-dark" onclick="closeCustomInfobox()" aria-label="Cerrar"></button>
            </div>
            <div class="card-body">
                <p class="card-text text-muted">
                    ${pin.metadata.description}
                </p>
            </div>
        </div>
    `;

    // Ajustar para que el infobox se muestre junto al icono del mapa
    const mapBounds = document.getElementById('myMap').getBoundingClientRect();
    let infoboxX = pixelLocation.x + 20;
    let infoboxY = pixelLocation.y - 50;

    if (infoboxX + customInfobox.offsetWidth > mapBounds.width) {
        infoboxX -= customInfobox.offsetWidth + 40;
    }
    if (infoboxY + customInfobox.offsetHeight > mapBounds.height) {
        infoboxY -= customInfobox.offsetHeight + 20;
    }

    customInfobox.style.left = `${infoboxX}px`;
    customInfobox.style.top = `${infoboxY}px`;
    customInfobox.style.display = 'block';

    clearTimeout(closeInfoboxTimer);
}

function closeCustomInfobox() {
    const customInfobox = document.getElementById('customInfobox');
    customInfobox.style.display = 'none';
}
