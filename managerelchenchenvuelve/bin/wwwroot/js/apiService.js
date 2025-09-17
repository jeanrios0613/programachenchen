document.addEventListener("DOMContentLoaded", function () {
    fetch('/api/ServicesApi/users')
        .then(response => response.json())
        .then(data => {
            const select = document.getElementById('usuarioAsignadoSelect');

            data.forEach(user => {
                const option = document.createElement('option');
                option.value = user.userName;
                option.textContent = user.userName;

                // Si el usuario ya está asignado en el modelo, seleccionarlo
                if (user.userName === "@Model.UsuarioAsignado") {
                    option.selected = true;
                }

                select.appendChild(option);
            });
        })
        .catch(error => console.error('Error cargando usuarios:', error));
});