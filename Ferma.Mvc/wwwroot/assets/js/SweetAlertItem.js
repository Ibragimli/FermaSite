$(function () {
    $(document).on("click", ".delete-btn", function (e) {
        e.preventDefault();
        console.log(this)
        let url = $(this).attr("href");
        Swal.fire({
            title: 'Silmək  istəyirsinizmi?',
            text: "Sildikdən sonra geri qaytarmaq mümkün deyil!",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#32CD32',
            iconColor: "#32CD32",
            cancelButtonColor: '#d33',
            confirmButtonText: 'Bəli',
            cancelButtonText: 'Xeyr',
            width: "26em",
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => {
                        if (response.ok) {
                            window.location.reload(true)
                        }
                        else {

                            alert("xeta bas verdi")
                            console.log(url)

                            console.log(response.status)
                            console.log(response.statusText)
                            console.log(response.body)
                            console.log(response)
                        }
                    })
            }
            else {
                console.log("cancel")
            }
        })
    })
})