// If everything goes well, this script will not appear in the source when we're on the Admin UI.

// This technique limits the scope of the $ variable to the block. Thus it's possible to use jQuery event if there's
// another JS library using the same global variable.
(function ($) {
    // The below technique adds "namespaces" to the jQuery object. This is a common way of storing global JS services, 
    // without cluttering the global scope.
    $.extend(true, {
        orchardHUN: {
            trainngDemo: {
                addCommentToBody: function () {
                    $("body").prepend("This is a useless insertion.");
                }
            }
        }
    });

    // This will run when the document is ready
    $(function () {
        $.orchardHUN.trainngDemo.addCommentToBody();
    });
})(jQuery);