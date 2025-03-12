function copyToClipboard(text) {
    var isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
    
    if (isMobile) {
        // For mobile devices, strip HTML and use plain text
        var div = document.createElement("div");
        div.innerHTML = text;
        text = div.textContent || div.innerText || "";
        
        navigator.clipboard.writeText(text).then(() => {
            console.log('Copied to clipboard successfully!');
        }, (err) => {
            console.error('Failed to copy text: ', err);
        });
    } else {
        // For non-mobile devices, use the standard Clipboard API
        navigator.clipboard.writeText(text).then(() => {
            console.log('Copied to clipboard successfully!');
        }, (err) => {
            console.error('Failed to copy text: ', err);
        });
    }
}

// Fallback for browsers that do not support the Clipboard API
function copyToClipboardFallback(text) {
    var textArea = document.createElement('textarea');
    textArea.value = text;
    document.body.appendChild(textArea);
    textArea.select();
    document.execCommand('copy');
    document.body.removeChild(textArea);
}

// Paste from clipboard
async function pasteFromClipboard() {
    if (navigator.clipboard && navigator.clipboard.readText) {
        try {
            const text = await navigator.clipboard.readText();
            return text;
        } catch (err) {
            console.error('Failed to read clipboard contents using Clipboard API: ', err);
        }
    }

    // Fallback for browsers that do not support the Clipboard API
    try {
        const textArea = document.createElement('textarea');
        document.body.appendChild(textArea);
        textArea.focus();
        document.execCommand('paste');
        const text = textArea.value;
        document.body.removeChild(textArea);
        return text;
    } catch (err) {
        console.error('Failed to read clipboard contents using execCommand fallback: ', err);
        return '';
    }
}
