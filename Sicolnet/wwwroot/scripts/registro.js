class RegistroView {
    constructor() {
        this.clickBtnRegistroHandler = this.clickBtnRegistroHandler.bind(this);
        this.postRegistrar = this.postRegistrar.bind(this);
        this.postGenerarToken = this.postGenerarToken.bind(this);
        this.setMunicipios = this.setMunicipios.bind(this);
        this.registrar = this.registrar.bind(this);

        this._initHtml()
        this._initConstants()
        this._initEventBindings();
        this._init();
    }

    _initHtml() {
        this.$selectMunicipios = $("#cboMunicipio");
        this.$pickFechaNacimiento = $("#pickFechaNacimiento");
        this.$btnRegistrar = $("#btnRegistrar");
        this.$txtReferente = $("#txtReferente");
    }

    _initConstants() {
        this.REGISTRAR_FORM = 'form-registro';
        this.API_GET_MUNICIPIOS = SetUrlForQuery('/Municipio/GetMunicipios');
        this.API_POST_REGISTRADO = SetUrlForQuery('/Registro/Save');
        this.API_POST_GERERAR_TOKEN = SetUrlForQuery('/Registro/GenerarToken');
    }

    _initEventBindings() {
        $(`#${this.REGISTRAR_FORM} .change`).on("change", (event) => {
            inputChangeHandlerFunction.call(this, event);
        });
        this.$btnRegistrar.on("click", this.clickBtnRegistroHandler);
    }

    _init() {
        this.registro = {};
        $("#pickFechaNacimiento").pickadate(seleccionFecha);
        this.$pickFechaNacimiento.pickadate(seleccionFecha);
        this.setMunicipios();
        createValidation.call(this, this.REGISTRAR_FORM, this.getValidationConfig());
    }

    getValidationConfig() {
        const validationConfig = {
            cedula: {
                required: true,
                digits: true
            },
            nombres: {
                required: true
            },
            apellidos: {
                required: true
            },
            celular: {
                required: true,
                digits: true,
                maxlength: 10,
                minlength: 10
            },
            email: {
                required: true,
                email2: true
            },
            fechaNacimiento: {
                required: true
            },
            idMunicipio: {
                required: true
            },
        }
        return validationConfig;
    }

    async setMunicipios() {
        this.municipios = await fetchGet(this.API_GET_MUNICIPIOS);
        let items_html = `<option value="">Seleccionar Municipio</option>`
        $.each(this.municipios, (index, item) => {
            items_html += `<option value="${item.idMunicipio}">${item.nombre} [${item.departamento.nombre}]</option>`;
        });
        this.$selectMunicipios.html(items_html);
        this.$selectMunicipios.select2({ dropdownAutoWidth: true, dropdownParent: this.$selectMunicipios.parent() });

    }

    async postRegistrar(token) {
        this.registro.idReferente = this.$txtReferente.val();
        const jsonRegistro = JSON.parse(JSON.stringify(this.registro));
        return new Promise((resolve, reject) => {
            $.ajax({
                url: this.API_POST_REGISTRADO,
                content: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: { "persona": jsonRegistro, "token": token  },
                success: function (data) {
                    resolve(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    reject(new Error(`${errorThrown} - ${this.API_POST_REGISTRADO}`));
                }
            });
        });
    }

    testSwal() {
        swal.fire({
            title: "¡Registro exitoso!",
            type: "success",
            allowOutsideClick: false,
            confirmButtonText: "Ingresar",
            customClass: {
                confirmButton: "btn btn-secondary"
            }
        });
    }

    async clickBtnRegistroHandler() {
        if (this.formValidator[this.REGISTRAR_FORM].form()) {
            ShowLoading(true);
            if (!localStorage.publicSessionLast) {
                const tokenResult = await this.postGenerarToken();
                if (!tokenResult.is_Error) {
                    console.log(tokenResult);
                    Swal.fire({
                        title: "¡Código de validación!",
                        html: "<br/>Ingrese el código que fue enviado a su número de celular para validar el registro.",
                        input: 'text',
                        showCancelButton: true,
                        closeOnConfirm: false,
                        confirmButtonText: "Aceptar",
                        cancelButtonText: "Cancelar",
                        animation: "slide-from-top",
                        allowOutsideClick: false,
                        inputPlaceholder: "Ingrese el código",
                        inputAttributes: {
                            autocapitalize: 'off'
                        },
                        allowOutsideClick: false,
                        customClass: {
                            input: "form-control text-center",
                            cancelButton: "btn border-secondary text-secondary",
                            confirmButton: "btn btn-secondary"
                        }
                    }).then(async (result) => {
                        if (result.value) {
                            if (result.value === false) return false;
                            if (result.value === "") {
                                swal.showInputError("Por favor ingrese un valor valido!");
                                return false;
                            }
                            await this.registrar(result);
                        }
                    });
                } else {
                    swal.fire({
                        title: "¡Error!",
                        text: tokenResult.msj,
                        type: "error",
                        customClass: {
                            confirmButton: "btn btn-secondary"
                        }
                    });
                }
            } else {
                await this.registrar();
            }
        }
    }

    async registrar(result) {
        const registerResult = await this.postRegistrar(localStorage.publicSessionLast ? "NVD" : result.value);
        if (!registerResult.is_Error) {
            swal.fire({
                title: "¡Registro exitoso!",
                type: "success",
                allowOutsideClick: false,
                confirmButtonText: "Ingresar",
                customClass: {
                    confirmButton: "btn btn-secondary"
                }
            }).then(() => {
                window.location.href = SetUrlForQuery('/')
            });
        } else {
            swal.fire({
                title: "¡Error!",
                text: registerResult.msj,
                type: "error",
                customClass: {
                    confirmButton: "btn btn-secondary"
                }
            });
        }
    }

    postGenerarToken() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: this.API_POST_GERERAR_TOKEN,
                content: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: {
                    "celular": this.registro.celular,
                    "cedula": this.registro.cedula
                },
                success: function (data) {
                    resolve(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    reject(new Error(`${errorThrown} - ${this.API_POST_REGISTRADO}`));
                }
            });
        });
    }

}

export default RegistroView;