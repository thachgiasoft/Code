function formattedDate(date) {
    var d = new Date(date || Date.now()),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('/');
}
function getMapColor(dataColor, value) {
    var color = "#CCCCCC";
    for (var i = 0; i < dataColor.length; i++) {
        var min = dataColor[i].MIN;
        var max = dataColor[i].MAX;
        if (min != null && min != undefined && min != '') {
            if (max != null && max != undefined && max != '') {
                if (value >= min && value <= max) {
                    color = dataColor[i].COLOR;
                    break;
                }
            } else {
                if (value >= min) {
                    color = dataColor[i].COLOR;
                    break;
                }
            }
        } else {
            if (max != null && max != undefined && max != '') {
                if (value <= max) {
                    color = dataColor[i].COLOR;
                    break;
                }
            }
        }
    }
    return color;
}
function createNoteColor(dataColor) {
    var str = "<table>";
    for (var i = 0; i < dataColor.length; i++) {
        str += "<tr>";
        str += "  <td class=\"tr-cl\">";
        str += "          <div class=\"note-color\"  style=\"background-color: " + dataColor[i].COLOR + "\"></div>";
        str += "  </td>";
        str += "    <td class=\"text-left\">" + dataColor[i].GHICHU + "</td>";
        str += "  </tr>";
    }
    str += "</table>";
    return str;

}
function hienThiThongBao(msg, t) {
    if(t)
        common.showSuccess(msg);
    else if(!t)
        common.showError(msg);
    else
        common.showWarning(msg);
}
function setDataYearDropDown(id, count) {
    if (count == undefined)
        count= 5
    var dNow = new Date();
    var year = dNow.getFullYear();
    var select = document.getElementById(id);
    select.innerHTML = '';
    for (var i = 0; i < count; i++) {
        var opt = document.createElement('option');
        opt.value = year - i;
        opt.innerHTML =year- i;
        select.appendChild(opt);
    }
}

//CHECK BOX
var textSeparator = ",";
function OnListBoxSelectionChanged(listBox, args) {
    if (args.index == 0)
        args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
    UpdateSelectAllItemState(listBox);
    UpdateText(listBox);
}
function UpdateSelectAllItemState(listBox) {
    IsAllSelected(listBox) ? listBox.SelectIndices([0]) : listBox.UnselectIndices([0]);
}
function IsAllSelected(listBox) {
    for (var i = 1; i < listBox.GetItemCount() ; i++)
        if (!listBox.GetItem(i).selected)
            return false;
    return true;
}
function UpdateText(listBox) {
    var namelistbox = listBox.name;
    var nameDropdown = namelistbox.substring(4)
    var controls = ASPxClientControl.GetControlCollection();
    var cbo = controls.GetByName(nameDropdown);
    var selectedItems = listBox.GetSelectedItems();
    cbo.SetText(GetSelectedItemsText(selectedItems));
}
function SynchronizeListBoxValues(dropDown, args) {
    var name = 'lbox' + dropDown.name;
    var controls = ASPxClientControl.GetControlCollection();
    var lbox = controls.GetByName(name);
    lbox.UnselectAll();
    var texts = dropDown.GetText().split(textSeparator);
    var values = GetValuesByTexts(texts, lbox);
    lbox.SelectValues(values);
    UpdateSelectAllItemState(listBox);
    UpdateText(listBox); // for remove non-existing texts
}
function GetSelectedItemsText(items) {
    var texts = [];
    for (var i = 0; i < items.length; i++)
        if (items[i].index != 0)
            texts.push(items[i].text);
    return texts.join(textSeparator);
}
function GetValuesByTexts(texts, lbox) {
    var actualValues = [];
    var item;
    for (var i = 0; i < texts.length; i++) {
        item = lbox.FindItemByText(texts[i]);
        if (item != null)
            actualValues.push(item.value);
    }
    return actualValues;
}
//tocken box 
function OnComboBoxKeyDown_BV(s, e) {
    // 'Delete' button key code = 46
    if (e.htmlEvent.keyCode == 46) {
        //if (s == cbbKQGDSetting) {
        //    s.SetSelectedIndex(-1);
        //}
        //else {
        //    s.SetValue(null);
        //}
    }
    else if (e.htmlEvent.keyCode == 13) {
        // FilterBV();
        // 'F4' button key code = 115
    }
    else if (e.htmlEvent.keyCode == 115) {
        s.ShowDropDown();
    }
}

function onKeyPressCbCSKCBSetting() {
    //if (cbbTinhThanhSetting.GetValue() <= 0 || cbbTinhThanhSetting.GetValue() == null) {
    //    popup_message.SetContentHtml(IDrawMessageTable('error2', 'Chọn giá trị!'));
    //    popup_message.Show();
    //    cbx_cskcbSettings.ClearItems();
    //    return false;
    //}
}
function onChangedCbCSKCBSetting(s, e) {
    var tokenLength = s.GetValue();
    var arrTokenValue = tokenLength.split(",");
    var collection = s.GetTokenCollection();
    if (arrTokenValue.length > 8) {
        popup_message.SetContentHtml(IDrawMessageTable('error2', 'Chỉ được phép chọn tối đa 8 cơ sở khám bệnh!'));
        popup_message.Show();
        s.RemoveTokenByText(collection[8]);
        return false;
    }
}
function CSKCBEndCallback(s, e) {
    // chỉ set lần đầu
    //if (inittimes++ == 1) {
    //    TokenBoxSetValue(cbx_cskcbSettings, '@ViewBag.CSKCBAXID');
    //}
}
