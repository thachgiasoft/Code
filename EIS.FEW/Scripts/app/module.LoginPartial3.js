(function ($, appName) {
    /// <summary>Populates global ko object.</summary>
    /// <param name="Created Date">26 - 08 - 2015.</param>
    /// <param name="$">Reference to jquery object.</param>
    /// <param name="appName">namespace of application.</param>
    'use strict';

    function changePassword() {
        var url = common.moduleUrl("/Account/ChangePassword");
        serviceInvoker.get(url, null, {
            complete: function (response) {
                $.Dialog({
                    overlay: true,
                    shadow: true,
                    flat: false,
                    draggable: true,
                    title: 'Change password',
                    content: '',
                    onShow: function (_dialog) {
                        var content = _dialog.children('.content');
                        content.html(response.responseText);
                    }
                });
            }
        });
    }

    $(document).ready(function () {
        $('#login-partial3').on('click', '#a-change-pass', changePassword)
    });

}(jQuery, window.aiApp = window.aiApp || {}));