window.common = window.common || {};

common.pieChart = function (source, x, y, title, subtitle) {
    var data = source, targetId = '#chartReport';
    if (source.targetId) {
        targetId = source.targetId;
    }
    if (source.data) {
        data = source.data;
    }
    if (x == null) {
        throw 'Truc x chua dinh nghia';
    }
    if (y == null) {
        throw 'Truc y chua dinh nghia';
    }

    if (title == null) {
        title = '';
    }
    if (subtitle == null) {
        subtitle = '';
    }

    var chartdata = [];
    if (data != null && data.length > 0) {
        var sort = 1;
        // sort danh sach rows
        data.sort(function (a, b) {
            var vala = a[x];
            var valb = b[x];
            if ($.isNumeric(vala) && $.isNumeric(valb)) {
                try {
                    vala = parseFloat(vala);
                    valb = parseFloat(valb);
                } catch (ex) {
                    console.log(ex);
                }
            }
            return (vala == valb) ? 0 : ((vala > valb) ? sort : -1 * sort);
        });

        if (typeof data[0][x] == "undefined")
            throw $.StringFormat('Cot {0} chua dinh nghia', x);
        if (typeof data[0][y] == "undefined")
            throw $.StringFormat('Cot {0} chua dinh nghia', y);

        for (var i = 0; i < data.length; i++) {
            chartdata.push([data[i][x], data[i][y]]);
        }
    }

    $(targetId).highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        subtitle: {
            text: $.StringFormat('<span class="chart-title">{0}</span>', subtitle),
            useHTML: true
        },
        title: {
            text: title
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
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
                    }
                },
                showInLegend: true
            }
        },
        series: [{
            name: x,
            data: chartdata
        }]
    });
}

common.lineChart = function (source, x, yArray, title, subtitle) {
    var data = source, targetId = '#chartReport';
    if (source.targetId) {
        targetId = source.targetId;
    }
    if (source.data) {
        data = source.data;
    }
    if (x == null) {
        throw 'Truc x chua dinh nghia';
    }
    if (yArray == null || yArray.length == 0) {
        throw 'Truc y chua dinh nghia';
    }
    if (title == null) {
        title = '';
    }
    if (subtitle == null) {
        subtitle = '';
    }
    var series = [];
    var categories = [];
    var yAxis = [];
    if (data != null && data.length > 0) {
        var sort = 1;
        // sort danh sach rows
        data.sort(function (a, b) {
            var vala = a[x];
            var valb = b[x];
            if ($.isNumeric(vala) && $.isNumeric(valb)) {
                try {
                    vala = parseFloat(vala);
                    valb = parseFloat(valb);
                } catch (ex) {
                    console.log(ex);
                }
            }
            return (vala == valb) ? 0 : ((vala > valb) ? sort : -1 * sort);
        });

        if (typeof data[0][x] == "undefined")
            throw $.StringFormat('Cot {0} chua dinh nghia', x);
        for (var j = 0; j < yArray.length; j++) {
            var name = yArray[j];
            if (typeof name === "object") {
                name = name.name;
            }
            if (typeof data[0][name] == "undefined")
                throw $.StringFormat('Cot {0} chua dinh nghia', name);
        }

        for (var i = 0; i < data.length; i++) {
            categories.push(data[i][x]);
        }

        //kiem tra so truc y
        var yNum = 1;
        for (var j = 0; j < yArray.length; j++) {
            var yObj = yArray[j];
            if (typeof yObj === "object") {
                if (yObj.yOrder > yNum)
                    yNum = yObj.yOrder;
            }
        }

        for (var l = 0; l <= yNum; l++) {
            var yText = '';
            for (var j = 0; j < yArray.length; j++) {
                var yObj = yArray[j];
                if (typeof yObj === "object") {
                    if (yObj.yOrder == null || yObj.yOrder == l)
                        if (yObj.yText != null)
                            yText = yObj.yText;
                }
            }

            yAxis.push({
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[l]
                    }
                },
                title: {
                    style: {
                        color: Highcharts.getOptions().colors[l]
                    },
                    text: yText
                },
                min: 0,
                opposite: l > 0
            });
        }

        for (var j = 0; j < yArray.length; j++) {
            var serie = [], text;
            var yObj = yArray[j];
            if (typeof yObj === "object") {
                yNum = yObj.yOrder;
                text = yObj.text || yObj.name;
            } else {
                yNum = 0;
                text = yArray[j];
            }
            for (var k = 0; k < data.length; k++) {
                serie.push(data[k][name]);
            }

            series.push({
                name: text,
                yAxis: yNum,
                data: serie
            });
        }
    }

    $(targetId).highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'line'
        },
        subtitle: {
            text: $.StringFormat('<span class="chart-title">{0}</span>', subtitle),
            useHTML: true
        },
        title: {
            text: title
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}</b>'
        },
        legend: {
            layout: 'vertical',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0
        },
        xAxis: {
            categories: categories
        },
        yAxis: yAxis,
        series: series
    });
}

common.columnChart = function (source, x, yArray, title, subtitle) {
    var data = source, targetId = '#chartReport';
    if (source.targetId) {
        targetId = source.targetId;
    }
    if (source.data) {
        data = source.data;
    }
    if (x == null) {
        throw 'Truc x chua dinh nghia';
    }
    if (yArray == null || yArray.length == 0) {
        throw 'Truc y chua dinh nghia';
    }
    if (title == null) {
        title = '';
    }
    if (subtitle == null) {
        subtitle = '';
    }
    var series = [];
    var categories = [];
    var yAxis = [];
    if (data != null && data.length > 0) {
        var sort = 1;
        // sort danh sach rows
        data.sort(function (a, b) {
            var vala = a[x];
            var valb = b[x];
            if ($.isNumeric(vala) && $.isNumeric(valb)) {
                try {
                    vala = parseFloat(vala);
                    valb = parseFloat(valb);
                } catch (ex) {
                    console.log(ex);
                }
            }
            return (vala == valb) ? 0 : ((vala > valb) ? sort : -1 * sort);
        });

        if (typeof data[0][x] == "undefined")
            throw $.StringFormat('Cot {0} chua dinh nghia', x);
        for (var j = 0; j < yArray.length; j++) {
            var name = yArray[j];
            if (typeof name === "object") {
                name = name.name;
            }
            if (typeof data[0][name] == "undefined")
                throw $.StringFormat('Cot {0} chua dinh nghia', name);
        }

        for (var i = 0; i < data.length; i++) {
            categories.push(data[i][x]);
        }

        //kiem tra so truc y
        var yNum = 0;
        for (var j = 0; j < yArray.length; j++) {
            var yObj = yArray[j];
            if (typeof yObj === "object") {
                if (yObj.yOrder > yNum)
                    yNum = yObj.yOrder;
            }
        }

        for (var l = 0; l <= yNum; l++) {
            var yText = '';
            for (var j = 0; j < yArray.length; j++) {
                var yObj = yArray[j];
                if (typeof yObj === "object") {
                    if (yObj.yOrder == null || yObj.yOrder == l)
                        if (yObj.yText != null)
                            yText = yObj.yText;
                }
            }

            yAxis.push({
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[l]
                    }
                },
                title: {
                    style: {
                        color: Highcharts.getOptions().colors[l]
                    },
                    text: yText
                },
                opposite: l > 0
            });
        }

        for (var j = 0; j < yArray.length; j++) {
            var serie = [], text;
            var yObj = yArray[j];
            if (typeof yObj === "object") {
                yNum = yObj.yOrder;
                text = yObj.text || yObj.name;
            } else {
                yNum = 0;
                text = yArray[j];
            }
            for (var k = 0; k < data.length; k++) {
                serie.push(data[k][name]);
            }

            series.push({
                name: text,
                yAxis: yNum,
                data: serie
            });
        }
    }

    $(targetId).highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'column'
        },
        subtitle: {
            text: $.StringFormat('<span class="chart-title">{0}</span>', subtitle),
            useHTML: true
        },
        title: {
            text: title
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}</b>'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        xAxis: {
            categories: categories
        },
        yAxis: yAxis,
        series: series
    });
}