var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "Product/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "serialNumber", "width": "15%" },
            { "data": "imeiCode", "width": "15%" },
            { "data": "color", "width": "15%" },
            { "data": "mark", "width": "15%" },
            { "data": "size", "width": "15%" },            
            { "data": "productCategory.type", "width": "15%" },
            { "data": "price", "width": "15%" },
            {
                "data": "id", "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/Product/Upsert?id=${data}" class="btn btn-primary mx-2">Düzenle</a>
                            <a onClick=Delete('/Product/Delete/${data}')  class="btn btn-danger mx-2">Sil</a>                            
                        </div>`
                }, "width": "15%"
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url, type: 'DELETE',
                success: function (data) {                    
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => { location.reload()}, 500);
                       
                        
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}