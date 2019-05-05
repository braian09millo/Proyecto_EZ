$(document).ready(function () {

    //Obtenemos la lista de localidades para filtrarla
    var optLocalidades = $('#cmbLocalidades option');

    //Cargamos la grilla al iniciar
    CargarDatos();

    function Limpiar() {
        $("#tableClientes").dataTable().fnDestroy();
    }

    $('#NuevoForm').on('hidden.bs.modal', function (e) {

        $(this)
          .find("input[type=text],textarea,select")
             .val('')
             .end()
          .find("input[type=checkbox], input[type=radio]")
             .prop("checked", "")
             .end();

        $('#cmbLocalidades').val(0).prop('disabled', true);


    });

    function CargarDatos() {

        $.ajax({
            method: 'GET',
            url: '/Cliente/GetClientes',
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                var tabla = $('#tableClientes').DataTable({
                    data: response,
                    select: true,
                    "info": false,
                    columns: [
                        { 'data': 'cli_nombre' },
                        { 'data': 'cli_provincia' },
                        { 'data': 'cli_localidad' },
                        { 'data': 'cli_direccion' },
                        { 'data': 'cli_telefono' },
                        { 'data': 'cli_celular' },
                        { 'defaultContent': '<button class="btn btn-primary btn-xs" title="Ver pedidos"><i class="fas fa-list-alt"></i></button> <button class="btn btn-success btn-xs" disabled title="Habilitar"><i class="fas fa-user-check"></i></button> <button class="btn btn-info btn-xs" title="Editar"><i class="fas fa-user-edit"></i></button> <button class="btn btn-danger btn-xs" title="Eliminar"><i class="fas fa-user-minus"></i></button>' }
                    ],
                    aoColumnDefs: [
                        { "aTargets": [4], "bSortable": false },
                        { "aTargets": [5], "bSortable": false },
                        { "aTargets": [6], "bSortable": false },
                    ],
                    "createdRow": function (row, data, dataIndex) {
                        if (data.cli_delet == "S") {
                            $(row).css('background-color', '#B8DBD9');
                            $(row).find("td:eq(6)").find('.btn-danger').prop('disabled', true);
                            $(row).find("td:eq(6)").find('.btn-success').prop('disabled', false);
                        }
                    },
                    "language": {
                        "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
                    }
                });

                verPedidos('#tableClientes tbody', tabla);
                editarCliente('#tableClientes tbody', tabla);
                eliminarCliente('#tableClientes tbody', tabla);
                habilitarCliente('#tableClientes tbody', tabla);

            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    var verPedidos = function (tbody, tabla) {

        $(tbody).off('click', 'button.btn-primary.btn-xs');
        $(tbody).on('click', 'button.btn-primary.btn-xs', function () {

            var data = tabla.row($(this).parents('tr')).data();
            var url = '/Pedido/PedidosCliente?xiId=' + data.cli_id;
            window.open(url);
            
        });

    }

    var habilitarCliente = function (tbody, tabla) {


        $(tbody).off('click', 'button.btn-success.btn-xs');
        $(tbody).on('click', 'button.btn-success.btn-xs', function () {

            var data = tabla.row($(this).parents('tr')).data();

            var dataToPost = {
                xiId: data.cli_id
            }

            swal({
                title: "¿Está seguro que desea habilitar el cliente?",
                text: "",
                icon: "warning",
                buttons: ['Cancelar', 'Confirmar'],
                dangerMode: true,
            }).then(function (bHabilita) {
                if (bHabilita) {
                    $.ajax({
                        method: 'POST',
                        url: '/Cliente/PostHabilitarCliente',
                        data: JSON.stringify(dataToPost),
                        contentType: 'application/json; charset=utf-8',
                        success: function (response) {
                            if (response == '') {
                                swal("El cliente se habilitó correctamente", {
                                    icon: "success",
                                });
                                Limpiar();
                                CargarDatos();
                            }
                            else {
                                swal(response, {
                                    icon: "error",
                                    buttons: "Aceptar"
                                });
                            }
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            })
        });

    }

    var editarCliente = function (tbody, table) {

        $(tbody).off('click', 'button.btn-info.btn-xs');
        $(tbody).on('click', 'button.btn-info.btn-xs', function () {

            var data = table.row($(this).parents('tr')).data();

            $('#txtId').val(data.cli_id);
            $('#txtNombre').val(data.cli_nombre);
            $('#cmbProvincias').val(data.cli_proid);
            $('#cmbLocalidades').prop('disabled', false).val(data.cli_locid);
            $('#txtDireccion').val(data.cli_direccion);
            $('#txtMail').val(data.cli_mail);
            $('#txtTelefono').val(data.cli_telefono);
            $('#txtCelular').val(data.cli_celular);
            $('#txtCelular2').val(data.cli_celular2);

            $('#NuevoForm').modal('show');

        });

    }

    var eliminarCliente = function (tbody, table) {

        $(tbody).off('click', 'button.btn-danger.btn-xs');
        $(tbody).on('click', 'button.btn-danger.btn-xs', function () {

            var data = table.row($(this).parents('tr')).data();
            var dataToPost = {
                xiId: data.cli_id
            }

            swal({
                title: "¿Está seguro que desea deshabilitar el siguiente cliente?",
                text: "",
                icon: "warning",
                buttons: ['Cancelar', 'Confirmar'],
                dangerMode: true,
            }).then(function (bElimina) {
                if (bElimina) {
                    $.ajax({
                        method: 'POST',
                        url: '/Cliente/PostEliminarCliente',
                        data: JSON.stringify(dataToPost),
                        contentType: 'application/json; charset=utf-8',
                        success: function (response) {
                            if (response == '') {
                                swal("El registro se eliminó correctamente", {
                                    icon: "success",
                                });
                                Limpiar();
                                CargarDatos();
                            }
                            else {
                                swal(response, {
                                    icon: "error",
                                    buttons: "Aceptar"
                                });
                            }
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
            })
        });
    }

    //Habilitamos el botón cuando los campos requeridos esten llenos
    function ValidarFormulario() {

        var nombre = $('#txtNombre').val();
        var direccion = $('#txtDireccion').val();
        var provincia = $('#cmbProvincias').val();
        var localidad = $('#cmbLocalidades').val();

        if (nombre == "" || direccion == "" || localidad == "" || provincia == "")
            return false;
        else
            return true;
    }

    $('#btnAgregar').click(function (e) {

        //Prevenimos que se realice un nuevo submit
        e.preventDefault();

        if (ValidarFormulario()) {

            //Cargamos el objeto cliente
            var dataToPost = {
                cli_id: $('#txtId').val(),
                cli_nombre: $('#txtNombre').val(),
                cli_provincia: $('#cmbProvincias').val(),
                cli_localidad: $('#cmbLocalidades').val(),
                cli_direccion: $('#txtDireccion').val(),
                cli_mail: $('#txtMail').val(),
                cli_telefono: $('#txtTelefono').val(),
                cli_celular: $('#txtCelular').val(),
                cli_celular2: $('#txtCelular2').val(),
                cli_delet: $('#chkHabilitar').is(':checked') ? 'S' : 'N'
            };

            //Hacemos la llamada AJAX al controller CLIENTE
            $.ajax({
                method: 'POST',
                data: JSON.stringify(dataToPost),
                url: '/Cliente/PostGuardarCliente',
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    if (response == '') {
                        $('#NuevoForm').modal('hide');

                        swal({
                            title: "Exito",
                            text: "Cliente agregado correctamente",
                            icon: "success",
                            button: "Aceptar",
                        });

                        Limpiar();
                        CargarDatos();
                    }
                    else {
                        swal({
                            title: "Error",
                            text: "Fallo la carga de clientes",
                            icon: "error",
                            button: "Ok",
                        });
                    }
                },
                error: function (response) {
                    swal({
                        title: "Error",
                        text: response,
                        icon: "Error",
                        button: "Ok",
                    });
                }

            });
        }
        else {
            swal({
                title: "Error",
                text: "Los campos NOMBRE, DIRECCION y LOCALIDAD deben completarse",
                icon: "warning",
                button: "Aceptar",
            });

        }
    });

    $("#cmbProvincias").on('change', function () {

        var valor = $(this).val();

        if (valor === null || valor === "" || valor === undefined)
            $("#cmbLocalidades").prop("disabled", true).val('');
        else {
            $("#cmbLocalidades").prop("disabled", false);
            $("#cmbLocalidades").empty();
            $(optLocalidades).each(function () {
                if ($(this).data("info") == valor) {
                    console.log($(this).data("info") == valor);
                    $("#cmbLocalidades")
                        .append($("<option></option>")
                        .val($(this).val())
                        .html($(this).html()));
                }
            });
            $('#cmbLocalidades').append("<option value=''>Elija una localidad</option>");
        }
    });

});