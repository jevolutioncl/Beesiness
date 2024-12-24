document.addEventListener("DOMContentLoaded", async () => {
    try {
        const response = await fetch('/api/colmenas/estadisticas');
        const data = await response.json();

        //agregado para ver las tareas        
        //const response2 = await fetch(`/api/tareas/tareasusuario`);
        //const data2 = await response2.json();

        // Actualizar tarjetas
        document.getElementById('colmenas-activas').innerText = `${data.colmenasActivas} activas`;
        document.getElementById('produccion-total').innerText = `${data.produccionTotal} kg de miel`;
        document.getElementById('alertas-total').innerText = `${data.alertasTotales} en riesgo`;

        // Producción mensual
        const productionCtx = document.getElementById("productionChart").getContext("2d");
        new Chart(productionCtx, {
            type: "line",
            data: {
                labels: data.produccionMensual.labels,
                datasets: [{
                    label: "Producción (kg)",
                    data: data.produccionMensual.data,
                    borderColor: "#ffcc00",
                    backgroundColor: "rgba(255, 204, 0, 0.2)",
                    tension: 0.4,
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: "top" },
                },
            }
        });

        // Tipos de colmenas
        const typesCtx = document.getElementById("colmenaTypesChart").getContext("2d");
        new Chart(typesCtx, {
            type: "doughnut",
            data: {
                labels: data.tiposColmenas.labels, // Usamos los labels del controlador
                datasets: [{
                    label: "Tipos de Colmenas",
                    data: data.tiposColmenas.data, // Usamos los datos del controlador
                    backgroundColor: ["#ffcc00", "#66b3ff", "#ff6666"],
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: "top" },
                },
            }
        });

        // Alertas recientes
        const alertsCtx = document.getElementById("alertsChart").getContext("2d");
        new Chart(alertsCtx, {
            type: "pie",
            data: {
                labels: ["Baja producción", "Plaga", "Otros"],
                datasets: [{
                    data: data.alertasRecientes,
                    backgroundColor: ["#ff6666", "#ffcc00", "#66b3ff"],
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: "right" },
                },
            }
        });

        
        //dashboard para tareas
        //const tareas = JSON.parse(data2);

        const taskListElement = document.getElementById('lista-tareas');
        const taskDetailPanel = document.getElementById('detalle-tarea');
        const taskTitleElement = document.getElementById('titulo-tarea');
        const taskDescriptionElement = document.getElementById('descripcion-tarea');
        const taskButton = document.getElementById('boton-tarea');

        // Función para cargar las tareas en el panel
        function cargarTareas() {
            //agregado para ver las tareas        
            //const response2 = await fetch(`/api/tareas/tareasusuario`);
            //const data2 = await response2.json();
            //const tareas = JSON.parse(data2);

            tareas.forEach(tarea => {
                const taskItem = document.createElement('div');
                taskItem.classList.add('item-tarea');
                taskItem.textContent = tarea.Nombre;
                taskItem.dataset.id = tarea.Id; // Guardar el ID de la tarea

                // Agregar evento de clic para mostrar el detalle
                taskItem.addEventListener('click', () => mostrarDetalleTarea(tarea));

                taskListElement.appendChild(taskItem);
            });
        }

        //Funcion alternativa a vargar tareas
        async function cargarTareas2() {
            await fetch('/api/tareas/tareasusuario')
                .then(response => response.json())
                .then(data => {
                    // Eliminar elementos antiguos
                    while (taskListElement.firstChild) {
                        taskListElement.removeChild(taskListElement.firstChild);
                    }

                    // Creamos nuevos elementos
                    let tareas = JSON.parse(data);
                    tareas.forEach(tarea => {
                        const taskItem = document.createElement('div');
                        taskItem.classList.add('item-tarea');
                        taskItem.textContent = tarea.Nombre;
                        taskItem.dataset.id = tarea.Id; // Guardar el ID de la tarea

                        // Agregar evento de clic para mostrar el detalle
                        taskItem.addEventListener('click', () => mostrarDetalleTarea(tarea));

                        taskListElement.appendChild(taskItem);
                    });
                    
                })
                .catch(error => console.error('Error:', error));
        }       

        // Función para mostrar el detalle de la tarea seleccionada
        function mostrarDetalleTarea(tarea) {
            taskTitleElement.textContent = `Título: ${tarea.Nombre}`;
            taskDescriptionElement.textContent = `Descripción: ${tarea.Descripcion}`;
            taskButton.textContent = `Tarea lista`
            taskButton.addEventListener('click', () => cambiarEstadoTarea(tarea));
            taskDetailPanel.style.display = 'block';

        }

        // Función para cambiar el estado de la tarea seleccionada
        async function cambiarEstadoTarea(tarea) {
            try {
                console.log("estamos en cambiarestadotarea");
                console.log(tarea.Id);

                //const respuesta = await fetch('/api/tareas/hola');
                //const prueba = await respuesta.status();

                const jsontarea = JSON.stringify(tarea);

                console.log(jsontarea);

                const respuesta = await fetch(`/api/tareas/cambiarestado`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: jsontarea
                });

                if (respuesta.ok) {                    
                    cargarTareas2();
                    //alert('Tarea marcada como realizada.');
                } else {
                    alert('Error al cambiar el estado de la tarea.');
                }
            } catch (error) {
                console.error('Error:', error);
                alert('No se pudo conectar con el servidor.');
            }
        }

        // Inicializar la carga de tareas
        cargarTareas2();         

    } catch (error) {
        console.error("Error al cargar datos del dashboard:", error);
    }
});


