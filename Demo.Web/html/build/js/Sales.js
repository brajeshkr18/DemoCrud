//Load Data in Table when documents is ready  
$(document).ready(function () {
   // loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "../Customer/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
           
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td style="display:none">' + item.Id + '</td>';
                html += '<td>' + item.Name + '</td>';
                html += '<td>' + item.Age + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '<td>' + item.ModifiedDate + '</td>';
                html += '<td><a href="#" onclick="return getbyID(' + item.Id + ')">Edit</a> | <a href="#" onclick="Delele(' + item.Id + ')">Delete</a></td>';
                html += '</tr>';
            });
           
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Add Data Function   
function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var objsales = {
        Id: $('#Id').val(),
        ProductId: $('#ddlProduct').val(),
        CustomerId: $('#ddlCustomer').val(),
        StoreId: $('#ddlStores').val(),
        PurchasedDate: $('#txtDate').val()
    };
    console.log(objsales);
    console.log(JSON.stringify(objsales));
    $.ajax({
        url: "/Sales/Add",
        data: JSON.stringify(objsales),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result)
            loadData();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            console.log(result)
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the Data Based upon Employee ID  
function getbyID(EmpID) {
    $('#Name').css('border-color', 'lightgrey');
    //$('#Age').css('border-color', 'lightgrey');

    $.ajax({
        url: "../Sales/GetbyID/" + EmpID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result[0].Id);
            $('#ddlProduct').val(result[0].ProductId);
            $('#ddlCustomer').val(result[0].CustomerId);
            $('#ddlStores').val(result[0].StoreId);
            $('#txtDate').val(result[0].PurchasedDate);
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record  
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        Id: $('#EmployeeID').val(),
        Name: $('#Name').val(),
        Age: $('#Age').val(),
        Address: $('#Address').val()
    };
    $.ajax({
        url: "/Sales/Update",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result)
            //loadData();
            $('#myModal').modal('hide');
            $('#EmployeeID').val("");
            $('#Name').val("");
            $('#Age').val("");
            $('#State').val("");
            $('#Country').val("");
        },
        error: function (errormessage) {
            console.log(result)
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record  
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Sales/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    //$('#EmployeeID').val("");
    //$('#Name').val("");
    //$('#Age').val("");
    //$('#State').val("");
    //$('#Country').val("");
    //$('#btnUpdate').hide();
    //$('#btnAdd').show();
    //$('#Name').css('border-color', 'lightgrey');
    //$('#Age').css('border-color', 'lightgrey');
    //$('#State').css('border-color', 'lightgrey');
    //$('#Country').css('border-color', 'lightgrey');
}
//Valdidation using jquery  
function validate() {
    var isValid = true;
    if ($('#txtDate').val().trim() == "") {
        $('#txtDate').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtDate').css('border-color', 'lightgrey');
    }
    if ($("ddlProduct").val() == undefined || $("ddlProduct").val() == "") {
        $('#ddlProduct').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlProduct').css('border-color', 'lightgrey');
    }
    //if ($('#Address').val().trim() == "") {
    //    $('#Address').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#Address').css('border-color', 'lightgrey');
    //}
    
    return isValid;
}  
