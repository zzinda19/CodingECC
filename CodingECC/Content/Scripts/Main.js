var MainViewModel = {

    Curve: {},

    Initialize: function () {
        $.ajax({
            url: "/api/main/index",
            method: "GET",
            success: function (data) {
                MainViewModel.Curve = data;
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
        $("#keyLittleA").prop('max', order-1);
        $("#keyLittleB").prop('max', order-1);
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
                keyLittleB: keyLittleB,
                eCCurve: MainViewModel.Curve
            };

            $.ajax({
                url: "/api/main/calculate",
                method: "POST",
                data: dHKeyModule,
                success: function (data) {
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

var EditCurveModalForm = {
    Dialog: {},
    Initialize: function (form) {
        this.Dialog = bootbox.dialog({
            title: "Edit Curve Properties",
            message: form,
            buttons: {
                cancel: {
                    label: "Cancel",
                    className: "btn btn-danger"
                },
                ok: {
                    label: "Save",
                    className: "btn btn-primary",
                    callback: function () {
                        EditCurveForm.Submit();
                    }
                }
            }
        });
    }
}

var EditCurveButton = {
    SetOnClickEventHandler: function () {
        EditCurveForm.Initialize();
        var editCurveButton = $("#editButton");
        editCurveButton.click(function () {
            $("#formA").val(MainViewModel.Curve.a);
            $("#formB").val(MainViewModel.Curve.b);
            $("#formP").val(MainViewModel.Curve.prime);
            $("#formX").val(MainViewModel.Curve.g.x);
            $("#formY").val(MainViewModel.Curve.g.y);
            EditCurveModalForm.Initialize(EditCurveForm.Form);
        });
    }
}

var EditCurveForm = {
    Form: {},
    Initialize: function () {
        this.Form = $("#editCurveForm");
    },
    Submit: function () {
        var newCurve = {
            a: $("#formA").val(),
            b: $("#formB").val(),
            g: {
                x: $("#formX").val(),
                y: $("#formY").val(),
                z: 1
            },
            prime: $("#formP").val()
        };
        $.ajax({
            url: "/api/main/update",
            method: "POST",
            data: newCurve,
            success: function (data) {
                MainViewModel.Curve = data;
                MainViewModel.PopulateCurveValues(data);
                MainViewModel.SetKeyConstraints(data.order);
            },
            error: function (xhr) {
                toastr.error("An error occured: " + xhr.responseJSON.message);
            }
        });
    }
}

var Results = {
    Initialize: function (dHKeyModule) {
        a = dHKeyModule.keyLittleA;
        b = dHKeyModule.keyLittleB;
        o = dHKeyModule.order;
        ab = Modulo(a + b, o);
        $("#pointAG").text(a + "G = " + dHKeyModule.ag);
        $("#pointBG").text(b + "G = " + dHKeyModule.bg);
        $("#pointM1").text(ab + "G = " + dHKeyModule.m1);
        $("#pointM2").text(ab + "G = " + dHKeyModule.m2);
    }
}

function Modulo(a, n) {
    return (a % n + n) % n;
}

$(document).ready(function () {
    MainViewModel.Initialize();
    CalculateButton.SetOnClickEventHandler();
    EditCurveButton.SetOnClickEventHandler();
});