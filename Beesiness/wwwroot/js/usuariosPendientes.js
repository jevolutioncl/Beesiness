﻿let currentFilterType = 'Nombre'; // Valor predeterminado

function setFilterType(filterType) {
    currentFilterType = filterType;
    document.getElementById('currentFilterType').innerText = filterType;
    // Ocultar los botones de ordenamiento de fecha
    document.getElementById('dateSortButtons').style.display = 'none';
    // Mostrar el campo de búsqueda
    document.getElementById('searchString').style.display = 'block';
    filterUsers(); // Filtrar inmediatamente al cambiar el tipo
}

function filterUsers() {
    const searchString = document.getElementById('searchString').value;
    fetch(`/Auth/RequestRegistrationIndex?searchString=${searchString}&filterType=${currentFilterType}`)
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
function showDateSortButtons() {
    // Actualizar el texto del filtro actual a "Fecha"
    document.getElementById('currentFilterType').innerText = 'Fecha';
    // Ocultar campo de búsqueda
    document.getElementById('searchString').style.display = 'none';
    // Mostrar botones de ordenamiento de fecha
    document.getElementById('dateSortButtons').style.display = 'block';
}

function sortUsers(sortOrder) {
    document.getElementById('currentSortOrder').value = sortOrder;
    const searchString = document.getElementById('searchString').value;
    fetch(`/Auth/RequestRegistrationIndex?searchString=${searchString}&sortOrder=${sortOrder}`)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table').innerHTML = doc.querySelector('.table').innerHTML;
            // Actualizar paginación, etc.
        })
        .catch(error => console.error('Error al ordenar usuarios:', error));
}
function goToPage(pageNumber) {
    const searchString = document.getElementById('searchString').value;
    let sortOrder = document.getElementById('currentSortOrder').value;
    let url = `/Auth/RequestRegistrationIndex?searchString=${searchString}&filterType=${currentFilterType}&pageNumber=${pageNumber}`;

    if (sortOrder) {
        url += `&sortOrder=${sortOrder}`;
    }

    fetch(url)
        .then(response => response.text())
        .then(html => {
            const doc = new DOMParser().parseFromString(html, "text/html");
            document.querySelector('.table').innerHTML = doc.querySelector('.table').innerHTML;
            document.querySelector('.pagination').innerHTML = doc.querySelector('.pagination').innerHTML;
        })
        .catch(error => console.error('Error al cambiar de página:', error));
}

window.goToPage = goToPage;