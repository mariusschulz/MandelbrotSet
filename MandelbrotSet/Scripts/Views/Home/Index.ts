/// <reference path="../../Libraries/knockout.js/knockout.d.ts" />
/// <reference path="../../Libraries/jQuery/jquery.d.ts" />
/// <reference path="ComplexNumber.ts" />

class MandelbrotViewModel {
    constructor (private width: number, private height: number) {

    }

    plot(topLeft: ComplexNumber, bottomRight: ComplexNumber) {
        var $canvas = $("#mandelbrot-canvas");
        $canvas.attr("src", $canvas.data("drawingActionUrl") + "/"
            + this.width + "-" + this.height + "/"
            + "(" + topLeft.getRealPart() + "," + topLeft.getImaginaryPart() + ")-"
            + "(" + bottomRight.getRealPart() + "," + bottomRight.getImaginaryPart() + ")");
    }
}

$(function () {
    var viewModel = new MandelbrotViewModel(581, 400);
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));

    viewModel.plot(new ComplexNumber(-2.1, 1.1), new ComplexNumber(1.1, -1.1));
});