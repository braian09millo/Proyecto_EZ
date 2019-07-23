$('.DropDownMultiSearch').multiselect({
    includeSelectAllOption: true,
    selectAllValue: "",
    selectAllText: $('#Traduccion_Todos').val(),
    nSelectedText: ' - ' + $('#Traduccion_OpcionesSeleccionadas').val(),
    nonSelectedText: $('#Traduccion_SeleccioneUnaOpcion').val(),
    enableFiltering: true,
    enableCaseInsensitiveFiltering: true,
    filterPlaceholder: $('#Traduccion_Buscar').val(),
    numberDisplayed: 1,
    allSelectedText: $('#Traduccion_Todos').val(),
    dropUp: false
});

function reloadDropDownList(id, elements) {
    var ddl = $(id)
    ddl.empty();
                        
    for (var i = 0; i < elements.length; i++) {
        ddl.append($('<option></option>').val(elements[i].Value).html(elements[i].Text));
    }
                        
    ddl.multiselect('rebuild')
    ddl.multiselect('selectAll',false)
    ddl.multiselect('updateButtonText')
}
