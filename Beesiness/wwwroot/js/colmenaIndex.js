let currentFilterType = 'tipoColmena'; // Filtro predeterminado

function toggleFilterDropdown() {
    const dropdown = document.getElementById('filterOptions');
    if (dropdown) {
        dropdown.classList.toggle('show'); // Alterna la clase 'show'
    } else {
        console.error('El elemento del dropdown no se encontró.');
    }
}

// Cierra el dropdown si se hace clic fuera
window.addEventListener('click', function (event) {
    if (!event.target.matches('.btn-filter') && !event.target.closest('#filterOptions')) {
        const dropdown = document.getElementById('filterOptions');
        if (dropdown && dropdown.classList.contains('show')) {
            dropdown.classList.remove('show');
        }
    }
});

function setFilterType(filterType) {
    currentFilterType = filterType;
    const dropdownButton = document.getElementById('filterDropdown');
    if (dropdownButton) {
        dropdownButton.innerHTML = `<i class="fa-solid fa-filter"></i> Filtrar por: ${filterType}`;
    }
    filterUsers(); // Llama al filtro inmediatamente
}

function filterUsers() {
    const searchString = document.getElementById('searchString').value;

    fetch(`/Colmena/ColmenaIndex?searchString=${encodeURIComponent(searchString)}&filterType=${encodeURIComponent(currentFilterType)}`)
        .then(response => response.text())
        .then(html => {
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');
            const tableBody = doc.querySelector('tbody');
            if (tableBody) {
                document.querySelector('.table tbody').innerHTML = tableBody.innerHTML;
            } else {
                console.error('No se encontró el tbody en la respuesta del servidor.');
            }
        })
        .catch(error => console.error('Error al filtrar:', error));
}

function goToPage(pageNumber) {
    const searchString = document.getElementById('searchString').value;

    fetch(`/Colmena/ColmenaIndex?searchString=${encodeURIComponent(searchString)}&filterType=${encodeURIComponent(currentFilterType)}&pageNumber=${pageNumber}`)
        .then(response => response.text())
        .then(html => {
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');
            const tableBody = doc.querySelector('tbody');
            document.querySelector('.table tbody').innerHTML = tableBody.innerHTML;

            const pagination = doc.querySelector('.pagination');
            document.querySelector('.pagination').innerHTML = pagination.innerHTML;
        })
        .catch(error => console.error('Error al cambiar de página:', error));
}

window.goToPage = goToPage;
