window.pasteFromClipboard = async function () {
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
};

window.copyToClipboard = (text) => {
    navigator.clipboard.writeText(text).then(() => {
        console.log('Copied to clipboard successfully!');
    }, (err) => {
        console.error('Failed to copy text: ', err);
    });
}

window.pasteFromClipboard = async () => {
    try {
        const text = await navigator.clipboard.readText();
        return text;
    } catch (err) {
        console.error('Failed to read text from clipboard: ', err);
        return '';
    }
}