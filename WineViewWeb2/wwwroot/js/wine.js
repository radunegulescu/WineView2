$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Wine/Getall' },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'color.name', "width": "15%" },
            { data: 'winery.name', "width": "15%" },
            { data: 'style.name', "width": "15%" },
            {
                "data": "grapes",
                "render": function (data) {
                    var grapes = data[0].name;
                    for (var i = 1; i < data.length; i++) {
                        grapes = grapes.concat(", ", data[i].name);
                    }
                    return grapes;
                },
                "width": "15%"
            },
            { "data": "volume", "width": "15%" },
            { "data": "isInClasifier", "width": "15%" },
            { "data": "clasifierId", "width": "15%" },  
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/wine/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/admin/wine/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                     <a onClick=Forecast('/admin/wine/forecast?id=${data}') class="btn btn-secondary mx-2"> <i class="bi bi-activity"></i> Forecast</a>               
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}

function Forecast(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "Do you want to download this file?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, download it!"
    }).then((result) => {
        if (result.isConfirmed) {
            // Show the loading spinner
            document.getElementById('loadingSpinner').style.display = 'block';

            $.ajax({
                url: url,
                type: 'GET',
                xhrFields: {
                    responseType: 'blob' // Important for binary data
                },
                success: function (data, status, xhr) {
                    // Hide the loading spinner
                    document.getElementById('loadingSpinner').style.display = 'none';

                    var filename = "";
                    var disposition = xhr.getResponseHeader('Content-Disposition');
                    if (disposition && disposition.indexOf('attachment') !== -1) {
                        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                        var matches = filenameRegex.exec(disposition);
                        if (matches != null && matches[1]) {
                            filename = matches[1].replace(/['"]/g, '');
                        }
                    }
                    var link = document.createElement('a');
                    var url = window.URL.createObjectURL(data);
                    link.href = url;
                    link.download = filename;
                    document.body.append(link);
                    link.click();
                    link.remove();
                    window.URL.revokeObjectURL(url);
                    toastr.success("Download Successful");
                },
                error: function () {
                    // Hide the loading spinner
                    document.getElementById('loadingSpinner').style.display = 'none';

                    toastr.error("Download failed");
                }
            });
        }
    });
}