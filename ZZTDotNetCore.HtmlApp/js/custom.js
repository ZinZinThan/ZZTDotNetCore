function successMessage(message) {
    Swal.fire({
        title: "Success!",
        text: message,
        icon: "success"
    });
}

function warningMessage(message) {
    Swal.fire({
        title: "Warning!",
        text: message,
        icon: "warning"
    });
}

function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

function readUser() {
    $("#tbTbody").html('');

    var users = localStorage.getItem("tbl_user");
    users = JSON.parse(users);

    let htmlString = '';
    $.each(users, function(index, value) {
        console.log(index + ": " + value.UserName);

        htmlString += `
            <tr>
                <td>
                    <buttom type="buttom" class="btn btn-warning" onclick="editUser('${value.UserId}')">
                        <i class="fa-solid fa-pen-to-square"></i>
                    </buttom>
                    <buttom type="buttom" class="btn btn-danger">
                        <i class="fa-solid fa-trash"></i>
                    </buttom>
                </td>
                <td>${index + 1}</td>
                <td>${value.UserName}</td>
            </tr>`
    });

    $("#tbTbody").html(htmlString);
}