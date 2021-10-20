

class PersonaView {
    constructor() {
        this.clickBtnConsultarHandler = this.clickBtnConsultarHandler.bind(this);
        this.postGetAllData = this.postGetAllData.bind(this);

        this._initHtml()
        this._initConstants()
        this._initEventBindings();
        this._init();
    }

    _initHtml() {
        this.$inputCedula = $("#txtCedula");
        this.$formConsulta = $("#form-consulta-persona");
        this.$divContentConsulta = $("#divContentConsulta");
        this.$lblNombre = $("#lblNombre");
        this.$lblAmigos = $("#lblAmigos");
        this.$linkWap = $("#linkWap");
        this.$lblEnlace = $("#lblEnlace");
        this.$btnConsultar = $("#btnConsultar");
    }

    _initConstants() {
        this.CONSULTAR_FORM = 'form-consulta-persona';
        //this.API_GET_MUNICIPIOS = SetUrlForQuery('/Municipio/GetMunicipios');
        //this.API_POST_REGISTRADO = SetUrlForQuery('/Registro/Save');
        this.API_GET_CONSULTA = SetUrlForQuery('/Registro/ConsutarPersona');
        this.API_GET_DATA = SetUrlForQuery("/Registro/ConsultarArbolPersona");
    }

    _initEventBindings() {
        //$(`#${this.REGISTRAR_FORM} .change`).on("change", (event) => {
        //    inputChangeHandlerFunction.call(this, event);
        //});
        this.$btnConsultar.on("click", this.clickBtnConsultarHandler);
    }

    _init() {
        createValidation.call(this, this.CONSULTAR_FORM, this.getValidationConfig());
    }

    getValidationConfig() {
        const validationConfig = {
            cedula: {
                required: true,
                digits: true,
                minlength: 3
            }
        }
        return validationConfig;
    }

    async clickBtnConsultarHandler() {
        if (this.formValidator[this.CONSULTAR_FORM].form()) {
            ShowLoading(true);
            const tokenResult = await fetchGet(this.API_GET_CONSULTA, { "cedula" : this.$inputCedula.val() })
            if (!tokenResult.is_Error) {
                console.log(tokenResult);
                Swal.fire({
                    title: "¡Codigo generado!",
                    html: "Se ha enviado un código electrónico a el numero celular registrado para validar su identidad.",
                    input: 'text',
                    showCancelButton: true,
                    closeOnConfirm: false,
                    confirmButtonText: "Aceptar",
                    cancelButtonText: "Cancelar",
                    animation: "slide-from-top",
                    allowOutsideClick: false,
                    inputPlaceholder: "Ingrese Token",
                    inputAttributes: {
                        autocapitalize: 'off'
                    },
                    allowOutsideClick: false,
                }).then(async (result) => {
                    if (result.value) {
                        if (result.value === false) return false;
                        if (result.value === "") {
                            swal.showInputError("Por favor ingrese un valor valido!");
                            return false;
                        }
                        this.postGetAllData(result.value);
                    }
                });
            } else {
                swal.fire({
                    title: "¡Error!",
                    text: tokenResult.msj,
                    confirmButtonColor: "#66BB6A",
                    type: "error"
                });
            }
        }
    }

    async postGetAllData(token) {
        const tokenResult = await fetchGet(this.API_GET_DATA, { "cedula": this.$inputCedula.val(), "token": token });
        if (!tokenResult.is_Error) {
            this.data = tokenResult.objeto;
            this.data.url = SetUrlForQuery("/Registro/Index?Id=" + this.data.idEnlace);

            this.$divContentConsulta.removeClass("d-none");
            this.$formConsulta.addClass("d-none");
            this.$lblNombre.html(this.data.persona.nombres);
            this.$lblAmigos.html(this.data.numeroAmigos);
            this.$lblEnlace.html(this.data.url);

            this.$lblEnlace.attr("href", this.data.url);
            this.$linkWap.attr("href", "whatsapp://send?text=" + this.data.url);
           
        }
    }
}

export default PersonaView;