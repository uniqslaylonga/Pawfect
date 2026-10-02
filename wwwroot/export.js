// Small helpers used by the Reports and Audit Trail pages (called through IJSRuntime).
window.pawfectExport = {
    // Saves text as a file. A BOM is added so Excel reads UTF-8 (e.g. the peso sign) correctly.
    download: function (filename, content, mime) {
        const blob = new Blob(['\uFEFF' + content], { type: mime || 'text/csv;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        a.remove();
        setTimeout(function () { URL.revokeObjectURL(url); }, 2000);
    },

    // Opens the browser print dialog for a standalone HTML document ("Save as PDF" works there).
    print: function (html) {
        const frame = document.createElement('iframe');
        frame.style.cssText = 'position:fixed;right:0;bottom:0;width:0;height:0;border:0;visibility:hidden';
        document.body.appendChild(frame);
        const doc = frame.contentWindow.document;
        doc.open();
        doc.write(html);
        doc.close();
        setTimeout(function () {
            frame.contentWindow.focus();
            frame.contentWindow.print();
            setTimeout(function () { frame.remove(); }, 60000);
        }, 300);
    }
};
