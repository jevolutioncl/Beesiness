let currentFilterType = 'Nombre'; // Valor predeterminado

function setFilterType(filterType) {
    currentFilterType = filterType;
    document.getElementById('filterDropdown').innerText = filterType; // Actualiza el texto del botón
    filterUsers(); // Filtrar inmediatamente al cambiar el tipo
}

function filterUsers() {
    const searchString = document.getElementById('searchString').value;
    fetch(`/Usuarios/GestionUsuario?searchString=${searchString}&filterType=${currentFilterType}`)
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
    fetch(`/Usuarios/GestionUsuario?searchString=${searchString}&filterType=${currentFilterType}&pageNumber=${pageNumber}`)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table').innerHTML = doc.querySelector('.table').innerHTML;
            document.querySelector('.pagination').innerHTML = doc.querySelector('.pagination').innerHTML;
        })
        .catch(error => console.error('Error al cambiar de página:', error));
}

function toggleFilterDropdown() {
    const dropdown = document.getElementById('filterOptions');
    dropdown.style.display = dropdown.style.display === 'block' ? 'none' : 'block';
}

document.addEventListener('click', (event) => {
    const dropdown = document.getElementById('filterOptions');
    if (!event.target.closest('.filter-group')) {
        dropdown.style.display = 'none';
    }
});

window.goToPage = goToPage;
