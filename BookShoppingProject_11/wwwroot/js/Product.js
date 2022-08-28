var datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    datatable = $('#tbldata').DataTable({        
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "description", "width": "25%" },
            { "data": "author", "width": "15%" },
            { "data": "isbn", "width": "10%" },
            { "data": "price", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                        <a class="btn btn-info" href="/Admin/Product/Upsert/${data}">
                        <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete("/Admin/Product/Delete/${data}")>
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
        text : "delete Information!!!",
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