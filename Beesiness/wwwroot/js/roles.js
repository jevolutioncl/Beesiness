let currentFilterType = 'Nombre'; // Valor predeterminado

// Función para alternar la visibilidad del menú de filtros
function toggleFilterDropdown() {
    const dropdown = document.getElementById('filterOptions');
    const isVisible = dropdown.style.display === 'block';
    dropdown.style.display = isVisible ? 'none' : 'block'; // Alternar entre mostrar y ocultar
}

// Función para establecer el tipo de filtro
function setFilterType(filterType) {
    currentFilterType = filterType;
    document.getElementById('filterDropdownText').innerText = filterType; // Actualiza el texto del botón
    document.getElementById('filterOptions').style.display = 'none'; // Oculta el menú después de seleccionar un filtro
    filterUsers(); // Filtrar inmediatamente al cambiar el tipo
}

// Función para filtrar roles según el texto y tipo de filtro
function filterUsers() {
    const searchString = document.getElementById('searchString').value;
    fetch(`/Rol/RolIndex?searchString=${searchString}&filterType=${currentFilterType}`)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table tbody').innerHTML = doc.querySelector('.table tbody').innerHTML;
            const pagination = doc.querySelector('.pagination');
            if (pagination) {
                document.querySelector('.pagination').innerHTML = pagination.innerHTML;
            }
        })
        .catch(error => console.error('Error al filtrar roles:', error));
}

// Función para cambiar de página en la tabla
function goToPage(pageNumber) {
    const searchString = document.getElementById('searchString').value;
    fetch(`/Rol/RolIndex?searchString=${searchString}&filterType=${currentFilterType}&pageNumber=${pageNumber}`)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table tbody').innerHTML = doc.querySelector('.table tbody').innerHTML;
            document.querySelector('.pagination').innerHTML = doc.querySelector('.pagination').innerHTML;
        })
        .catch(error => console.error('Error al cambiar de página:', error));
}

// Escucha eventos para cerrar el menú si se hace clic fuera de él
document.addEventListener('click', function (event) {
    const filterButton = document.getElementById('filterDropdown');
    const filterOptions = document.getElementById('filterOptions');

    if (!filterButton.contains(event.target) && !filterOptions.contains(event.target)) {
        filterOptions.style.display = 'none'; // Cierra el menú si se hace clic fuera de él
    }
});

window.goToPage = goToPage;
