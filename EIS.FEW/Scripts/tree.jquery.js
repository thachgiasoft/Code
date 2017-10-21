$.fn.extend({
    getTreeCss3: function () {
        var s = $.data(document, this.attr("id"));
        return s;
    }
});

(function ($, appName) {
    appName.TreeCss3 = appName.TreeCss3 || function (treeId, expandAll) {
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

            if (!expandAll)
                $("li input[type='checkbox']").prop("checked", false);

            $.data(document, treeId, self);
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
