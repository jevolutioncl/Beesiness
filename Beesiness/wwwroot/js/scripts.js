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