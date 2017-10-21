(function ($) {
    $.fn.extend({
        showAndHideColumns: function (options) {
            var defaults = {
                urlsaveconfig: null,
                urlgetconfig: null,
                useconfig: false,
                serversort: false,
                methodbinddata: '',
                name: null
            };

            options = $.extend(defaults, options);

            var self = {};
            var tdInActive = null,
                tdIndexInActive = -1;
            self.columns = [];
            self.table = this;
            self.tableId = null;

            self.tableId = self.table.attr('id');
            if (self.tableId == null) {
                self.tableId = 'table-' + Math.floor((Math.random() * 10) + 1);
                self.table.attr('id', self.tableId);
            }

            function init() {
                var children = $("#" + self.tableId + " thead tr").children();
                for (var i = 0; i < children.length; i++) {
                    var $this = $(children[i]),
                        colspan = $this.attr('colspan');
                    if (typeof colspan != 'undefined') {
                        if (parseInt(colspan) > 1) {
                            continue;
                        }
                    }
                    $this.hover(showOption);
                    $this.on('click', '.x-column-header-trigger', showOption2);
                    $this.on('click', '.x-column-a-choosen', showOption3);
                    $this.on('click', '.item-column-choosen', showAndHideColumn);
                    $this.on('click', '.x-column-a-sort', sortColumn);
                }

                getColumns(function () {
                    reRenderHeader();
                });

                $(document).mouseup(function (e) {
                    if (tdInActive == null)
                        return;
                    var container = tdInActive;
                    if (!container.is(e.target)
                        && container.has(e.target).length === 0) {
                        var $div = tdInActive.children("div.x-column-div-header");
                        $div.hide();
                        tdInActive = null;
                        tdIndexInActive = -1;
                    }
                });
            };

            function getColumns(callback) {
                var config = [];
                if (!options.useconfig) {
                    $('#' + self.tableId + ' thead tr').children().each(function (index) {
                        var text = $.trim($(this).text()),
                            style = '';
                        
                        self.columns.push({
                            Index: index,
                            Text: text,
                            Style: style,
                            Hidden: false
                        });
                    });
                    callback();
                }
                else {//get config from database
                    var url = options.urlgetconfig + '?name=' + options.name;
                    serviceInvoker.get(url, null, {
                        success: function (response) {
                            if (response.Data) {
                                try {
                                    config = $.parseJSON(response.Data.Value);
                                } catch (ex) {
                                    console.debug(ex);
                                }
                            }
                        },
                        error: function (response) {
                            console.debug(response);
                        },
                        complete: function (response) {
                            $('#' + self.tableId + ' thead tr').children().each(function (index) {
                                var text = $.trim($(this).text()),
                                    hidden = false,
                                    style = '';
                                if (config.length > 0) {
                                    for (var i = 0; i < config.length; i++) {
                                        if (config[i].Text == text) {
                                            hidden = config[i].Hidden;
                                            style = config[i].Style || '';
                                            break;
                                        }
                                    }
                                }
                                self.columns.push({
                                    Index: index,
                                    Text: text,
                                    Style: style,
                                    Hidden: hidden
                                });
                            });
                            callback();
                        }
                    }, null, null, true);
                }
            };

            function genCullumnsHtml(e) {
                var html = "<ul id='x-columns-ul-choosen' data-role='dropdown' class='x-dropdown-menu vinaphone'>";
                for (var i = 0; i < self.columns.length; i++) {
                    var disabled = i == tdIndexInActive;

                    html += '<li>';
                    html += "<a index=" + self.columns[i].Index;

                    if (disabled) {
                        html += " disabled='true' class='pointer disabled item-column-choosen'>";
                        html += "<span><input type='checkbox' disabled='disabled' index=" + self.columns[i].Index;
                        if (!self.columns[i].Hidden) {
                            html += " checked='checked'";
                        }
                        html += " />";
                        html += "&nbsp;&nbsp;" + self.columns[i].Text;
                        html += "</span>";
                    } else {
                        html += " class='pointer item-column-choosen'>";
                        html += "<span><input type='checkbox' index=" + self.columns[i].Index;
                        if (!self.columns[i].Hidden) {
                            html += " checked='checked'";
                        }
                        html += " />";
                        html += "&nbsp;&nbsp;" + self.columns[i].Text;
                        html += "</span>";
                    }

                    html += '</a>';
                    html += '</li>';
                }
                html += '</ul>';
                return html;
            };

            function genOptionHtml(e) {
                var html = "<div class='no-tablet-portrait no-phone x-column-div-header'>";
                html += "<div class='pointer element place-right'>";
                html += "<div class='x-column-header-trigger dropdown-toggle' role='presentation' data-ref='triggerEl'></div>";
                html += '</div>';
                html += '</div>';
                return html;
            };

            function genOptionHtml2(e) {
                var html = "<ul class='x-dropdown-menu inverse x-column-ul-header'>";
                html += '<li>';
                html += "<a class='pointer x-column-a-sort' value='desc'><span class='icon-arrow-down'></span>&nbsp;Giảm dần</a>";
                html += '</li>';
                html += '<li>';
                html += "<a class='pointer x-column-a-sort' value='asc'><span class='icon-arrow-up'></span>&nbsp;Tăng dần</a>";
                html += '</li>';
                html += '<li>';
                html += "<a class='pointer dropdown-toggle x-column-a-choosen'><span class='icon-grid'></span>&nbsp;Cột hiển thị</a>";
                html += '</li>';
                html += '</ul>';
                return html;
            };

            function showOption(e) {
                var $div;
                if (tdInActive != null) {
                    $div = tdInActive.children("div.x-column-div-header");
                    var $ul = tdInActive.find('ul.x-column-ul-header');
                    $ul.remove();
                    $div.hide();
                }

                tdInActive = $(this);
                tdIndexInActive = tdInActive.attr('index');

                $div = tdInActive.children("div.x-column-div-header");
                $div.show();
            };

            function showOption2(e) {
                var $this = $(e.target);
                var $div = $this.closest('div');
                var $ul = $div.children('ul.x-column-ul-header');
                if ($ul.length > 0) {
                    return false;
                }

                $div.append(genOptionHtml2());

                $ul = $div.children('ul.x-column-ul-header');
                if (tdInActive[0].getBoundingClientRect) {
                    var s = tdInActive[0].getBoundingClientRect();
                    $ul.css('margin-top', s.height - 1);
                } else {
                    $ul.css('margin-top', tdInActive.height() + 15);
                }

                var endingRight = ($(window).width() - ($this.offset().left + $this.outerWidth()));
                if (endingRight < 50) {
                    //set to left
                    $ul.css('right', 0);
                }
                $ul.show();
                return false;
            };

            function showOption3(e) {
                var $a = $(e.target);
                var $li = $a.closest('li');
                var $ul = $li.children('ul');
                if ($ul.length > 0) {
                    $ul.remove();
                }
                $li.append(genCullumnsHtml());
                $ul = $li.children('ul');

                var endingRight = ($(window).width() - ($a.offset().left + $a.outerWidth()));
                if (endingRight < 50) {
                    //set to left
                    $ul.css('right', '100%').css('left', '-100%');
                }
                $ul.show();
            };

            function findColumn(index) {
                if (self.columns.length == 0) {
                    return null;
                }
                for (var i = 0; i < self.columns.length; i++) {
                    if (self.columns[i].Index == index) {
                        return self.columns[i];
                    }
                }
                return null;
            };

            function reRenderHeader() {
                $("#" + self.tableId + " thead tr").children().each(function (index) {
                    
                    var $td = $(this),
                        sortKey = $td.attr('colname'),
                        column = findColumn(index),
                        html;
                    if (typeof sortKey != "undefined") {
                        html = '<coltext colname=' + sortKey + '>' + $td.html() + '</coltext>' + genOptionHtml();
                    } else {
                        html = '<coltext>' + $td.html() + '</coltext>' + genOptionHtml();
                    }
                    

                    $td.html(html);
                    $td.attr('index', index);
                    if (column) {
                        if (column.Style)
                            $td.attr('style', column.Style);

                        //an cot neu duoc cau hinh an
                        if (column.Hidden) {
                            $('#' + self.tableId + ' tr').find('th:eq(' + index + ')').hide();
                            $('#' + self.tableId + ' tr').find('td:eq(' + index + ')').hide();
                        }
                    }
                });
            };

            function showAndHideColumn(e) {
                var $a = $(e.currentTarget);
                if ($a.attr('disabled'))
                    return false;

                var $input = $a.find('input');
                var itemIndex = $input.attr('index');
                if (self.columns[itemIndex].Hidden) {
                    $input.prop('checked', true);
                    $input.attr('checked', 'checked');
                    self.columns[itemIndex].Hidden = false;
                    $('#' + self.tableId + ' tr').find('th:eq(' + itemIndex + ')').show();
                    $('#' + self.tableId + ' tr').find('td:eq(' + itemIndex + ')').show();
                } else {
                    $input.prop('checked', false);
                    $input.removeAttr('checked');
                    self.columns[itemIndex].Hidden = true;
                    $('#' + self.tableId + ' tr').find('th:eq(' + itemIndex + ')').hide();
                    $('#' + self.tableId + ' tr').find('td:eq(' + itemIndex + ')').hide();
                }

                if (options.useconfig) {
                    //save columns hide only
                    var columns = self.columns.filter(function (item) {
                        return item.Hidden == true;
                    });
                    var data = {
                        Name: options.name,
                        Value: $.JsObjectToJSON(columns)
                    }

                    serviceInvoker.post(options.urlsaveconfig, data, {
                        error: function (response) {
                            console.log(response);
                        }
                    }, null, null, true);
                }

                return false;
            };

            function checkIsTh($thead) {
                var $tr = $thead.children('tr');
                var $children = $tr.children('th');
                return $children.length > 0;
            }

            function sortColumn(e) {
                /// thuc hien sx table
                var $a = $(e.currentTarget),
                    $thead = $a.closest('thead'),
                    $coltext = null,
                    colname = null,
                    key = $a.attr('value');

                if (options.serversort) {

                    var isth = checkIsTh($thead);
                    if (isth) {
                        var $th = $a.closest('th');
                        $coltext = $th.children('coltext');
                    } else {
                        var $td = $a.closest('td');
                        $coltext = $td.children('coltext');
                    }
                    colname = $coltext.attr('colname');
                    //thuc hien sort data in server
                    if (options.methodbinddata != '') {
                        if (typeof colname != 'undefined') {
                            common.extentReportParams = common.FunctionTypeReport.Sort;
                            common.sortModel = {
                                SortType: key,
                                SortKey: colname
                            }

                            common.callBindReport();
                            return false;
                        }
                    }
                }
                var sort = key == 'asc' ? 1 : -1;
                var col = tdInActive.attr('index');

                var rows = $('#' + self.tableId + ' tbody tr'),
                    rlen = rows.length,
                    arr = new Array(),
                    i, j, cells, clen;
                // fill danh sach cells
                for (i = 0; i < rlen; i++) {
                    cells = rows[i].cells;
                    clen = cells.length;
                    arr[i] = new Array();
                    for (j = 0; j < clen; j++) {
                        arr[i][j] = $.getOuterHTML(cells[j]);
                    }
                }

                // sort danh sach rows
                arr.sort(function (a, b) {
                    var vala = $.trim($(a[col]).text()).replace(",", ".");
                    var valb = $.trim($(b[col]).text()).replace(",", ".");
                    if ($.isNumeric(vala) && $.isNumeric(valb)) {
                        try {
                            vala = parseFloat(vala);
                            valb = parseFloat(valb);
                        } catch (ex) {
                            console.log(ex);
                        }
                    }
                    return (vala == valb) ? 0 : ((vala > valb) ? sort : -1 * sort);
                });
                // replace existing rows with new rows created from the sorted array
                for (i = 0; i < rlen; i++) {
                    rows[i].innerHTML = arr[i].join('\n');
                }

                return false;
            };

            init();

            return self;
        }
    });
})(jQuery);