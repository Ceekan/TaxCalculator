var Tax = window.Tax || {};
Tax.Calculator = Tax.Calculator || {};

Tax.Calculator.Services = Tax.Calculator.Services || {};
Tax.Calculator.Services.Get = function (url, callbackFunction) {
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        }
    })
    .done(function (data, response) {
        if (response == "success") {
            callbackFunction(Tax.Calculator.Utils.Json(data));
        }
    })
    .fail(function (XMLHttpRequest, textStatus, errorThrown) {
        var errorThrown = "Request failed, " + textStatus + ", " + errorThrown;
        callbackFunction(errorThrown);
    });
};

Tax.Calculator.Services.Post = function (url, dataObj, callbackFunction) {
    $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: dataObj,
        dataType: 'json',
        cache: false,
        async: true
    })
    .done(function (data, response) {
        if (response == "success") {
            callbackFunction(Tax.Calculator.Utils.Json(data));
        }
    })
    .fail(function (XMLHttpRequest, textStatus, errorThrown) {
        var errorThrown = "Request failed, " + textStatus + ", " + errorThrown;
        callbackFunction(errorThrown);
    });
};

Tax.Calculator.Utils = Tax.Calculator.Utils || {};
Tax.Calculator.Utils.Json = function (data) {
    if (data == '' || data == 'undefined') return null;
    return data;
};