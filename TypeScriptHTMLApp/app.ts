class Greeter {
    element: HTMLElement;
    span: HTMLElement;
    timerToken: number;

    constructor(element: HTMLElement) {
        this.element = element;
        this.element.innerHTML += "The time is: ";
        this.span = document.createElement('span');
        this.element.appendChild(this.span);
        this.span.innerText = new Date().toUTCString();
    }
    start() { this.timerToken = setInterval(() => this.span.innerHTML = new Date().toUTCString(), 500); }
    stop() { clearTimeout(this.timerToken); }
}
class FileOperations {
    element: HTMLElement;
    span: HTMLElement;
    constructor(element: HTMLElement) {
        this.element = element;
    }
}
function funcdragover() {
    this.className = 'dropzone dragover';
    this.innerHTML = "you are here ";
    return false;
}
function funcondragleave() {
    this.className = 'dropzone';
    this.innerHTML = "moved away ";
    return false;
}

window.onload = () => {
    var el = document.getElementById('content');
    var greeter = new Greeter(el);
    greeter.start();

    var dropzone = document.getElementById("dropzone");
    var fileOperation = new FileOperations(dropzone);
    dropzone.onclick = function () {
        dropzone.innerHTML = 'clicked!';
        dropzone.className = 'dropzone dragover';
    }
    dropzone.ondblclick = function () {
        dropzone.innerHTML = 'Roll back clicked!';
        dropzone.className = 'dropzone';
    }
    dropzone.ondragover = funcdragover;
    dropzone.ondragleave = funcondragleave;

    dropzone.ondrop = function (e) {
        e.preventDefault();
        // e.dataTransfer.files
        dropzone.className = 'dropzone';
        alert(e.dataTransfer.files.length.toString() + e.dataTransfer.files[0])
        upload(e);
        return false;
    }

    document.getElementById('files').addEventListener('change', handleFileSelect1, false);

    var dropZone1 = document.getElementById('drop_zone');
   dropZone1.addEventListener('dragover', handleDragOver, false);
dropZone1.addEventListener('drop', handleFileSelect, false);
};

function upload(e) { console.log(e.dataTransfer.files[0]); }

function handleFileSelect1(evt) {
    var files = evt.target.files; // FileList object

    // files is a FileList of File objects. List some properties.
    var output = [];
    for (var i = 0, f; f = files[i]; i++) {
        output.push('<li><strong>', (f.name), '</strong> (', f.type || 'n/a', ') - ',
            f.size, ' bytes, last modified: ',
            f.lastModifiedDate ? f.lastModifiedDate.toLocaleDateString() : 'n/a',
            '</li>');
    }
    document.getElementById('list').innerHTML = '<ul>' + output.join('') + '</ul>';
}

function handleFileSelect(evt) {
    evt.stopPropagation();
    evt.preventDefault();

    var files = evt.dataTransfer.files; // FileList object.

    // files is a FileList of File objects. List some properties.
    var output = [];
    for (var i = 0, f; f = files[i]; i++) {
        output.push('<li><strong>', (f.name), '</strong> (', f.type || 'n/a', ') - ',
            f.size, ' bytes, last modified: ',
            f.lastModifiedDate ? f.lastModifiedDate.toLocaleDateString() : 'n/a',
            '</li>');
    }
    document.getElementById('list').innerHTML = '<ul>' + output.join('') + '</ul>';
}

function handleDragOver(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    evt.dataTransfer.dropEffect = 'copy'; // Explicitly show this is a copy.
}