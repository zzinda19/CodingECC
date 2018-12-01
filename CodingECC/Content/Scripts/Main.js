var MainViewModel = {

    Initialize: function () {
        $.ajax({
            url: "/Api/Main",
            method: "GET",
            success: function (data) {
                MainViewModel.PopulateCurveValues(data);
                MainViewModel.SetKeyConstraints(data.order);
            },
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.statusCode + " " + xhr.statusText);
            }
        });
    },

    PopulateCurveValues(data) {
        $("#curveLittleA").text(data.a);
        $("#curveLittleB").text(data.b);
        $("#fieldPrimeP").text(data.prime);
        $("#basepointG").text(data.g.x + ", " + data.g.y + ", " + data.g.z);
        $("#basepointGOrder").text(data.order);
    },

    SetKeyConstraints(order) {
        $("input[type='number']").prop('max', order);
    }
};

var CalculateButton = {
    SetOnClickEventHandler: function () {
        var form = $("#ECDH");
        form.submit(function (event) {
            event.preventDefault();

            var keyLittleA = $("#keyLittleA").val();
            var keyLittleB = $("#keyLittleB").val();

            var dHKeyModule = {
                keyLittleA: keyLittleA,
                keyLittleB: keyLittleB
            };

            console.log(dHKeyModule);

            $.ajax({
                url: "/Api/Main/",
                method: "POST",
                data: dHKeyModule,
                success: function () {
                    console.log("success");
                },
                error: function (xhr) {
                    toastr.error("An error occured: " + xhr.statusCode);
                }
            });

            return false;
        });
    }
}

var Results = {
    Initialize: function (dHKeyModule) {
        console.log(dHKeyModule);
    }
}

$(document).ready(function () {
    MainViewModel.Initialize();
    CalculateButton.SetOnClickEventHandler();
});