var Tax = window.Tax || {};
Tax.Calculator = Tax.Calculator || {};

function TaxDTO(annualIncome, postalCodeId) {
    return {
        annualIncome: annualIncome,
        postalCodeId: postalCodeId
    };
}

Tax.Calculator.Form = (function () {
    const uri = "https://localhost:5001";

    function Form() {
        var self = this;
        self.onLoad = false;
        self.annualIncome = null;
        self.postalCodes = null;
    }

    Form.prototype.Init = function () {
        var self = this;
        Tax.Calculator.Services.Get(`${uri}/api/tax/getpostalcodes`, $.proxy(self.Load, this));
    };

    Form.prototype.Load = function (data) {
        var self = this;
        var response;

        if (data == null) {
            return;
        } else {
            response = data;
            self.onLoad = true;
        }

        // Set Page Properties
        self.annualIncome = $("#annualIncomeInpt");
        self.postalCodes = $("#postalCodeDrpdown");

        $.each(response, function (key, value) {
            self.postalCodes.append($('<option></option>').val(value.id).html(value.name));
        });
    };

    var defaultOptions = {
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        },
        onfocusout: function (element) {
            $(element).valid();
        }
    };

    $.validator.setDefaults(defaultOptions);

    $('#taxForm').validate({
        rules: {
            annualIncomeInpt: {
                required: true,
                digits: true
            }
        },
        messages: {
            annualIncomeInpt: {
                required: "Please enter your annual income",
                digits: "Only digits is allowed"
            }
        },
        submitHandler: function (form) {
            $('input[type=submit]').prop('disabled', true);

            var annualIncome = $("#annualIncomeInpt").val();
            var postalCodeId = $("[id$='postalCodeDrpdown']").val();

            var taxDto = new TaxDTO(annualIncome, postalCodeId);

            Tax.Calculator.Services.Post(`${uri}/api/tax/addtaxresult`, JSON.stringify(taxDto), function (data) {
                $('input[type=submit]').prop('disabled', false);

                $(".alert-display").css('display', 'block');
                $('#alertMessage').text(data.resultMessage); 

                setTimeout(function () {
                    $(".alert-display").css('display', 'none');
                }, 2000);
            });

            false;
        }
    });

    return Form;
})();
