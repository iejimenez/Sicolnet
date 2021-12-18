var seleccionFecha = {
    labelMonthNext: 'Ir al siguiente mes',
    labelMonthPrev: 'Ir al mes anterior',
    labelMonthSelect: 'Seleccionar mes',
    labelYearSelect: 'Seleccionar año',
    labelDaySelect: 'aqui',
    klass: {
        navPrev: '',
        navNext: ''
    },

    selectMonths: true,
    selectYears: 100,
    //min: new Date(1800, 1, 1),
    today: 'Hoy',
    close: 'Cerrar',
    clear: '',
    format: 'yyyy-mm-dd',
    onSet: function (context) {

        //adjustStyling({ target: this.$node[0] });
    }
}

function fetchGet(url, data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: `${url}`,
            type: "GET",
            dataType: "json",
            data: data,
            success: function (data) {
                resolve(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                reject(new Error(`${errorThrown} - ${url}`));
            }
        });
    });
}

function SetUrlForQuery(stringrelativeserver) {
    return window.location.origin + stringrelativeserver;
}

function Get_Meses(Num, Id) {

    var ListaMesesString = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
    var html_rol = '';
    $.each(ListaMesesString, function (key, value) {
        if (key <= Num)
            html_rol += '<option value="' + value + '">' + value + '</option>';
    });

    $('#' + Id).html(html_rol);
    $('#' + Id).val("");
    $('#' + Id).select2();
}


function DescargarPDF(tipo, id) {
    var formURL = '/report/generate?tipo=' + tipo + "&Id=" + id;
    window.open(formURL, "_black");
}



function Validador(idform, rules, mensajes, onlyOnSubmit = false) {

    if (mensajes == undefined)
        mensajes = []
    var validator = $("#" + idform).validate({
        lang: "ES",


        ignore: ':hidden:not(.do-not-ignore), input[type=hidden], .select2-search__field .ignore', // ignore hidden fields
        errorClass: 'validation-invalid-label',
        successClass: 'validation-valid-label',
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        },
        unhighlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        },
        //ignore: '',
        // Different components require proper error label placement
        errorPlacement: function (error, element) {

            // Styled checkboxes, radios, bootstrap switch
            if (element.parents('div').hasClass("checker") || element.parents('div').hasClass("choice") || element.parent().hasClass('bootstrap-switch-container')) {
                if (element.parents('label').hasClass('checkbox-inline') || element.parents('label').hasClass('radio-inline')) {
                    error.appendTo(element.parent().parent().parent().parent());
                }
                else {
                    error.appendTo(element.parent().parent().parent().parent().parent());
                }
            }
            else if (element.hasClass('form-check-input-styled-danger') || element.hasClass('form-check-input-styled-success') || element.hasClass('form-check-input-styled-primary') || element.hasClass('form-check-input-styled-warning')) {
                error.appendTo(element.parent().parent().parent().parent().parent());
            }

            else if (element.hasClass('form-check-input-styled-danger') || element.hasClass('form-check-input-styled-success') || element.hasClass('form-check-input-styled-primary') || element.hasClass('form-check-input-styled-warning')) {
                error.appendTo(element.parent().parent().parent().parent().parent());
            }


            // Unstyled checkboxes, radios
            else if (element.parents('div').hasClass('checkbox') || element.parents('div').hasClass('radio')) {
                error.appendTo(element.parent().parent().parent());
            }

            // Input with icons and Select2
            else if (element.parents('div').hasClass('has-feedback') || element.hasClass('select2-hidden-accessible')) {
                error.appendTo(element.parent());
            }

            // Inline checkboxes, radios
            else if (element.parents('label').hasClass('checkbox-inline') || element.parents('label').hasClass('radio-inline')) {
                error.appendTo(element.parent().parent());
            }

            // Input group, styled file input
            else if (element.parent().hasClass('uploader') || element.parents().hasClass('input-group')) {
                error.appendTo(element.parent().parent().parent().parent());
            }

            else if (element.parent().hasClass('multiselect-native-select')) {
                error.appendTo(element.parent().parent());
            }

            else {
                error.insertAfter(element);
            }
        },
        rules: rules,
        messages: mensajes
    });
    return validator;
}


function createValidation(formName, validationConfig) {
    //const validationConfig = this.getValidationConfig();
    this.formValidator = this.formValidator === null || this.formValidator === undefined ? {} : this.formValidator;
    this.formValidator[formName] = Validador(formName, validationConfig);
    this.formValidator[formName].resetForm();
}

function inputChangeHandlerFunction(event) {
    const input = $(event.target);
    const type = input.data('type');
    const model = input.data("model");
    if (type == "money") {
        this[model][event.target.name] = input.val() ? formatMoneyToNumber(input.val().toUpperCase()) : 0;
        format(event.target, 1);
    }
    else if (type == 'moneyDecla') {
        const decimals = input.data('decimals');

        this[model][event.target.name] = input.val() ? formatMoneyToNumber(input.val().toUpperCase(), decimals) : 0;

        this[model][event.target.name]

        event.target.value = FormatoConPuntosSinRed(this[model][event.target.name], decimals);
    }
    else
        this[model][event.target.name] = input.val() ? input.val().trim().toUpperCase() : "";

}



var LoadingPetition = 0;
function ShowLoading(reset) {
    if (reset)
        LoadingPetition = 0;
    if (!Swal.isVisible()) {
        LoadingPetition++;
        Swal.fire({
            allowOutsideClick: false,
            allowEscapeKey: false,
            background: 'transparent ',
            showConfirmButton: false,
            confrimButtonClass: "d-none",
            onBeforeOpen: () => {
                Swal.showLoading();
            }
        });
    } else {
        if (!Swal.isLoading()) {
            LoadingPetition++;
            Swal.fire({
                allowOutsideClick: false,
                allowEscapeKey: false,
                background: 'transparent ',
                showConfirmButton: false,
                confrimButtonClass: "d-none",
                onBeforeOpen: () => {
                    Swal.showLoading();
                }
            });
        } else
            LoadingPetition++;
    }
}

function CloseLoading(onlypetition) {
    LoadingPetition--;
    if (Swal.isVisible()) {
        if (LoadingPetition <= 0 && onlypetition !== true && Swal.isLoading())
            Swal.close();
    }
}


function RenderTable(id, ncol, anchoColum, parametros, orden, checkColorClass) {

    $.extend($.fn.dataTable.defaults, {
        columnDefs: [{
            targets: ncol,
            orderable: false
        }],
        "columns": anchoColum,
        autoWidth: false,
        "ordering": false,
        "bSort": false,
        order: orden,
        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
        language: {
            search: '_INPUT_',
            lengthMenu: '_MENU_',
            paginate: {
                first: 'Primero',
                last: 'Último',
                next: '&rarr;',
                previous: '&larr;'
            },

            zeroRecords: "No se encontraron resultados",
            emptyTable: "Ningún dato disponible en esta tabla",
            info: "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            infoEmpty: "Ningún dato disponible",

            infoFiltered: "(filtrado de un total de _MAX_ registros)",
            infoPostFix: "",
            infoThousands: ",",
            loadingRecords: "Cargando...",
            aria: {
                sortAscending: ": Activar para ordenar la columna de manera ascendente",
                sortDescending: ": Activar para ordenar la columna de manera descendente"
            }
        }
    });

    if (parametros != null && parametros != undefined) {
        $('#' + id).DataTable(parametros);
    } else {
        // Basic datatable
        $('#' + id).DataTable();
    }

    //// Alternative pagination
    //$('.datatable-pagination').DataTable({
    //    pagingType: "simple",
    //    language: {
    //        "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
    //    }
    //});


    // Datatable with saving state
    $('.datatable-save-state').DataTable({
        stateSave: true
    });


    // Scrollable datatable
    $('.datatable-scroll-y').DataTable({
        autoWidth: true,
        scrollY: 300
    });

    // External table additions
    // ------------------------------

    // Add placeholder to the datatable filter option
    $('.dataTables_filter input[type=search]').attr('placeholder', 'Filtro...');
    $('.dataTables_filter input[type=search]').addClass("form-control");

    // Enable Select2 select for the length option
    $('.dataTables_length select').select2({
        minimumResultsForSearch: Infinity,
        width: 'auto'
    });

    //if (checkColorClass == undefined) {
    //    $("#" + id).on('draw.dt', function () {
    //        $(".styled").uniform({
    //            radioClass: 'choice'
    //        });
    //    });
    //} else {
    //    $("#" + id).on('draw.dt', function () {
    //        $(".styled").uniform({
    //            radioClass: 'choice',
    //            wrapperClass: checkColorClass
    //        });
    //    });
    //}
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


var lastFocusedItem = null;
function soloNumerosYDecimal(e, replaceComma, decimals) {
    key = e.keyCode || e.which;
    if (key == 13) {
        e.preventDefault();
        const inputs = $(':input')
        const nextInput = inputs.get(inputs.index(e.target) + 1);
        if (nextInput) {
            lastFocusedItem = nextInput.id;
            nextInput.focus();
        }
        return true;
    }
    tecla = String.fromCharCode(key).toLowerCase();
    letras = "0123456789.,";
    especiales = "8-37-39-46";

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (replaceComma && key == "46") {
        e.preventDefault();
        if (!decimals)
            decimals = 0;
        const valorActual = $(e.target).val();
        const cursorPosition = e.target.selectionStart;
        $(e.target).val(valorActual.substring(0, cursorPosition).replace(/[^0123456789]/g, '') + "," + valorActual.substring(cursorPosition, valorActual.length).replace(/[^0123456789]/g, '').substring(0, decimals));
        if (cursorPosition != 0 && cursorPosition != valorActual.length) {
            $(e.target).trigger("change");
            $(e.target).focus();
        }
        return false;
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}


function FormatoConPuntosSinRed(num, decimals) {
    var op = true;
    let originalNum = num;
    const isInteger = Number.isInteger(num);
    num = num == "" ? "0" : typeof (num) == 'string' ? num : num.toFixed(decimals ? decimals : 0);
    num = num.replace(/[.,]/gi, "");
    if (!isNaN(num)) {
        if (num < 0) {
            op = false;
            num = num * -1;
        }
        let decimalsVal = '';
        if (decimals) {
            originalNum = originalNum.toFixed(decimals) * 1;
            num = originalNum.toString();
            const decimalPart = originalNum.toString().split('.')[1];
            const integerPart = originalNum.toString().split('.')[0];
            const countDecimals = decimalPart ? decimalPart.length : 2;
            const numString = countDecimals < 2 ? originalNum.toString() + "0" : originalNum.toString();
            decimalsVal = isInteger ? ",00" : numString.length < 2 ? ',' + numString : ',' + numString.substring(numString.length - decimals, numString.length);
            num = isInteger ? num : num.toString().length < decimals ? 0 : integerPart;
        }
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        num = num.split('').reverse().join('').replace(/^[\.]/, '') + decimalsVal;
        return op ? num : "-" + num;
    } else {
        num = "0";
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        return num;
    }
}