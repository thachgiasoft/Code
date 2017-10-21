var province_names = [];
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function createMap(showName, data) {
	var h = $(document).height()*90/100;
	var w = $(document).width()*40/100;
	console.log("height:"  + h);
	console.log("width:"  + w)
	
    var paper = new ScaleRaphael('vietnammap', '1620.464', '1913.021'); // ThoPH: thư viện vẽ vector
	//var paper = new ScaleRaphael('vietnammap', w, h); // ThoPH: thư viện vẽ vector
    paper.scaleAll((window.innerHeight - 140) / 1913.021); // ThoPH: tỷ lệ scale hiển thị trên màn hình
    //paper.scaleAll(1);

    paper.setStart();
    var idx = 0;
    for (var province in vietnam.shapes) {
        //console.log(vietnam.names[province] + " " + province + ": " + colors[mapdata[province]]);
        var p = paper.path(vietnam.shapes[province]);

        if (province == "A3831"){
            province = 91
        }
        if ( typeof mapdata[province] != 'undefined'){
            var color = mapdata[province];
         }else{
            var color = "#555555"; //ThoPH: set mau mac dinh cho ban do
			// var color = colors[5]; //ThoPH: set mau mac dinh cho ban do
            console.log("color default #CCCCCC");
        }
        //console.log(province + ": " + mapdata[province] + " " + colors[mapdata[province]] + " " + color);
        p.attr({
            stroke: "#FEFEFE",// thoph: màu đường viền bo tỉnh
            fill: color,//màu fill cho tỉnh
            title: getTitleHover(province),// title hover tooltip
            "stroke-width": .2,
            "stroke-linejoin": "round",
            "stroke-opacity": 0.25
        });
        (function (p, province) {
            p.onclick = function () {
                 //alert("Click vào: " + vietnam.names[province]);
                 //thoph: đoạn này set cho phú quốc ăn theo Kiên giang mã 91
                 if (province == "A3831") {
                     chooseProvince("91")//thoph: hàm get dữ liệu hiển thị trên danh sách
                 } else {
                     if (province >= 10) {
                         chooseProvince(province);
                     }else{
                         chooseProvince("0" + province);
                     }
                 }
            };
        })(p[0], province);
        idx++;
    }
    if (showName) {
        for (var province in vietnam.shapes) {
            var text = paper.text(vietnam.position[province][0], vietnam.position[province][1], vietnam.names[province]);
            text.attr({
                "font-size": 18,
                "font-weight": "bold"
            });
            province_names[province_names.length] = text;
        }
    }
    //chu thich
    //for (var i = 0; i < notes.length; i++) {
    //    var box_note = paper.rect(200, 800 + 70 * i, 120, 50);
    //    box_note.attr({
    //        stroke: "#FEFEFE",
    //        fill: colors[i],
    //        "stroke-linejoin": "round",
    //        "stroke-opacity": 0.25
    //    })
    //
    //    var text_note = paper.text(400, 825 + 70 * i, notes[i]);
    //    text_note.attr({
    //        "font-size": 25,
    //        "font-weight": 'bold',
    //        "text-align": 'left'
    //    })
    //}
    var vn = paper.setFinish();

    var over = function () {
            if (this.attr('title') != "" && this.attr('title') != "Raphael") {
                this.c = this.c || this.attr("fill");
                this.stop().animate({fill: "#AAA"}, 300);
            }
        },
        out = function () {
            if (this.attr('title') != "" && this.attr('title') != "Raphael") {
                this.stop().animate({fill: this.c}, 300);
            }
        };

    vn.hover(over, out);
    $("#truongsa").css('left', $("#vietnammap").width()-100 + "px");
    $("#hoangsa").css('left', $("#vietnammap").width() - 100 + "px");

	var panZoom = window.panZoom = svgPanZoom('#svg-vietnam', {
          zoomEnabled: true,
          controlIconsEnabled: true,
          fit: 1,
          center: 1
        });

        $(window).resize(function(){
          panZoom.resize();
          panZoom.fit();
          panZoom.center();
        })
}