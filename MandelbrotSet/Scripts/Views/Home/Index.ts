/// <reference path="../../Libraries/knockout.js/knockout.d.ts" />
/// <reference path="../../Libraries/jQuery/jquery.d.ts" />

class MandelbrotViewModel {

}

$(function () {
    var viewModel = new MandelbrotViewModel();
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));
});