var colors = ['#38a700', '#89d100', '#feff00', '#ff7e00', '#fd0000'];
var notes = ['80% - 100%', '60% - 80%', '40% - 60%', '20% - 40%', '0% - 20%'];
var province_names = [];
$(document).ready(function () {
    createMap(true);
    $("#truongsa").css('left', $("#vietnammap").width() + "px");
    $("#hoangsa").css('left', $("#vietnammap").width() + "px");
});

function createMap(showName) {
    var paper = new ScaleRaphael('vietnammap', '1220.464', '1913.021');
    //paper.scaleAll(window.innerWidth / 2 / 1220.464);
    //paper.scaleAll(1);
    paper.scaleAll((window.innerHeight - 50) / 1913.021);

    paper.setStart();
    var idx = 0;
    for (var province in vietnam.shapes) {
        var p = paper.path(vietnam.shapes[province]);
        p.attr({
            stroke: "#FEFEFE",
            fill: colors[Math.floor(Math.random() * colors.length)],
            title: "Thông tin" + vietnam.names[province] + " \n Thông tin abcxyz...",
            "stroke-width": .2,
            "stroke-linejoin": "round",
            "stroke-opacity": 0.25
        });
        (function (p, province) {
            p.onclick = function () {
                //alert("Click vào: " + vietnam.names[province]);
                $("#prov-name").html(vietnam.names[province]);
                $("html, body").animate({scrollTop: 0}, "slow");
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
}