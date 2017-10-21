(function ($, appName) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">26 - 08 - 2015.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="appName">namespace of application.</param>
    'use strict';

    common.extentReportParams = null;
    common.extFunc = {
        onSuccess : function(){},
    };
    common.debugLevel = 0;

    $(document).ready(function () {
        common.resizePage();
        if ($("table.resizable").length > 0) {
            $("table.resizable").colResizable();
        }
        common.applyTextboxNumber();
        $(window).resize(function () {
            common.resizePage();
            common.autoFixLoadingPosition();
        });
    });


    common.callPrint = common.callPrint = function (strid, imgPrint) {
        if (imgPrint) {
            $("#" + strid).print2();
        } else {
            $("#" + strid).print();
        }
        return false;
    }


    common.applyCodeMirror = common.applyCodeMirror || function (id, modeName, option) {
        window['codeMirror-' + id] = CodeMirror.fromTextArea(document.getElementById(id), {
            lineNumbers: true,
            matchBrackets: true,
            autoCloseBrackets: true,
            styleActiveLine: true,
            lineWrapping: true,
            gutters: ["CodeMirror-lint-markers"],
            lint: true,
            extraKeys: {
                "Ctrl-Space": "autocomplete",
                "F11": function (cm) {
                    cm.setOption("fullScreen", !cm.getOption("fullScreen"));
                },
                "Esc": function (cm) {
                    if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
                }
            },
            mode: { name: modeName || "text/html", globalVars: true }
        });
        return window['codeMirror-' + id];
    }

    common.applyCodeMirrorSql = common.applyCodeMirrorSql || function (id, modeName, tableArray) {
        window['codeMirror-' + id] = CodeMirror.fromTextArea(document.getElementById(id), {
            lineNumbers: true,
            matchBrackets: true,
            autoCloseBrackets: true,
            styleActiveLine: true,
            lineWrapping: true,
            gutters: ["CodeMirror-lint-markers"],
            lint: true,
            extraKeys: {
                "Ctrl-Space": "autocomplete",
                "F11": function (cm) {
                    cm.setOption("fullScreen", !cm.getOption("fullScreen"));
                },
                "Esc": function (cm) {
                    if (cm.getOption("fullScreen")) cm.setOption("fullScreen", false);
                }
            },
            mode: { name: modeName || "text/x-sql", globalVars: true },
            hint: CodeMirror.hint.sql,
            hintOptions: {
                tables: tableArray
            }
        });
        return window['codeMirror-' + id];
    }

    common.codeMirror = common.codeMirror || function (id) {
        return window['codeMirror-' + id];
    }

    common.applyCheckbox = function (tableId) {
        var checkboxes = $('#' + tableId + ' ' + '.singleCheckBox');
        var checkboxAll = $('#' + tableId + ' ' + '#allCheckBox');

        checkboxAll.on('click', function () {
            var isChecked = $(this).is(':checked'),
            $span = checkboxes.parent();
            checkboxes.prop('checked', isChecked);
            if (isChecked) {
                $span.addClass('checked');
            } else {
                $span.removeClass('checked');
            }
            checkboxes.closest('tr').addClass(isChecked ? 'selected-row' : 'not-selected-row');
            checkboxes.closest('tr').removeClass(isChecked ? 'not-selected-row' : 'selected-row');
        });

        checkboxes.on('click', function () {
            var isChecked = $(this).is(':checked');
            $(this).closest('tr').addClass(isChecked ? 'selected-row' : 'not-selected-row');
            $(this).closest('tr').removeClass(isChecked ? 'not-selected-row' : 'selected-row');
            var selectedrows = 0;
            checkboxes.each(function () {
                if ($(this).closest('tr').hasClass('selected-row')) {
                    selectedrows++;
                }
            });
            if (isChecked && checkboxes.length === selectedrows) {
                checkboxAll.prop('checked', true);
                var $span = checkboxes.parent();
                $span.addClass('checked');
            } else {
                checkboxAll.prop('checked', false);
                var $span = checkboxes.parent();
                $span.removeClass('checked');
            }

        });
    }

    common.autoFixLoadingPosition = common.autoFixLoadingPosition || function () {
        var winH = $(window).height(),
            winW = $(window).width(),
            $winring = $('#flatwindow').find('.win-ring'),
            winringH = $winring.outerHeight(),
            winringW = $winring.outerHeight();
        $winring.css('top', (winH - winringH) / 2);
        $winring.css('left', (winW - winringW) / 2);
    }

    common.showUploader = common.showUploader || function (fname, multiSelection) {
        aifinder.show({
            fieldName: fname,
            url: common.moduleUrl("Explore/FileManager"),
            type: 1,
            win: false,
            multiSelection: multiSelection || false
        });
        return false;
    }

    common.redirect = common.redirect || function (url) {
        common.showWaiting(true);
        window.setTimeout(function () {
            location.href = url;
            common.showWaiting(false);
        }, 2000);
        return false;
    }

    common.resizePage = function () {
        var w = $(window).height();
        var banner = $("#banner").height();
        var footer = $("#footer").height();
        var mainmenu = $("#mainmenu").height();
        var bodyH = w - banner - footer - mainmenu - 80 - 9;
        $(".content").each(function () {
            var $this = $(this);
            var $parent = $this.parent();
            if ($parent.hasClass('window')) {
                var childs = $(this).children();
                var bodycontent = 0;
                if (childs.length >= 2) {
                    for (var i = 0; i < childs.length; i++) {
                        bodycontent += $(childs[i]).height();
                    }
                } else {
                    bodycontent = $(childs[0]).height();
                }

                if (bodycontent < bodyH) {
                    $this.css("min-height", bodyH + "px");
                } else {
                    $this.css("height", "auto");
                }
            }
        });
    }

    common.applyTextboxNumber = function () {
        $(".number").each(function (index) {
            var $this = $(this);
            var currency = 'f-currency';
            var group = 'f-group';
            var dft = 'f-default';
            if (typeof $this.autoNumeric == "function") {
                if ($this.hasClass(currency)) {
                    $this.autoNumeric('init', { aSign: ' đ', pSign: 's', vMin: '0', aSep: '.', aDec: ',' });
                }
                else if ($this.hasClass(group)) {
                    $this.autoNumeric('init', { vMin: '0', mDec: '0', aSep: '.', aDec: ',' });
                }
                else if ($this.hasClass(dft)) {
                    $this.autoNumeric('init', { vMin: '0', aSep: '.', aDec: ',' });
                } else {
                    $this.keypress(function (e) {
                        //if the letter is not digit then display error and don't type anything
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            //display error message
                            return false;
                        }
                    });
                }
            } else {
                $this.keypress(function (e) {
                    //if the letter is not digit then display error and don't type anything
                    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                        //display error message
                        return false;
                    }
                });
            }

        });

    }

    common.applyViewSoThueBao = function () {
        $('#contentReport').on('click', '.sothuebao', function () {
            var $this = $(this),
                soThueBao = $this.attr('stb') || $.trim($this.text()),
                loaiThueBao = $this.attr('ltb');
            if (soThueBao == null || soThueBao == '') {
                return false;
            }
            if (loaiThueBao == null) {
                return false;
            }
            return common.showInfoThueBao(soThueBao, loaiThueBao);
        });
    }

    common.showSuccess = function (content, caption, timeout) {
        if (!content) {
            content = "Nội dung: ";
        } else {
            content = "Nội dung: " + content;
        }
        if (!caption) {
            caption = "Thông báo";
        }
        var not = $.Notify({
            caption: caption,
            content: content,
            style: { background: '#60a917', color: '#ffffff' },
            timeout: timeout || 10000 // 10 seconds
        });
    }

    common.showError = function (content, caption, callback) {
        if (!content) {
            content = "Nội dung: ";
        } else {
            content = "Nội dung: " + content;
        }
        if (!caption) {
            caption = "Thông báo lỗi";
        }
        var not = $.Notify({
            caption: caption,
            content: content,
            style: { background: 'red', color: 'white' },
            timeout: 10000 // 10 seconds
        });
        if (typeof callback == 'function') {
            window.setTimeout(function () {
                callback();
            }, 5000);
        }
    }

    common.showWarning = function (content, caption) {
        if (!content) {
            content = "Nội dung: ";
        } else {
            content = "Nội dung: " + content;
        }
        if (!caption) {
            caption = "Cảnh báo";
        }
        var not = $.Notify({
            caption: caption,
            content: content,
            style: { background: 'yellow', color: 'red' },
            timeout: 10000 // 10 seconds
        });
    }

    common.showWaiting = function (isshow) {
        var waitFormStr = '<div id="flatwindow" style="display:none; opacity:0.5;z-index:1050 ;background-color: rgba(0, 0, 0, 0.7);position:fixed;top:0;bottom:0;left:0;right:0">' +
                        '   <div id="progess" class="progress progress-indeterminate" style="z-index:1050">' +
                        '       <div class="win-ring "></div>' +
                        '   </div>' +
                        '</div>';
        var $waitForm = $('#flatwindow');
        if ($waitForm.length == 0) {
            $waitForm = $(waitFormStr).appendTo('body');
            common.autoFixLoadingPosition();
        }
        if (isshow) {
            $waitForm.show();
        } else {
            $waitForm.hide();
        }
    }

    common.showDialog = function (config) {
        var dialogStr = '<div id="customDialog"></div>';
        var $dialogForm = $('#customDialog');
        if ($dialogForm.length == 0) {
            $dialogForm = $(dialogStr).appendTo('body');
        }
        $dialogForm.html(config.content);
        config.content = '';
        $dialogForm.dialog(config);
    }

    common.alert = function (content, caption) {
        common.showSuccess(content, caption);
    }

    common.closeDialog = function () {
        if ($.Dialog != null) {
            if (typeof $.Dialog == "function") {
                $.Dialog().close();
            } else {
                $.Dialog.close();
            }
        }
        return false;
    }

    common.colorpicker = common.colorpicker || function (element) {
        $('.sp-container').remove();

        $(element).spectrum({
            color: "#ECC",
            flat: false,
            showInput: true,
            showInitial: true,
            showPalette: true,
            maxPaletteSize: 10,
            preferredFormat: "hex",
            className: "full-spectrum",
            localStorageKey: "spectrum.example",
            showSelectionPalette: true,
            palette: [
                ["rgb(0, 0, 0)", "rgb(67, 67, 67)", "rgb(102, 102, 102)",
                "rgb(204, 204, 204)", "rgb(217, 217, 217)", "rgb(255, 255, 255)"],
                ["rgb(152, 0, 0)", "rgb(255, 0, 0)", "rgb(255, 153, 0)", "rgb(255, 255, 0)", "rgb(0, 255, 0)",
                "rgb(0, 255, 255)", "rgb(74, 134, 232)", "rgb(0, 0, 255)", "rgb(153, 0, 255)", "rgb(255, 0, 255)"],
                ["rgb(230, 184, 175)", "rgb(244, 204, 204)", "rgb(252, 229, 205)", "rgb(255, 242, 204)", "rgb(217, 234, 211)",
                "rgb(208, 224, 227)", "rgb(201, 218, 248)", "rgb(207, 226, 243)", "rgb(217, 210, 233)", "rgb(234, 209, 220)",
                "rgb(221, 126, 107)", "rgb(234, 153, 153)", "rgb(249, 203, 156)", "rgb(255, 229, 153)", "rgb(182, 215, 168)",
                "rgb(162, 196, 201)", "rgb(164, 194, 244)", "rgb(159, 197, 232)", "rgb(180, 167, 214)", "rgb(213, 166, 189)",
                "rgb(204, 65, 37)", "rgb(224, 102, 102)", "rgb(246, 178, 107)", "rgb(255, 217, 102)", "rgb(147, 196, 125)",
                "rgb(118, 165, 175)", "rgb(109, 158, 235)", "rgb(111, 168, 220)", "rgb(142, 124, 195)", "rgb(194, 123, 160)",
                "rgb(166, 28, 0)", "rgb(204, 0, 0)", "rgb(230, 145, 56)", "rgb(241, 194, 50)", "rgb(106, 168, 79)",
                "rgb(69, 129, 142)", "rgb(60, 120, 216)", "rgb(61, 133, 198)", "rgb(103, 78, 167)", "rgb(166, 77, 121)",
                /*"rgb(133, 32, 12)", "rgb(153, 0, 0)", "rgb(180, 95, 6)", "rgb(191, 144, 0)", "rgb(56, 118, 29)",
                "rgb(19, 79, 92)", "rgb(17, 85, 204)", "rgb(11, 83, 148)", "rgb(53, 28, 117)", "rgb(116, 27, 71)",*/
                "rgb(91, 15, 0)", "rgb(102, 0, 0)", "rgb(120, 63, 4)", "rgb(127, 96, 0)", "rgb(39, 78, 19)",
                "rgb(12, 52, 61)", "rgb(28, 69, 135)", "rgb(7, 55, 99)", "rgb(32, 18, 77)", "rgb(76, 17, 48)"]
            ],
            cancelText: "Huỷ",
            chooseText: "Chọn",
            clearText: "Xoá"
        });
    }

    common.resizeDialog = function () {
        if ($.Dialog != null) {
            $.Dialog.autoResize();
        }
    }

    common.applyPreview = function (sender, idContent) {

        if (typeof $(sender).tooltipster != "function")
            return;
        $(sender).tooltipster({
            content: 'loading...',
            trigger: 'click',
            theme: 'metro tooltipster-default',
            contentAsHTML: true,
            functionBefore: function (origin, continueTooltip) {
                // we'll make this function asynchronous and allow the tooltip to go ahead and show the loading notification while fetching our data
                continueTooltip();
                var str = '';
                var obj = common.codeMirror(idContent);
                if (obj != null) {
                    str = obj.getValue();
                } else {
                    if (idContent[0] != '#') {
                        idContent = '#' + idContent;
                    }
                    str = $(idContent).val();
                }

                str = str.replace(/#=([\w\d\(\)\:\,\.\{\}]*)#/g, "Data Bound");
                var data = "<div class='window'><div class='content'>" + str + "</div></div>";
                origin.tooltipster('content', data);
            }
        });
        $(window).keypress(function () {
            $(sender).tooltipster('hide');
        });
        $(document).mouseup(function (e) {
            var container = $(sender);
            if (!container.is(e.target)
                && container.has(e.target).length === 0) {
                $(sender).tooltipster('hide');
            }
        });
    }

    common.callBindReport = function (self) {
        if (typeof window.BindReport == "undefined") {
            common.showError('Hàm BindReport chưa được định nghĩa', 'Thông báo lỗi');
            return false;
        }
        try {
            window.BindReport();
            //thuc hien show cac button an (chamsoc, xuat file...)
            var $navbtn = $(self).closest('div.nav_btn');
            if ($navbtn.length > 0) {
                var $btnhides = $navbtn.find('button.hide');
                if ($btnhides.length > 0) {
                    $btnhides.removeClass('hide');
                }
            }
        } catch (ex) {
            console.log(ex);
            common.showError(ex, 'Thông báo lỗi');
        }
        return false;
    }

    common.callBindChart = function (self) {
        if (typeof window.BindChart == "undefined") {
            common.showError('Hàm BindChart chưa được định nghĩa', 'Thông báo lỗi');
            return false;
        }
        try {
            window.BindChart();
        } catch (ex) {
            console.log(ex);
            common.showError(ex, 'Thông báo lỗi');
        }
        return false;
    }

    common.callCarePlus = function (self) {
        var $careRegion = $('#div-care-plus');
        if ($careRegion.length > 0) {
            $careRegion.show();
            common.scrollTo($careRegion);
        }

        return false;
    }

    common.moduleUrl = function (url) {
        if (url == '') {
            throw 'url is empty';
        }
        if (url[0] == '~') {
            url = url.substring(1);
        }
        if (url[0] == '/') {
            url = url.substring(1);
        }
        return common.baseUrl + url;
    }

    common.folderUrl = function (url) {
        if (url == null) {
            url = window.location.href;
        }
        var re = new RegExp(/^.*\//);
        return re.exec(url);
    }

    $.ajaxPrefilter(function (options, originalOptions, jqXhr) {
        //thuc hien de chan ham bindreport mac dinh => muc dich tan dung ham nay de lay du lieu cho cac nghiep vu khac 
        // ve bieu do, xuat file, gui du lieu outbound... etc
        if (options.url.indexOf('BindReport') > -1) {
            var data = {}
            if (options.data != null) {
                data = $.parseJSON(options.data);
            }
            if (data.model == null) {
                data.model = {};
            }
            //bind sort column
            if (common.sortModel != null) {
                data.model.SortModel = common.sortModel;
            }

            if (common.extentReportParams != null) {
                if (options.url.indexOf('?') > -1)
                    options.url += '&extentReportParams=' + common.extentReportParams;
                else
                    options.url += '?extentReportParams=' + common.extentReportParams;

                if (common.carePlusModel != null) {
                    data.model.CarePlusModel = common.carePlusModel;
                }

                if (common.exportFileModel != null) {
                    data.model.ExportFileModel = common.exportFileModel;
                }

                if (typeof common.extentReportMethod != 'undefined') {
                    if (typeof common.extentReportMethod == "function") {//phuc vu nghiep vu ve bieu do nen can giu nguyen 
                        options.success = function (response) {
                            try {
                                if (response.Data != null) {
                                    common.extentReportMethod(response.Data);
                                }
                            } catch (e) {
                                console.log(e);
                            }
                        }

                        options.originalcomplete = options.complete;

                        options.complete = function (response) {
                            try {
                                common.extentReportParams = null;
                                common.carePlusModel = null;
                                common.extentReportMethod = null;
                                //call originalcomplete
                                options.originalcomplete();
                            } catch (e) {
                                console.log(e);
                            }
                        }
                    }
                    else if (typeof common.extentReportMethod == "object") {//call override method if defined
                        if (typeof common.extentReportMethod.success == 'function') {
                            options.success = common.extentReportMethod.success;
                        }
                        if (typeof common.extentReportMethod.error == 'function') {
                            options.originalerror = options.error;
                            options.overrideerror = common.extentReportMethod.error;

                            options.error = function (response) {
                                try {
                                    //call originalerror
                                    options.originalerror(response);
                                    //call overrideerror
                                    options.overrideerror(response);
                                } catch (e) {
                                    console.log(e);
                                }
                            }
                        }

                        //complete
                        options.originalcomplete = options.complete;
                        options.overridecomplete = common.extentReportMethod.complete;

                        options.complete = function (response) {
                            try {
                                common.extentReportParams = null;
                                common.carePlusModel = null;
                                common.extentReportMethod = null;
                                //call originalcomplete
                                options.originalcomplete(response);
                                //call extentcomplete
                                if (typeof options.overridecomplete == 'function') {
                                    options.overridecomplete(response);
                                }
                            } catch (e) {
                                console.log(e);
                            }
                        }

                    }
                } else {//default
                    options.originalcomplete = options.complete;

                    options.complete = function (response) {
                        try {
                            common.extentReportParams = null;
                            //call originalcomplete
                            options.originalcomplete();
                        } catch (e) {
                            console.log(e);
                        }
                    }
                }
            }

            //compile data
            options.data = JSON.stringify(data);
        }
    });

    common.formatCurrency = common.formatCurrency || function (value, scale, unit) {
        if (!$.isNumeric(value))
            return value;

        value = value + '';
        unit = unit || '';
        scale = scale || 0;

        var n = value.split('.');
        var n1 = n[0].split('').reverse().join("");
        var n11 = n1.replace(/\d\d\d(?!$)/g, "$&,");
        var n2 = n.length == 2 ? n[1] : '';
        var n22 = scale > 0 && n2.length > 0 ? '.' + n2.substring(0, scale) : '';
        return n11.split('').reverse().join('') + n22 + ' ' + unit;
    }

    common.removeSignVn = common.removeSignVn || function (inputStr, removeSpace) {
        try {
            inputStr = inputStr.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
            inputStr = inputStr.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
            inputStr = inputStr.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            inputStr = inputStr.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
            inputStr = inputStr.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            inputStr = inputStr.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
            inputStr = inputStr.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
            inputStr = inputStr.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
            inputStr = inputStr.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            inputStr = inputStr.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
            inputStr = inputStr.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            inputStr = inputStr.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
            inputStr = inputStr.replace(/đ/g, "d");
            inputStr = inputStr.replace(/Đ/g, "D");

            if (removeSpace) {
                inputStr = inputStr.replace(/ /g, "");
            }
        } catch (e) {
            console.log(e);
        }
        return inputStr;
    }

    common.scrollTo = common.scrollTo || function (id) {
        if (id == null)
            return;

        if (typeof id == "object") {

        }
        else if (typeof id == "string") {
            if (id[0] != '#') {
                id = '#' + id;
            }
        }
        $('html, body').animate({
            scrollTop: $(id).offset().top
        }, 1000);
    }

    common.FileType = {
        Excel: 0,
        Word: 1
    }
    common.FunctionTypeReport = {
        Json: 'json',
        Care: 'care',
        Sort: 'sort',
        Export: 'export'
    }

    common.getFullName = common.getFileName || function (path) {
        var index = path.lastIndexOf('/');
        if (index >= 0) {
            if (path.length >= index + 1) {
                return path.substr(index + 1);
            }
        }
        return "";
    }

    common.getFileName = common.getFileName || function (path) {
        var index = path.lastIndexOf('/');
        if (index >= 0) {
            if (path.length >= index + 1) {
                var fullname = path.substr(index + 1);
                index = fullname.indexOf('.');
                return fullname.substr(0, index);
            }
        }
        return "";
    }

    common.getFileExt = common.getFileExt || function (path) {
        var index = path.lastIndexOf('/');
        if (index >= 0) {
            if (path.length >= index + 1) {
                var fullname = path.substr(index + 1);
                index = fullname.indexOf('.');
                return fullname.substr(index + 1);
            }
        }
        return "";
    }

}(jQuery, window.common = window.common || {}));
