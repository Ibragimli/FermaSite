var p = document.getElementById("imageErrorMessage")

const fileInput = document.getElementById('imageInput')
var previewImages = document.getElementById('temperoryImages')
var selectedFiles = []
const dt = new DataTransfer();
var files = []

fileInput.onchange = (e) => {
    //files = Object.values(e.target.files)
    const files = Array.from(e.target.files);
    if (files.length > 8) {
        p.innerHTML = "* Maksimum 8 şəkil əlavə edə bilərsiz!";
        fileInput.value = ''; // Inputun değerini sıfırla
    }
    else {
        if (selectedFiles.length > 7) {
            p.innerHTML = "* Maksimum 8 şəkil əlavə edə bilərsiz!";
            fileInput.value = ''; // Inputun değerini sıfırla

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
                        if (selectedFiles.length <= 8) {
                            setImages();
                        }
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
    previewImages.innerHTML = html;
    updateInputFiles();

}
const updateInputFiles = () => {
    dt.items.clear();
    selectedFiles.forEach(file => {
        dt.items.add(file.file);
    });
    fileInput.files = dt.files;
};
const removeFile = (index) => {
    const fileInput = document.getElementById('imageInput')
    dt.items.remove(index - 1)
    fileInput.files = dt.files
    selectedFiles.splice(index - 1, 1)
    setImages()
}
