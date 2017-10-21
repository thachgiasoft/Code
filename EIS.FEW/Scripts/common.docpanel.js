(function ($, appName) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">26 - 08 - 2015.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="appName">namespace of application.</param>
    'use strict';

    appName.DocPanel = function () {
        var self = this;
        self.target = {};
        self.iconMinimize = {};
        self.iconMaximize = {};
        self.contentResize = {};
        self.mode = "max";
        self.width = 300;

        function hideContent(callback) {
            var $content = $(self.target).find('.dock-panel-content');
            var $heading = $(self.target).find('.dock-panel-heading');
            $content.fadeOut('slow', function() {
                $(this).hide(callback);
                $(self.contentResize).css('padding-left', '50px');
                $heading.css('transform', 'rotate(90deg)')
                    .css('transform-origin', 'left top 0')
                    .css('transform-origin', 'left top 0')
                    .css('margin-left', '40px')
                    .css('padding', '5px')
                    .css('float', 'left');
            })
        }

        function showContent(callback) {
            var $content = $(self.target).find('.dock-panel-content');
            var $heading = $(self.target).find('.dock-panel-heading');
            $content.fadeIn('slow', function() {
                $(this).show(callback);
                $(self.contentResize).css('padding-left', (self.width + 5) + 'px');
                $heading.css('transform', '')
                    .css('transform-origin', '')
                    .css('transform-origin', '')
                    .css('margin-left', '')
                    .css('padding', '')
                    .css('float', '');
            });
        }

        function process(e) {
            var $this = $(this);
            var $parent = $this.parent();
            if ($this.hasClass('icon-pop-in')) {
                hideContent(function() {
                    $this.hide(function() {
                        $parent.find('i.icon-pop-out').show();
                        self.mode = "min";
                    });
                });
            } else {
                showContent(function() {
                    $this.hide(function() {
                        $parent.find('i.icon-pop-in').show();
                        self.mode = "max";
                    });
                });
            }
        }

        function dockResize() {
            var $window = $(this),
                width = $window.width();
            if (width > 1000) {
                if (self.mode !== "max") {
                    self.iconMaximize.click();
                }
            }
            if (width <= 960) {
                if (self.mode !== "min") {
                    self.iconMinimize.click();
                }
            }
        }

        self.init = function (element) {
            self.target = element;
            var $parent = $(self.target).parent();
            self.width = $(self.target).width();
            self.contentResize = $parent.children('.dock-panel-resize');

            $(self.target).css('max-width', self.width + 'px')
                        .css('position', 'absolute');//dock-panel-resize
            $(self.contentResize).css('padding-left', (self.width + 5) + 'px');

            var $heading = $(self.target).find('.menu-title-heading');
            self.iconMinimize = $heading.find('i.icon-pop-in');
            self.iconMaximize = $heading.find('i.icon-pop-out');
            self.iconMinimize.click(process);
            self.iconMaximize.click(process);

            $(window).on('resize', dockResize);

            return self;
        }
    }

}(jQuery, window.aicommon = window.aicommon || {}));

$(document).ready(function () {
    //apply aifinder to ckeditor
    $(document).find('div.dock-panel').each(function (index) {
        var docpnl = new aicommon.DocPanel();
        docpnl.init(this);
    });
});