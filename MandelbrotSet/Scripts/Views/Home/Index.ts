/// <reference path="../../Libraries/knockout.js/knockout.d.ts" />
/// <reference path="../../Libraries/jQuery/jquery.d.ts" />
/// <reference path="ComplexNumber.ts" />

class MandelbrotViewModel {
    constructor (private width: number, private height: number) {

    }

    plot(topLeft: ComplexNumber, bottomRight: ComplexNumber, maxIterationDepth: number, threshold: number) {
        var $canvas = $("#mandelbrot-canvas");
        $canvas.attr("src", $canvas.data("drawingActionUrl") + "/"
            + this.width + "-" + this.height + "/"
            + "(" + topLeft.getRealPart() + "," + topLeft.getImaginaryPart() + ")-"
            + "(" + bottomRight.getRealPart() + "," + bottomRight.getImaginaryPart() + ")" + "/"
            + "(" + maxIterationDepth + "," + threshold + ")");
    }
}

$(function () {
    var realFrom = -2.1,
        imaginaryFrom = 1.1,
        realTo = 1.1,
        imaginaryTo = -1.1;

    var aspectRatio = (realTo - realFrom) / (imaginaryFrom - imaginaryTo),
        width = 1200,
        height = Math.round(width / aspectRatio);

    var viewModel = new MandelbrotViewModel(width, height);
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));

    var topLeft = new ComplexNumber(realFrom, imaginaryFrom),
        bottomRight = new ComplexNumber(realTo, imaginaryTo);

    viewModel.plot(topLeft, bottomRight, 199, 2);
});