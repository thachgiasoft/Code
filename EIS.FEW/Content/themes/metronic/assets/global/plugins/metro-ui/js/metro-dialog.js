(function ($) {
    window.METRO_DIALOGs = window.METRO_DIALOGs || [];

    $.Dialog = function (params) {
        var id = "dialog-default";
        params = params || "";

        if (typeof params == "string") {
            if (params === '')
                params = id;
            if (METRO_DIALOGs.length > 0) {
                for (var i = 0; i < METRO_DIALOGs.length; i++) {
                    if (METRO_DIALOGs[i].id === params) {
                        return METRO_DIALOGs[i];
                    }
                }
            }
            throw "type of dialog with id = " + params + " not defined!";
        }
        var dialog;
        if (!params.id || params.id === '') {
            params.id = id;
        }
        if (typeof params.id == "string") {
            if (METRO_DIALOGs.length > 0) {
                for (var i = 0; i < METRO_DIALOGs.length; i++) {
                    if (METRO_DIALOGs[i].id === params.id) {
                        dialog = METRO_DIALOGs[i];
                        break;
                    }
                }
            }
        }
        else if (METRO_DIALOGs.length > 0) {
            dialog = METRO_DIALOGs[METRO_DIALOGs.length - 1];
        }

        if (dialog != null) {
            if (!dialog.opened) {
                dialog.opened = true;
            } else {
                return dialog;
            }
        }

        dialog = {
            id: params.id,
            opened: true,
            settings: {
                icon: false,
                id: null,
                title: '',
                content: '',
                flat: false,
                shadow: false,
                overlay: false,
                width: 'auto',
                height: 'auto',
                position: 'default',
                padding: false,
                overlayClickClose: true,
                sysButtons: {
                    btnClose: true
                },
                onShow: function (_dialog) { },
                onClosed: function (_dialog) { },
                sysBtnCloseClick: function (event) { },
                sysBtnMinClick: function (event) { },
                sysBtnMaxClick: function (event) { }
            }
        };
        params = $.extend(dialog.settings, params);

        var _overlay, _window, _caption, _content;

        _overlay = $("<div/>").addClass("window-overlay");

        if (params.overlay) {
            _overlay.css({
                backgroundColor: 'rgba(0,0,0,.7)'
            });
        }

        if (params.zIndex) {
            _overlay.css({
                'z-index': params.zIndex
            });
        }

        _window = $("<div/>").addClass("window");
        if (params.flat) _window.addClass("flat");
        if (params.shadow) _window.addClass("shadow").css('overflow', 'hidden');
        _caption = $("<div/>").addClass("caption").addClass("window-caption").addClass("bg-blue");
        _content = $("<div/>").addClass("content").addClass("window-content");
        _content.css({
            paddingTop: 5 + params.padding,
            paddingLeft: params.padding,
            paddingRight: params.padding,
            paddingBottom: params.padding
        });

        if (params.sysButtons) {
            if (params.sysButtons.btnClose) {
                $("<button/>").addClass("btn-close").on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    dialog.close();
                    params.sysBtnCloseClick(e);
                }).appendTo(_caption);
            }
            if (params.sysButtons.btnMax) {
                $("<button/>").addClass("btn-max").on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    dialog.sysBtnMaxClick();
                    params.sysBtnMaxClick(e);
                }).appendTo(_caption);
            }
            if (params.sysButtons.btnMin) {
                $("<button/>").addClass("btn-min").on('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    dialog.sysBtnMinClick();
                    params.sysBtnMinClick(e);
                }).appendTo(_caption);
            }
        }

        if (params.icon) $(params.icon).addClass("window-caption-icon").appendTo(_caption);
        $("<span/>").addClass("window-caption-title").html(params.title).appendTo(_caption);

        _content.html(params.content);

        _caption.appendTo(_window);
        _content.appendTo(_window);
        _window.appendTo(_overlay);

        if (params.width != 'auto') _window.css('width', params.width);
        if (params.height != 'auto') _window.css('height', params.height);

        _overlay.hide().appendTo('body').fadeIn('fast');

        _window
            .css("position", "fixed")
            .css("z-index", parseInt(_overlay.css('z-index')) + 1)
            .css("top", ($(window).height() - _window.outerHeight()) / 2)
            .css("left", ($(window).width() - _window.outerWidth()) / 2)
        ;

        //addTouchEvents(_window[0]);

        if (params.draggable) {
            _caption.on("mousedown", function (e) {
                _window.draggable();
                dialog.drag = true;
                _caption.css('cursor', 'move');
            }).on("mouseup", function () {
                _window.draggable('destroy');
                dialog.drag = false;
                _caption.css('cursor', 'default');
            }).on('dblclick', dialog.sysBtnMaxClick);
        }

        _window.on('click', function (e) {
            e.stopPropagation();
        });

        if (params.overlayClickClose) {
            _overlay.on('click', function (e) {
                e.preventDefault();
                dialog.close();
            });
        }

        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                e.preventDefault();
                dialog.close();
            }   // esc
        });



        dialog.content = function (newContent) {
            if (!dialog.opened || dialog.window == undefined) {
                return false;
            }

            if (newContent) {
                dialog.window.children(".window-content").html(newContent);
                dialog.autoResize();
                return true;
            } else {
                return dialog.window.children(".window-content").html();
            }
        }

        dialog.title = function (newTitle) {
            if (!dialog.opened || dialog.window == undefined) {
                return false;
            }

            var _title = dialog.window.children('.window-caption').children('.window-caption-title');

            if (newTitle) {
                _title.html(newTitle);
            } else {
                _title.html();
            }

            return true;
        }

        dialog.autoResize = function () {
            if (!dialog.opened || dialog.window == undefined) {
                return false;
            }

            var oldWidth = dialog.window.width();
            var oldHeight = dialog.window.height();

            dialog.window.css({
                width: "auto",
                "height": "auto"
            });

            var content = dialog.window.children(".window-content");
            var caption = dialog.window.children(".window-caption");

            var wHeight = $(window).height();
            var wWidth = $(window).width();
            var height = content.outerHeight();
            var cheight = caption.outerHeight();
            var width = content.outerWidth();

            if (height > wHeight - cheight) {
                height = wHeight;
                if (content.css("overflow-y") != "auto") {
                    content
                        .css("overflow-y", "auto")
                        .css("max-height", height - caption.outerHeight() - 10);
                }
            }

            if (width > wWidth) {
                width = wWidth;
                if (content.css("overflow-x") != "auto") {
                    content
                        .css("overflow-x", "auto")
                        .css("max-width", width);
                }
            }
            else if (width < oldWidth) {
                width = oldWidth;
            }

            dialog.window.css({
                width: width,
                "min-height": height
            });

            var top = ($(window).height() - dialog.window.outerHeight()) / 2;
            var left = ($(window).width() - dialog.window.outerWidth()) / 2;
            if (top < 0) {
                top = 0;
            }

            dialog.window.css({
                top: top,
                left: left
            });

            return true;
        }

        dialog.close = function () {
            if (!dialog.opened || dialog.window == undefined) {
                return false;
            }

            dialog.opened = false;
            var _overlay = dialog.window.parent(".window-overlay");
            _overlay.remove();
            //call onClosed
            if (typeof dialog.settings.onClosed == 'function')
                dialog.settings.onClosed();

            for (var i = 0; i < METRO_DIALOGs.length; i++) {
                if (METRO_DIALOGs[i].id === dialog.id) {
                    METRO_DIALOGs.remove(i);
                    break;
                }
            }
            //_overlay.fadeOut(function () {

            //});
            return false;
        }

        dialog.sysBtnMinClick = function () {
            if (dialog.window.state == null || dialog.window.state == "medium") {
                dialog.window.oldHeight = dialog.window.css("height");
                dialog.window.oldWidth = dialog.window.css("width");
                dialog.window.oldTop = dialog.window.css("top");
                dialog.window.oldLeft = dialog.window.css("left");
            }

            if (dialog.window.state == "minimum") {
                dialog.window.css("min-height", "");
                dialog.window.css("top", dialog.window.oldTop);
                dialog.window.css("left", dialog.window.oldLeft);
                dialog.window.css("bottom", "");
                dialog.window.css("height", dialog.window.oldHeight);
                dialog.window.css("width", dialog.window.oldWidth);

                dialog.window.state = "medium";
            } else {
                var title = $(dialog.window.find(".window-caption .window-caption-title")[0]);
                dialog.window.css("min-height", "32px");
                dialog.window.css("height", "0");
                dialog.window.css("width", title.width() + 150 + "px");
                dialog.window.css("top", "");
                dialog.window.css("left", "0");
                dialog.window.css("bottom", "0");

                dialog.window.state = "minimum";
            }
            dialog.autoResize();
            return false;
        }

        dialog.sysBtnMaxClick = function () {
            if (dialog.window.state == null || dialog.window.state == "medium") {
                dialog.window.oldHeight = dialog.window.css("height");
                dialog.window.oldWidth = dialog.window.css("width");
                dialog.window.oldTop = dialog.window.css("top");
                dialog.window.oldLeft = dialog.window.css("left");
            }

            if (dialog.window.state == "maximum") {
                dialog.window.css("min-height", "");
                dialog.window.css("top", dialog.window.oldTop);
                dialog.window.css("left", dialog.window.oldLeft);
                dialog.window.css("bottom", "");
                dialog.window.css("height", dialog.window.oldHeight);
                dialog.window.css("width", dialog.window.oldWidth);

                dialog.window.state = "medium";
            } else {
                dialog.window.css("min-height", "");
                dialog.window.css("top", "0");
                dialog.window.css("left", "0");
                dialog.window.css("bottom", "");
                dialog.window.css("height", "100%");
                dialog.window.css("width", "100%");

                dialog.window.state = "maximum";
            }
            dialog.autoResize();
            return false;
        }

        dialog.window = _window;

        //finished
        params.onShow(dialog);

        _window.resizable();

        dialog.autoResize();

        //cache
        METRO_DIALOGs.push(dialog);

        return dialog;
    }

})(jQuery);
