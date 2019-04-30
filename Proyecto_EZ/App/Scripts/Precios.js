/// <reference path="jquery-3.3.1.min.js" />
$(document).ready(function () {

    $('#EditorTable tbody tr').click(function () {

        ($(this).find("td:eq(2)").find("input[type='text']")).keyup(function () {

            var valorPorc = parseFloat($(this).val());
            var valorCosto = parseFloat($(this).parent().parent().find("td:eq(3)").find("input[type='text']").val());
            var valorPV = (valorCosto / (1 - (valorPorc / 100)));
            $(this).parent()
                   .parent()
                   .find("td:eq(4)")
                   .find("input[type='text']")
                   .val(Math.round(valorPV * 100) / 100);

        });

        ($(this).find("td:eq(3)").find("input[type='text']")).keyup(function () {

            var valorCosto = parseFloat($(this).val());
            var valorPorc = parseFloat($(this).parent().parent().find("td:eq(2)").find("input[type='text']").val());
            var valorPV = (valorCosto / (1 - (valorPorc / 100)));
            $(this).parent()
                   .parent()
                   .find("td:eq(4)")
                   .find("input[type='text']")
                   .val(Math.round(valorPV * 100) / 100);

        });

    });

    $('#btnGuardar').click(function () {

        var productos = [];

        $('#EditorTable tbody tr').each(function () {

            var nuevo = {
                IdMarca: ($(this).find("td:eq(0)").data("info")),
                IdTamanio: ($(this).find("td:eq(1)").data("info")),
                Porcentaje: ($(this).find("td:eq(2)").find("input[type='text']").val()),
                Costo: ($(this).find("td:eq(3)").find("input[type='text']").val()),
                PrecioVenta: ($(this).find("td:eq(4)").find("input[type='text']").val())
            }

            productos.push(nuevo);

        });

        $.ajax({

            method: 'POST',
            data: JSON.stringify(productos),
            url: '/Producto/PostActualizarPrecios',
            contentType: 'application/json; charset=utf-8',
            success: function (response) {

                if (response == '') {
                    swal({
                        title: "Exito!",
                        text: "Precios actualizados correctamente",
                        icon: "success",
                        button: "Aceptar",
                    });
                }
                else {
                    swal({
                        title: "Error",
                        text: response,
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
    });

});