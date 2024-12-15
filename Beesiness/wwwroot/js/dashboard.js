document.addEventListener("DOMContentLoaded", async () => {
    try {
        const response = await fetch('/api/colmenas/estadisticas');
        const data = await response.json();

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
    } catch (error) {
        console.error("Error al cargar datos del dashboard:", error);
    }
});
