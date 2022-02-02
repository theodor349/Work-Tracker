// https://github.com/CuriousDrive/BlazingChat/blob/main/src/BlazingChat.Components/wwwroot/js/site.js
// https://www.youtube.com/watch?v=qUbbtcZZqaI
function test() {
    console.log("Script -1");
}
function downloadFile(mimeType, base64String, fileName) {
    console.log("Script Starting");
    var fileDataUrl = "data:" + mimeType + ";base64," + base64String;
    console.log("Script: 1");
    fetch(fileDataUrl)
        .then(response => response.blob())
        .then(blob => {

            //create a link
            var link = window.document.createElement("a");
            link.href = window.URL.createObjectURL(blob, { type: mimeType });
            link.download = fileName;

            //add, click and remove
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        });
    console.log("Script: 2");
}

export  {
    test, downloadFile
};
