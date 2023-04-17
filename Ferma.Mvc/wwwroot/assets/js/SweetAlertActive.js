$(function () {
    $(document).on("click", ".active-btn",  function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
         Swal.fire({
            title: 'Elanı aktiv etmək istəyirsinizmi?',
            icon: 'question'    ,
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
                        }
                    })
            }
            else {
                console.log("cancel")
            }
        })
    })
})