var ComplexNumber = (function () {
    function ComplexNumber(realPart, imaginaryPart) {
        this.realPart = realPart;
        this.imaginaryPart = imaginaryPart;
    }
    ComplexNumber.prototype.getRealPart = function () {
        return this.realPart;
    };
    ComplexNumber.prototype.getImaginaryPart = function () {
        return this.imaginaryPart;
    };
    return ComplexNumber;
})();
