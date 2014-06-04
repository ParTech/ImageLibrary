$(function() {

    $(".draggableimage").draggable({
        helper: "clone"
    });

    $("#shoppingcart").droppable({
        accept: ".draggableimage",
        drop: function (event, ui) {
            var image = ui.draggable.find(".imagecontainer");
            var imageid = $(image).attr("data-imageid");
            $.ajax("http://" + window.location.hostname + "/byer/addtoshoppingcart?imageid=" + imageid)
            .done(function (data) {
                if (data.Status == "success") {
                    $("#noofimages").text(data.Count);
                    $("#noofimagestop").text(data.Count);
                    $("#cartitems").empty();
                    $("#cartImageTemplate").tmpl(data.CartImages).appendTo("#cartitems");
                    $("#downloadimages").removeClass("disabled");
                    $("#deletecart").removeClass("disabled");
                }
                else {
                    alert("Something went wrong and the image could\r\nnot be added to the shopping cart. Try \r\nagain later.\r\n[" + data.AdditionalInformation + "]");
                }
            })
            .fail(function (data) {
                alert("Something went wrong and the image could\r\nnot be added to the shopping cart. Try \r\nagain later.\r\n[ajax call error]");
            });
        }
    })

    $('#downloadimages').click(function (e) {
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            return false;
        }
        else {
            window.location.href = $(this).attr('href');
        }
    });

    $('#deletecart').click(function (e) {
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            return false;
        }
        else {
            if (confirm("Are you sure you want to delete the shopping cart?")) {
                window.location.href = $(this).attr('href');
            }
        }
    });

    $(document).ready(function () {
        $.ajax("http://" + window.location.hostname + "/byer/getshoppingcart")
        .done(function (data) {
            if (data.Status == "success") {
                if (data.Count != "0") {
                    $("#noofimages").text(data.Count);
                    $("#cartImageTemplate").tmpl(data.CartImages).appendTo("#cartitems");
                    $("#downloadimages").removeClass("disabled");
                    $("#deletecart").removeClass("disabled");
                }
            }
            else {
                alert("Something went wrong and the shopping \r\ncart could not be retrieved. Try \r\nagain later.\r\n[" + data.AdditionalInformation + "]");
            }
        })
        .fail(function (data) {
            alert("Something went wrong and the shopping \r\ncart could not be retrieved. Try \r\nagain later.\r\n[ajax call error]");
        });
    });
});