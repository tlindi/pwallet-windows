function startQrScanner(dotNetHelper) {
    const video = document.getElementById('qr-video');
    const constraints = {
        video: { facingMode: "environment" }
    };

    navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
        video.srcObject = stream;
        video.setAttribute("playsinline", true);
        video.play();
        requestAnimationFrame(tick);

        window.currentStream = stream;

        // Add brackets overlay
        const qrScannerContainer = video.parentElement;

        const brackets = document.createElement('div');
        brackets.className = 'qr-guidelines';
        brackets.innerHTML = `
            <div class="top-left-bracket"></div>
            <div class="top-right-bracket"></div>
            <div class="bottom-left-bracket"></div>
            <div class="bottom-right-bracket"></div>
        `;

        qrScannerContainer.appendChild(brackets);
    });

    function tick() {
        if (video.readyState === video.HAVE_ENOUGH_DATA) {
            const canvas = document.createElement('canvas');
            canvas.height = video.videoHeight;
            canvas.width = video.videoWidth;
            const context = canvas.getContext('2d');
            context.drawImage(video, 0, 0, canvas.width, canvas.height);

            const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
            const code = jsQR(imageData.data, imageData.width, imageData.height, {
                inversionAttempts: "dontInvert",
            });

            if (code) {
                video.srcObject.getTracks().forEach(track => track.stop());
                dotNetHelper.invokeMethodAsync('HandleQrCodeResult', code.data);
                hideModal('qrScannerModal');
            }
        }

        requestAnimationFrame(tick);
    }
}

function stopQrScanner() {
    if (window.currentStream) {
        window.currentStream.getTracks().forEach(track => track.stop());
        window.currentStream = null;

        const qrGuidelines = document.querySelector('.qr-guidelines');
        if (qrGuidelines) {
            qrGuidelines.remove();
        }
    }
}

function hideModal(modalId) {
    var modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modal) {
        modal.hide();
    }
}