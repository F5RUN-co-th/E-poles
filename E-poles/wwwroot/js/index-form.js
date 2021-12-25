'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var IndexForm = function () {
    function IndexForm() {
        _classCallCheck(this, IndexForm);

        this.Loading = $("#loadingDiv");
        this.dtList = null;
        this.columns = null;
        this.order = [[0, "desc"]];
        this.dom = null;
        this.searching = false;
        this.inputDate = $('.Date');
        this.inputStartDate = $('.startDate');
        this.inputEndDate = $('.endDate');
        this.search = $('#searchForm');
        this.btnSearch = $('#btn_search');
        this.getUrl = null;
        this.isOrdering = false;
    }

    _createClass(IndexForm, [{
        key: 'init',
        value: function init() {
            var me = this;

            this.createDatatable();

            this.initDatePicker();

            this.btnSearch.click(function () {
                me.dt.ajax.reload();
            });
        }
    }, {
        key: 'initDatePicker',
        value: function initDatePicker() {
            var me = this;

            if (this.inputDate.length > 0) {
                this.inputDate.datepicker({ dateFormat: "dd-mm-yy" });
            }
            if (this.inputStartDate.length > 0) {
                this.inputStartDate.datepicker({ dateFormat: "dd-mm-yy" });
            }
            if (this.inputEndDate.length > 0) {
                this.inputEndDate.datepicker({ dateFormat: "dd-mm-yy" });
            }
        }
    }, {
        key: 'createDatatable',
        value: function createDatatable() {
            var me = this;
            this.dt = this.dtList.DataTable({
                "dom": '<"top"i>rt<"table-footer"<"col-sm-6"<"pull-left"fl>><"col-sm-6"<"pull-right"p>>>',
                "pageLength": 20,
                "processing": true,
                "serverSide": true,
                "searching": me.searching,
                "ordering": me.isOrdering,
                "order": me.order,
                "responsive": true,
                "ajax": {
                    "url": me.getUrl,
                    "type": "POST",
                    "contentType": 'application/json',
                    "data": function data(_data) {
                        var formValues = FormSerialize.getFormArray(me.search, _data);
                        return JSON.stringify(formValues);
                    }
                },
                "columns": me.columns
            });

            this.dtList.on('click', 'a.del', function (e) {
                var obj = me.dt.row($(this).closest('tr')).data();
                confirmDelete("คุณต้องการที่จะยกเลิกข้อมูลนี้หรือไม่?", function (event) {
                    if (event) {
                        $.ajax({
                            type: 'POST',
                            url: me.delUrl,
                            data: JSON.stringify({ 'model': obj }),

                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function beforeSend() {
                                me.Loading.show();
                            },
                            success: function success(result) {
                                if (result > 0) {
                                    me.Loading.hide();
                                    successMsgAlert("ลบข้อมูลนี้สำเร็จ");
                                    setTimeout(function () { window.location.href = me.indexUrl }, 1000);
                                }
                                else {
                                    me.Loading.hide();
                                    warningMsgAlert("ไม่พบข้อมูล");
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                me.Loading.hide();
                                warningMsgAlert(thrownError);
                            }
                        });
                    }
                });

            });
        }
    }, {
        key: 'newEvent',
        value: function newEvent() {

        }
    }, {
        key: 'newEvent2',
        value: function newEvent2() {

        }
    }]);

    return IndexForm;
}();