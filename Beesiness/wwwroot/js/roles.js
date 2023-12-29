let currentFilterType = 'Nombre'; // Valor predeterminado

function setFilterType(filterType) {
    currentFilterType = filterType;
    document.getElementById('currentFilterType').innerText = filterType;
    filterUsers(); // Filtrar inmediatamente al cambiar el tipo
}


function filterUsers() {
    const searchString = document.getElementById('searchString').value;
    fetch(`/Rol/RolIndex?searchString=${searchString}&filterType=${currentFilterType}`)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table').innerHTML = doc.querySelector('.table').innerHTML;
            const pagination = doc.querySelector('.pagination');
            if (pagination) {
                document.querySelector('.pagination').innerHTML = pagination.innerHTML;
            }
        })
        .catch(error => console.error('Error al filtrar usuarios:', error));
}
function goToPage(pageNumber) {
    const searchString = document.getElementById('searchString').value;
    fetch(`/Rol/RolIndex?searchString=${searchString}&filterType=${currentFilterType}&pageNumber=${pageNumber}`)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table').innerHTML = doc.querySelector('.table').innerHTML;
            document.querySelector('.pagination').innerHTML = doc.querySelector('.pagination').innerHTML;
        })
        .catch(error => console.error('Error al cambiar de página:', error));
}

window.goToPage = goToPage;