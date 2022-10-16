$(document).ready(function () {
    var table = $('#orders').DataTable();
    table.columns([4,5]).visible(false);
    $('#orders tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });

    $('#button').click(function () {
        var dataArray = table.rows('.selected').data().toArray();

        var data = new Array();

        $.each(dataArray, function (index, value) {

            var csvDto = {};
            consignmentInformation.OrderNumber = value[0];
            consignmentInformation.ConsignmentNumber = value[1];
            consignmentInformation.ParcelCode = value[2];
            consignmentInformation.ConsigneeName = value[3];
            consignmentInformation.AddressOne = value[4];
            consignmentInformation.AddressTwo = value[5];
            consignmentInformation.City = value[6];
            consignmentInformation.CountryCode = value[7];
            consignmentInformation.ItemQuantity = value[8];
            consignmentInformation.ItemValue = value[9];
            consignmentInformation.ItemWeight = value[10];
            consignmentInformation.ItemDesciption = value[11];
            consignmentInformation.ItemCurrency = value[12];
            data.push(consignmentInformation);
        });

        $.ajax({
            type: 'POST',
            url: '/Home/SendOrder',
            data: JSON.stringify(data),
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                alert("success");
            },
            error: function (response) {
                alert("error");
            }
        });

    });
});