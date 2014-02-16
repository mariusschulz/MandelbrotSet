$(function() {
    var realFrom = -2.1,
        imaginaryFrom = 1.2,
        realTo = 1.1,
        imaginaryTo = -imaginaryFrom,
        width = 1024;

    var aspectRatio = (realTo - realFrom) / (imaginaryFrom - imaginaryTo),
        height = Math.round(width / aspectRatio);

    var $image = $("#mandelbrot-image");
    $image.attr("src", $image.data("drawingActionUrl") + "/"
        + width + "-" + height + "/"
        + "(" + realFrom + "," + imaginaryFrom + ")-"
        + "(" + realTo + "," + imaginaryTo + ")/"
        + "(" + 200 + "," + 1000 + ")");
});
