

class PersonaView {
    constructor() {
        this.clickBtnConsultarHandler = this.clickBtnConsultarHandler.bind(this);
        this.postGetAllData = this.postGetAllData.bind(this);
        this.cerrarSession = this.cerrarSession.bind(this);
        this.clickBtnCopyUrlToClipboardHandler = this.clickBtnCopyUrlToClipboardHandler.bind(this);
        this.renderTableFriends = this.renderTableFriends.bind(this);

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
        this.$lblSalir = $("#lblSalir");
        this.$btnConsultar = $("#btnConsultar");
        this.$btnCopyUrl = $("#btnCopyUrl");
    }

    _initConstants() {
        this.CONSULTAR_FORM = 'form-consulta-persona';
        this.API_GET_CONSULTA = SetUrlForQuery('/Registro/ConsutarPersona');
        this.API_GET_DATA = SetUrlForQuery("/Registro/ConsultarArbolPersona");
        this.IdTableFriends = "datatable-friends";
    }

    _initEventBindings() {
        this.$btnConsultar.on("click", this.clickBtnConsultarHandler);
        this.$btnCopyUrl.on("click", this.clickBtnCopyUrlToClipboardHandler);
        this.$lblSalir.on("click", this.cerrarSession);
    }

    _init() {
        this.friendsTable = null;
        createValidation.call(this, this.CONSULTAR_FORM, this.getValidationConfig());
        if (localStorage .publicSessionLast) {
            this.postGetAllData("NVD");
        }
        
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

    cerrarSession() {
        localStorage .removeItem("publicSessionLast");
        window.location.reload();
    }

    testSwal() {
        Swal.fire({
            title: "¡Atención!",
            html: "<br/>Este número de cédula no se encuentra registrado.",
            showCancelButton: true,
            closeOnConfirm: false,
            confirmButtonText: "Regístrate",
            cancelButtonText: "Cancelar",
            animation: "slide-from-top",
            allowOutsideClick: false,
            customClass: {
                cancelButton: "btn border-secondary text-secondary",
                confirmButton: "btn btn-secondary"
            }
        }).then(async (result) => {
            if (result.value) {
                window.location.href = "/Registro";
            }
        });
    }

    async clickBtnConsultarHandler() {
        if (this.formValidator[this.CONSULTAR_FORM].form()) {
            ShowLoading(true);
            const tokenResult = await fetchGet(this.API_GET_CONSULTA, { "cedula" : this.$inputCedula.val() })
            if (!tokenResult.is_Error) {
                console.log(tokenResult);
                Swal.fire({
                    title: "¡Código de validación!",
                    html: "<br/>Ingrese el código que fue enviado a su número de celular para validar el ingreso.",
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
                    customClass: {
                        input: "form-control text-center",
                        cancelButton: "btn border-secondary text-secondary",
                        confirmButton: "btn btn-secondary"
                    },
                    allowOutsideClick: false,
                }).then(async (result) => {
                    if (result.value) {
                        if (result.value === false) return false;
                        if (result.value === "") {
                            swal.showInputError("Por favor ingrese un valor valido!");
                            return false;
                        }
                        localStorage .setItem("publicSessionLast", this.$inputCedula.val());
                        this.postGetAllData(result.value);
                    }
                });
            } else {
                if (tokenResult.order_Switch == -1) {
                    Swal.fire({
                        title: "¡Atención!",
                        html: "<br/>Este número de cédula no se encuentra registrado.",
                        showCancelButton: true,
                        closeOnConfirm: false,
                        confirmButtonText: "Regístrate",
                        cancelButtonText: "Cancelar",
                        animation: "slide-from-top",
                        allowOutsideClick: false,
                        customClass: {
                            cancelButton: "btn border-secondary text-secondary",
                            confirmButton: "btn btn-secondary"
                        }
                    }).then(async (result) => {
                        if (result.value) {
                            window.location.href = "/Registro";
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
            }
        }
    }

    async postGetAllData(token) {
        ShowLoading(true);
        const cedula = localStorage.publicSessionLast ? localStorage.publicSessionLast : this.$inputCedula.val();
        const tokenResult = await fetchGet(this.API_GET_DATA, { "cedula": cedula, "token": token });
        if (!tokenResult.is_Error) {
            this.data = tokenResult.objeto;
            this.persona = this.data.persona;
            this.amigos = this.data.amigos;
            this.$divContentConsulta.removeClass("d-none");
            this.$formConsulta.addClass("d-none");
            this.$lblNombre.html(this.data.persona.nombres);
            this.$lblAmigos.html(this.data.numeroAmigos);
            this.$lblEnlace.html(this.data.persona.shortUrl);

            this.$lblEnlace.attr("href", this.data.persona.shortUrl);
            this.$linkWap.attr("href", "whatsapp://send?text=" + this.data.persona.shortUrl);
            this.renderTableFriends();
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
        CloseLoading();
    }

    clickBtnCopyUrlToClipboardHandler() {
        navigator.clipboard.writeText(this.persona.shortUrl);
    }

    renderTableFriends() {
        if (this.friendsTable != undefined && this.friendsTable != null) {
            this.friendsTable.clear().draw();
            this.friendsTable.destroy();
        }
        RenderTable(this.IdTableFriends, [0, 1, 2], [
            {
                data: 'nombres', className: "dt-left", render: (data, type, row) => {
                    return data + " " + row.apellidos;
                }
            },
            { data: 'celular', className: "dt-center" },
            { data: 'numeroInvitados', className: "dt-center"},
           ], {
            data: this.amigos,
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,
            "dom": '<"top"fl>rt<"bottom"ip><"clear">',
            "bSort": false
        });

        console.log($(`#${this.IdTableFriends} .review`));


        this.friendsTable = $("#" + this.IdTableFriends).DataTable();
    }
}

export default PersonaView;