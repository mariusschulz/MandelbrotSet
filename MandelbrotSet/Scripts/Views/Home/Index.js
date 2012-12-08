var MandelbrotViewModel = (function () {
    function MandelbrotViewModel(width, height) {
        this.width = width;
        this.height = height;
    }
    MandelbrotViewModel.prototype.plot = function (topLeft, bottomRight, maxIterationDepth, threshold) {
        var $canvas = $("#mandelbrot-canvas");
        $canvas.attr("src", $canvas.data("drawingActionUrl") + "/" + this.width + "-" + this.height + "/" + "(" + topLeft.getRealPart() + "," + topLeft.getImaginaryPart() + ")-" + "(" + bottomRight.getRealPart() + "," + bottomRight.getImaginaryPart() + ")" + "/" + "(" + maxIterationDepth + "," + threshold + ")");
    };
    return MandelbrotViewModel;
})();
$(function () {
    var realFrom = -2.1;
    var imaginaryFrom = 1.1;
    var realTo = 1.1;
    var imaginaryTo = -1.1;

    var aspectRatio = (realTo - realFrom) / (imaginaryFrom - imaginaryTo);
    var width = 1200;
    var height = Math.round(width / aspectRatio);

    var viewModel = new MandelbrotViewModel(width, height);
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));
    var topLeft = new ComplexNumber(realFrom, imaginaryFrom);
    var bottomRight = new ComplexNumber(realTo, imaginaryTo);

    viewModel.plot(topLeft, bottomRight, 199, 2);
});
