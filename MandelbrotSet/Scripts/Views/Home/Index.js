var MandelbrotViewModel = (function () {
    function MandelbrotViewModel() {
    }
    return MandelbrotViewModel;
})();
$(function () {
    var viewModel = new MandelbrotViewModel();
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));
    viewModel.plot(new ComplexNumber(-2.1, 1.1), new ComplexNumber(1.1, -1.1));
});
