
async function GraficoBarras(canvas, labels, data, title) {  
    
    new Chart(canvas, {
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


//cargamos todos los graficos cuando esta lista la pagina
$(document).ready(() => {
    GraficoBarras();
    GraficoColmenasFecha();
})      