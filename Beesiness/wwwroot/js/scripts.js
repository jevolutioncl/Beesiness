const container = document.getElementById('container');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});

function togglePasswordFields() {
    var passwordFields = document.getElementById('passwordFields');
    var isChecked = document.getElementById('changePasswordCheckbox').checked;
    passwordFields.style.display = isChecked ? 'block' : 'none';
}

document.addEventListener("DOMContentLoaded", function () {
    const hamBurger = document.querySelector(".toggle-btn");
    if (hamBurger) {
        hamBurger.addEventListener("click", function () {
            const sidebar = document.querySelector("#sidebar");
            if (sidebar) {
                sidebar.classList.toggle("expand");
            }
        });
    } else {
        console.warn("Elemento .toggle-btn no encontrado en esta página.");
    }
});

