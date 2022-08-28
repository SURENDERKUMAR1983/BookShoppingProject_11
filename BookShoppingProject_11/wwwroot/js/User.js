var datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    datatable = $('#tbldata').DataTable({
        "ajax": {
            "url":"/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data" : {
                    id : "id", lockoutEnd : "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var LockOut = new Date(data.lockoutEnd).getTime();
                    if (LockOut > today) {
                        //user Locked
                        return `
                            <div class="text-center">
                            <a onclick=LockUnLock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                            Unlock
                            </a>
                            </div>
                        `;
                    }
                    else
                    {  //user Unlocked
                        return `
                            <div class="text-center">
                            <a onclick=LockUnLock('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                            Lock
                            </a>
                           </div >
                        `;                        
                    }
                }
            }            
        ]
    })
}
function LockUnLock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnLock",
        data: JSON.stringify(id),
        contentType: "application/Json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                datatable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}
    