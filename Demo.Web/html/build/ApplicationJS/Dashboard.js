$(document).ready(function () {

    //Getlocations();
    fillYears();
    $("#filters").append('<span class="customtag" data-toggle="tooltip" data-original-title="Duration" id="yearfilter"><span><i class="fa fa-clock-o"></i> Year ' + $("#year :selected").text() + ' <i class="fa fa-remove customAnchor" id="yearremove"></i></span></span>');
    //fillMonths();
    $("[name='searchField']").keydown(function (event) {
        if (event.keyCode == 13) {
            event.returnValue = false;
            event.cancel = true;
            event.preventDefault();
            search();
            $('#searchModel').modal('toggle');
        }
    });

    $(document).on("click", "#search", function () {
        search();
        $("#filters").html("");
        if ($("#locationId").val() != undefined && $("#locationId").val() !== "") {
            $("#filters").append('<span class="customtag" data-toggle="tooltip" data-original-title="Location" id="locationfilter"><span><i class="fa fa-map-marker"></i>  ' + $("#locationId :selected").text() + ' <i class="fa fa-remove customAnchor" id="locationremove"></i></span></span>');
        }
        if ($("#year").val() != undefined && $("#year").val() !== "") {
            $("#filters").append('<span class="customtag" data-toggle="tooltip" data-original-title="Duration" id="yearfilter"><span><i class="fa fa-clock-o"></i> Year ' + $("#year :selected").text() + ' <i class="fa fa-remove customAnchor" id="yearremove"></i></span></span>');
        }
        $('[data-toggle="tooltip"]').tooltip();

    });

    $(document).on("click", ".fa-remove", function () {
        if (this.id == "locationremove") {
            $("#locationId").val("");
            $("#locationfilter").hide();
        }
        else if (this.id == "yearremove") {
            $("#yearfilter").hide();
            //$("#year").val(new Date().getFullYear());
            $("#year").val("");
            $("#durationType").val("1");
            //$('#year').find('option[value="' + new Date().getFullYear() + '"]').attr("selected", "selected");

        }
        search();
    }
    );
    $(document).on("click", "#reset", function () {
        $("#locationId").val("");
        $("#durationType").val("2");
        $("#year").val(new Date().getFullYear());
        $("#search").click();
        $("#filters").html("");
    });
    $(document).on("change", "#year", function () {

        if ($("#year").val() != undefined && $("#year").val() !== "") {
            $("#durationType").val("2");
            //fillMonths();
            //$("#monthSection").show();
        }
        else {
            $("#durationType").val("1");
            //$('#month').html("");
            //$("#monthSection").hide();
        }
    });
});

function fillYears() {
    var totalYears = 5;
    var currentYear = new Date().getFullYear();
    var i = 0;
    var pointedYear = currentYear - totalYears;
    $("#year").html(""); // clear before appending new list
    $("#year").append($('<option></option>').val("").html("-- All --"));
    for (i = 0; i < totalYears; i++) {
        $("#year").append($('<option></option>').val(pointedYear + i + 1).html(pointedYear + i + 1));
    }

    $('#year').find('option[value="' + currentYear + '"]').attr("selected", "selected");
}

function fillMonths() {
    $('#month').html("");
    $("#month").append($('<option></option>').val("").html("--- Choose Month ---"));
    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var html = monthNames.slice().map(function (month) {
        return '<option>' + month + '</option>';
    }).join('');
    $('#month').append(html);
}
//function loadAdsPanel() {
//    var width = $("#ads-report-pane").width();
//    var height = $("#ads-report-pane").height();
//    $("#ads-report-loading").css({
//        top: ((height / 2) - 25),
//        left: ((width / 2) - 50)
//    }).fadeIn(200);
//}


//function loadUserPanel() {
//    var width = $("#user-report-pane").width();
//    var height = $("#user-report-pane").height();
//    $("#user-report-loading").css({
//        top: ((height / 2) - 25),
//        left: ((width / 2) - 50)
//    }).fadeIn(200);
//}




function loadPanel(panel, loader) {
    var width = $("#" + panel).width();
    var height = $("#" + panel).height();
    $("#" + loader).css({
        top: ((height / 2) - 25),
        left: ((width / 2) - 50)
    }).fadeIn(200);
}


function search() {
    getSummary();

    if ($("#ads-report-pane").length > 0) {
        loadPanel("ads-report-pane", "ads-report-loading");
        createAdsChart();
    }


    if ($("#user-report-pane").length > 0) {        
        loadPanel("user-report-pane", "user-report-loading");
        createUserChart();
    }

    if ($("#request-report-pane").length > 0) {        
        loadPanel("request-report-pane", "request-report-loading");
        createRequestChart();
    }

    if ($("#advertisement-report-pane").length > 0) {        
        loadPanel("advertisement-report-pane", "advertisement-report-loading");
        createAdvertisementChart();
    }
}

function GetSearchData() {
    var locationId = $("#locationId").val();
    var durationType = $("#durationType").val();
    var year = $("#year").val();
    var searchingData = {
        LocationId: locationId,
        DurationType: durationType,
        Year: year
    };

    return JSON.stringify(searchingData);
}


function Getlocations() {
    $.ajax({
        type: "GET", //HTTP GET Method
        url: "/Dashboard/GetLocations", // Controller/View
        datatype: "json", // refer notes below
        success: function (data) {
            $("#locationId").html(""); // clear before appending new list
            $("#locationId").append($('<option></option>').val("").html("--- Select Location ---"));
            $.each(data, function (i, location) {
                $("#locationId").append(
                    $('<option></option>').val(location.Id).html(location.Name));
            });
        },
        error: function (jqXHR, status, err) {
        }
    });
}
function createAdsChart() {
    var searchData = GetSearchData();
    $.ajax({
        type: "GET", //HTTP GET Method
        url: "/Dashboard/GetBookedAds", // Controller/View
        data: { //Passing data
            search: searchData,
        },
        datatype: "json", // refer notes below
        success: function (data) {
            if (data.length > 0) {
                bindAdsChart(data);
            }
            else {
                $("#container").hide();
                $("#ads-notfound").show();
            }

            $("#ads-report-loading").fadeOut(1000);

        },
        error: function (jqXHR, status, err) {
        }
    });
}

function createUserChart() {
    var searchData = GetSearchData();
    $.ajax({
        type: "GET", //HTTP GET Method
        url: "/Dashboard/GetUsers", // Controller/View
        data: { //Passing data
            search: searchData,
        },
        datatype: "json", // refer notes below
        success: function (data) {
            if (data.length > 0) {
                bindUserChart(data);
                $("#user-notfound").hide();
                $("#userDetails").show();
            }
            else {
                $("#userDetails").hide();
                $("#user-notfound").show();
            }

            $("#user-report-loading").fadeOut(1000);

        },
        error: function (jqXHR, status, err) {
            $("#user-report-loading").fadeOut(1000);
        }
    });
}
function createRequestChart() {
    var searchData = GetSearchData();
    $.ajax({
        type: "GET", //HTTP GET Method
        url: "/Dashboard/GetRequests", // Controller/View
        data: { //Passing data
            search: searchData,
        },
        datatype: "json", // refer notes below
        success: function (data) {

            if (data.length > 0) {
                bindRequestChart(data);
                $("#request-notfound").hide();
                $("#requestDetails").show();
            }
            else {
                $("#requestDetails").hide();
                $("#request-notfound").show();
            }

            $("#request-report-loading").fadeOut(1000);

        },
        error: function (jqXHR, status, err) {
        }
    });
}

function createAdvertisementChart() {
    var searchData = GetSearchData();
    $.ajax({
        type: "GET", //HTTP GET Method
        url: "/Dashboard/GetAdvertisements", // Controller/View
        data: { //Passing data
            search: searchData,
        },
        datatype: "json", // refer notes below
        success: function (data) {
            if (data.length > 0) {
                bindAdvertisementChart(data);
                $("#advertisement-notfound").hide();
                $("#AdvertisementChart").show();
            }
            else {
                $("#AdvertisementChart").hide();
                $("#advertisement-notfound").show();
            }

            $("#advertisement-report-loading").fadeOut(1000);

        },
        error: function (jqXHR, status, err) {
        }
    });
}


function getSummary() {
    var searchData = GetSearchData();
    $.ajax({
        type: "GET", //HTTP GET Method
        url: "/Dashboard/GetSummary", // Controller/View
        data: { //Passing data
            search: searchData,
        },
        datatype: "json", // refer notes below
        success: function (data) {
            bindSummary(data);
        },
        error: function (jqXHR, status, err) {
        }
    });
}

function bindSummary(data) {
    $('#sumary').html("");
    $('#sumary').html(data);
}
function bindAdsChart(data) {

    var chartTitle = '';
    var count = 0;
    var chartData = [];

    for (count = 0; count < data.length; count++) {
        chartData.push({ name: data[count].Key, description: data[count].Description, y: data[count].Value })

    }
    if ($("#durationType").val() == undefined || $("#durationType").val() === "" || $("#durationType").val() == 2) {
        chartTitle = "Booked Advertisements of year " + $("#year").val();
    }
    else {
        chartTitle = "Booked Advertisements of years " + $('#year option:nth-child(2)').val() + "-" + $('#year option:last-child').val();
    }

    // Create the chart
    Highcharts.chart('container', {
        chart: {
            type: 'column'
        },
        title: {
            text: chartTitle
        },
        //subtitle: {
        //    text: 'Click the columns to view versions. Source: <a href="http://netmarketshare.com">netmarketshare.com</a>.'
        //},
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Total Booked Advertisements'
            }

        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y}'
                }
            }
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.description}</span>: <b>{point.y}</b><br/>'
        },

        series: [{
            name: 'Advertisements',
            colorByPoint: true,
            data: chartData,
        }],
    });
}
function bindUserChart(data) {
    var chartTitle = '';
    var count = 0;
    var chartData = [];

    for (count = 0; count < data.length; count++) {
        chartData.push({ name: data[count].Key, description: data[count].Description, y: data[count].Value })

    }


    if ($("#durationType").val() == undefined || $("#durationType").val() === "" || $("#durationType").val() == 2) {
        chartTitle = "Users of year " + $("#year").val();
    }
    else {
        chartTitle = "Users of years " + $('#year option:nth-child(2)').val() + "-" + $('#year option:last-child').val();
    }
    // Create the chart
    Highcharts.chart('userDetails', {
        chart: {
            type: 'column'
        },
        title: {
            text: chartTitle
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Total Users'
            }

        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y}'
                }
            }
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.description}</span>: <b>{point.y}</b><br/>'
        },

        series: [{
            name: 'Users',
            colorByPoint: true,
            data: chartData,
        }],

    });
}

function allRequests(data, status, requiredParam) {
    var count = 0;
    var chartData = [];
    var result = data.filter(function (record) {
        return record.Status == status;

    });
    if (requiredParam == "value") {
        for (count = 0; count < result.length; count++) {
            chartData.push(result[count].Value)
        }
    }
    else if (requiredParam == "key") {
        for (count = 0; count < result.length; count++) {
            chartData.push(result[count].Key)
        }
    }


    return chartData;
}

function bindRequestChart(data) {
    var chartTitle = '';
    var count = 0;
    var chartData = [];

    var firstRecord = {};
    if (data.length > 0) {
        firstRecord.Key = data[0].Key;
        firstRecord.Status = data[0].Status;
    }
    // Categories 
    var categories = allRequests(data, firstRecord.Status, "key");

    //First groups of columns
    var result = data.filter(function (record) {
        return record.Key == firstRecord.Key;
    });
    result.forEach(function (element) {
        chartData.push({ name: element.Status, description: element.Description, color: element.Color, data: allRequests(data, element.Status, "value") })
    });


    if ($("#durationType").val() == undefined || $("#durationType").val() === "" || $("#durationType").val() == 2) {
        chartTitle = "Requests of year " + $("#year").val();
    }
    else {
        chartTitle = "Requests of years " + $('#year option:nth-child(2)').val() + "-" + $('#year option:last-child').val();
    }
    Highcharts.chart('requestDetails', {
        chart: {
            type: 'column'
        },
        title: {
            text: chartTitle
        },
        xAxis: {
            categories: categories,
            crosshair: true
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total Requests'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                '<td style="padding:0"><b>{point.y}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            }
        },
        series: chartData
    });
}
function bindAdvertisementChart(data) {

    var chartTitle = '';
    var count = 0;
    var chartData = [];

    for (count = 0; count < data.length; count++) {
        chartData.push({ name: data[count].Key, description: data[count].Description, y: data[count].Value, color: data[count].Color })

    }
    if ($("#durationType").val() == undefined || $("#durationType").val() === "" || $("#durationType").val() == 2) {
        chartTitle = "Advertisements of year " + $("#year").val();
    }
    else {
        chartTitle = "Advertisements of years " + $('#year option:nth-child(2)').val() + "-" + $('#year option:last-child').val();
    }


    // Radialize the colors
    //Highcharts.setOptions({
    //    colors: Highcharts.map(Highcharts.getOptions().colors, function (color) {
    //        return {
    //            radialGradient: {
    //                cx: 0.5,
    //                cy: 0.3,
    //                r: 0.7
    //            },
    //            stops: [
    //                [0, color],
    //                [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
    //            ]
    //        };
    //    })
    //});

    // Build the chart
    Highcharts.chart('AdvertisementChart', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: chartTitle
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    },
                    connectorColor: 'silver'
                }
            }
        },
        series: [{
            name: 'Advertisements',
            data: chartData
        }]
    });
}