window.blazorDownloadFile = (filename, contentType, base64Data) => {
    const link = document.createElement('a');
    link.download = filename;
    link.href = `data:${contentType};base64,${base64Data}`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    return true;
};

window.printHtml = (htmlContent) => {
    try {
        const printWindow = window.open('', '_blank');
        if (!printWindow) throw new Error('Unable to open print window (popup blocked)');

        const doc = printWindow.document;
        doc.open();
        doc.write(`<!doctype html><html><head><title>Supplier List</title><meta charset="utf-8" /><meta name="viewport" content="width=device-width,initial-scale=1" />`);
        doc.write('<style>body{font-family: Arial, Helvetica, sans-serif; margin:20px;} table{width:100%; border-collapse:collapse;} th, td{border:1px solid #ddd; padding:8px; text-align:left; vertical-align:top;} th{background:#f3f4f6;}</style>');
        doc.write('</head><body>');
        doc.write(htmlContent);
        doc.write('</body></html>');
        doc.close();

        printWindow.focus();
        setTimeout(() => {
            try {
                printWindow.print();
                printWindow.close();
            } catch (err) {
                console.error('Print failed', err);
                throw err;
            }
        }, 250);

        return true;
    } catch (err) {
        console.error('printHtml failed:', err);
        throw err;
    }
};