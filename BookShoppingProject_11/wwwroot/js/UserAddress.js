var datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    datatable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/UserAddress/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data":"postalCode","width":"10%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "isHomeAddressType", 

                "render": function (data) {
                    if (data) {
                        return `
                     <input type="checkbox" disabled checked/>`;

                    }
                    else {
                        return `
                       <input type="checkbox" disabled/>`;
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                        <a class="btn btn-info" href="/Admin/UserAddress/Upsert/${data}">
                        <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete("/Admin/UserAddress/Delete/${data}")>
                        <i class="fas fa-trash-alt"></i>
                        </a>
                        </div>
                    `;
                }

            }
        ]
    })
}
function Delete(url) {
    swal({
        title: "want to delete data!!!",
        text: "delete Information!!!",
        buttons: true,
        dangerModel: true,
        icon: "warning"
    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.Error(data.message)
                    }
                }
            })
        }
    })
}