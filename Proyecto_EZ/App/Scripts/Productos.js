/// <reference path="jquery-3.3.1.min.js" />
$(document).ready(function () {

    //Cargamos la grilla en el LOAD
    CargarDatos();

    //Obtenemos la lista de localidades para filtrarla
    var optModelos = $('#cmbModelos option');

    //Función para volver a crear la tabla
    function Limpiar() {
        $("#tableProductos").dataTable().fnDestroy();
    }

    //Funcion para limpiar el modal al cerrarse
    $('#NuevoForm').on('hidden.bs.modal', function (e) {

        $(this)
          .find("input[type=text],textarea,select")
             .val('')
             .end()
          .find("input[type=checkbox], input[type=radio]")
             .prop("checked", "")
             .end();
    });

    //Funcion para modificar el valor PV
    $('#txtPorcentaje').keyup(function () {

        var valorPorc = parseFloat($(this).val());
        var valorCosto = parseFloat($('#txtCosto').val());
        var valorPV = (valorCosto / (1 - (valorPorc / 100)));

        if (valorCosto == 'NaN' || valorCosto == NaN) {
            $('#txtPV').val(0);
        } else {
            $('#txtPV').val(Math.round(valorPV));
        }

    });

    $('#txtCosto').keyup(function () {

        var valorCosto = parseFloat($(this).val());
        var valorPorc = parseFloat($('#txtPorcentaje').val());
        var valorPV = (valorCosto / (1 - (valorPorc / 100)));

        if (valorPorc == 'NaN' || valorPorc == NaN) {
            $('#txtPV').val(0);
        } else {
            $('#txtPV').val(Math.round(valorPV));
        }


    });

    //Habilitamos el botón cuando los campos requeridos esten llenos
    function ValidarFormulario() {

        var marca = $('#cmbMarca').val();
        var modelo = $('#cmbModelo').val();
        var tamanio = $('#cmbTamanio').val();
        var tipo = $('#cmbTipos').val();
        var costo = $('#txtCosto').val();
        var cantPack = $('#txtCantPack').val();
        var porc = $('#txtPorcentaje').val();

        if (marca == "" || modelo == "" ||
            tamanio == "" || tipo == "" ||
            costo == "" || cantPack == "" ||
            porc == "")
            return false;
        else
            return true;
    }

    $('#btnNuevo').click(function () {
        $('#txtPorcentaje').prop('readonly', false);
        $('#txtCosto').prop('readonly', false);
    });

    $('#btnAgregar').click(function (e) {

        //Prevenimos que se realice un nuevo submit
        e.preventDefault();

        //Cargamos el objeto ProductoForm
        var dataToPost = {
            Id: $('#txtId').val(),
            Marca: $('#cmbMarcas').val(),
            Modelo: $('#cmbModelos').val(),
            Tamanio: $('#cmbTamanios').val(),
            Tipo: $('#cmbTipos').val(),
            CantidadPack: $('#txtCantPack').val(),
            Costo: $('#txtCosto').val(),
            Porcentaje: $('#txtPorcentaje').val(),
            PrecioVenta: $('#txtPV').val()
        }

        //Si la validación es correcta, hacemos la llamada AJAX
        if (ValidarFormulario()) {

            $.ajax({
                method: 'POST',
                data: JSON.stringify(dataToPost),
                url: '/Producto/PostGuardarProducto',
                contentType: 'application/json; charset=utf-8',
                success: function (response) {

                    if (response == '') {

                        $('#NuevoForm').modal('hide');

                        swal({
                            title: "Exito!",
                            text: "Producto agregado correctamente",
                            icon: "success",
                            button: "Aceptar",
                        });

                        Limpiar();
                        CargarDatos();
                    }
                    else {
                        swal({
                            title: "Error",
                            text: response,
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
                text: "Ningún campo debe estar vacío",
                icon: "warning",
                button: "Aceptar",
            });
        }
    });

    $("#cmbMarcas").on('change', function () {

        var valor = $(this).val();

        if (valor === null || valor === "" || valor === undefined)
            $("#cmbModelos").val('');
        else {
            $("#cmbModelos").empty();
            $('#cmbModelos').append("<option value=''>Elija un modelo</option>");
            $(optModelos).each(function () {
                if ($(this).data("info") == valor) {
                    $("#cmbModelos")
                        .append($("<option></option>")
                        .val($(this).val())
                        .html($(this).html()));
                }
            });

            console.log($('#cmbModelos').children('option').length);

            if ($('#cmbModelos').children('option').length == 1) {
                $('#cmbModelos').prop('disabled', true);
            } else {
                $('#cmbModelos').prop('disabled', false);
            }
        }
    });

    function CargarDatos() {
        $.ajax({
            method: 'GET',
            url: '/Producto/GetProductos',
            success: function (response) {
                console.log(response);
                if (response.error == "") {
                    var tabla = $('#tableProductos').DataTable({
                        data: response.data,
                        "info": false,
                        columns: [
                            { 'data': 'Marca' },
                            { 'data': 'Modelo' },
                            { 'data': 'Tamanio' },
                            { 'data': 'Tipo' },
                            { 'data': 'CantidadPack' },
                            { 'data': 'PrecioUnitario' },
                            { 'data': 'Costo' },
                            { 'data': 'Porcentaje' },
                            { 'data': 'PrecioVenta' },
                            { 'defaultContent': '<button class="btn btn-success btn-xs" disabled title="Habilitar"><i class="fas fa-check"></i></button> <button class="btn btn-info btn-xs" title="Editar"><i class="fas fa-pencil-alt"></i></button> <button class="btn btn-danger btn-xs" title="Eliminar"><i class="fas fa-trash-alt"></i></button>' }
                        ],
                        aoColumnDefs: [
                            { "aTargets": [4], "width": "10%", "bSortable": false },
                            { "aTargets": [5], "bSortable": false, type: 'currency' },
                            { "aTargets": [6], "bSortable": false, type: 'currency' },
                            { "aTargets": [7], "bSortable": false, type: 'currency' },
                            { "aTargets": [8], "bSortable": false, type: 'currency' },
                            { "aTargets": [9], "bSortable": false }
                        ],
                        "createdRow": function (row, data, dataIndex) {
                            if (data.prod_delet == "S") {
                                $(row).css('background-color', '#B8DBD9');
                                $(row).find("td:eq(9)").find('.btn-danger').prop('disabled', true);
                                $(row).find("td:eq(9)").find('.btn-success').prop('disabled', false);
                            }
                        },
                        "language": {
                            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
                        }
                    });

                    habilitarProducto('#tableProductos tbody', tabla);
                    editarProducto('#tableProductos tbody', tabla);
                    eliminarProducto('#tableProductos tbody', tabla);
                }
                else {
                    swal({
                        title: "Error",
                        text: response.error,
                        icon: "error",
                        button: "Aceptar",
                    });
                }
            },
            error: function (response) {
                swal({
                    title: "Error",
                    text: response,
                    icon: "error",
                    button: "Aceptar",
                });
            }
        });
    }


    var habilitarProducto = function (tbody, tabla) {

        $(tbody).off('click', 'button.btn-success.btn-xs');
        $(tbody).on('click', 'button.btn-success.btn-xs', function () {

            var data = tabla.row($(this).parents('tr')).data();

            var dataToPost = {
                xiId: data.IdProducto
            }

            swal({
                title: "¿Está seguro que desea habilitar el producto?",
                text: "",
                icon: "warning",
                buttons: ['Cancelar', 'Confirmar'],
                dangerMode: true,
            }).then(function (bHabilita) {
                if (bHabilita) {
                    $.ajax({
                        method: 'POST',
                        url: '/Producto/PostHabilitarProducto',
                        data: JSON.stringify(dataToPost),
                        contentType: 'application/json; charset=utf-8',
                        success: function (response) {
                            if (response == '') {
                                swal("El producto se habilitó correctamente", {
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

    var editarProducto = function (tbody, tabla) {

        $(tbody).off('click', 'button.btn-info.btn-xs');
        $(tbody).on('click', 'button.btn-info.btn-xs', function () {

            var data = tabla.row($(this).parents('tr')).data();

            $('#txtId').val(data.IdProducto);
            $('#cmbMarcas').val(data.IdMarca);
            $('#cmbModelos').val(data.IdModelo);
            $('#cmbTamanios').val(data.IdTamanio);
            $('#cmbTipos').val(data.IdTipo);
            $('#txtCantPack').val(data.CantidadPack);
            $('#txtPorcentaje').val(data.Porcentaje).prop('readonly', true);
            $('#txtCosto').val(data.Costo).prop('readonly', true);
            $('#txtPV').val(data.PrecioVenta);

            $('#NuevoForm').modal('show');

        });

    }

    var eliminarProducto = function (tbody, table) {

        $(tbody).off('click', 'button.btn-danger.btn-xs');
        $(tbody).on('click', 'button.btn-danger.btn-xs', function () {

            var data = table.row($(this).parents('tr')).data();
            var dataToPost = {
                xiId: data.IdProducto
            }

            swal({
                title: "¿Está seguro que desea eliminar el producto?",
                text: "",
                icon: "warning",
                buttons: ['Cancelar', 'Confirmar'],
                dangerMode: true,
            }).then(function (bElimina) {
                if (bElimina) {
                    $.ajax({
                        method: 'POST',
                        url: '/Producto/PostEliminarProducto',
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


});