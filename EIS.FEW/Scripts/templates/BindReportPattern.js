function validateInput() {
    var msg = '';
    return msg;
}

function BindReport(page) {
    //validated
    var msg = validateInput();
    if (msg != '') {
        common.showWarning(msg);
        return false;
    }

    var baoCaoId = $("#BaoCaoId").val();
    var para = [];
    var control = [];
    $("#tablepara tbody tr").each(function (index) {
        var $checkbox = $(this).find("input[name='ts-choosen']"),
            $value = $(this).find("input[name='GiaTri']"),
            value = '',
            choosen = true;

        if ($checkbox != null && $checkbox.length > 0)//--doi voi nhung bao cao co nut checkbox chon tham so dau vao
            choosen = $checkbox.is(':checked');//--kiem tra xem tham so co duoc chon de bao cao hay khong

        if ($value != null && $value.length > 0)
            value = $value.val();
        //neu khong dc chon thi truyen gia tri mac dinh
        if (!choosen) {
            value = '';//--gan gia tri la trang de ben duoi thu tuc bo qua tham so nay
            para.push({
                MaThamSo: $(this).find("input[name='MaThamSo']").val(),
                GiaTri: value
            });
        } else {
            para.push({
                MaThamSo: $(this).find("input[name='MaThamSo']").val(),
                GiaTri: value
            });
        }
    });
    $("#controls .inputParameter").each(function (index) {
        var $this = $(this);
        var region = $this.closest("region");
        var input = region.find("input[type='hidden'][name$='controlName']");
        var parameterName = $(input).attr("ParameterName");
        var webControlId = $(input).attr("WebControlName");
        var value = '';// ? $this.autoNumeric('get') : $this.val();
        if ($this.hasClass('multi-control')) {
            value = $this.getValueOfMultiControl();
        }
        else if ($this.hasClass('number')) {
            value = $this.autoNumeric('get');
        }
        else {
            value = $this.val();
        }
        control.push({
            ParameterName: parameterName,
            WebControlId: webControlId,
            Value: value
        });
        para.push({
            MaThamSo: parameterName,
            GiaTri: value
        });

    });
    var formData = {
        model: {
            BaoCaoId: baoCaoId,
            ReportControlModels: control,
            ReportParameterModels: para,
            PageIndex: page
        }
    };
    var url = common.moduleUrl("BaoCao/BindReport");
    serviceInvoker.post(url, formData,
    {
        success: function (response) {
            if (response.Message && response.Message != '')
                common.showSuccess(response.Message);
            if ($("#contentReport").length > 0)
                $("#contentReport").html(response.Data);
        },
        error: function (response) {
            if (response.responseJSON != null && response.responseJSON.ErrorCode > 0) {
                common.showError(response.responseJSON.Message);
            } else { common.showError('Có lỗi xảy ra'); }
        }
    });
    return false;
}

function bindChart() {
    common.extentReportParams = 'json';
    common.extentReportMethod = function (data) {
        try {
            common.lineChart(data, 'Truc X', ['Y1', 'Y2', 'Y3'], 'Title');
        } catch (e) {
            console.log(e);
        }
    }
    return BindReport();
}

function ShowCarePlus() {
    var url = common.moduleUrl("BaoCao/ShowChamSoc");
    serviceInvoker.get(url, {}, {
        complete: function (response) {
            if (response != null) {
                $('#contentChamSoc').html(response.responseText);
                common.scrollTo('#contentChamSoc');
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}
