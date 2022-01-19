'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EPoles = function () {
    function EPoles() {
        _classCallCheck(this, EPoles);
        this.dtList = $('#gv_poleslist');
        this.columns = null;
        this.selectDt = [{
            style: 'os',
            selector: 'td:first-child'
        }];
        this.columnDefs = [
            {
                orderable: false,
                className: 'select-checkbox',
                targets: 0
            }
        ];
        this.order = [[1, "desc"]];
        this.dom = '<"top"i>rt<"table-footer"<"col-sm-6"f<"pull-left"<"#extra">>><"col-sm-6"<"pull-right"lp>>>';
        this.searching = false;
        this.inputStartDate = $('.startDate');
        this.inputEndDate = $('.endDate');
        this.inputEffDate = $('.effDate');
        this.search = $('#searchForm');
        this.btnSearch = $('#btn_search');
    }

    _createClass(EPoles, [{
        key: 'init',
        value: function init() {
            var me = this;
            $('#gv_poleslist').dataTable({
            });
            //this.createDatatable();
        }
    },
    {
        key: 'createDatatable',
        value: function createDatatable() {

            var me = this;
            this.dt = this.dtList.DataTable({
                "ordering": false,
                "pageLength": 20,
                "processing": true,
                "serverSide": true,
                "searching": me.searching,
                "order": me.order,
                "ajax": {
                    "url": me.getUrl,
                    "type": "POST",
                    "contentType": 'application/json',
                    "data": function data(_data) {
                        var formValues = FormSerialize.getFormArray(me.search, _data);
                        return JSON.stringify(formValues);
                    }
                },
                "columns": [
                    { "data": "id", "name": "ID", "autoWidth": true },
                    { "data": "Name", "name": "Name", "autoWidth": true },
                    { "data": "Latitude", "name": "Latitude", "autoWidth": true },
                    { "data": "Longitude", "name": "Longitude", "autoWidth": true }
                ]
            });
        }
    },
    ]);


    return EPoles;
}();