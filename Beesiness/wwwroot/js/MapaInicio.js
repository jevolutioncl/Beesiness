let map;
let infobox;


function loadMapScenario() {

    map = new Microsoft.Maps.Map('#myMap', {
        center: new Microsoft.Maps.Location(-35.6, -71.4),
        zoom: 8,
        mapTypeId: Microsoft.Maps.MapTypeId.aerial
    });

    infobox = new Microsoft.Maps.Infobox(map.getCenter(), {
        visible: false,
        offset: new Microsoft.Maps.Point(0, 20) 
    });
    infobox.setMap(map);


    const colmenasGenericas = [
        {
            id: 1,
            latitude: -35.5,
            longitude: -71.5,
            tipoColmena: 'Langstroth',
            numIdentificador: 'G-001',
            fechaIngreso: '2023-01-15',
            descripcion: 'Colmena en buen estado.'
        },
        {
            id: 2,
            latitude: -35.5,
            longitude: -71.5,
            tipoColmena: 'Dadant',
            numIdentificador: 'G-002',
            fechaIngreso: '2023-02-20',
            descripcion: 'Colmena nueva.'
        },
        {
            id: 3,
            latitude: -35.5,
            longitude: -71.5,
            tipoColmena: 'Warré',
            numIdentificador: 'G-003',
            fechaIngreso: '2023-03-10',
            descripcion: 'Colmena en revisión.'
        }
    ];

    const offsetIncrement = 0.002; 
    let offsetCount = 0;

    colmenasGenericas.forEach(colmena => {

        const location = new Microsoft.Maps.Location(
            colmena.latitude + offsetIncrement * offsetCount,
            colmena.longitude + offsetIncrement * offsetCount
        );
        offsetCount++;

        const pin = new Microsoft.Maps.Pushpin(location, {
            icon: '/css/bee-box.png',
            anchor: new Microsoft.Maps.Point(16, 16)
        });


        pin.metadata = {
            title: `Colmena ${colmena.numIdentificador}`,
            description: `
                <div><strong>Tipo:</strong> ${colmena.tipoColmena}</div>
                <div><strong>Fecha de Ingreso:</strong> ${new Date(colmena.fechaIngreso).toLocaleDateString()}</div>
                <div><strong>Descripción:</strong> ${colmena.descripcion}</div>
            `
        };
        Microsoft.Maps.Events.addHandler(pin, 'click', function (e) {
            infobox.setOptions({
                location: e.target.getLocation(),
                title: e.target.metadata.title,
                description: e.target.metadata.description,
                visible: true
            });
        });

        map.entities.push(pin);
    });
}
