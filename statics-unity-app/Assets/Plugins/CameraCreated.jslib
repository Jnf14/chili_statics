mergeInto(LibraryManager.library, {
    Hello: function () {
      document.dispatchEvent(new Event("UnityStarted"));
    }
});