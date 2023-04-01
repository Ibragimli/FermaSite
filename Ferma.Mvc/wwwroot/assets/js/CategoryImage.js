

var p = document.getElementById("imageCategoryErrorMessage")

const fileInput = document.getElementById('imageInputCategory')
var previewImages = document.getElementById('tempImage')
var selectedFiles = []
const dt = new DataTransfer();
var files = []
debugger
fileInput.onchange = (e) => {
    files = Object.values(e.target.files)

    if (files.length > 2) {
        p.innerHTML = "*Maksimum 8 şəkil əlavə edə bilərsiz!";
        // for (let index = 1; index < 9; index++) {
        //     removeFile(index)
        // }
        // e.target.value = null\
    }
    else {
        if (selectedFiles.length > 2) {
            p.innerHTML = "*Maksimum 8 şəkil əlavə edə bilərsiz!";
        }
        else {
            if (files.length > 0) {
                files.forEach(file => {
                    const reader = new FileReader();
                    reader.onload = (e) => {
                        const src = e.target.result;
                        dt.items.add(file)
                        selectedFiles.push({
                            file: file,
                            src: src,
                            degree: 0,
                        });

                    };
                    if (typeof file != "undefined") {
                        reader.readAsDataURL(file);
                    }
                })
                setTimeout(() => setImages(), 1000)
            }
        }
    }


}
const setImages = () => {

    let html = ''
    previewImages.textContent = '';

    let i = 0

    selectedFiles.forEach(file => {
        i++
        if (i > 0 && i < 9) {
            html += `<div  class="imageBox">
            <p id="${i}" onclick="removeFile(${i})"><i class="fa-solid fa-trash"></i></p>
            <img data-image-id="${i}" class="temperorayImage" src="${file.src}" alt="">
        </div>`
        }
    })
    previewImages.innerHTML = html
}

const removeFile = (index) => {
    const fileInput = document.getElementById('imageInput')
    dt.items.remove(index - 1)
    fileInput.files = dt.files
    selectedFiles.splice(index - 1, 1)
    setImages()
}









