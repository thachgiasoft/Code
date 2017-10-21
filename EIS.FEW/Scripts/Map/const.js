var colors = ['#fd0000', '#ff7e00', '#feff00', '#89d100', '#38a700', '#AAA'];
var DOMAIN = "http://125.212.205.39:4953";
//var DOMAIN = "http://localhost:3019";
//ThoPH: định nghĩa loại dịch vụ
var TYPE_DV = {
    1: "X-QUANG THƯỜNG",
    2: "X-QUANG CAN THIỆP",
    3: "X-QUANG CHẨN ĐOÁN XÁC ĐỊNH",
    4: "CỘNG HƯỞNG TỪ MRI",
    5: "PET/CT",
    6: "CT 32 DÃY",
    7: "CT 64-128 DÃY",
    8: "CT 256 DÃY",
    9: "SIÊU ÂM THƯỜNG",
    10: "SIÊU ÂM TIM MẠCH",
    11: "SIÊU ÂM CHẨN ĐOÁN XÁC ĐỊNH",
    12: "ĐỊNH NHÓM MÁU",
    13: "HBA1C",
    14: "TIỂU ĐƯỜNG",
    15: "TĂNG HUYẾT ÁP"
}

Number.prototype.format = function(n, x) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
    return this.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$&,');
};