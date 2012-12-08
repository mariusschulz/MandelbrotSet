var MandelbrotViewModel = (function () {
    function MandelbrotViewModel(width, height) {
        this.width = width;
        this.height = height;
    }
    MandelbrotViewModel.prototype.plot = function (topLeft, bottomRight) {
        var $canvas = $("#mandelbrot-canvas");
        $canvas.attr("src", $canvas.data("drawingActionUrl") + "/" + this.width + "-" + this.height + "/" + "(" + topLeft.getRealPart() + "," + topLeft.getImaginaryPart() + ")-" + "(" + bottomRight.getRealPart() + "," + bottomRight.getImaginaryPart() + ")");
    };
    return MandelbrotViewModel;
})();
$(function () {
    var viewModel = new MandelbrotViewModel(581, 400);
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));
    viewModel.plot(new ComplexNumber(-2.1, 1.1), new ComplexNumber(1.1, -1.1));
});
