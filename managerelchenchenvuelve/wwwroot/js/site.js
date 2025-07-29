////////////////////////////////
let temporizador;

$(document).ready(function () {
    $("#TareaUser").change(function () {

        var seleccion = $(this).val();

        if (seleccion == "C") {
            $("#TareaC").show();
            $("#TareaP").hide();
            $("#TareaA").hide();
        
             
        } else if (seleccion == "P") {
            $("#TareaC").hide();
            $("#TareaP").show();
            $("#TareaA").hide();
        
        } else {
            $("#TareaC").hide();
            $("#TareaP").hide();
            $("#TareaA").show();
    
        }

    }).change();

 
    $("#gestionreal").change(function () {
        var seleccion = $(this).val();

        if (seleccion == "Atendido") {
            $("#atencion").show();
            $("#razon").hide();
        } else if (seleccion == "Llamada realizada sin exito") {
            $("#atencion").hide();
            $("#razon").show();
        } else {
            $("#atencion").hide();
            $("#razon").hide();
        }
    }).change();

 
});


$('#buscarUsuario').on('keyup', function () {
    clearTimeout(temporizador);
    var query = $(this).val();

    temporizador = setTimeout(function () {

         window.location.href = '../Usuario/Index?search=' + query;
        
    }, 800); 
});

function showCancelModal() {
    $('#confirmationModal').modal('show');
}

function submitForm() {
    $('#approvalModal').modal('hide');
    $('form').submit();
}

function showCommentModal(title, type) {
 
    $('#commentModalLabel').text(title);
    $('#commentModalMessage').text(`¿Está seguro que desea ${title.toLowerCase()} la solicitud?`);
    
 
    $('#commentType').val(type);
     
    $('#commentText').val(''); 
 
    $('#commentModal').modal('show');
}

function submitComment(textareaId, TypeRequest) {
    var commentText = $('#' + textareaId).val();
    var typeRequest = $('#' + TypeRequest).val();
    var requestCode = $('#CodigoDeSolicitud').val(); 
    var gestor = $('#Gestor').val(); 
    var etapa = $('#Etapa').val(); 

    if (!commentText || !requestCode || !gestor) {
        mostraralertModal('Por favor complete todos los campos requeridos');
        return;
    }

    $.ajax({
        url: '/Requests/AddComment',
        type: 'POST',
        data: {
            requestCode: requestCode,
            commentText: commentText,
            gestor: gestor,
            Etapa: etapa,
            TypeRequest: typeRequest
        },
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (response.success) {
                $('#commentModal').modal('hide'); 
                $('#commentText').val('');
                mostraralertModal('Comentario agregado exitosamente');  
            } else {
                mostrarErrorModal(response.message || 'Error al agregar el comentario');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error details:', { xhr: xhr, status: status, error: error });
            mostrarErrorModal('Error al procesar la solicitud: ' + requestCode);
        }
    });
}


$('#buscaform').on('keyup', function () {
   
    clearTimeout(temporizador);
    var query = $(this).val();

    temporizador = setTimeout(function () {

        if (query) {
            window.location.href = '../Process/Index?tarea=B&search=' + query;
        } else {
            window.location.href = '../Process/Index?';
        }

    }, 1000);
});


$(document).ready(function () { 
    var urlParams = new URLSearchParams(window.location.search);
    var searchValue = urlParams.get('search');
     
    if (searchValue) {
        $('#buscaform').val(searchValue);
    }
});

 


function actualizarSeleccion() {
    const checkboxes = document.querySelectorAll('.tarea-checkbox:checked');
    const actionBar = document.getElementById('action-bar');
    const selectedCount = document.getElementById('selected-count');

    selectedCount.textContent = checkboxes.length;

    if (checkboxes.length > 0) {
        actionBar.style.display = 'flex';
        setTimeout(() => actionBar.classList.add('visible'), 10);
    } else {
        actionBar.classList.remove('visible');
        setTimeout(() => {
            if (checkboxes.length === 0) {
                actionBar.style.display = 'none';
            }
        }, 300);
    }
}

function seleccionarTodos() {

    const checkAll = document.getElementById('check-all');

    const checkboxes = document.querySelectorAll('tbody .tarea-checkbox');

    checkboxes.forEach(checkbox => {
        checkbox.checked = checkAll.checked;
    });

    actualizarSeleccion();
}

function limpiarSeleccion() {
    const checkboxes = document.querySelectorAll('.tarea-checkbox');
    checkboxes.forEach(checkbox => checkbox.checked = false);
    actualizarSeleccion();
}

function asignarSeleccionados() {
    const selectedIds = Array.from(document.querySelectorAll('.tarea-checkbox:checked'))
        .map(checkbox => checkbox.dataset.id); 

    console.log('IDs seleccionados:', selectedIds);
}

function mostrarModalUsuarios() {
    const selectedIds = Array.from(document.querySelectorAll('.tarea-checkbox:checked'))
        .map(checkbox => checkbox.dataset.id);

    if (selectedIds.length === 0) {
        mostraralertModal("Selecciona al menos una tarea.");
        return;
    }

    const modal = new bootstrap.Modal(document.getElementById('modalUsuarios'));
    modal.show();
}

function asignarSeleccionadosA(nombreUsuario) {
    const selectedIds = Array.from(document.querySelectorAll('.tarea-checkbox:checked'))
        .map(checkbox => checkbox.dataset.id);

    if (selectedIds.length === 0) {
        mostraralertModal("Selecciona al menos una tarea.");
        return;
    }

    fetch('/Process/AsignarTareas', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
        },
        body: JSON.stringify({
            ids: selectedIds,
            usuario: nombreUsuario
        })
    })
        .then(response => {
            if (response.ok) {
                mostraralertModal("Tareas asignadas correctamente.");
               
            } else {
                mostrarErrorModal("Error al asignar tareas.");
            }
        })
        .catch(error => {
            console.error('Error:', error);
            mostrarErrorModal("Error al asignar tareas.");
        });

  
    const modal = bootstrap.Modal.getInstance(document.getElementById('modalUsuarios'));
    modal.hide();
}



function CargaInfo(data) {
    window.location.href = '../Archivos/SubirArchivo?ProcessId=' + data;

}


function GenereReporte() {
    var startDate = document.getElementById('startDate').value;
    var endDate = document.getElementById('endDate').value;


    var url = '../Reportes/Descargar_Reportes';
    var params = [];

    if (startDate) params.push('Fecin=' + startDate);
    if (endDate) params.push('Fecfin=' + endDate);


    if (params.length > 0) {
        url += '?' + params.join('&');
    }

    window.location.href = url;
}


document.addEventListener('DOMContentLoaded', function () {

    new AutoNumeric('#QuantityToInvert', {
        decimalCharacter: '.',
        digitGroupSeparator: ',',
        decimalPlaces: 2
    });

    new AutoNumeric('#MonthlySales', {
        decimalCharacter: '.',
        digitGroupSeparator: ',',
        decimalPlaces: 2
    });

    new AutoNumeric('#ProyectedSales', {
        decimalCharacter: '.',
        digitGroupSeparator: ',',
        decimalPlaces: 2
    });
});

// Modal de error reutilizable
function mostrarErrorModal(mensaje) {
    document.getElementById('errorModalMessage').textContent = mensaje;
    var modal = new bootstrap.Modal(document.getElementById('errorModal'));
    modal.show();
}
    
 
function mostraralertModal(mensaje) {
    document.getElementById('alertModalBody').textContent = mensaje;
    var modal = new bootstrap.Modal(document.getElementById('alertModal'), {
    backdrop: 'static',
keyboard: false
    });
modal.show();
}

function mostrarConfirmaModal(mensaje) {
    document.getElementById('ConfirmaBody').textContent = mensaje;
    var modal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'), {
        backdrop: 'static',
        keyboard: false
    });
    modal.show();
}
