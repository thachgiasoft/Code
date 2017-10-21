jQuery.browser = {};
(function ($) {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    var navig = navigator.userAgent.toLowerCase();
    if (navig.match(/msie ([0-9]+)\./) || navig.match(/trident\/([0-9\.]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
    else if (navig.match(/chrome\/([0-9\.]+)\./)) {
        jQuery.browser.chrome = true;
        jQuery.browser.version = RegExp.$1;
    }
    else if (navig.match(/firefox\/([0-9\.]+)\./)) {
        jQuery.browser.mozilla = true;
        jQuery.browser.version = RegExp.$1;
    }

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    $.urlParam = $.urlParam || function (name) {
        var url = decodeURIComponent(window.location.href.toLowerCase());
        name = name.toLowerCase();
        var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(url);
        if (!results) {
            return "";
        }
        return results[1] || 0;
    };

    $.getOuterHTML = $.getOuterHTML || function (el) {
        var outerHtml = $(el).clone().wrap('<p>').parent().html();
        return outerHtml;
    };

    $.JsObjectToJSON = $.JsObjectToJSON = function (o) {
        return JSON.stringify(o);
    };

    $.StringFormat = $.StringFormat || function () {
        var s = arguments[0];
        for (var i = 0; i < arguments.length - 1; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "gm");
            s = s.replace(reg, arguments[i + 1]);
        }

        return s;
    };

    $.fn.getValueOfMultiControl = function () {
        var $this = $(this),
        $controls = $this.find('.multi-control-value'),
        value = '';
        if ($controls.length > 0) {
            for (var i = 0; i < $controls.length; i++) {
                var val = $($controls[i]).val();
                if (val == '') {
                    var defaultVal = $($controls[i]).attr('placeholder');
                    if (typeof (defaultVal) != 'undefined'
                        && defaultVal != '') {
                        val = defaultVal;
                    }
                }
                value += val;
            }
        }
        return value;
    };

    $.fn.getOriginalValueOfMultiControl = function () {
        var $this = $(this),
        $controls = $this.find('input.multi-control-value'),
        value = '';
        if ($controls.length > 0) {
            var val = $($controls).val();
            if (val == '') {
                var defaultVal = $($controls).attr('placeholder');
                if (typeof (defaultVal) != 'undefined'
                    && defaultVal != '') {
                    val = defaultVal;
                }
            }
            value = val;
        }
        return value;
    };

    $.fn.getCursorPosition = $.fn.getCursorPosition || function () {
        var el = $(this).get(0);
        var pos = 0;
        if ('selectionStart' in el) {
            pos = el.selectionStart;
        } else if ('selection' in document) {
            el.focus();
            var sel = document.selection.createRange();
            var selLength = document.selection.createRange().text.length;
            sel.moveStart('character', -el.value.length);
            pos = sel.text.length - selLength;
        }
        return pos;
    };

    $.fn.isBound = $.fn.isBound || function (type, fn) {
        var events = this.data('events');
        if (events === undefined) {
            return false;
        }
        var data = events[type];

        // ReSharper disable once QualifiedExpressionMaybeNull
        if (data === undefined || data.length === 0) {
            return false;
        }

        return (-1 !== $.inArray(fn, data));
    };

    $.fixDate = $.fixDate || function (d, check) { // force d to be on check's YMD, for daylight savings purposes
        var HOUR_MS = 3600000;
        if (+d) { // prevent infinite looping on invalid dates
            while (d.getDate() != check.getDate()) {
                d.setTime(+d + (d < check ? 1 : -1) * HOUR_MS);
            }
        }
    };

    $.clearTime = $.clearTime || function (oldDate) {
        var d = $.cloneDate(oldDate);
        d.setHours(0);
        d.setMinutes(0);
        d.setSeconds(0);
        d.setMilliseconds(0);
        return d;
    };

    $.cloneDate = $.cloneDate || function (d, dontKeepTime) {
        if (dontKeepTime) {
            return clearTime(new Date(+d));
        }
        return new Date(+d);
    };

    String.prototype.parseVnDate = String.prototype.parseVnDate ||
        function () {
            var d = this + "";
            var formatted = new Date();
            var rxDatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})(\T)(\d{1,2})(\:)(\d{1,2})(\:)(\d{1,2})/;
            var dtArray = d.match(rxDatePattern); // is format OK? y-M-dTh:m:s
            var dtDay, dtMonth, dtYear;
            if (dtArray != null && dtArray.length > 5) {
                dtDay = dtArray[5];
                dtMonth = parseInt(dtArray[3]) - 1;
                dtYear = dtArray[1];
                formatted = new Date(dtYear, dtMonth, dtDay);
            } else {
                rxDatePattern = /^(\/Date\()(\d{1,13})(\)\/)$/;
                dtArray = d.match(rxDatePattern); // is format OK? /Date(1224043200000)/
                if (dtArray != null && dtArray.length > 0) {
                    var date = new Date(parseInt(dtArray[dtArray.length - 2]));
                    formatted = new Date(parseInt(date));
                } else {
                    rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})/;
                    dtArray = d.match(rxDatePattern); // is format OK? y-M-d
                    if (dtArray != null && dtArray.length > 5) {
                        dtDay = dtArray[1];
                        dtMonth = parseInt(dtArray[3]) - 1;
                        dtYear = dtArray[5];
                        formatted = new Date(dtYear, dtMonth, dtDay);
                    }
                }
            }
            return formatted;
        };

    String.prototype.parseFullVnDate = String.prototype.parseFullVnDate ||
        function () {
            var d = this + "";
            var formatted = new Date();
            var rxDatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})(\T)(\d{1,2})(\:)(\d{1,2})(\:)(\d{1,2})/;
            var dtArray = d.match(rxDatePattern); // is format OK? y-M-dTh:m:s
            var dtDay, dtMonth, dtYear, dtHour, dtMinus;
            if (dtArray != null && dtArray.length > 5) {
                dtDay = dtArray[5];
                dtMonth = parseInt(dtArray[3]) - 1;
                dtYear = dtArray[1];
                dtHour = dtArray[7];
                dtMinus = dtArray[9];
                formatted = new Date(dtYear, dtMonth, dtDay, dtHour, dtMinus).addHours(7);
            } else {
                rxDatePattern = /^(\/Date\()(\d{1,13})(\)\/)$/;
                dtArray = d.match(rxDatePattern); // is format OK? /Date(1224043200000)/
                if (dtArray != null && dtArray.length > 0) {
                    var date = new Date(parseInt(dtArray[dtArray.length - 2]));
                    formatted = new Date(parseInt(date)).addHours(7);
                }
            }
            return formatted;
        };

    String.prototype.formatDateFromJsonString = String.prototype.formatDateFromJsonString ||
        function () {
            var d = this + "";
            var formatted = "";
            var rxDatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})(\T)(\d{1,2})(\:)(\d{1,2})(\:)(\d{1,2})/;
            var dtArray = d.match(rxDatePattern); // is format OK? y-M-dTh:m:s
            var dtDay, dtMonth, dtYear;
            if (dtArray != null && dtArray.length > 5) {
                dtDay = dtArray[5];
                dtMonth = dtArray[3];
                dtYear = dtArray[1];
                formatted = dtDay + "-" + dtMonth + "-" + dtYear;
            } else {
                rxDatePattern = /^(\/Date\()(\d{1,13})(\)\/)$/;
                dtArray = d.match(rxDatePattern); // is format OK? /Date(1224043200000)/
                if (dtArray != null && dtArray.length > 0) {
                    var date = new Date(parseInt(dtArray[dtArray.length - 2]));
                    formatted = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
                }
            }
            return formatted;
        };

    String.prototype.formatFullDateFromJsonString = String.prototype.formatFullDateFromJsonString ||
        function () {
            var d = this + "";
            var formatted = "";
            var rxDatePattern = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})(\T)(\d{1,2})(\:)(\d{1,2})(\:)(\d{1,2})/;
            var dtArray = d.match(rxDatePattern); // is format OK? y-M-dTh:m:s
            var dtDay, dtMonth, dtYear, dtHour, dtMinus;
            if (dtArray != null && dtArray.length > 5) {
                dtDay = dtArray[5];
                dtMonth = dtArray[3];
                dtYear = dtArray[1];
                dtHour = dtArray[7];
                dtMinus = dtArray[9];
                formatted = dtHour + ":" + dtMinus + " " + dtDay + "-" + dtMonth + "-" + dtYear;
            } else {
                rxDatePattern = /^(\/Date\()(\d{1,13})(\)\/)$/;
                dtArray = d.match(rxDatePattern); // is format OK? /Date(1224043200000)/
                if (dtArray != null && dtArray.length > 0) {
                    var date = new Date(parseInt(dtArray[dtArray.length - 2]));
                    formatted = date.getHours() + ":" + date.getMinutes() + " " + date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
                }
            }
            return formatted;
        };

    Date.prototype.addMonths = Date.prototype.addMonths || function (n, keepTime) { // prevents day overflow/underflow
        var d = this;
        if (+d) { // prevent infinite looping on invalid dates
            var m = d.getMonth() + n,
                check = $.cloneDate(d);
            check.setDate(1);
            check.setMonth(m);
            d.setMonth(m);
            if (!keepTime) {
                $.clearTime(d);
            }
            while (d.getMonth() != check.getMonth()) {
                d.setDate(d.getDate() + (d < check ? 1 : -1));
            }
        }
        return d;
    };

    Date.prototype.addDays = Date.prototype.addDays || function (n, keepTime) { // deals with daylight savings
        var d = this;
        if (+d) {
            var dd = d.getDate() + n,
                check = $.cloneDate(d);
            check.setHours(9); // set to middle of day
            check.setDate(dd);
            d.setDate(dd);
            if (!keepTime) {
                $.clearTime(d);
            }
            $.fixDate(d, check);
        }
        return d;
    }

    Date.prototype.addHours = Date.prototype.addHours || function (h) {
        this.setHours(this.getHours() + h);
        return this;
    };

    Date.prototype.addSeconds = Date.prototype.addSeconds || function (s) {
        this.setSeconds(this.getSeconds() + s);
        return this;
    };

    Date.prototype.addMinutes = Date.prototype.addMinutes || function (m) {
        this.setMinutes(this.getMinutes() + m);
        return this;
    };

    Array.prototype.remove = Array.prototype.remove || function (from, to) {
        var rest = this.slice((to || from) + 1 || this.length);
        this.length = from < 0 ? this.length + from : from;
        return this.push.apply(this, rest);
    };

    String.prototype.isUpperCase = String.prototype.isUpperCase || function () {
        return this.valueOf().toUpperCase() === this.valueOf();
    };

    $.fn.dialog = function (data) {
        var html = this.html();
        var onOpened = data.onShow || function (dlg) { };
        var onClosed = data.onClosed || function (dlg) { };
        this.html("");
        $["Dialog"]({
            id: data.id,
            icon: '<span class="fa fa-windows"></span>',
            overlay: true,
            shadow: true,
            flat: false,
            width: data.width || 'auto',
            height: data.height || 'auto',
            zIndex: data.zIndex,
            overlayClickClose: false,
            sysButtons: { btnClose: true, btnMax: true, btnMin: true },
            draggable: true,
            title: data.title,
            content: '',
            onShow: function (dlg) {
                var content = dlg.window.children('.window-content');
                content.html(html);
                if (typeof data.open == "function") {
                    data.open();
                }

                $(dlg).delegate(".tabs li a, .nav.nav-tabs li a", "click", function () {
                    dlg.autoResize();
                });
                //call delegate function
                onOpened();
                dlg.autoResize();
            },
            onClosed: function (dlg) {
                onClosed(dlg);
            }
        });
    };

    //trigger show/hide
    $.each(['show', 'hide'], function (i, ev) {
        var el = $.fn[ev];
        $.fn[ev] = function () {
            this.trigger(ev);
            return el.apply(this, arguments);
        };
    });

    // Create a jquery plugin that prints the given element.
    $.fn.print = function () {
        // NOTE: We are trimming the jQuery collection down to the
        // first element in the collection.
        if (this.size() > 1) {
            this.eq(0).print();
            return;
        } else if (!this.size()) {
            return;
        }

        // ASSERT: At this point, we know that the current jQuery
        // collection (as defined by THIS), contains only one
        // printable element.

        // Create a random name for the print frame.
        var strFrameName = ("printer-" + (new Date()).getTime());

        // Create an iFrame with the new name.
        var jFrame = $("<iframe name='" + strFrameName + "'>");

        // Hide the frame (sort of) and attach to the body.
        jFrame
        .css("width", "1px")
        .css("height", "1px")
        .css("position", "absolute")
        .css("left", "-9999px")
        .appendTo($("body:first"))
        ;

        // Get a FRAMES reference to the new frame.
        var objFrame = window.frames[strFrameName];

        // Get a reference to the DOM in the new frame.
        var objDoc = objFrame.document;

        // Grab all the style tags and copy to the new
        // document so that we capture look and feel of
        // the current document.

        // Create a temp document DIV to hold the style tags.
        // This is the only way I could find to get the style
        // tags into IE.
        var jStyleDiv = $("<div>").append(
        $("style").clone()
        );

        // Write the HTML for the document. In this, we will
        // write out the HTML of the current element.
        objDoc.open();
        objDoc.write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        objDoc.write("<html>");
        objDoc.write("<body>");
        objDoc.write("<head>");
        objDoc.write("<title>");
        objDoc.write(document.title);
        objDoc.write("</title>");
        objDoc.write(jStyleDiv.html());
        objDoc.write("</head>");
        objDoc.write(this.html());
        objDoc.write("</body>");
        objDoc.write("</html>");
        objDoc.close();

        // Print the document.
        objFrame.focus();
        objFrame.print();

        // Have the frame remove itself in about a minute so that
        // we don't build up too many of these frames.
        setTimeout(
        function () {
            jFrame.remove();
        },
        (60 * 1000)
        );
    }

    // Create a jquery plugin that prints the given element.
    $.fn.print2 = function () {
        // NOTE: We are trimming the jQuery collection down to the
        // first element in the collection.
        if (this.size() > 1) {
            this.eq(0).print();
            return;
        } else if (!this.size()) {
            return;
        }
        common.showWaiting(true);
        // Print the document.
        html2canvas(this, {
            onrendered: function (canvas) {

                // ASSERT: At this point, we know that the current jQuery
                // collection (as defined by THIS), contains only one
                // printable element.

                // Create a random name for the print frame.
                var strFrameName = ("printer-" + (new Date()).getTime());

                // Create an iFrame with the new name.
                var jFrame = $("<iframe name='" + strFrameName + "'>");

                // Hide the frame (sort of) and attach to the body.
                jFrame
                .css("width", "1px")
                .css("height", "1px")
                .css("position", "absolute")
                .css("left", "-9999px")
                .appendTo($("body:first"))
                ;

                // Get a FRAMES reference to the new frame.
                var objFrame = window.frames[strFrameName];

                // Get a reference to the DOM in the new frame.
                var objDoc = objFrame.document;

                // Grab all the style tags and copy to the new
                // document so that we capture look and feel of
                // the current document.

                // Create a temp document DIV to hold the style tags.
                // This is the only way I could find to get the style
                // tags into IE.
                var jStyleDiv = $("<div>").append(
                $("style").clone()
                );

                // Write the HTML for the document. In this, we will
                // write out the HTML of the current element.
                objDoc.open();
                objDoc.write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                objDoc.write("<html>");
                objDoc.write("<head>");
                objDoc.write("<title>");
                objDoc.write("&nbsp");
                objDoc.write("</title>");
                objDoc.write(jStyleDiv.html());
                objDoc.write("</head>");
                objDoc.write("<body>");
                objDoc.write("</body>");
                objDoc.write("</html>");

                objDoc.body.appendChild(canvas);
                objDoc.close();
                common.showWaiting(false);
                objFrame.focus();
                objFrame.print();

                // Have the frame remove itself in about a minute so that
                // we don't build up too many of these frames.
                setTimeout(
                function () {
                    jFrame.remove();
                },
                (60 * 1000)
                );
            }
        });
    }

})(jQuery);