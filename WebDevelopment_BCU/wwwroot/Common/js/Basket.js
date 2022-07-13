function basket(ProductId, Quantity) {
    this.ProductId = ProductId;
    this.Quantity = Quantity;
}

var _basket = new basket(0, 0);

function GetDataBasket() {

    b_ajax("/UserBasket/GetDataBasket", "Get", null, function (data) {
        console.log(data)
        var src = "";
        for (var i in data) {
            src += '<h4>Cart <span class="price" style="color:white"><i class="fa fa-shopping-cart"></i> <b>' + data[i].totalCount + '</b></span></h4>';
            src += '<button onclick="RemoveAllBasket(' + data[i].productId + ')" class="btn btn-danger fa fa-trash-o"></button>';
            src += '<div class="row">';
            src += '<div class="col-sm-4">';
            src += '<a href="#">' + data[i].name + '</a>';
            src += '</div>';
            src += '<div class="col-sm-4">';
            src += '<span class="price">$' + data[i].price + '</span>';
            src += '</div>';
            src += '<div class="col-sm-4">';
            src += '<div class="btn-group">';
            src += '<button class="btn btn-danger fa fa-minus" style="background-color:red" onclick="ReduceItemBasket(' + data[i].productId + ')"></button>';
            src += '<input style="width:100%" id="Count_' + data[i].productId + '" value="' + data[i].count + '" />';
            src += '<button class="btn btn-success fa fa-plus" onclick="PlusToBasket(' + data[i].productId + ')"></button>';
            src += '</div>';
            src += '</div>';
            src += '</div>';
            src += '<p>Total <span id="_totalPrice" class="price" style="color:white"><b>' + data[i].totalPrice +'</b></span></p>';
        }
        Cart_Basket.innerHTML = src;
        //asdasdqdqw.innerHTML = totalprice;

    })
}

function RemoveItemBasket(ProductId) {
    _basket.ProductId = ProductId;

    b_ajax("/UserBasket/RemoveItemBasket", "Post", { basket: _basket }, function () {
        GetDataBasket()
    }, "ok")
}

function ReduceItemBasket(ProductId) {
    var count = $("#Count_" + ProductId).val();

    _basket.Quantity = count;
    _basket.ProductId = ProductId;

    b_ajax("/UserBasket/ReduceItemBasket", "Post", { basket: _basket }, function () {
        GetDataBasket()
    }, "ok")
}

function RemoveAllBasket() {

    b_ajax("/UserBasket/RemoveAllBasket", "Post", null, function () {
        GetDataBasket()
    }, "ok")
}
function AddToBasket(ProductId) {
    var count = $("#Count_" + ProductId).val();
    console.log(count)
    if (count <= 0) {
        error_ajax("Count Not Valid");
        return;
    }
    else {
        _basket.Quantity = count;
        _basket.ProductId = ProductId;
        b_ajax("/UserBasket/AddToBasket", "Post", { basket: _basket }, function () {
            GetDataBasket()
        }, "ok");
    }

}

function PlusToBasket(ProductId) {
    var count = $("#Count_" + ProductId).val();

    if (count <= 0) {
        error_ajax("Count Not Valid");
        return;
    }
    else {
        _basket.Quantity = parseInt(count) + 1;
        _basket.ProductId = ProductId;
        b_ajax("/UserBasket/AddToBasket", "Post", { basket: _basket }, function () {
            GetDataBasket()
        }, "ok");
    }

}