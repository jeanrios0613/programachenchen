$(document).ready(function () {
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

    // Province, District and Corregimiento initialization
    const provinciaSelect = document.getElementById("provincia");
    const distritoSelect = document.getElementById("distrito");
    const corregimientoSelect = document.getElementById("corregimiento");

    const provincia = ["Panama", "Panama Oeste", "Colon", "Bocas del Toro", "Chiriqui", "Darien", "Veraguas", "Los Santos", "Cocle", "Herrera"];

    const distritos = {
        "Bocas del Toro": ["Bocas del Toro", "Changuinola", "Chiriquí Grande"],
        "Cocle": ["Aguadulce", "Antón", "La Pintada", "Natá", "Olá", "Penonomé"],
        "Colon": ["Colón", "Chagres", "Donoso", "Portobelo", "Santa Isabel"],
        "Chiriqui": ["Alanje", "Barú", "Boquerón", "Boquete", "Bugaba", "David", "Dolega", "Gualaca", "Remedios", "Renacimiento", "San Félix", "San Lorenzo", "Tolé"],
        "Darien": ["Chepigana", "Pinogana", "Santa Fe"],
        "Herrera": ["Chitré", "Las Minas", "Los Pozos", "Ocú", "Parita", "Pesé", "Santa María"],
        "Los Santos": ["Guararé", "Las Tablas", "Los Santos", "Macaracas", "Pedasí", "Pocrí", "Tonosí"],
        "Panama": ["Balboa", "Chepo", "Chimán", "Panamá", "San Miguelito", "Taboga"],
        "Panama Oeste": ["Arraiján", "Capira", "Chame", "La Chorrera", "San Carlos"],
        "Veraguas": ["Atalaya", "Calobre", "Cañazas", "La Mesa", "Las Palmas", "Mariato", "Montijo", "Río de Jesús", "San Francisco", "Santa Fe", "Santiago", "Soná"]
    };

    const corregimientos = {
        "Bocas del Toro": ["Bastimentos", "Bocas del Toro", "Cauchero", "Punta Laurel"],
        "Changuinola": ["Barriada 4 de Abril", "El Empalme", "Finca 30", "Finca 6", "Finca 60", "Guabito", "La Gloria", "Las Tablas", "El Silencio", "Valle del Risco"],
        "Chiriquí Grande": ["Bajo Cedro", "Chiriquí Grande", "Miramar", "Punta Peña", "Rambala"],
        "Aguadulce": ["Aguadulce", "El Cristo", "El Roble", "Pocrí", "Barrios Unidos"],
        "Antón": ["Antón", "Caballero", "El Chiru", "El Retiro", "El Valle", "Juan Díaz", "Río Hato", "San Juan de Dios", "Santa Rita"],
        "La Pintada": ["La Pintada", "El Harino", "El Potrero", "Llano Grande", "Piedras Gordas"],
        "Natá": ["Natá", "Capellanía", "El Caño", "Guzmán", "Las Huacas", "Toza"],
        "Olá": ["Olá", "El Copé", "El Palmar", "La Pava"],
        "Penonomé": ["Penonomé", "Cañaveral", "Coclé", "El Coco", "Pajonal", "Río Grande", "Río Indio", "Toabré", "Tulú"],
        "Colón": ["Barrio Norte", "Barrio Sur", "Buena Vista", "Cativá", "Cristóbal", "Escobal", "Sabanitas"],
        "Chagres": ["Achiote", "El Guabo", "La Encantada", "Nuevo Chagres", "Palmas Bellas"],
        "Donoso": ["Coclé del Norte", "El Guásimo", "Miguel de La Borda", "Río Indio"],
        "Portobelo": ["Cacique", "Garrote", "Isla Grande", "María Chiquita", "Portobelo"],
        "Santa Isabel": ["Cuango", "Miramar", "Nombre de Dios", "Palmira", "Palma Real", "Santa Isabel"],
        "David": ["Bijagual", "Cochea", "David", "Guaca", "Las Lomas", "Pedregal", "San Carlos", "San Pablo Nuevo", "San Pablo Viejo"],
        "Boquete": ["Alto Boquete", "Bajo Boquete", "Caldera", "Palmira"],
        "Bugaba": ["Bugaba", "Cerro Punta", "El Bongo", "Gómez", "La Concepción", "Santa Marta", "Santo Domingo", "Sortová"],
        "Barú": ["Baco", "Progreso", "Puerto Armuelles"],
        "Alanje": ["Alanje", "Divala", "Guarumal", "Palo Grande", "Querévalo"],
        "Boquerón": ["Boquerón", "Bágala", "Cordillera", "Guabal", "Guayabal", "Paraíso"],
        "Dolega": ["Dolega", "Dos Ríos", "Los Algarrobos", "Potrerillos", "Tinajas"],
        "Gualaca": ["Gualaca", "Hornito", "Los Angeles", "Río Sereno"],
        "Remedios": ["El Nancito", "El Porvenir", "Remedios", "Santa Lucía"],
        "Renacimiento": ["Breñón", "Jaramillo", "Monte Lirio", "Plaza de Caisán", "Río Sereno"],
        "San Félix": ["Juay", "Las Lajas", "San Félix"],
        "San Lorenzo": ["Boca Chica", "Horconcitos", "San Lorenzo"],
        "Tolé": ["Bella Vista", "Cerro Viejo", "El Cristo", "Justo Fidel Palacios", "Potrero de Caña", "Quebrada de Piedra", "Tolé"],
        "Chepigana": ["Garachiné", "Jaqué", "Puerto Piña", "Sambú", "Setegantí"],
        "Pinogana": ["Boca de Cupe", "Metetí", "Paya", "Púcuro", "Yaviza"],
        "Santa Fe": ["Santa Fe"],
        "Chitré": ["Chitré", "La Arena", "Los Pozos", "Monagrillo", "San Juan Bautista"],
        "Las Minas": ["Chepo", "El Toro", "Las Minas", "Quebrada del Rosario"],
        "Los Pozos": ["La Pitaloza", "Los Pozos", "El Capurí"],
        "Ocú": ["Ocú", "Los Llanos", "Peñas Chatas"],
        "Parita": ["Cabecera", "Parita", "Portobelillo"],
        "Pesé": ["El Barrero", "Pesé", "Rincón Hondo"],
        "Santa María": ["Santa María"],
        "Las Tablas": ["La Palma", "Las Tablas", "San José", "Santo Domingo"],
        "Santiago": ["Canto del Llano", "Edwin Fábrega", "La Colorada", "La Peña", "Santiago"],
        "Atalaya": ["Atalaya"],
        "Calobre": ["Calobre", "El María", "San José"],
        "Cañazas": ["Cañazas", "El Picador"],
        "La Mesa": ["La Mesa"],
        "Las Palmas": ["Las Palmas"],
        "Mariato": ["Mariato", "Quebro"],
        "Montijo": ["Montijo"],
        "Río de Jesús": ["Río de Jesús"],
        "San Francisco": ["San Francisco"],
        "Soná": ["Soná"],
        "Cémaco": ["Lajas Blancas", "Manené"],
        "Sambú": ["Sambú"],
        "Ailigandí": ["Ailigandí"],
        "Cartí Sugdup": ["Cartí Sugdup"],
        "Narganá": ["Narganá"],
        "Besikó": ["Soloy"],
        "Kankintú": ["Kankintú"],
        "Kusapín": ["Kusapín"],
        "Müna": ["Müna"],
        "Ñürüm": ["Ñürüm"],
        "Mironó": ["Mironó"],
        "Nole Duima": ["Nole Duima"],
        "Santa Catalina o Calovébora": ["Santa Catalina o Calovébora"],
        "Panamá": ["24 de Diciembre", "Alcalde Díaz", "Ancón", "Bella Vista", "Betania", "Chilibre", "Curundú", "El Chorrillo", "Juan Díaz", "Las Cumbres", "Pacora", "Parque Lefevre", "Pedregal", "Pueblo Nuevo", "Río Abajo", "San Felipe", "San Francisco", "San Martín", "Santa Ana", "Tocumen"],
        "San Miguelito": ["Amelia Denis de Icaza", "Belisario Frías", "Belisario Porras", "José Domingo Espinar", "Mateo Iturralde", "Omar Torrijos", "Rufina Alfaro", "Victoriano Lorenzo"],
        "Chepo": ["Chepo", "Cañita", "Chepillo", "Las Margaritas", "Santa Cruz"],
        "Chimán": ["Brujas", "Chimán", "Gonzalo Vásquez", "Pásiga", "Unión Santeña"],
        "Taboga": ["Otoque Oriente", "Otoque Occidente", "Taboga"],
        "Arraiján": ["Arraiján", "Burunga", "Cerro Silvestre", "Juan Demóstenes Arosemena", "Nuevo Emperador", "Santa Clara", "Vista Alegre"],
        "La Chorrera": ["Barrio Balboa", "Barrio Colón", "El Arado", "Guadalupe", "Hurtado", "Iturralde", "La Represa", "Los Díaz", "Obaldía", "Playa Leona"],
        "Capira": ["Capira", "Cirí Grande", "Cirí de Los Sotos", "El Cacao", "La Trinidad", "Villa Carmen"],
        "Chame": ["Bejuco", "Buenos Aires", "Cabuya", "Chame", "El Líbano", "Las Lajas", "Nueva Gorgona", "Punta Chame", "Sajalices", "Sorá"],
        "San Carlos": ["El Espino", "Guayabito", "La Ermita", "La Laguna", "Las Uvas", "Los Llanitos", "San Carlos", "San José"],
        "Colón": ["Barrio Norte", "Barrio Sur", "Buena Vista", "Cativá", "Ciricito", "Cristóbal", "Escobal", "Limón", "Nueva Providencia", "Sabanitas", "Salamanca"],
        "Chagres": ["Achiote", "El Guabo", "La Encantada", "Nuevo Chagres", "Palmas Bellas"],
        "Donoso": ["Coclé del Norte", "El Guásimo", "Miguel de La Borda", "Río Indio"],
        "Portobelo": ["Cacique", "Garrote", "Isla Grande", "María Chiquita", "Portobelo"],
        "Santa Isabel": ["Cuango", "Miramar", "Nombre de Dios", "Palmira", "Palma Real", "Santa Isabel"]
    };

    // Initialize province select
    if (provinciaSelect) {
        provinciaSelect.innerHTML = '<option value="">Seleccione una opción</option>';
        provincia.forEach(provincia => {
            const option = document.createElement("option");
            option.textContent = provincia;
            option.value = provincia;
            provinciaSelect.appendChild(option);
        });

        // Add change event listener for province
        provinciaSelect.addEventListener("change", function () {
            const provincia = this.value;
            if (distritoSelect) {
                distritoSelect.innerHTML = '<option value="">Seleccione una opción</option>';
                if (corregimientoSelect) {
                    corregimientoSelect.innerHTML = '<option value="">Seleccione una opción</option>';
                }

                if (provincia in distritos) {
                    distritos[provincia].forEach(distrito => {
                        const option = document.createElement("option");
                        option.textContent = distrito;
                        option.value = distrito;
                        distritoSelect.appendChild(option);
                    });
                }
            }
        });
    }

    // Add change event listener for district
    if (distritoSelect) {
        distritoSelect.addEventListener("change", function () {
            const distrito = this.value;
            if (corregimientoSelect) {
                corregimientoSelect.innerHTML = '<option value="">Seleccione una opción</option>';

                if (distrito in corregimientos) {
                    corregimientos[distrito].forEach(corregimiento => {
                        const option = document.createElement("option");
                        option.textContent = corregimiento;
                        option.value = corregimiento;
                        corregimientoSelect.appendChild(option);
                    });
                }
            }
        });
    }
});

 
document.getElementById("termsCheckbox").addEventListener("change", function () {
    if (this.checked) { 
        let modal = new bootstrap.Modal(document.getElementById('termsModal'));
        modal.show();
    }
});


document.addEventListener('DOMContentLoaded', function () {
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
    const openModalBtn = document.getElementById('openModal');
    const confirmationModal = new bootstrap.Modal(document.getElementById('confirmationModal'));
    const form = document.querySelector('form');
    const termsCheckbox = document.getElementById('termsCheckbox');

    openModalBtn.addEventListener('click', function () {
        if (!termsCheckbox.checked) {
            mostrarToast('Debe aceptar los términos y condiciones para continuar');
            return;
        }

        const requiredFields = form.querySelectorAll('[required]');
        let isValid = true;
        let firstInvalidField = null;

        requiredFields.forEach(field => {
            if (!field.value.trim()) {
                isValid = false;
                field.classList.add('is-invalid');
                if (!firstInvalidField) {
                    firstInvalidField = field;
                }
            } else {
                field.classList.remove('is-invalid');
            }
        });

        if (!isValid) {
            mostrarToast('Por favor complete todos los campos requeridos');
            if (firstInvalidField) {
                firstInvalidField.focus();
            }
            return;
        }

        const validationMessages = form.querySelectorAll('.text-danger');
        let hasValidationErrors = false;

        validationMessages.forEach(span => {
            if (span.textContent.trim() !== '') {
                hasValidationErrors = true;
            }
        });

        if (hasValidationErrors) {
            mostrarToast('Por favor corrija los errores en el formulario antes de continuar.');
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
    const form = tipoFormularioSelect.closest('form');

    const sectionEmpre = document.getElementById('EnterprisesEmpre');
    const sectionNegoc = document.getElementById('EnterprisesNegoc');
    const ayudaEmpre = document.getElementById('Emprendedor');
    const ayudaNegoc = document.getElementById('Negocio');

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
    }

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
 


