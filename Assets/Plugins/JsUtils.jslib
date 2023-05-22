mergeInto(LibraryManager.library, {

    OpenTab: function(url){
        url = Pointer_stringify(url);
        window.open(url,'_top');
    },

    ClearStorage: function(){
        localStorage.clear();
    },

    Hello: function () {
      window.alert("Hello, world!");
    },

});