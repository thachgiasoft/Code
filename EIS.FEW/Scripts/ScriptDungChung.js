/*
* đặt tooltip cho các combobox có 1 cột
* */
function SetItemsToolTip1Columns(s, e) {
    for (i = 0; i < s.GetItemCount() ; i++) {
        var item2 = s.listBox.GetItemRow(i);
        item2.title = item2.cells[0].innerText;
    }
}
/*
 * đặt tooltip cho các combobox có 2 cột
 * */
function SetItemsToolTip2Columns(s, e) {
    for (i = 0; i < s.GetItemCount() ; i++) {
        var item2 = s.listBox.GetItemRow(i);
        item2.title = item2.cells[0].innerText + ' - ' + item2.cells[1].innerText;
    }
}
/*
 * đặt tooltip cho treelist
 * */
function SetItemsToolTipTreeList(s, e) {
    for (i = 0; i < s.rowCount ; i++) {
        //var item2 = s.listBox.GetItemRow(i);
        //item2.title = item2.cells[0].innerText;
    }
}
/*
 * check min max cho cbx giai đoạn
 * */
function checkMax(s, e) {
    var now = new Date();
    if (dt_denngay.GetValue() != "") {
        dt_tungay.SetMaxDate(dt_denngay.GetValue());
    }
}
function checkMin(s, e) {
    var now = new Date();
    if (dt_tungay.GetValue() != "") {
        dt_denngay.SetMinDate(dt_tungay.GetValue());
    }
    var maxDate = addDays(now, 0);
    dt_denngay.SetMaxDate(maxDate);
}
function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}
/*
 * vẽ nội dung cho popup thông báo
 * */
function IDrawMessageTable(icon, message) {
    return "<table width=\"100%\"><tr><td width=\"30px\">"
    + "<img id=\"messageicon\" src=\"/Content/Images/" + icon + ".png\" width=\"16px\" height=\"16px\"></td><td id=\"messageContent\">" + message + "</td>"
    + "</tr></table>";
}


function EncodeUrlNDH(str){
    if ( typeof (str) == "string") {
        str = str.replace(/"/g, "%H1");
        str = str.replace(/'/g, "%H2");
        str = str.replace(/|/g, "%H3");
        str = str.replace(/ /g, "%H4");
        str = str.replace(/\?/g, "%H5");
        str = str.replace(/&/g, "%H6");
        str = str.replace(/\+/g, "%H7");
    }
    return str;
}

function DecodeUrlNDH(str){
    if ( typeof (str) == "string") {
        str = str.replace(/%H1/ig, "\"");
        str = str.replace(/%H2/ig, "'");
        str = str.replace(/%H3/ig, "|");
        str = str.replace(/%H4/ig, " ");
        str = str.replace(/%H5/ig, "?");
        str = str.replace(/%H6/ig, "&");
        str = str.replace(/%H7/ig, "+");
    }
    return str;
}