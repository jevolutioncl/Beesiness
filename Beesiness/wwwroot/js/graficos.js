
let barChartInstance = null;
let doughnutChartInstance = null;
async function GraficoBarras(canvas, labels, data, title) {
    if (barChartInstance !== null) {
        barChartInstance.destroy();
    }
    barChartInstance = new Chart(canvas, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: title,
                data: data,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}
function GraficoDoughnut(canvas, labels, data, title) {
    if (doughnutChartInstance != null) {
        doughnutChartInstance.destroy();
    }
    doughnutChartInstance = new Chart(canvas, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                label: title,
                data: data,
                backgroundColor: [
                    // Aquí puedes definir colores para cada segmento
                    'red', 'blue', 'green', 'yellow', 'orange'
                ]
            }]
        },
        options: {
            responsive: true
        }
    });
}


//cargamos todos los graficos cuando esta lista la pagina
$(document).ready(() => {
    // Esta llamada a GraficoBarras() ha sido eliminada porque se está llamando sin parámetros
    GraficoColmenasFecha();
    GraficoTipoColmenas();
});  