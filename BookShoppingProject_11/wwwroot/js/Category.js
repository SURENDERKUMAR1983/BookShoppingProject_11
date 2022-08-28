var datatable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    datatable = $('#tbldata').DataTable({
        "aLengthMenu": [[1, 2, 3], [1, 2, 3, "All"]],
        "iDisplayLength": [246],       
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "name", "width": "70%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                        <a class="btn btn-info" href="/Admin/Category/upsert/${data}">
                            <i class="fas fa-edit"></i></a>
                        <a class="btn btn-danger" onClick=Delete("/Admin/Category/Delete/${data}")>
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
        title: "want to delete data",
        text: "Delete information!!!",
        buttons: true,
        dangerModel: true,
        icon:"warning"
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
                        toastr.Error(data.message);
                    }
                }
            })
        }
    })
}
