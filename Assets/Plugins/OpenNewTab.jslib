mergeInto(LibraryManager.library, {
    OpenBrowserNewTab: function(url) {
        window.open(Pointer_stringify(url), '_blank');
    }
});