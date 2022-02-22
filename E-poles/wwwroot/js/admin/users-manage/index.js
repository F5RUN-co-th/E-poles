'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var Users = function () {
    function Users() {
        _classCallCheck(this, Users);
        this.dtList = $('#gv_userslist');
        this.order = [[1, "desc"]];//<'domInput'>
        this.dom = "<'row'<'col-sm-12 col-md-4'l><'col-sm-12 col-md-8'<'domInput dataTables_filter'>>><'row'<'col-sm-12'tr>><'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>"
        this.searching = false;
        this.search = $('#searchForm');
        this.btnSearch = $('#btn_search');
    }
    /*
        l - length changing input control
        f - filtering input
        r - processing display element
        t - The table!
        i - Table information summary
        p - pagination control
     */
    _createClass(Users, [{
        key: 'init',
        value: function init() {
            var me = this;

            this.btnSearch.click(function () {

                me.dt.ajax.reload(function (json) {

                });
            });

            this.createDatatable();
        }
    },
    {
        key: 'createDatatable',
        value: function createDatatable() {

            var me = this;
            this.dt = this.dtList.DataTable({
                "dom": me.dom,
                "pageLength": 20,
                "processing": true, // for show progress bar
                "serverSide": true, // process server side
                "filter": true, // this is for disable filter (search box)
                //"order": me.order,
                //"orderMulti": false, // for disable multiple column at once    
                "searching": false,
                "ajax": {
                    "url": me.getUrl,
                    "type": "POST",
                    "contentType": 'application/json',
                    "data": function data(_data) {
                        var formValues = FormSerialize.getFormArray(me.search, _data);
                        return JSON.stringify(formValues);
                    }
                },
                "columnDefs": [{
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                }],
                "columns": [
                    { "data": "id", "name": "Id", "autoWidth": true },
                    { "data": "userName", "name": "Name", "autoWidth": true },
                    {
                        className: "text-center",
                        "data": "id",
                        "orderable": false,
                        "render": function (data, row) {
                            if (data) {
                                return '<a href=' + me.updateUrl + "/" + data + ' class="btn-shadow btn btn-info"><span class="btn-icon-wrapper"><i class="fa fa-edit fa-w-20"></i></span>แก้ไข</a>';
                            }
                        }
                    }
                ],
                "initComplete": function (settings, json) {
                    $("div.domInput").html($(".searchArea").removeAttr("hidden"));
                }
            });
        }
    },
    ]);


    return Users;
}();