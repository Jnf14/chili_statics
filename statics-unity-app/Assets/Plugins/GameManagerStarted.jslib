mergeInto(LibraryManager.library, {
    GameManagerStarted: function () {
      document.dispatchEvent(new Event("UnityGameManagerStarted"));
    }
});