/// <reference path="../../Libraries/knockout.js/knockout.d.ts" />
/// <reference path="../../Libraries/jQuery/jquery.d.ts" />
/// <reference path="ComplexNumber.ts" />

class MandelbrotViewModel {
    constructor () {
        
    }

    
}

$(function () {
    var viewModel = new MandelbrotViewModel();
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));

    viewModel.plot(new ComplexNumber(-2.1, 1.1), new ComplexNumber(1.1, -1.1));
});