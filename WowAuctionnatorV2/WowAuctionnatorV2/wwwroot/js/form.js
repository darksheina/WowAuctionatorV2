// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$form = $("#form");
$unitPriceWithTaxInput = $(".unitPriceWithTaxJs", $form);
$totalPriceWithTaxInput = $(".totalPriceWithTaxJs", $form);
$totalPriceWithoutTaxInput = $(".totalPriceWithoutTaxJs", $form);

$unitPriceWithTaxInput.on("keypress", function (event) {
    if (!isEnterDecimalCaracter(event)) {
        return false;
    }
}).on("keyup", function () {
    if (this.value.indexOf(".") != this.value.length - 1) {
        setAllPrice((+this.value), "unitPriceWithTax");
    }
});

$totalPriceWithTaxInput.on("keypress", function (event) {
    if (!isEnterDecimalCaracter(event)) {
        return false;
    }
}).on("keyup", function () {
    if (this.value.indexOf(".") != this.value.length - 1) {
        setAllPrice((+this.value), "totalPriceWithTax");
    }
});

$totalPriceWithoutTaxInput.on("keypress", function (event) {
    if (!isEnterDecimalCaracter(event)) {
        return false;
    }
}).on("keyup", function () {
    if (this.value.indexOf(".") != this.value.length - 1) {
        setAllPrice((+this.value), "totalPriceWithoutTax");
    }
});

$(".quantityJs", $form).on("keypress", function (event) {
    if (!isEnterIntegerCaracter(event)) {
        return false;
    }
}).on("keyup", function () {
    updatePrice();
});

$(".taxJs", $form).on("keypress", function (event) {
    if (!isEnterDecimalCaracter(event)) {
        return false;
    }
}).on("keyup", function () {
    updatePrice();
});

function isEnterIntegerCaracter(event) {
    if ((event.charCode == 8 || event.charCode == 0 || event.charCode == 13) || event.charCode >= 48 && event.charCode <= 57) {
        return true;
    }

    return false;
}

function isEnterDecimalCaracter(event) {
    if (event.charCode == 44 && event.currentTarget.value.indexOf(".") == -1 || isEnterIntegerCaracter(event)) {
        return true;
    }

    return false;
}

function setAllPrice(montant, input) {
    if ($(".quantityJs", $form).val() != "") {
        var quantity = (+$(".quantityJs", $form).val());
        var tax = (+$(".taxJs", $form).val()) / 100;

        switch (input) {
            case "unitPriceWithTax":
                $totalPriceWithTaxInput.val(montant * quantity);
                $totalPriceWithoutTaxInput.val((montant * quantity) * (1 - tax));
                break;
            case "totalPriceWithTax":
                $unitPriceWithTaxInput.val(montant / quantity);
                $totalPriceWithoutTaxInput.val(montant * (1 - tax));
                break;
            case "totalPriceWithoutTax":
                $unitPriceWithTaxInput.val(montant * (1 + tax) / quantity);
                $totalPriceWithTaxInput.val(montant * (1 + tax));
                break;
        }

        if ($unitPriceWithTaxInput.val().length > $unitPriceWithTaxInput.val().indexOf(".") + 4) {
            $unitPriceWithTaxInput.val((+$unitPriceWithTaxInput.val()).toFixed(4));
        }

        if ($totalPriceWithTaxInput.val().length > $totalPriceWithTaxInput.val().indexOf(".") + 4) {
            $totalPriceWithTaxInput.val((+$totalPriceWithTaxInput.val()).toFixed(4));
        }

        if ($totalPriceWithoutTaxInput.val().length > $totalPriceWithoutTaxInput.val().indexOf(".") + 4) {
            $totalPriceWithoutTaxInput.val((+$totalPriceWithoutTaxInput.val()).toFixed(4));
        }
    }
}

function updatePrice() {
    if ($unitPriceWithTaxInput.val() != "") {
        setAllPrice($unitPriceWithTaxInput.val(), "unitPriceWithTax");
    }
    else if ($totalPriceWithTaxInput.val() != "") {
        setAllPrice($totalPriceWithTaxInput.val(), "totalPriceWithTax");
    }
    else if ($totalPriceWithoutTaxInput.val() != "") {
        setAllPrice($totalPriceWithoutTaxInput.val(), "totalPriceWithoutTax");
    }
}

$(function () {
    $("#datepicker").datepicker().datepicker("setDate", new Date());
});

$(".itemNameJs", $form).on("keyup", function () {
    $.ajax({
        type: "GET",
        url: "/Item/GetItemListByName",
        data: { "name": this.value },
        success: function (response) {
            $("#blizzardAutocomplete", $form).html(response);
            $("#blizzardAutocomplete .list-group-item", $form).on("click", function () {
                $(".itemNameJs", $form).val(this.innerText);
                $("#blizzardAutocomplete", $form).html("");
                $("input[name='ItemId']", $form).val($(this).data("id"))
                $.ajax({
                    type: "GET",
                    url: "/Item/GetItemIconUrl",
                    data: { "id": $(this).data("wowid") },
                    success: function (urlImage) {
                        var img = $('<img id="dynamicItemIcon">');
                        img.attr('src', urlImage);
                        $(".itemIconJs", $form).addClass("itemIconJsWithoutPadding").html(img);
                    },
                    error: function (response) {
                        
                    }
                });
            })
        },
        error: function (response) {
            alert("Can't get items");
        }
    });
})

$(".dropdown-item", $form).on("clic", function () {
    $(".dropdown-toggle", $form).html(this.innerText);
})