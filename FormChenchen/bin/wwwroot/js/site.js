$(document).ready(function () {

    // Activa y desactiva las seccione dentro del formulario por cada opcion que se presiona. 
    var empreSections = $("#EnterprisesEmpre");
    var negocSections = $("#EnterprisesNegoc");


    $("#envio, #Details, #Conctact, #EnterprisesEmpre, #EnterprisesNegoc, #Emprendedor, #Negocio").hide();

    $("#TipoFormulario").on("change", function () {
        var seleccion = $(this).val();


        if (empreSections.parent().length === 0) {
            empreSections.insertAfter("#Conctact").parent();
        }
        if (negocSections.parent().length === 0) {
            negocSections.insertAfter("#Conctact").parent();
        }
 
        $("#envio, #Details, #Conctact, #EnterprisesEmpre, #EnterprisesNegoc, #Emprendedor, #Negocio").hide();

        if (seleccion == "Emprendimiento") {
            $("#envio, #Details, #EnterprisesEmpre, #Emprendedor, #Conctact").show();
            $("#Negocio").hide();
            negocSections.detach();
            $('div').removeClass('position-absolute');

        } else if (seleccion == "Negocio existente") {
            $("#envio, #Details, #EnterprisesNegoc, #Negocio, #Conctact").show();

            empreSections.detach();
            $('div').removeClass('position-absolute');
        }
    }).trigger('change');
 
   

    if ('@TempData["SuccessMessage"]' != '') {
        $('#successModal').modal('show');
    }


    const charCountBusiness   = document.getElementById("charCountBusiness");
    const ReasonForMoney      = document.getElementById("ReasonForMoney");
    const BusinessDescription = document.getElementById("BusinessDescription");
    

    const charCount           = document.getElementById("charCount");
    const maxLength           = 500// textarea.getAttribute("maxlength");
    const provinciaSelect     = document.getElementById("provincia");
    const distritoSelect      = document.getElementById("distrito");
    const corregimientoSelect = document.getElementById("corregimiento");


    // cuenta la cantidad de caraceter para en que lo invertiras

    BusinessDescription.addEventListener("input", () => {
        charCountBusiness.textContent = `${BusinessDescription.value.length} / ${maxLength}`;
    });

    ReasonForMoney.addEventListener("input", () => {
        charCount.textContent = `${ReasonForMoney.value.length} / ${maxLength}`;
    });
 
    async function loadEconomicActivities() {
        const response   = await fetch("/api/ubicaciones/EconomicActivities");
        const activities = await response.json();

        const select = document.getElementById("EconomicActivity");

        activities.forEach(a => { const option = document.createElement("option");
        option.value       = a.name;
        option.textContent = a.name;
        select.appendChild(option);
       });
     }

    document.getElementById("EconomicActivity").addEventListener("change", function () {
             const otherDiv = document.getElementById("EconomicActivityOther");
               otherDiv.style.display = this.value === "Otros" ? "block" : "none";
          });

    // Cargar lista al iniciar
    loadEconomicActivities(); 


    // Inicializar Provincias
    fetch("/api/ubicaciones/provincias")
        .then(res => res.json())
        .then(data => {
            provinciaSelect.innerHTML = '<option selected disabled value="">Seleccione una opción</option>';
            data.forEach(prov => {
                const option = document.createElement("option");
                option.textContent = prov.name;
                option.value = prov.id;
                provinciaSelect.appendChild(option);
            });
        });

    // Al cambiar provincia, cargar distritos
    provinciaSelect.addEventListener("change", function () {
        const provinciaId = this.value;
        distritoSelect.innerHTML = '<option selected disabled value="">Seleccione una opción</option>';
        corregimientoSelect.innerHTML = '<option selected disabled value="">Seleccione una opción</option>';

        if (!provinciaId) return;

        fetch(`/api/ubicaciones/distritos/${provinciaId}`)
            .then(res => res.json())
            .then(data => {
                data.forEach(dist => {
                    const option = document.createElement("option");
                    option.textContent = dist.name;
                    option.value = dist.id;
                    distritoSelect.appendChild(option);
                });
            });
    });

    // Al cambiar distrito, cargar corregimientos
    distritoSelect.addEventListener("change", function () {
        const distritoId = this.value;
        corregimientoSelect.innerHTML = '<option value="">Seleccione una opción</option>';

        if (!distritoId) return;

        fetch(`/api/ubicaciones/corregimientos/${distritoId}`)
            .then(res => res.json())
            .then(data => {
                data.forEach(corr => {
                    const option = document.createElement("option");
                    option.textContent = corr.name;
                    option.value = corr.id;
                    corregimientoSelect.appendChild(option);
                });
            });
    });

});


// Activa el model de terminos y condiciones cuando el check es activado. 
document.getElementById("termsCheckbox").addEventListener("change", function () {
    if (this.checked) { 
        let modal = new bootstrap.Modal(document.getElementById('termsModal'));
        modal.show();
    }
});


document.addEventListener('DOMContentLoaded', function () {

    //Para validar los numero de telefono
    var phoneInput = document.getElementById('phoneNumber');
    if (phoneInput) {
        phoneInput.addEventListener('input', function (e) {
            let value = phoneInput.value.replace(/\D/g, '');  
            if (value.length > 4) {
                value = value.slice(0, 4) + '-' + value.slice(4, 8);
            }
            phoneInput.value = value;
        });
 }
     
    var dvInput = document.getElementById('DvEmpresa');
    if (dvInput) {
        dvInput.addEventListener('input', function (e) {
            // Solo permitir números
            let value = this.value.replace(/\D/g, '');
            this.value = value;
            
            
            if (value.trim()) {
                this.classList.remove('is-invalid'); 
                const validationSpan = this.parentNode.querySelector('.text-danger');
                if (validationSpan) {
                    validationSpan.textContent = '';
                }
            }
        });
         
        dvInput.addEventListener('focus', function () {
            if (this.value.trim()) {
                this.classList.remove('is-invalid');
                const validationSpan = this.parentNode.querySelector('.text-danger');
                if (validationSpan) {
                    validationSpan.textContent = '';
                }
            }
        });
    }
});
 

document.addEventListener('DOMContentLoaded', function () {

    const form = document.querySelector('form');
   
    new AutoNumeric('#ProyectedSales', {
        decimalCharacter: '.',
        digitGroupSeparator: ',',
        decimalPlaces: 2
    });

    new AutoNumeric('#MonthlySales', {
        decimalCharacter: '.',
        digitGroupSeparator: ',',
        decimalPlaces: 2
    });

    new AutoNumeric('#QuantityToInvert', {
        decimalCharacter: '.',
        digitGroupSeparator: ',',
        decimalPlaces: 2
    });

            
    // Accion para los botones que se habren del temino y condiciones. 
    document.getElementById('closeModal').addEventListener('click', function () {
        confirmationModal.hide();
    });

    document.getElementById('confirmButton').addEventListener('click', function () {
        form.submit();
    });
});

    function clearSelection(idSeleccion) {
        document.getElementById(idSeleccion).value = '';
}


document.addEventListener('DOMContentLoaded', function () {

    /* ===  Cuando presiona el boton de Enviar solicitud valida el checkbox de termino y condiciones. ===
       ===  SI EL BOTON DE CHECK NO ESTA ACTIVO EL BOTON NO REALIZA EL SUBMIT                         === 
     */
    const openModalBtn = document.getElementById('openModal');
    const confirmationModal = new bootstrap.Modal(document.getElementById('confirmationModal'));
    const form = document.querySelector('form');
    const termsCheckbox = document.getElementById('termsCheckbox');
    const validationMessages = form.querySelectorAll('.text-danger');
    let hasValidationErrors = false;
    const requiredFields = form.querySelectorAll('[required]');
    let isValid = true;
    let firstInvalidField = null;

    function obtenerNombreCampo(field) {
        if (field.id) {
            const lbl = document.querySelector('label[for="' + field.id + '"]');
            if (lbl && lbl.textContent.trim()) return lbl.textContent.trim();
        }
        const containerLabel = field.closest('div')?.querySelector('label');
        if (containerLabel && containerLabel.textContent.trim()) return containerLabel.textContent.trim();
        if (field.getAttribute('aria-label')) return field.getAttribute('aria-label');
        if (field.placeholder) return field.placeholder;
        return field.name || field.id || 'Campo';
    }


    openModalBtn.addEventListener('click', function () {
        if (!termsCheckbox.checked) {
            mostrarToast('Debe aceptar los términos y condiciones para continuar');
            return;
        }
     

        const camposInvalidos = [];
        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                isValid = false;
                field.classList.add('is-invalid');
                camposInvalidos.push(obtenerNombreCampo(field));
                if (!firstInvalidField) {
                     firstInvalidField = field;
                }
            } else {
                field.classList.remove('is-invalid');
            }
        });


        // ESTO ACTIVA EL RECUADRO EN ROJO DE ADVERTENCIA
  
 
        validationMessages.forEach(span => {
            if (span.textContent.trim() !== '') {
                hasValidationErrors = true;
            }
        });

        if (hasValidationErrors) {
            const errores = [];
            validationMessages.forEach(span => {
                const txt = span.textContent.trim();
                if (txt) errores.push(txt);
            });
            const detalle = errores.length ? ' Detalles: ' + [...new Set(errores)].join(' | ') : '';
            mostrarToast('Por favor corrija los errores en el formulario antes de continuar.' + detalle);
            return;
        }

        confirmationModal.show();
    });


    document.getElementById('confirmButton').addEventListener('click', function () {
        form.submit();
    });

    document.getElementById('closeModal').addEventListener('click', function () {
        confirmationModal.hide();
    });

    form.querySelectorAll('input, select, textarea').forEach(field => {
        field.addEventListener('input', function () {
            if (this.value.trim()) {
                this.classList.remove('is-invalid');
            }
        });
    });
});


document.addEventListener('DOMContentLoaded', function () {
    const select = document.getElementById('EconomicActivity');
    const otherInput = document.getElementById('EconomicActivityOther');

    select.addEventListener('change', function () {
        if (select.value === 'Otros') {
            otherInput.style.display = 'block';
            otherInput.required = true;
            select.name = ''; // Para que no se envíe el valor "Otros"
            otherInput.name = 'Enterprise.EconomicActivity';
        } else {
            otherInput.style.display = 'none';
            otherInput.required = false;
            otherInput.value = '';
            select.name = 'Enterprise.EconomicActivity';
            otherInput.name = '';
        }
    });

    // Si el formulario se recarga y "Otros" estaba seleccionado, muestra el input
    if (select.value === 'Otros') {
        otherInput.style.display = 'block';
        otherInput.required = true;
        select.name = '';
        otherInput.name = 'Enterprise.EconomicActivity';
    }
});
 

document.addEventListener('DOMContentLoaded', function () {
    const tipoFormularioSelect = document.getElementById('TipoFormulario');
    const form         = tipoFormularioSelect.closest('form');
    const sectionEmpre = document.getElementById('EnterprisesEmpre');
    const sectionNegoc = document.getElementById('EnterprisesNegoc');
    const ayudaEmpre   = document.getElementById('Emprendedor');
    const ayudaNegoc   = document.getElementById('Negocio');

    const selectActividad = document.getElementById('Enterprise.EconomicActivity');
    const otherInput = document.getElementById('EconomicActivityOther');

    function limpiarCamposDe(seccion) {
        const inputs = seccion.querySelectorAll('input, select, textarea');
        inputs.forEach(el => {
            if (el.type === 'checkbox' || el.type === 'radio') {
                el.checked = false;
            } else {
                el.value = '';
            }
        });
    }

 /*
    function actualizarActividadEconomica() {
        if (!selectActividad || !otherInput) return;

        if (selectActividad.value === 'Otros') {
            otherInput.style.display = 'block';
            otherInput.required = true;
            selectActividad.name = '';
            otherInput.name = 'Enterprise.EconomicActivity';
        } else {
            otherInput.style.display = 'none';
            otherInput.required = false;
            otherInput.value = '';
            selectActividad.name = 'Enterprise.EconomicActivity';
            otherInput.name = '';
        }
    } */

    function actualizarSecciones() {
        if (tipoFormularioSelect.value === "Emprendimiento") {
            sectionEmpre.style.display = "block";
            ayudaEmpre.style.display = "block";
            sectionNegoc.style.display = "none";
            ayudaNegoc.style.display = "none";
            limpiarCamposDe(sectionNegoc);
        } else if (tipoFormularioSelect.value === "Negocio existente") {
            sectionEmpre.style.display = "none";
            ayudaEmpre.style.display = "none";
            sectionNegoc.style.display = "block";
            ayudaNegoc.style.display = "block";
            limpiarCamposDe(sectionEmpre);
        } else {
            sectionEmpre.style.display = "none";
            ayudaEmpre.style.display = "none";
            sectionNegoc.style.display = "none";
            ayudaNegoc.style.display = "none";
            limpiarCamposDe(sectionEmpre);
            limpiarCamposDe(sectionNegoc);
        }

        if (selectActividad) {
            selectActividad.selectedIndex = 0;
        }
        actualizarActividadEconomica();
    }


    tipoFormularioSelect.addEventListener('change', actualizarSecciones);

    if (selectActividad) {
        selectActividad.addEventListener('change', actualizarActividadEconomica);
    }

    actualizarSecciones();
    actualizarActividadEconomica();
});


 
    function mostrarToast(mensaje) {
        const toastBody = document.getElementById('toastBody');
    toastBody.innerHTML = `<strong>Ocurrió un error</strong><br>${mensaje}`;
        
        const toastEl = document.getElementById('alertToast');
        const toast = new bootstrap.Toast(toastEl, { delay: 5000 });
        toast.show();
    }

 
$(document).ready(function () { 
    $.validator.addMethod("fechamaximahoy", function (value, element, param) { 
        
        var inputDate = new Date(value);
        var today = new Date();
        today.setHours(0, 0, 0, 0);  
        
        return inputDate <= today;
    }, "La fecha no puede ser mayor a hoy");
     
    $.validator.unobtrusive.adapters.add("fechamaximahoy", ["maxdate"], function (options) {
        options.rules["fechamaximahoy"] = options.params.maxdate;
        options.messages["fechamaximahoy"] = options.message;
    });

 
    $('#OperationsStartDate').on('change', function () {
        var inputDate = new Date($(this).val());
        var today = new Date();
        today.setHours(0, 0, 0, 0);
        
        if ($(this).val() && inputDate > today) {
            $(this).addClass('is-invalid');
            var errorSpan = $(this).siblings('.text-danger');
            if (errorSpan.length === 0) {
                $(this).after('<span class="text-danger" style="font-size: 14px">La fecha no puede ser mayor a hoy</span>');
            } else {
                errorSpan.text('La fecha no puede ser mayor a hoy');
            }
        } else {
            $(this).removeClass('is-invalid');
            $(this).siblings('.text-danger').text('');
        }
    });
});
 


