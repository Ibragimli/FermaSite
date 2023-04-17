$(function () {
    $(document).on("click", ".addWish", function (e) {
        e.preventDefault();
        let Url = $(this).attr("href")

        fetch(Url)
            .then(response => {
                if (response.ok) {
                    window.location.reload(true);

                }
                else {
                    window.location.reload(true);
                }
            })
    })
})

$(function () {
    $(document).on("click", ".deleteWish", function (e) {
        e.preventDefault();
        let Url = $(this).attr("href")

        fetch(Url)
            .then(response => {
                window.location.reload(true);
            })
    })
})