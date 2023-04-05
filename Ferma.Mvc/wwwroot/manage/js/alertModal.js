$(function () {
    $(document).on("click", ".delete-btn", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        fetch(url)
            .then(response => {
                if (response.ok) {
                    window.location.reload(true)
                }
                else {
                    alert("xeta bas verdi")
                }
            })

    })
})