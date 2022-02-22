'use strict';

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

function formatNumeric(data) {
    if (!data) return data;
    return data.toLocaleString(undefined, {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

var FormSerialize = function () {
    return {
        getFormArray: function getFormArray($form, data) {
            var unindexed_array = $form.serializeArray();

            var indexed_array = data == null ? {} : data;

            var indexed_array2 = new Array();

            for (var unindex in unindexed_array) {
                if (indexed_array2.length == 0) indexed_array2.push(unindexed_array[unindex]);
                for (var index in indexed_array2) {
                    //check object.name ที่ซ้ำกันเพื่อไม่ให้ save ลงตัวแปรที่ไว้กรอง data
                    if (indexed_array2[index].name == unindexed_array[unindex].name) {
                        break;
                    } else if (index == indexed_array2.length - 1) {
                        indexed_array2.push(unindexed_array[unindex]);
                        break;
                    }
                    //end
                }
            }

            //map array เป็น object เพือนำไป postdata
            $.map(indexed_array2, function (n, i) {
                indexed_array[n['name']] = n['value'];
            });
            //end

            return indexed_array;
        },

        getFormStackingObjectsArray: function getFormStackingObjectsArray($form, data, objName) {

            var unindexed_array = $form.serializeArray();

            var indexed_array2 = new Array();

            if (objName != null) {
                $.map(data, function (mval, mname) {
                    if (typeof mval === '[object Array]') {
                        console.log('ok');
                    } else {
                        if ((typeof mval === 'undefined' ? 'undefined' : _typeof(mval)) === 'object') {
                            $.map(mval, function (sval, sname) {
                                if ((typeof sval === 'undefined' ? 'undefined' : _typeof(sval)) === 'object') {
                                    $.map(sval, function (val, name) {
                                        unindexed_array.push({ name: objName + '.' + mname + '[' + sname + '].' + name, value: val });
                                    });
                                } else unindexed_array.push({ name: objName + '.' + mname + '.' + sname, value: sval });
                            });
                        } else unindexed_array.push({ name: objName + '.' + mname, value: mval });
                    }
                });
            }

            return unindexed_array;
        }
    };
}();

String.prototype.replaceArray = function (find, replace) {
    var replaceString = this;
    if (!replaceString) return replaceString;
    var regex;
    for (var i = 0; i < find.length; i++) {
        replaceString = replaceString.replace(find[i], replace[i]);
    }
    return replaceString;
};

var inputName = [];

$(document).ready(function () {
    jQuery.ajaxSettings.traditional = true;

    var x = window.matchMedia("(max-width: 767px)");
    reSizeSm(x);
    x.addListener(reSizeSm);

    window.onbeforeunload = function (event) {
        loading.start();
    };

    $.event.special.inputchange = {
        setup: function setup() {
            var self = this,
                val;
            $.data(this, 'timer', window.setInterval(function () {
                val = self.value;
                if ($.data(self, 'cache') != val) {
                    $.data(self, 'cache', val);
                    $(self).trigger('inputchange');
                }
            }, 20));
        },
        teardown: function teardown() {
            window.clearInterval($.data(this, 'timer'));
        },
        add: function add() {
            $.data(this, 'cache', this.value);
        }

        //datetimepickerInit()
        //select2Init()
    };

    if ($.fn.dataTable) {
        $.extend($.fn.dataTable.defaults, {
            "lengthMenu": [10, 20, 50, 100],
            language: {
                "lengthMenu": 'Show <select class="custom-select custom-select-sm form-control form-control-sm">' +
                    '<option value="10">10</option>' +
                    '<option value="20">20</option>' +
                    '<option value="50">50</option>' +
                    '<option value="100">100</option>' +
                    '</select> records'
            }
        });
    }

    //
    // Pipelining function for DataTables. To be used to the `ajax` option of DataTables
    //
    $.fn.dataTable.pipeline = function (opts) {
        // Configuration options
        var conf = $.extend({
            pages: 5,     // number of pages to cache
            url: '',      // script url
            data: null,   // function or object with parameters to send to the server
            // matching how `ajax.data` works in DataTables
            method: 'GET' // Ajax HTTP method
        }, opts);

        // Private variables for storing the cache
        var cacheLower = -1;
        var cacheUpper = null;
        var cacheLastRequest = null;
        var cacheLastJson = null;

        return function (request, drawCallback, settings) {
            var ajax = true;
            var requestStart = request.start;
            var drawStart = request.start;
            var requestLength = request.length;
            var requestEnd = requestStart + requestLength;

            if (requestStart < cacheLower) {
                requestStart = requestStart - (requestLength * (conf.pages - 1));

                if (requestStart < 0) {
                    requestStart = 0;
                }
            }

            cacheLower = requestStart;
            //cacheUpper = requestStart + (requestLength * conf.pages);

            request.start = requestStart;
            //request.length = requestLength * conf.pages;

            // Provide the same `data` options as DataTables.
            if (typeof conf.data === 'function') {
                // As a function it is executed with the data object as an arg
                // for manipulation. If an object is returned, it is used as the
                // data object to submit
                var d = conf.data(request);
                if (d) {
                    $.extend(request, d);
                }
            }
            else if ($.isPlainObject(conf.data)) {
                // As an object, the data given extends the default
                $.extend(request, conf.data);
            }

            return $.ajax({
                "type": conf.method,
                "url": conf.url,
                "data": request,
                "dataType": "json",
                "cache": false,
                "success": function (json) {
                    json = $.extend(true, {}, json);
                    json.draw = request.draw; // Update the echo for each response

                    if (cacheLower != drawStart) {
                        //json.data.splice(0, drawStart - cacheLower);

                        json.data.splice(0, requestStart - cacheLower);
                    }
                    if (requestLength >= -1) {
                        json.data.splice(requestLength, json.data.length);
                    }

                    drawCallback(json);
                    //cacheLastJson = $.extend(true, {}, json);

                    //if (cacheLower != drawStart) {
                    //    json.data.splice(0, drawStart - cacheLower);
                    //}
                    //if (requestLength >= -1) {
                    //    json.data.splice(requestLength, json.data.length);
                    //}

                    //drawCallback(json);
                }
            });
        }
    };

    // Register an API method that will empty the pipelined data, forcing an Ajax
    // fetch on the next draw (i.e. `table.clearPipeline().draw()`)
    $.fn.dataTable.Api.register('clearPipeline()', function () {
        return this.iterator('table', function (settings) {
            settings.clearCache = true;
        });
    });

    //
    // DataTables initialisation
    //
    //$(document).ready(function () {
    //    $('#example').DataTable({
    //        "processing": true,
    //        "serverSide": true,
    //        "ajax": $.fn.dataTable.pipeline({
    //            url: 'scripts/server_processing.php',
    //            pages: 5 // number of pages to cache
    //        })
    //    });
    //});

    (function ($) {
        var originalVal = $.fn.val;
        inputName = [];


        $.fn.val = function (value) {

            if (arguments.length >= 1) {
                // setter invoked, do processing
                if (value && $.isNumeric(value) && +value >= 1000 && inputName.indexOf(this.attr('name')) > -1) value = value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                return originalVal.call(this, value);
            }
            if (originalVal.call(this) && originalVal.call(this).indexOf(',') > -1) return originalVal.call(this).replace(/\,/g, '');
            //getter invoked do processing
            return originalVal.call(this);
        };
    })(jQuery);
});

function createPopup(_width, result) {
    var _height = "auto";
    if (_width > $(window).width()) {
        _width = $(window).width();
    }

    if (_height > $(window).height()) {
        _height = $(window).height();
    }
    $("body").append('<div id="popup_normal" class="popup-dialog">' + result + '</div>');

    $("#popup_normal").dialog({
        autoOpen: false,
        height: _height,
        width: _width,
        title: "",
        modal: true,
        buttons: {
            Close: {
                'class': 'dialog-close',
                text: 'Close',
                click: function () {
                    $(this).dialog("close");
                }
            }
        },
        open: function (event, ui) {
            RemovePreload();
        },
        close: function () {
            $(this).dialog("destroy").remove();

        }
    });

    $("#popup_normal").dialog("open");
}

function createPopupPrint(_title, result) {
    $("body").append('<div id="popup_print" class="popup-dialog">' + result + '</div>');

    $("#popup_print").dialog({
        autoOpen: false,
        height: "auto",
        width: 780,
        title: _title,
        modal: true,
        buttons: {
            Ok: {
                'class': 'dialog-close printModalPopup',
                //'data-href': _link.data("printurl"),
                text: 'Print',
                click: function () {
                    //$(this).dialog("close");
                }
            }
        },
        open: function (event, ui) {
            RemovePreload();
        },
        close: function () {
            $(this).dialog("destroy").remove();

        }
    });

    $("#popup_print").dialog("open");
}

$(document).on("click", ".printModalPopup", function () {
    var _this = $(this);

    if ($("#prePrint").length <= 0) {
        $("<iframe id='prePrint'>").hide().appendTo(document.getElementById('popup_print').innerHTML);
    }

    $("#prePrint").appendTo(document.getElementById('popup_print').innerHTML);

    print(document.getElementById('popup_print').innerHTML);

    return false;
});

function print(data) {
    var contents = data;

    var frame1 = document.createElement('iframe');

    frame1.name = "frame1";
    frame1.style.display = 'none';
    document.body.appendChild(frame1);

    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;

    frameDoc.document.open();

    frameDoc.document.write(contents);

    frameDoc.document.close();

    frameDoc.print();
}

function reSizeSm(x) {
    if (x.matches) {
        $('.hidden-xs-more').addClass('readmore hidden');
    } else {
        $('.hidden-xs-more').removeClass('readmore hidden');
    }
}

function commaFormSubmit(myForm) {
    myForm.submit(function (event) {
        inputName.forEach(function (name) {
            if (document.getElementsByName(name)[0]) document.getElementsByName(name)[0].value = $("[name='" + name + "']").val();
        });
        return true;
    });
}

function warningAlert() {
    swal({
        icon: "warning",
        button: "ตกลง",
        allowOutsideClick: false,
        closeOnClickOutside: false
    })
}

function warningMsgAlert(massage) {
    swal({
        text: massage,
        icon: "warning",
        button: "ตกลง",
        allowOutsideClick: false,
        closeOnClickOutside: false
    })
}

function successAlert() {
    swal({
        icon: 'success',
        timer: 1000,
    })
}

function successMsgAlert(massage) {
    swal({
        text: massage,
        icon: 'success',
        timer: 1000,
    })
}

function successMsgAlertdUrl(massage, url) {
    swal({
        text: massage,
        icon: 'success',
        type: "success"
    }).then(function () {

        window.location.href = url;
    });
}

function confirmAlert(massage, functional) {
    swal({
        text: massage,
        icon: "warning",
        buttons: true,
        buttons: [
            'ยกเลิก', 'ตกลง'
        ],
        allowOutsideClick: false,
        closeOnClickOutside: false
    }).then(functional)

}

function confirmHTMLAlert(html, functional) {
    swal({
        content: html,
        icon: "warning",
        buttons: true,
        buttons: [
            'ยกเลิก', 'ตกลง'
        ],
        allowOutsideClick: false,
        closeOnClickOutside: false
    }).then(functional)

}

function confirmDelete(massage, functional) {
    swal({
        text: massage,
        icon: "warning",
        buttons: true,
        buttons: [
            'ยกเลิก', 'ตกลง'
        ],
        allowOutsideClick: false,
        closeOnClickOutside: false
    }).then(functional)
}

function downloadAlert(message) {
    swal({
        html: true,
        icon: "warning",
        text: message,
        buttons: {
            cancel: "Cancel",
            catch: {
                text: "Download",
                value: "download",
            },
        },
    })
        .then((value) => {
            switch (value) {
                case "download":
                    swal({
                        html: true,
                        title: 'Download Excel',
                        icon: 'success',
                    });
                    //var objButton = document.getElementById("btnExport");
                    //objButton.click();
                    $.post("ExportToExcelSheet").done(function (data) {
                        document.getElementById('my_iframeFail_Update').src = "Download/?file=" + data.fileName;
                    });
                    break;

                default:
                    swal({
                        text: "กรุณาเลือกข้อมูลไฟล์เพื่อทำการนำข้อมูลเข้าใหม่อีกครั้ง",
                        icon: '../Content/img/ICON/warning-icon.png'
                    });
                    //swal("กรุณาเลือกข้อมูลไฟล์เพื่อทำการนำข้อมูลเข้าใหม่อีกครั้ง");
                    return false;
            }
        });
}