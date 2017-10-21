(function ($, appName) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">26 - 08 - 2015.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="appName">namespace of application.</param>
    'use strict';

    var fileDialog;
    var mediaBrowserUrl = '/Explore/FileManager';
    var config = {};
    var typeDisplay = 2;
    var actionFileMode = "";
    var arraysFileManaged = [];
    var arraysFileSelected = [];
    var nodeActived = {};

    function showFileBroswers() {
        var div = $("#div-filemanager");
        if (div.length === 0) {
            div = $("<div id='div-filemanager'/>").appendTo('body');
        }

        div.css("z-index", 10);
        if (config.url !== "" && config.url !== null) {
            mediaBrowserUrl = config.url;
        }
        if (mediaBrowserUrl.indexOf('?') < 0) {
            mediaBrowserUrl += '?fixed=1';
        }
        if (mediaBrowserUrl.indexOf('type') < 0) {
            mediaBrowserUrl += '&type=0';
        }

        serviceInvoker.get(mediaBrowserUrl + '&t=0', {}, {
            success: function (response) {
                div.html(response.Html);

                div.dialog({
                    id: 'dialog-filemanager',
                    width: 960,
                    height: 610,
                    title: 'File broswer',
                    resizable: false,
                    multiDialog: true,
                    model: true,
                    zIndex: 99998,
                    onShow: function () {
                        appName.init();
                    },
                    onClosed: function () {
                        appName.onClosed();
                    }
                });
            },
            error: function (response) {
                if (response.responseJSON && response.responseJSON.ErrorCode > 0) {
                    common.showError(response.responseJSON.Message);
                }
            }
        });

        fileDialog = div;
    }

    function checkDefaultFile() {
        for (var i = 0; i < arraysFileSelected.length; i++) {
            var path = arraysFileSelected[i].trim();
            if (path.indexOf(folder) == 0) {
                var fName = path.substring(folder.length + 1);
                var obj = $("#file_" + fName);
                obj.addClass("moxman-checked");
                obj.data('o-color', obj.css("background-color"));
                obj.css('background-color', 'highlight');
            }
        }
    }

    function manageSelectedFile() {
        arraysFileManaged = [];
        for (var i = 0; i < arraysFileSelected.length; i++)
            arraysFileManaged.push(arraysFileSelected[i]);
    }

    function registerMenu(slt, rclk) {
        $.contextMenu({
            selector: slt,
            callback: function (key, options) {
                switch (key) {
                    case "copy":
                    case "cut":
                        actionFileMode = key;
                        manageSelectedFile();
                        break;
                    case "paste":
                        doActionFile();
                        break;
                    case "rename":
                        actionFileMode = key;
                        break;
                    case "delete":
                        if (confirm('Xoá file')) {
                            actionFileMode = key;
                            manageSelectedFile();
                            doActionFile();
                        }
                        break;
                }
            },
            items: {
                "copy": { name: "Copy", icon: "copy", disabled: function () { return arraysFileSelected.length == 0; } },
                "sep1": "---------",
                "cut": { name: "Cut", icon: "cut", disabled: function () { return arraysFileSelected.length == 0; } },
                "sep2": "---------",
                "paste": { name: "Paste", icon: "paste", disabled: function () { return !checkPaste(); } },
                "sep3": "---------",
                "delete": { name: "Delete", icon: "delete", disabled: function () { return arraysFileSelected.length == 0; } },
                "sep4": "---------",
                "rename": { name: "Rename", disabled: function () { return arraysFileSelected.length == 0; } }
            },
            trigger: rclk ? "right" : "left"
        });
    }

    function checkPaste() {
        if (arraysFileManaged.length === 0) return false;
        return arraysFileManaged[0].indexOf($("#selectedFolderNode").val()) < 0;
    }

    function displayViewType() {
        if (typeDisplay === 2) {
            $("#btnListView").parent().addClass("moxman-active");
            $("#btnImageView").parent().removeClass("moxman-active");
        }
        else {
            $("#btnListView").parent().removeClass("moxman-active");
            $("#btnImageView").parent().addClass("moxman-active");
        }
    }

    function getFilter() {
        var r = $("#txtFilterFile").val().trim();
        if (r.trim() === "Filter") {
            r = "";
        }
        return r;
    }

    function selectFileAllFile(obj) {
        obj = $(obj);
        if (obj.hasClass("moxman-checked")) {
            arraysFileSelected = [];
            obj.removeClass("moxman-checked");
            $("tr[id^='file_']").removeClass("moxman-checked");
            $("tr[id^='file_']").removeAttr('style');
        }
        else {
            arraysFileSelected = [];
            obj.addClass("moxman-checked");
            $("tr[id^='file_']").addClass("moxman-checked");
            $("tr[id^='file_']").css('background-color', 'highlight');
            arraysFileSelected = $("tr[id^='file_']").map(function () {
                var id = $(this).attr('id');
                if (id.length > 5) {
                    return id.substring(5);
                }
                return "";
            }).get();
        }

        if (!config.win) {
            $("#div-manage-file").css("display", arraysFileSelected.length > 0 ? "" : "none");
        }
    }

    function selectFile($item) {
        var fileName = $item.attr('filename');
        var $input = $item.find('input');
        var path = $("#selectedFolderNode").val();
        var fullName = path + "/" + fileName;
        var checked = $input.is(':checked');
        var index;
        if (!checked) {
            index = arraysFileSelected.indexOf(fullName);
            if (index > -1) {
                arraysFileSelected.splice(index, 1);
            }
        }
        else {
            index = arraysFileSelected.indexOf(fullName);
            if (index === -1) {
                arraysFileSelected.push(fullName);
            }
        }
        if (!config.win) {
            $("#div-manage-file").css("display", arraysFileSelected.length > 0 ? "" : "none");
        }
    }

    function registerMenus() {
        registerMenu('#listFile', true);
        registerMenu('#btnmenumanager', false);
    };

    function resetParams() {
        arraysFileSelected = [];
    }

    function loadFolders() {
        $("#treeFolder").fancytree({
            selectMode: 1,
            activeVisible: true,
            init: function (e, data) {
                if (nodeActived.data != null)
                    data.tree.activateKey(nodeActived.key);
                else
                    data.tree.activateKey('/');
            },
            activate: function (event, data) {
                $("#selectedFolderNode").val(data.node.key);
                $("#selectedFolder").html(data.node.key);
                appName.refreshSelectedFolder();
                nodeActived = data.node;
                if (nodeActived.key === '/') {
                    $('#moxman-2-delete').prop('disabled', true);
                } else {
                    $('#moxman-2-delete').prop('disabled', false);
                }
            },
            source: {
                url: common.moduleUrl("Explore/Folder") + "?type=" + config.type
            }
        });
    }

    function doActionFile(callback) {
        var data = [];
        data.push(actionFileMode);
        data.push($("#selectedFolderNode").val());
        for (var i = 0; i < arraysFileManaged.length; i++) {
            data.push(arraysFileManaged[i]);
        }
        var pdata = "";
        for (var j = 0; j < data.length; j++) {
            pdata += data[j] + "*";
        }
        var url = common.moduleUrl("Explore/DoAction") + "?type=" + config.type;
        serviceInvoker.post(url,
            { "stFiles": pdata }, {
                success: function (response) {
                    common.showSuccess(response.Message);
                    if (data[0] === "createfolder") {
                        loadFolders();
                    }
                    else if (data[0] === "deletefolder") {
                        if (nodeActived.data !== "") {
                            var tempNode = nodeActived.parent;
                            nodeActived.remove();
                            nodeActived = tempNode;
                            loadFolders();
                        }
                        appName.refreshSelectedFolder();
                    }
                    else {
                        appName.refreshSelectedFolder();
                    }
                    actionFileMode = "";
                },
                error: function (response) {
                    if (response.responseJSON && response.responseJSON.ErrorCode > 0) {
                        common.showError(response.responseJSON.Message);
                    }
                },
                complete: function () {
                    if (typeof callback == "function") {
                        callback();
                    }
                }
            });
    }

    function showUpload() {
        var url = common.moduleUrl("Explore/UploadForm") + '?type=' + config.type + '&path=' + encodeURI($("#selectedFolderNode").val());
        serviceInvoker.get(url, {}, {
            complete: function (response) {
                common.showDialog({
                    id: "dialog-showuploadform",
                    title: "Tải file",
                    resize: "no",
                    modal: true,
                    multiDialog: true,
                    zIndex: 99999,
                    width: "300",
                    height: "auto",
                    onShow: function() {
                        $("#fm-fileupload").change(function() {
                            $(this).siblings('#fm-fileupload-input').val(this.value || '');
                        });
                    },
                    onClosed: function() {
                        appName.refreshSelectedFolder();
                    },
                    content: response.responseText
                });
            }
        });

    }

    function createFolder() {
        var url = common.moduleUrl("Explore/CreateFolder");
        serviceInvoker.get(url, {}, {
            success: function (response) {
                $("#dialogFolderName").html(response.Html);
                $("#dialogFolderName").dialog({
                    id: "dialog-taothumuc",
                    title: 'Tạo thư mục',
                    resize: "no",
                    modal: true,
                    zIndex: 99999,
                    multiDialog: true,
                    width: "400",
                    height: "auto"
                });
            },
            error: function (response) {
                if (response.responseJSON && response.responseJSON.ErrorCode > 0) {
                    common.showError(response.responseJSON.Message);
                }
            },
        });
    }

    function deleteFolder() {
        if (confirm('Xoá thư mục, bao gồm các thư mục con và files?')) {
            arraysFileManaged = [];
            arraysFileManaged.push($("#selectedFolderNode").val());
            actionFileMode = "deletefolder";
            doActionFile();
        }
    }

    appName.insertSelectedFile = appName.insertSelectedFile || function () {
        if (arraysFileSelected.length > 0) {
            var strArr = [];
            for (var i = 0; i < arraysFileSelected.length; i++) {
                strArr.push($("#rootFolder").val() + arraysFileSelected[i]);
            }
            if (typeof config.fieldName == "function") {
                config.fieldName(strArr);
            } else {
                var $control = $('#' + config.fieldName);
                if ($control.length === 0) {
                    throw config.fieldName + " were not found!";
                }
                $control.val(strArr.join(","));
            }
            appName.closeFileManager();
        }
    }

    appName.fileChoose = appName.fileChoose || function () {
        $("#fm-fileupload").click();
        return false;
    }

    appName.upload = appName.upload || function () {
        var formData = new FormData();
        var totalFiles = document.getElementById("fm-fileupload").files.length;
        if (totalFiles <= 0) {
            common.showError("Hãy chọn file template để upload");
            return false;
        }
        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("fm-fileupload").files[i];
            formData.append("FileUpload", file);
        }
        var url = common.moduleUrl("/Explore/UploadFile") + "?folder=" + encodeURI($("#selectedFolderNode").val());
        serviceInvoker.upload(url, formData, {
            error: function (response) {
                if (response.responseJSON && response.responseJSON.ErrorCode > 0) {
                    common.showError(response.responseJSON.Message);
                }
                else {
                    common.showError('Lỗi tải file');
                }
            },
            success: function (response) {
                common.showSuccess(response.Message);
                aifinder.refreshSelectedFolder();
                $.Dialog2('dialog-showuploadform').close();
            }
        });

        return false;
    }

    appName.refreshSelectedFolder = appName.refreshSelectedFolder || function () {
        var folder = $("#selectedFolderNode").val();
        var url = common.moduleUrl("Explore/ListFile") + "?type=" + config.type + "&Folder=" + encodeURI(folder) + "&display=" + typeDisplay + "&filter=" + getFilter();
        serviceInvoker.get(url, {}, {
            success: function (response) {
                $("#listFile").html(response.Html);
            },
            error: function (response) {
                if (response.responseJSON && response.responseJSON.ErrorCode > 0) {
                    common.showError(response.responseJSON.Message);
                }
            },
            commplete: function (response) {
                arraysFileSelected = [];
                checkDefaultFile();
            }
        });
    }

    appName.saveCreateFolder = appName.saveCreateFolder || function () {
        arraysFileManaged = [];
        arraysFileManaged.push($("#txtFolderName").val());
        actionFileMode = "createfolder";
        doActionFile(function () {
            $.Dialog2("dialog-taothumuc").close();
        });
    }

    appName.closeFileManager = appName.closeFileManager || function () {
        //remove menus has defined
        //$('.context-menu-list.context-menu-root').remove();
        $.Dialog2("dialog-filemanager").close();
    }

    function tileClick($this) {
        var $input = $this.find('input');
        if (!$this.hasClass('selected')) {
            $this.addClass('selected');
            $input.prop('checked', true)
        } else {
            $this.removeClass('selected');
            $input.prop('checked', false)
        }
    }

    function itemClick($this) {
        var $input = $this.find('input');
        if (!$this.hasClass('selected')) {
            $this.addClass('selected');
            $input.prop('checked', true)
        } else {
            $this.removeClass('selected');
            $input.prop('checked', false)
        }
    }

    appName.init = appName.init || function () {
        resetParams();
        loadFolders();
        $("#btnListView").on("click", function () {
            typeDisplay = 2;
            displayViewType();
            appName.refreshSelectedFolder();
        });
        $('#moxman-2-open').on('click', function (e) {
            createFolder();
            e.preventDefault();
        });
        $('#moxman-2-delete').on('click', function (e) {
            deleteFolder();
            e.preventDefault();
        });
        $('#moxman-2-upload').on('click', function (e) {
            showUpload();
            e.preventDefault();
        });


        $("#listFile").on("click", '.row-item', function () {
            var $this = $(this);
            if ($this.is('input')) {
                $this = $this.closest('.row-item');
            }

            if ($this.hasClass('tile')) {
                //tile click
                tileClick($this);
            } else {
                itemClick($this);
            }
            if (!config.multiSelection) {//neu chi cho single check
                //loai bo cac check box khac ve trang thai unchecked
                $("#listFile .row-item").each(function () {
                    var $item = $(this);
                    if ($item.attr('id') != $this.attr('id')) {
                        var $input = $item.find('input');
                        $item.removeClass("selected");
                        $input.prop('checked', false)
                    }
                });
            }
            selectFile($this);
        });

        $("#btnImageView").on("click", function () {
            typeDisplay = 1;
            displayViewType();
            appName.refreshSelectedFolder();
        });
        $("#moxman-7").on("click", function () {
            appName.refreshSelectedFolder();
        });
        $("#txtFilterFile").on("blur", function () {
            if ($("#txtFilterFile").val().trim() == "")
                $("#txtFilterFile").val("Filter");
        });
        $("#txtFilterFile").on("focusin", function () {
            if ($("#txtFilterFile").val().trim() == "Filter")
                $("#txtFilterFile").val("");
        });

        //registerMenus();
        displayViewType();
    }

    appName.onClosed = appName.onClosed || function () {

    }

    appName.show = function (params) {
        config = $.extend({
            fieldName: null,
            url: null,
            type: null,
            win: null,
            multiSelection: false
        }, params);
        if (config.win) {
            appName.init();
        }
        else {
            showFileBroswers();
        }
    };

    $(document).ready(function () {
        //apply aifinder to ckeditor
        $(document).on('click', '.cke_dialog_ui_hbox_last a.cke_dialog_ui_button:first', function () {
            var $a = $(this);
            var $tr = $a.closest('tr.cke_dialog_ui_hbox'),
                $textTd = $tr.find('td.cke_dialog_ui_hbox_first'),
                $text = $textTd.find('input.cke_dialog_ui_input_text');
            if ($a.text() !== 'Duyệt máy chủ') {
                return false;
            }
            common.showUploader(function (imgs) {
                $text.focus();
                var url = common.moduleUrl(imgs[0]);
                $text.val(url);
            });
        });
    });

}(jQuery, window.aifinder = window.aifinder || {}));
