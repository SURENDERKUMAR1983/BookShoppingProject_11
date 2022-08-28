var datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    datatable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "20%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            {"data":"isAuthorisedCompany",
                          
                "render": function (data) {
                    if (data){
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
                        <a  href="/Admin/Company/Upsert/${data}"class="btn btn-info">
                        <i class="fas fa-edit"></i>
                        </a>
                        <a class="btn btn-danger" onclick=Delete("/Admin/Company/Delete/${data}") >
                        <i class="fas fa-trash-alt"></i>
                        </a >
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