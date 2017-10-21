(function () {
    var serviceBase = "",
        getSvcUrl = function (method) {
            if (method.indexOf('http') === 0) {
                return method;
            }
            if (method[0] == "/") {
                return serviceBase + method;
            }
            return serviceBase + "/" + method;
        };

    window.serviceInvoker = (function () {
        var ajaxRequest = function (httpVerb, method, dataRequest, callbacks, paramName, keepOriginal, hideLoading) {
            //show loading
            if (!hideLoading)
                common.showWaiting(true);

            var dataToSend = keepOriginal === true ? dataRequest : JSON.stringify(dataRequest);
            dataToSend = paramName ? paramName + '=' + dataToSend : dataToSend;
            var options = {
                url: getSvcUrl(method),
                type: httpVerb,
                data: dataToSend,
                dataType: "json",
                contentType: "application/json"
            };

            options.success = function (response) {
                try {
                    if (callbacks && callbacks.success) {
                        callbacks.success(response);
                    }
                } catch (e) {
                    console.log(e);
                }
            };
            options.error = function (response) {
                switch (response.status) {
                    case 403://unauthority
                        if (response.responseJSON == null) {
                            response.responseJSON = {
                                ErrorCode: 403,
                                Message: response.statusText
                            }
                        }
                        break;
                    case 404:
                        break;
                    case 401:
                        if (response.responseJSON == null) {
                            response.responseJSON = {
                                ErrorCode: 401,
                                Message: response.statusText
                            }
                        }
                        break;
                    case 500:
                        break;
                    default:
                }
                try {
                    if (callbacks && callbacks.error) {
                        callbacks.error(response);
                    }
                } catch (e) {
                    console.log(e);
                }
            };
            options.complete = function (response) {
                try {
                    if (callbacks && callbacks.complete) {
                        callbacks.complete(response);
                    }
                } catch (e) {
                    console.log(e);
                }
                //hide loading
                if (!hideLoading)
                    common.showWaiting(false);
                if (common.debugLevel < 2) {
                    console.clear();
                }
            };
            return $.ajax(options);

        },
        uploadFile = function (httpVerb, method, dataRequest, callbacks) {
            //show loading
            common.showWaiting(true);

            var dataToSend = dataRequest;
            var options = {
                url: getSvcUrl(method),
                type: httpVerb,
                data: dataToSend,
                dataType: 'json',
                contentType: false,
                processData: false,
            };

            options.success = function (response) {
                if (callbacks && callbacks.success) {
                    callbacks.success(response);
                }
            };
            options.error = function (response) {
                switch (response.status) {
                    case 404:
                        break;
                    case 401:
                        break;
                    case 500:
                        break;
                    default:
                }
                if (callbacks && callbacks.error) {
                    callbacks.error(response);
                }
            };
            options.complete = function (response) {
                if (callbacks && callbacks.complete) {
                    callbacks.complete(response);
                }
                //hide loading
                common.showWaiting(false);
            };
            return $.ajax(options);

        }, get = function (method, request, callback, paramName, keepOriginal, hideLoading) {
            /// <summary>Perform a get request to web api</summary>
            /// <param name="method" type="Object">Method of web api to call</param>
            /// <param name="request" type="Object">Json data to be sent along with the request</param>
            /// <param name="callback" type="Object">
            /// Callback object contains callback functions to be called when request is success, error.
            /// </param>

            return ajaxRequest('get', method, request, callback, paramName, keepOriginal, hideLoading);
        },
            post = function (method, request, callback, paramName, keepOriginal, hideLoading) {
                /// <summary>Perform a post request to web api</summary>
                /// <param name="method" type="Object">Method of web api to call</param>
                /// <param name="request" type="Object">Json data to be sent along with the request</param>
                /// <param name="callback" type="Object">
                /// Callback object contains callback functions to be called when request is success, error
                /// </param>

                return ajaxRequest('post', method, request, callback, paramName, keepOriginal, hideLoading);
            },
            remove = function (method, request, callback, paramName, keepOriginal, hideLoading) {
                /// <summary>Perform a post request to web api</summary>
                /// <param name="method" type="Object">Method of web api to call</param>
                /// <param name="request" type="Object">Json data to be sent along with the request</param>
                /// <param name="callback" type="Object">
                /// Callback object contains callback functions to be called when request is success, error
                /// </param>

                return ajaxRequest('delete', method, request, callback, paramName, keepOriginal, hideLoading);
            },
            put = function (method, request, callback, paramName, keepOriginal, hideLoading) {
                /// <summary>Perform a put request to web api</summary>
                /// <param name="method" type="Object">Method of web api to call</param>
                /// <param name="request" type="Object">Json data to be sent along with the request</param>
                /// <param name="callback" type="Object">
                /// Callback object contains callback functions to be called when request is success, error
                /// </param>

                return ajaxRequest('put', method, request, callback, paramName, keepOriginal, hideLoading);
            },
            upload = function (method, request, callback, hideLoading) {
                /// <summary>Perform a put request to web api</summary>
                /// <param name="method" type="Object">Method of web api to call</param>
                /// <param name="request" type="Object">Json data to be sent along with the request</param>
                /// <param name="callback" type="Object">
                /// Callback object contains callback functions to be called when request is success, error
                /// </param>

                return uploadFile('post', method, request, callback, hideLoading);
            };
        return {
            get: get,
            post: post,
            put: put,
            remove: remove,
            upload: upload
        };
    })();
})();