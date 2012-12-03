var MandelbrotViewModel = (function () {
    function MandelbrotViewModel() { }
    return MandelbrotViewModel;
})();
$(function () {
    var viewModel = new MandelbrotViewModel();
    ko.applyBindings(viewModel, document.getElementById("mandelbrot-plot"));
});
