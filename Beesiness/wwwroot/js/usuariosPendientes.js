let currentFilterType = 'Nombre'; // Valor predeterminado

// Cambiar el tipo de filtro y actualizar la interfaz
function setFilterType(filterType) {
    currentFilterType = filterType;

    // Actualiza el texto del botón del filtro
    const filterDropdown = document.getElementById('filterDropdown');
    filterDropdown.innerHTML = `<i class="fa-solid fa-filter"></i> ${filterType}`;

    // Ocultar los botones de ordenamiento de fecha si no es "Fecha"
    if (filterType !== 'Fecha') {
        document.getElementById('dateSortButtons').style.display = 'none';
        document.getElementById('searchString').style.display = 'block';
    } else {
        document.getElementById('searchString').style.display = 'none';
        document.getElementById('dateSortButtons').style.display = 'block';
    }

    // Filtrar de inmediato
    filterUsers();
}


// Filtrar usuarios basándose en el tipo y la cadena de búsqueda
function filterUsers() {
    const searchString = document.getElementById('searchString').value;
    const url = `/Auth/RequestRegistrationIndex?searchString=${encodeURIComponent(searchString)}&filterType=${currentFilterType}`;

    fetch(url)
        .then(response => {
            if (!response.ok) throw new Error('Error al obtener los datos');
            return response.text();
        })
        .then(html => {
            updateTableAndPagination(html);
        })
        .catch(error => console.error('Error al filtrar usuarios:', error));
}

// Mostrar los botones de ordenamiento de fecha
function showDateSortButtons() {
    setFilterType('Fecha');
}

// Ordenar usuarios por fecha ascendente o descendente
function sortUsers(sortOrder) {
    document.getElementById('currentSortOrder').value = sortOrder;
    const url = `/Auth/RequestRegistrationIndex?filterType=Fecha&sortOrder=${sortOrder}`;

    fetch(url)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table').innerHTML = doc.querySelector('.table').innerHTML;
            const pagination = doc.querySelector('.pagination');
            if (pagination) {
                document.querySelector('.pagination').innerHTML = pagination.innerHTML;
            }
        })
        .catch(error => console.error('Error al ordenar usuarios:', error));
}


// Navegar entre páginas
function goToPage(pageNumber) {
    const searchString = document.getElementById('searchString').value;
    const sortOrder = document.getElementById('currentSortOrder').value;
    const url = `/Auth/RequestRegistrationIndex?searchString=${encodeURIComponent(searchString)}&filterType=${currentFilterType}&pageNumber=${pageNumber}&sortOrder=${sortOrder || ''}`;

    fetch(url)
        .then(response => {
            if (!response.ok) throw new Error('Error al cargar la página');
            return response.text();
        })
        .then(html => {
            updateTableAndPagination(html);
        })
        .catch(error => console.error('Error al cambiar de página:', error));
}

// Función para actualizar la tabla y la paginación
function updateTableAndPagination(html) {
    const doc = new DOMParser().parseFromString(html, 'text/html');
    const newTable = doc.querySelector('.table');
    const newPagination = doc.querySelector('.pagination');

    // Actualizar la tabla
    if (newTable) {
        document.querySelector('.table').innerHTML = newTable.innerHTML;
    }

    // Actualizar la paginación
    if (newPagination) {
        document.querySelector('.pagination').innerHTML = newPagination.innerHTML;
    }
}
// Alternar la visibilidad del menú desplegable de filtros
function toggleFilterDropdown() {
    const filterOptions = document.getElementById('filterOptions');
    const isVisible = filterOptions.style.display === 'block';
    filterOptions.style.display = isVisible ? 'none' : 'block';
}

document.addEventListener('click', (event) => {
    const filterDropdown = document.getElementById('filterDropdown');
    const filterOptions = document.getElementById('filterOptions');

    if (!filterDropdown.contains(event.target)) {
        filterOptions.style.display = 'none'; // Oculta el menú si se hace clic fuera
    }
});

function setFilterType(filterType) {
    currentFilterType = filterType;
    document.getElementById('filterDropdownText').innerText = filterType;
    document.getElementById('filterOptions').style.display = 'none';

    if (filterType === 'Fecha') {
        document.getElementById('dateSortButtons').style.display = 'flex';
        document.getElementById('searchString').style.display = 'none';
    } else {
        document.getElementById('dateSortButtons').style.display = 'none';
        document.getElementById('searchString').style.display = 'block';
    }

    filterUsers();
}


// Asignar la función de navegación globalmente
window.goToPage = goToPage;
