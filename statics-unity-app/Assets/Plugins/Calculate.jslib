mergeInto(LibraryManager.library, {
    CalculateStaticsEvent: function (nodes, edges, pointLoads) {
      document.dispatchEvent(new Event("UnityStaticsCalculate", { nodes, edges, pointLoads }));
    }
});