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
        a = data.a;
        if (a < 0) {
            a = Math.abs(a);
            $("#curveLittleA").text("- " + a);
        }
        else {
            $("#curveLittleA").text("+ " + a);
        }
        b = data.b;
        if (b < 0) {
            b = Math.abs(b);
            $("#curveLittleB").text("- " + b);
        }
        else {
            $("#curveLittleB").text("+ " + b);
        }
        $("#fieldPrimeP").text(data.prime);
        $("#basepointG").text(data.g.x + ", " + data.g.y + ", " + data.g.z);
        $("#basepointGOrder").text(data.order);
    },

    SetKeyConstraints(order) {
        $("input[type='number']").prop('max', order-1);
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

            $.ajax({
                url: "/Api/Main/",
                method: "POST",
                data: dHKeyModule,
                success: function (data) {
                    console.log("Success!");
                    Results.Initialize(data);
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
        a = dHKeyModule.keyLittleA;
        b = dHKeyModule.keyLittleB;
        o = dHKeyModule.order;
        ab = Modulo(a + b, o);
        $("#pointAG").text(a + "G = " + dHKeyModule.ag);
        $("#pointBG").text(b + "G = " + dHKeyModule.bg);
        $("#pointM1").text("M1 = " + ab + "G = " + dHKeyModule.m1);
        $("#pointM2").text("M2 = " + ab + "G = " + dHKeyModule.m2);
    }
}

function Modulo(a, n) {
    return (a % n + n) % n;
}

$(document).ready(function () {
    MainViewModel.Initialize();
    CalculateButton.SetOnClickEventHandler();
});