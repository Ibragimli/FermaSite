document.querySelector('posterPriceInput').addEventListener('change', (e) => {
    if (e.target.value > 10) {
        e.target.setAttribute('title', " 999999 - dəyərindən çox ola bilməz ");
    } else if (e.target.value < 0) {
        e.target.setAttribute('title', "0 - dəyərindən az ola bilməz");
    } else {
        e.target.setAttribute('title', "Zəhmət olmasa qiymət daxil edin");
    }
})