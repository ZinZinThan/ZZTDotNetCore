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

function confirmMessage(message) {
    return new Promise((resolve, reject) => {
        Swal.fire({
            title: "Confirm",
            text: message,
            icon: "question",
            showCancelButton: true,
        }).then((result) => {
            resolve(result.isConfirmed)
        });
    });

}

function notiflixConfirm(message) {
    return new Promise((resolve, reject) => {
        Notiflix.Confirm.show(
            'Confirm',
            message,
            'Yes',
            'No',
            function okCb() {
                resolve(true)
            },
            function cancelCb() {
                resolve(false)
            },
            {
            },
        );
    });
}

function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}



