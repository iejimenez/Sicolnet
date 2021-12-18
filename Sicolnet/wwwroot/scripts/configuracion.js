

class ConfigurationView {
    constructor() {
        this.clickBtnAgregarAdminHandler = this.clickBtnAgregarAdminHandler.bind(this);
        this.cerrarSession = this.cerrarSession.bind(this);
        this.setSelectUsuarios = this.setSelectUsuarios.bind(this);
        this.renderTableAdmins = this.renderTableAdmins.bind(this);
        this.getAdmins = this.getAdmins.bind(this);
        this.setClickEventOnTableItem = this.setClickEventOnTableItem.bind(this);
        this.clickBtnDeleteAdminHandler = this.clickBtnDeleteAdminHandler.bind(this);

        this._initHtml()
        this._initConstants()
        this._initEventBindings();
        this._init();
    }

    _initHtml() {
        this.$formConsulta = $("#form-add-admin");
        this.$selectPersona = $("#cboPersona");
        this.$lblSalir = $("#lblSalir");
        this.$btnAgregar = $("#btnAgregar");
    }

    _initConstants() {
        this.ADD_FORM = 'form-add-admin';
        this.API_GET_ADMINS = SetUrlForQuery("/Admin/GetAdmins");
        this.API_POST_ADD_ADMIN = SetUrlForQuery('/Admin/Save');
        this.API_DELETE_ADMIN = SetUrlForQuery('/Admin/Delete')
        this.idTableAdmins = "datatable-admins";
    }

    _initEventBindings() {
        this.$btnAgregar.on("click", this.clickBtnAgregarAdminHandler);
        this.$lblSalir.on("click", this.cerrarSession);
    }

    _init() {
        this.adminsTable = null;
        createValidation.call(this, this.ADD_FORM, this.getValidationConfig());
        this.setSelectUsuarios();
        this.getAdmins();
    }

    setSelectUsuarios() {
        this.$selectPersona.select2({
            ajax: {
                url: "/Registro/GetPersonaByCedulaOrNombre",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        q: params.term
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
               
                    for (var i = 0; i < data.length; i++) {
                        data[i] = { id: data[i].idPersona, text: data[i].cedula + " - " + data[i].nombres + " " + data[i].apellidos };
                    }
                    return {
                        results: data,
                        pagination: {
                            more: (params.page * 10) < data.count_filtered
                        }
                    };
                },
                cache: true
            },
            minimumInputLength: 1,
            language: {
                errorLoading: function () {
                    return "La carga falló";
                },
                inputTooLong: function (e) {
                    var t = e.input.length - e.maximum, n = "Por favor, elimine " + t + " car"; return t == 1 ? n += "ácter" : n += "acteres", n;
                },
                inputTooShort: function (e) {
                    var t = e.minimum - e.input.length, n = "Por favor, introduzca " + t + " car"; return t == 1 ? n += "ácter" : n += "acteres", n;
                },
                loadingMore: function () {
                    return "Cargando más resultados…";
                },
                maximumSelection: function (e) {
                    var t = "Sólo puede seleccionar " + e.maximum + " elemento"; return e.maximum != 1 && (t += "s"), t;
                },
                noResults: function () {
                    return "No se encontraron resultados";
                },
                searching: function () {
                    return "Buscando…";
                }
            }
        });
    }

    getValidationConfig() {
        const validationConfig = {
            idPersona: {
                required: true
            }
        }
        return validationConfig;
    }

    cerrarSession() {
        localStorage.removeItem("publicSessionLast");
        window.location.href = "/Registro/Persona";
    }

    async getAdmins() {
        const result = await fetchGet(this.API_GET_ADMINS);
        if (!result.is_Error) {
            this.admins = result.objeto;
            this.renderTableAdmins();
        }
    }

    async clickBtnAgregarAdminHandler() {
        if (this.formValidator[this.ADD_FORM].form()) {
            ShowLoading(true);
            const resultPost = await this.postAddAdmin();
            if (!resultPost.is_Error) {
                swal.fire({
                    title: "¡Administrador creado con exito!",
                    type: "success",
                    allowOutsideClick: false,
                    confirmButtonText: "Aceptar",
                    customClass: {
                        confirmButton: "btn btn-secondary"
                    }
                });
                await this.getAdmins();
                this.$selectPersona.val(null).trigger('change');
            } else {
                swal.fire({
                    title: "¡Error!",
                    text: resultPost.msj,
                    type: "error",
                    customClass: {
                        confirmButton: "btn btn-secondary"
                    }
                });
            }
        }
    }

    async postAddAdmin() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: this.API_POST_ADD_ADMIN,
                content: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: { "id": this.$selectPersona.val() },
                success: function (data) {
                    resolve(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    reject(new Error(`${errorThrown} - ${this.API_POST_REGISTRADO}`));
                }
            });
        });
    }

    async postDeleteAdmin(id) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: this.API_DELETE_ADMIN,
                content: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: { "id": id },
                success: function (data) {
                    resolve(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    reject(new Error(`${errorThrown} - ${this.API_POST_REGISTRADO}`));
                }
            });
        });
    }

    renderTableAdmins() {
        if (this.adminsTable != undefined && this.adminsTable != null) {
            this.adminsTable.clear().draw();
            this.adminsTable.destroy();
        }
        RenderTable(this.idTableAdmins, [0, 1, 2], [
            {
                data: 'idPersona', className: "dt-left", render: (data, type, row) => {
                    return row.tercero.nombres + " " + row.tercero.apellidos;
                }
            },
            {
                data: 'idPersona', className: "dt-center", render: (data, type, row) => {
                    return row.tercero.cedula;
                }
            },
            {
                data: 'idUsuario', className: "dt-center", render: (data, type, row) => {
                    return "<div class='list-icons'>" +
                        "<a class='deleteIconAdmin' data-idx='" + data + "' data-popup='tooltip' title='Eliminar'><i class='icon-trash' data-idx='" + data + "'></i></a>" +
                        "</div>"
                }
            },
        ], {
            data: this.admins,
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,
            "dom": '<"top"fl>rt<"bottom"ip><"clear">',
            "bSort": false
        });

        this.adminsTable = $("#" + this.idTableAdmins).DataTable();
        this.adminsTable.off('draw.dt').on('draw.dt', this.setClickEventOnTableItem);
        this.setClickEventOnTableItem();
    }

    setClickEventOnTableItem() {
        $(`#${this.idTableAdmins} .deleteIconAdmin`).off("click").click(this.clickBtnDeleteAdminHandler);
    }

    clickBtnDeleteAdminHandler(event) {
        Swal.fire({
            title: "¿Seguro que quiere eliminar el registro?",
            showCancelButton: true,
            closeOnConfirm: false,
            confirmButtonText: "Aceptar",
            cancelButtonText: "Cancelar",
            animation: "slide-from-top",
            allowOutsideClick: false,
            customClass: {
                cancelButton: "btn border-secondary text-secondary",
                confirmButton: "btn btn-secondary"
            },
            allowOutsideClick: false,
        }).then(async (resultModal) => {
            if (resultModal.value) {
                ShowLoading(true);
                const icon = $(event.target);
                const idToDelete = icon.data("idx") * 1;
                const result = this.postDeleteAdmin(idToDelete)
                if (!result.is_Error) {
                   
                    this.$selectPersona.select2("trigger", "select", { data: { id: "" } });

                    await this.getAdmins();
                    swal.fire({
                        title: "¡Administrador eliminado con exito!",
                        type: "success",
                        allowOutsideClick: false,
                        confirmButtonText: "Aceptar",
                        customClass: {
                            confirmButton: "btn btn-secondary"
                        }
                    });
                } else {
                    swal.fire({
                        title: "¡Error!",
                        text: result.msj,
                        type: "error",
                        customClass: {
                            confirmButton: "btn btn-secondary"
                        }
                    });
                }
            }
        });



        
    }
}

export default ConfigurationView;