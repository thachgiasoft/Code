window.layoutUtil = window.layoutUtil || {};
layoutUtil.divResizables = [];
layoutUtil.trSortables = [];
layoutUtil.isEnabled = false;
layoutUtil.url = '';

$(document).ready(function () {
    layoutUtil.getRegion();
    layoutUtil.isEnabled = layoutUtil.divResizables.length > 0;
    layoutUtil.url = $('#tb-layout-parameter').attr('url');
    $('#layout-edit-button').show();
    $('#layout-add-button, #layout-save-button, #layout-cancel-button').hide();
    layoutUtil.addTriggerShowHideControl();
});

layoutUtil.editLayout = function (evt) {
    if (!layoutUtil.isEnabled) {
        return false;
    }
    layoutUtil.preEditable();
    return false;
}

layoutUtil.cancelEditLayout = function (evt) {
    if (!layoutUtil.isEnabled) {
        return false;
    }
    $('.div-row-resizable').each(function (i) {
        var $this = $(this);

        $this.find('.resizable').each(function (j) {
            $(this).removeClass('resizing cursor-move');
            $(this).resizable('destroy');
            var $label = $(this).find('div.div-control-caption');
            $label.removeClass('resizing');
            $label.resizable('destroy');
        });

        $this.removeClass('div-row-resizable-editing');
        $this.sortable('destroy');
    });

    $('#layout-edit-button').show();
    $('#layout-add-button, #layout-save-button, #layout-cancel-button').hide();

    return false;
}

layoutUtil.addRow = function (evt) {
    var $parent = $('.div-row-resizable').parent();
    var $newElement = '<ul class="div-row-resizable div-row-resizable-editing">&nbsp;</ul>';
    $parent.append($newElement);

    var $last = $('.div-row-resizable').last();
    $last.sortable({
        opacity: 0.6,
        connectWith: ".div-row-resizable",
        cursor: 'move',
        update: function () {
        }
    });
    return false;
}

layoutUtil.preEditable = function () {
    $('.div-row-resizable').each(function (i) {
        var $this = $(this);
        $this.addClass('div-row-resizable-editing');

        $this.sortable({
            opacity: 0.6,
            connectWith: ".div-row-resizable",
            cursor: 'move',
            update: function () {
            }
        });
        $this.find('.resizable').each(function (j) {
            $(this).addClass('resizing cursor-move');
            $(this).resizable();
            var $label = $(this).find('div.div-control-caption');
            $label.addClass('resizing');
            $label.resizable();
        });
    });

    $('.ts-container-sortable').each(function (i) {
        $(this).sortable({
            items: '.ts-sortable',
            opacity: 0.6,
            connectWith: ".ts-container-sortable",
            cursor: 'move',
            start: function (e, ui) {
                layoutUtil.trSortables = $(this).sortable("toArray");
            },
            update: function (e, ui) {
                var element = $(ui.item[0]);
                var parentId = element.attr('parentid');
                var id = element.attr('id');
                var newArray = $(this).sortable("toArray");
                var newIndex = newArray.indexOf(id);
                //replace element
                var idReplaceElement = layoutUtil.trSortables[newIndex];
                var oldElement = $(this).children("#" + idReplaceElement);
                var parentIdReplaceElement = oldElement.attr('parentid');
                if (parentId != parentIdReplaceElement) {
                    $(this).sortable('cancel');
                    layoutUtil.trSortables = [];
                } else {
                    layoutUtil.trSortables = newArray;
                }
            }
        });
        $(this).find('.ts-sortable').each(function (j) {
            $(this).addClass('cursor-move');
        });
    });
    $('#layout-edit-button').hide();
    $('#layout-add-button, #layout-save-button, #layout-cancel-button').show();

    return false;
};

layoutUtil.getRegion = function () {
    $('.div-row-resizable').each(function (i) {
        var $this = $(this);
        $this.addClass('div-row-resizable-' + i);
        var divs = $this.find('.resizable');
        if (divs.length > 0) {
            $(divs).each(function (j) {
                layoutUtil.divResizables.push(this);
            });
        }
    });
}

layoutUtil.addTriggerShowHideControl = function () {
    //Add custom handler on show event and print message
    $('.inputParameter').on('show', function () {
        $(this).closest('li.webcontrol').show();
    });

    //Add custom handler on hide event and print message
    $('.inputParameter').on('hide', function () {
        $(this).closest('li.webcontrol').hide();
    });
}

layoutUtil.saveLayout = function (evt) {
    if (!layoutUtil.isEnabled) {
        return false;
    }

    var baoCaoParameterList = [];
    var thamSoBaoCaoList = [];
    var rowindex = 0;

    $('.div-row-resizable').each(function (i) {
        rowindex++;
        var colindex = 0;
        var divs = $(this).find('li.resizable');
        if (divs.length > 0) {
            for (var j = 0; j < divs.length; j++) {
                var $control = $(divs[j]),
                    $caption = $control.find('div.div-control-caption'),
                    $webcontrol = $control.find('region.input-control');
                colindex++;
                baoCaoParameterList.push({
                    Id: parseInt($control.attr('baocaoparameterid')),
                    BaoCaoId: parseInt($control.attr('baocaoid')),
                    Control_ID: parseInt($control.attr('webcontrolid')),
                    CssStyle: $control.attr('style'),
                    CssCaption: $caption.attr('style'),
                    CssControl: $webcontrol.attr('style'),
                    RowIndex: rowindex,
                    DisplayIndex: colindex
                });
            }
        }
    });
    for (var i = 0; i < layoutUtil.trSortables.length; i++) {
        var id = layoutUtil.trSortables[i];
        var index = i + 1;
        var element = $('.ts-container-sortable').children('#' + id);
        thamSoBaoCaoList.push({
            Id: parseInt(id),
            BaoCaoId: parseInt(element.attr('baocaoid')),
            ThamSoId: parseInt(element.attr('thamsoid')),
            ThuTu: index
        });
    }
    var data = {
        baoCaoParameterList: baoCaoParameterList,
        thamSoBaoCaoList: thamSoBaoCaoList
    }

    serviceInvoker.post(layoutUtil.url, data, {
        success: function (response) {
            common.showSuccess(response.Message);
            //location.reload();
        },
        error: function (response) {
            if (response.responseJSON && response.responseJSON.ErrorCode > 0) {
                common.showError(response.responseJSON.Message);
            } else {
                common.showError('Có lỗi xảy ra, liên hệ với quản trị viên")');
            }
        }
    });
    return false;
}
