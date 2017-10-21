$.fn.extend({
    getDropTree: function () {
        var s = $.data(document, this.attr("id"));
        return s;
    }
});

(function ($, appName) {
    appName.DropTree = appName.DropTree || function (treeId, expandAll) {
        var self = this;
        self.TreeId = '';
        self.Tree = {};

        function checkedParentIfAny(chk) {
            var checkbox = $(chk);
            var ul = checkbox.closest("ul");
            if (!ul)
                return;
            var idParent = $(chk).attr("parentid");
            var chkParent = $("#" + idParent);
            if (!chkParent)
                return;
            var otherCheckboxes = $(ul).find("input[parentid='" + idParent + "']");
            var numChecked = 0;
            var nChkeckbox = otherCheckboxes.length;
            for (var i = 0; i < nChkeckbox; i++) {
                if ($(otherCheckboxes[i]).is(":checked")) {
                    numChecked++;
                }
            }
            if (numChecked == nChkeckbox) {
                chkParent.prop("checked", true);
            } else {
                chkParent.prop("checked", false);
            }
        }

        function tree_GetValue(str) {
            if (!str)
                return '';
            var strArr = str.split('-');
            return strArr[strArr.length - 1];
        }

        function showAndHide() {
            if (self.Tree.hasClass("chosen-container-active")) {
                self.Tree.removeClass("chosen-container-active chosen-with-drop");
            } else {
                self.Tree.addClass("chosen-container-active chosen-with-drop");
            }
        }

        function init() {
            self.TreeId = treeId;
            self.Tree = $("#" + treeId);

            self.Tree.delegate("label input[type='checkbox']", "change", function () {
                var
                checkbox = $(this),
                nestedList = checkbox.parent().next().next(),
                selectNestedListCheckbox = nestedList.find("label:not([for]) input:checkbox");

                checkedParentIfAny(this);
                if (checkbox.is(":checked")) {
                    return selectNestedListCheckbox.prop("checked", true);
                }
                return selectNestedListCheckbox.prop("checked", false);
            });

            self.Tree.delegate(".chosen-search input[type='text']", "keyup", function () {
                suggession(this);
            });

            if (!expandAll)
                $("li input[type='checkbox']").prop("checked", false);

            $("#" + treeId + " a.chosen-default").on("click", showAndHide);
            $(document).mouseup(function (e) {
                var container = self.Tree;
                if (!container.is(e.target) // if the target of the click isn't the container...
                    && container.has(e.target).length === 0) // ... nor a descendant of the container
                {
                    self.Tree.removeClass("chosen-container-active chosen-with-drop");
                }
            });

            $.data(document, treeId, self);
        }

        function suggession(s) {
            var keywords = $(s).val(),
                    $ul = self.Tree.find('ul'),
                    $lis = $ul.children('li'),
                    sort = 'desc' == 'asc' ? 1 : -1;
            if (keywords == '') {
                $lis.each(function (index) {
                    var $liItem = $(this),
                        checkbox = $liItem.find('label input.chk-checkbox');
                    if (checkbox.is(':checked')) {
                        $liItem.attr('ontop', 1);
                    } else {
                        $liItem.attr('ontop', 0);
                    }
                });
                $lis.removeClass('hide');
                $lis.sort(function (a, b) {
                    var vala = $(a).attr('ontop');
                    var valb = $(b).attr('ontop');
                    return (vala == valb) ? 0 : ((vala > valb) ? sort : -1 * sort);
                });

                $ul.empty();
                $ul.append($lis);
            } else {
                var keys = [];
                if (keywords[0] == '"' && keywords[keywords.length - 1] == '"') {
                    keys.push(keywords.substr(1, keywords.length - 2));
                } else {
                    keys = keywords.split(' ');
                }

                var regx = new RegExp(keys.join('|'), "ig");

                $lis.each(function (index) {
                    var $this = $(this),
                        text = $this.text(),
                        matchs = 0;
                    var matched = text.match(regx);
                    if (matched != null && matched.length) {
                        $this.removeClass('hide');
                        matchs = matched.length;
                    } else {
                        $this.addClass('hide');
                        $this.removeAttr('order');
                    }
                    $this.attr('order', matchs);
                });
            }
        }

        self.getTreeChecked = function () {
            var chkArr = self.Tree.find('label input.chk-checkbox');
            var arr = [];
            for (var i = 0; i < chkArr.length; i++) {
                var checkbox = $(chkArr[i]);
                if (checkbox.is(':checked')) {
                    arr.push({
                        Id: tree_GetValue(checkbox.attr('id')),
                        ParentId: tree_GetValue(checkbox.attr('parentid')),
                    });
                }
            }
            return arr;
        }
        self.getValueTreeChecked = function() {
            var chkArr = self.Tree.find('label input.chk-checkbox');
            var arr = "";
            for (var i = 0; i < chkArr.length; i++) {
                var checkbox = $(chkArr[i]);
                if (checkbox.is(':checked')) {
                    arr += tree_GetValue(checkbox.attr('id')) + ',';
                }
            }
            
            return arr.substring(0, arr.lastIndexOf(',')) ;
        }
        self.unCheckAll = function () {
            var chkArr = self.Tree.find('label input.chk-checkbox');
            var arr = [];
            for (var i = 0; i < chkArr.length; i++) {
                var checkbox = $(chkArr[i]);
                checkbox.prop("checked", false);
            }
            return arr;
        }

        init();

        return self;
    };
}(jQuery, window.tree = window.tree || {}));
