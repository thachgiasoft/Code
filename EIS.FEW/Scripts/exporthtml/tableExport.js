(function ($) {
    $.fn.extend({
        tableExport: function (options) {
            var defaults = {
                separator: ',',
                ignoreColumn: [],
                tableName: 'yourTableName',
                fileName: 'filedata',
                type: 'csv',
                pdfFontSize: 14,
                pdfLeftMargin: 20,
                escape: 'true',
                htmlContent: 'false',
                consoleLog: 'false'
            };

            var options = $.extend(defaults, options);
            var el = this;

            if (defaults.type == 'csv' || defaults.type == 'txt') {

                // Header
                var tdData = "";
                $(el).find('thead').find('tr').each(function () {
                    tdData += "\n";
                    $(this).filter(':visible').children().each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                tdData += '"' + parseString($(this)) + '"' + defaults.separator;
                            }
                        }

                    });
                    tdData = $.trim(tdData);
                    tdData = $.trim(tdData).substring(0, tdData.length - 1);
                });

                // Row vs Column
                $(el).find('tbody').find('tr').each(function () {
                    tdData += "\n";
                    $(this).filter(':visible').find('td').each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                tdData += '"' + parseString($(this)) + '"' + defaults.separator;
                            }
                        }
                    });
                    //tdData = $.trim(tdData);
                    tdData = $.trim(tdData).substring(0, tdData.length - 1);
                });

                //output
                if (defaults.consoleLog == 'true') {
                    console.log(tdData);
                }
                var base64data = "base64," + btoa(unescape(encodeURIComponent(excelFile)));
                var url = 'data:application/' + defaults.type + ';' + base64data;
                exportFile(url);

            } else if (defaults.type == 'sql') {

                // Header
                var tdData = "INSERT INTO `" + defaults.tableName + "` (";
                $(el).find('thead').find('tr').each(function () {

                    $(this).filter(':visible').children().each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                tdData += '`' + parseString($(this)) + '`,';
                            }
                        }

                    });
                    tdData = $.trim(tdData);
                    tdData = $.trim(tdData).substring(0, tdData.length - 1);
                });
                tdData += ") VALUES ";
                // Row vs Column
                $(el).find('tbody').find('tr').each(function () {
                    tdData += "(";
                    $(this).filter(':visible').find('td').each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                tdData += '"' + parseString($(this)) + '",';
                            }
                        }
                    });

                    tdData = $.trim(tdData).substring(0, tdData.length - 1);
                    tdData += "),";
                });
                tdData = $.trim(tdData).substring(0, tdData.length - 1);
                tdData += ";";

                //output
                //console.log(tdData);

                if (defaults.consoleLog == 'true') {
                    console.log(tdData);
                }

                var base64data = "base64," + btoa(unescape(encodeURIComponent(excelFile)));
                var url = 'data:application/sql;' + base64data;
                exportFile(url);

            } else if (defaults.type == 'json') {

                var jsonHeaderArray = [];
                $(el).find('thead').find('tr').each(function () {
                    var tdData = "";
                    var jsonArrayTd = [];

                    $(this).filter(':visible').children().each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                jsonArrayTd.push(parseString($(this)));
                            }
                        }
                    });
                    jsonHeaderArray.push(jsonArrayTd);

                });

                var jsonArray = [];
                $(el).find('tbody').find('tr').each(function () {
                    var tdData = "";
                    var jsonArrayTd = [];

                    $(this).filter(':visible').find('td').each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                jsonArrayTd.push(parseString($(this)));
                            }
                        }
                    });
                    jsonArray.push(jsonArrayTd);

                });

                var jsonExportArray = [];
                jsonExportArray.push({ header: jsonHeaderArray, data: jsonArray });

                //Return as JSON
                //console.log(JSON.stringify(jsonExportArray));

                //Return as Array
                //console.log(jsonExportArray);
                if (defaults.consoleLog == 'true') {
                    console.log(JSON.stringify(jsonExportArray));
                }
                var base64data = "base64," + btoa(unescape(encodeURIComponent(excelFile)));
                var url = 'data:application/json;filename=exportData;' + base64data;
                exportFile(url);

            } else if (defaults.type == 'xml') {

                var xml = '<?xml version="1.0" encoding="utf-8"?>';
                xml += '<tabledata><fields>';

                // Header
                $(el).find('thead').find('tr').each(function () {
                    $(this).filter(':visible').children().each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                xml += "<field>" + parseString($(this)) + "</field>";
                            }
                        }
                    });
                });
                xml += '</fields><data>';

                // Row Vs Column
                var rowCount = 1;
                $(el).find('tbody').find('tr').each(function () {
                    xml += '<row id="' + rowCount + '">';
                    var colCount = 0;
                    $(this).filter(':visible').find('td').each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                xml += "<column-" + colCount + ">" + parseString($(this)) + "</column-" + colCount + ">";
                            }
                        }
                        colCount++;
                    });
                    rowCount++;
                    xml += '</row>';
                });
                xml += '</data></tabledata>';

                if (defaults.consoleLog == 'true') {
                    console.log(xml);
                }

                var base64data = "base64," + btoa(unescape(encodeURIComponent(excelFile)));
                var url = 'data:application/xml;filename=exportData;' + base64data;
                exportFile(url);

            } else if (defaults.type == 'excel' || defaults.type == 'doc' || defaults.type == 'powerpoint') {
                //console.log($(this).html());
                var excel = "<table>";
                // Header
                $(el).find('thead').find('tr').each(function () {
                    excel += "<tr>";
                    $(this).filter(':visible').children().each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                excel += "<td>" + parseString($(this)) + "</td>";
                            }
                        }
                    });
                    excel += '</tr>';

                });


                // Row Vs Column
                var rowCount = 1;
                $(el).find('tbody').find('tr').each(function () {
                    excel += "<tr>";
                    var colCount = 0;
                    $(this).filter(':visible').find('td').each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                excel += "<td>" + parseString($(this)) + "</td>";
                            }
                        }
                        colCount++;
                    });
                    rowCount++;
                    excel += '</tr>';
                });
                excel += '</table>';

                if (defaults.consoleLog == 'true') {
                    console.log(excel);
                }

                var excelFile = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:" + defaults.type + "' xmlns='http://www.w3.org/TR/REC-html40'>";
                excelFile += "<head>";
                excelFile += "<!--[if gte mso 9]>";
                excelFile += "<xml>";
                excelFile += "<x:ExcelWorkbook>";
                excelFile += "<x:ExcelWorksheets>";
                excelFile += "<x:ExcelWorksheet>";
                excelFile += "<x:Name>";
                excelFile += "{worksheet}";
                excelFile += "</x:Name>";
                excelFile += "<x:WorksheetOptions>";
                excelFile += "<x:DisplayGridlines/>";
                excelFile += "</x:WorksheetOptions>";
                excelFile += "</x:ExcelWorksheet>";
                excelFile += "</x:ExcelWorksheets>";
                excelFile += "</x:ExcelWorkbook>";
                excelFile += "</xml>";
                excelFile += "<![endif]-->";
                excelFile += "</head>";
                excelFile += "<body>";
                excelFile += excel;
                excelFile += "</body>";
                excelFile += "</html>";

                var base64data = "base64," + btoa(unescape(encodeURIComponent(excelFile)));
                var url = 'data:application/vnd.ms-' + defaults.type + ';' + base64data;
                exportFile(url);

            } else if (defaults.type == 'png') {
                html2canvas($(el), {
                    onrendered: function (canvas) {
                        var img = canvas.toDataURL("image/png");
                        window.open(img);
                    }
                });
            } else if (defaults.type == 'pdf') {

                var doc = new jsPDF('p', 'pt', 'a4', true);
                doc.setFontSize(defaults.pdfFontSize);

                // Header
                var startColPosition = defaults.pdfLeftMargin;
                $(el).find('thead').find('tr').each(function () {
                    $(this).filter(':visible').children().each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                var colPosition = startColPosition + (index * 50);
                                doc.text(colPosition, 20, parseString($(this)));
                            }
                        }
                    });
                });


                // Row Vs Column
                var startRowPosition = 20; var page = 1; var rowPosition = 0;
                $(el).find('tbody').find('tr').each(function (index, data) {
                    rowCalc = index + 1;

                    if (rowCalc % 26 == 0) {
                        doc.addPage();
                        page++;
                        startRowPosition = startRowPosition + 10;
                    }
                    rowPosition = (startRowPosition + (rowCalc * 10)) - ((page - 1) * 280);

                    $(this).filter(':visible').find('td').each(function (index, data) {
                        if ($(this).css('display') != 'none') {
                            if (defaults.ignoreColumn.indexOf(index) == -1) {
                                var colPosition = startColPosition + (index * 50);
                                doc.text(colPosition, rowPosition, parseString($(this)));
                            }
                        }

                    });

                });

                // Output as Data URI
                doc.output('datauri');

            }


            function parseString(data) {
                var contentData;
                if (defaults.htmlContent == 'true') {
                    contentData = data.html().trim();
                } else {
                    contentData = data.text().trim();
                }

                if (defaults.escape == 'true') {
                    contentData = escape(contentData);
                }



                return contentData;
            }

            function exportFile(dataurl) {
                var id = 'dlinkExport';
                var link = document.getElementById(id);
                if (link == null) {
                    var a = document.createElement('a');
                    a.style = "display:none;";
                    a.id = "dlinkExport";
                    document.body.appendChild(a);
                    link = a;
                }
                link.href = dataurl;

                var filename = prompt("File name", defaults.fileName);

                if (filename != null) {
                    if (filename.indexOf('.') == -1)
                        defaults.fileName = filename + '.' + defaults.type;
                    link.download = defaults.fileName;
                    link.click();
                } else {
                    link.download = defaults.fileName;
                    link.click();
                }
            }
        }
    });

    $.fn.wordExport = function (fileName, cssfiles) {
        cssfiles = [
            '/Content/themes/metro/metro-bootstrap.css',
            '/Content/themes/metro/metro-bootstrap-responsive.css',
            '/Content/themes/metro/metro-bootstap-custom.css'
        ];

        fileName = prompt("File name", fileName);

        fileName = typeof fileName !== 'undefined' ? fileName : "BaoCao";
        var jstatic = {
            mhtml: {
                top: "Mime-Version: 1.0\nContent-Base: " + location.href + "\nContent-Type: Multipart/related; boundary=\"NEXT.ITEM-BOUNDARY\";type=\"text/html\"\n\n--NEXT.ITEM-BOUNDARY\nContent-Type: text/html; charset=\"utf-8\"\nContent-Location: " + location.href + "\n\n<!DOCTYPE html>\n<html>\n_html_</html>",
                head: "<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n<style>\n_styles_\n</style>\n</head>\n",
                body: "<body>_body_</body>"
            }
        };
        var options = {
            maxWidth: 840
        };
        // Clone selected element before manipulating it
        var $this = $(this);
        $this.find(':hidden').addClass('markedForRemoval');
        var markup = $(this).clone();
        $this.find('.markedForRemoval').removeClass('markedForRemoval');

        // Remove hidden elements from the output
        markup.find('.markedForRemoval').remove();

        // Embed all images using Data URLs
        var images = Array();
        var img = markup.find('img');
        for (var i = 0; i < img.length; i++) {
            // Calculate dimensions of output image
            var w = Math.min(img[i].width, options.maxWidth);
            var h = img[i].height * (w / img[i].width);
            // Create canvas for converting image to data URL
            $('<canvas>').attr("id", "jQuery-Word-export_img_" + i).width(w).height(h).insertAfter(img[i]);
            var canvas = document.getElementById("jQuery-Word-export_img_" + i);
            canvas.width = w;
            canvas.height = h;
            // Draw image to canvas
            var context = canvas.getContext('2d');
            context.drawImage(img[i], 0, 0, w, h);
            // Get data URL encoding of image
            var uri = canvas.toDataURL();
            $(img[i]).attr("src", img[i].src);
            img[i].width = w;
            img[i].height = h;
            // Save encoded image to array
            images[i] = {
                type: uri.substring(uri.indexOf(":") + 1, uri.indexOf(";")),
                encoding: uri.substring(uri.indexOf(";") + 1, uri.indexOf(",")),
                location: $(img[i]).attr("src"),
                data: uri.substring(uri.indexOf(",") + 1)
            };
            // Remove canvas now that we no longer need it
            canvas.parentNode.removeChild(canvas);
        }

        // Prepare bottom of mhtml file with image data
        var mhtmlBottom = "\n";
        for (var i = 0; i < images.length; i++) {
            mhtmlBottom += "--NEXT.ITEM-BOUNDARY\n";
            mhtmlBottom += "Content-Location: " + images[i].contentLocation + "\n";
            mhtmlBottom += "Content-Type: " + images[i].contentType + "\n";
            mhtmlBottom += "Content-Transfer-Encoding: " + images[i].contentEncoding + "\n\n";
            mhtmlBottom += images[i].contentData + "\n\n";
        }
        mhtmlBottom += "--NEXT.ITEM-BOUNDARY--";


        getCss(cssfiles, function (styles) {

            // Aggregate parts of the file together 
            var fileContent = jstatic.mhtml.top.replace("_html_", jstatic.mhtml.head.replace("_styles_", styles) + jstatic.mhtml.body.replace("_body_", markup.html())) + mhtmlBottom;

            // Create a Blob with the file contents
            var blob = new Blob([fileContent], {
                type: "application/msword;charset=utf-8"
            });
            common.saveAs(blob, fileName + ".doc");
        });

    };

    $.fn.excelExport = function (fileName, cssfiles) {
        cssfiles = [
            '/Content/themes/metro/metro-bootstrap.css',
            '/Content/themes/metro/metro-bootstrap-responsive.css',
            '/Content/themes/metro/metro-bootstap-custom.css'
        ];

        fileName = prompt("File name", fileName);

        fileName = typeof fileName !== 'undefined' ? fileName : "BaoCao";
        var jstatic = {
            mhtml: {
                top: "Mime-Version: 1.0\nContent-Base: " + location.href + "\nContent-Type: Multipart/related; boundary=\"NEXT.ITEM-BOUNDARY\";type=\"text/html\"\n\n--NEXT.ITEM-BOUNDARY\nContent-Type: text/html; charset=\"utf-8\"\nContent-Location: " + location.href + "\n\n<!DOCTYPE html>\n<html>\n_html_</html>",
                head: "<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n<style>\n_styles_\n</style>\n</head>\n",
                body: "<body>_body_</body>"
            }
        };
        var options = {
            maxWidth: 840
        };
        // Clone selected element before manipulating it
        var $this = $(this);
        $this.find(':hidden').addClass('markedForRemoval');
        var markup = $(this).clone();
        $this.find('.markedForRemoval').removeClass('markedForRemoval');

        // Remove hidden elements from the output
        markup.find('.markedForRemoval').remove();

        // Embed all images using Data URLs
        var images = Array();
        var img = markup.find('img');
        for (var i = 0; i < img.length; i++) {
            // Calculate dimensions of output image
            var w = Math.min(img[i].width, options.maxWidth);
            var h = img[i].height * (w / img[i].width);
            // Create canvas for converting image to data URL
            $('<canvas>').attr("id", "jQuery-Word-export_img_" + i).width(w).height(h).insertAfter(img[i]);
            var canvas = document.getElementById("jQuery-Word-export_img_" + i);
            canvas.width = w;
            canvas.height = h;
            // Draw image to canvas
            var context = canvas.getContext('2d');
            context.drawImage(img[i], 0, 0, w, h);
            // Get data URL encoding of image
            var uri = canvas.toDataURL();
            $(img[i]).attr("src", img[i].src);
            img[i].width = w;
            img[i].height = h;
            // Save encoded image to array
            images[i] = {
                type: uri.substring(uri.indexOf(":") + 1, uri.indexOf(";")),
                encoding: uri.substring(uri.indexOf(";") + 1, uri.indexOf(",")),
                location: $(img[i]).attr("src"),
                data: uri.substring(uri.indexOf(",") + 1)
            };
            // Remove canvas now that we no longer need it
            canvas.parentNode.removeChild(canvas);
        }

        // Prepare bottom of mhtml file with image data
        var mhtmlBottom = "\n";
        for (var i = 0; i < images.length; i++) {
            mhtmlBottom += "--NEXT.ITEM-BOUNDARY\n";
            mhtmlBottom += "Content-Location: " + images[i].contentLocation + "\n";
            mhtmlBottom += "Content-Type: " + images[i].contentType + "\n";
            mhtmlBottom += "Content-Transfer-Encoding: " + images[i].contentEncoding + "\n\n";
            mhtmlBottom += images[i].contentData + "\n\n";
        }
        mhtmlBottom += "--NEXT.ITEM-BOUNDARY--";

        getCss(cssfiles, function (styles) {

            // Aggregate parts of the file together 
            var fileContent = jstatic.mhtml.top.replace("_html_", jstatic.mhtml.head.replace("_styles_", styles) + jstatic.mhtml.body.replace("_body_", markup.html())) + mhtmlBottom;

            // Create a Blob with the file contents
            var blob = new Blob([fileContent], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            common.saveAs(blob, fileName + ".xls");
        });

    };

    function getCss(cssfiles, callback)
    {
        var callAjax = [], styles = [];
        for (var i = 0; i < cssfiles.length; i++) {
            callAjax.push($.get(
                common.moduleUrl(cssfiles[i]),
                function (content) {
                styles.push(content);
            }));
        }
        $.when.apply(null, callAjax).then(function () {
            var cssContent = styles.join('\n');
            callback(cssContent);
        });
    }
})(jQuery);
