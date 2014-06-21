
$(function () {

    $("#dataGrid").jqGrid({
        url: '/api/query',
        datatype: 'json',
        mtype: 'GET',
        colModel: [
            { name: "fld_Timestamp", width: 200, label: 'Timestamp' },
            { name: "fld_CallerId", width: 200, label: 'Caller ID' },
            { name: "acdExt", width: 100, label: 'Console Name' },
            { name: "username", width: 100, label: 'Employee Name' },
        ],
        pager: "#dataGridPager",
        autowidth: true,
        height: 'auto',
        rowNum: 20,
        sortname: "fld_timestamp",
        sortorder: "asc",
        viewrecords: true,
        gridview: true,
        autoencode: true,
        caption: "Example jQuery Grid",
        gridComplete: function(e) {
            console.log('Grid Complete');
            console.log(e);
        },
        loadComplete: function (e) {
            console.log('Load Complete');
            console.log(e);
        }
    });

});

function getByCaller(callerid) {
    $("#dataGrid").jqGrid({
        url: '/api/query/' + callerid,
        datatype: 'json',
        mtype: 'GET',
        colModel: [
            { name: "fld_Timestamp", width: 200, label: 'Timestamp' },
            { name: "fld_CallerId", width: 200, label: 'Caller ID' },
            { name: "acdExt", width: 100, label: 'Console Name' },
            { name: "username", width: 100, label: 'Employee Name' },
        ],
        pager: "#dataGridPager",
        autowidth: true,
        height: 'auto',
        rowNum: 20,
        sortname: "fld_timestamp",
        sortorder: "asc",
        viewrecords: true,
        gridview: true,
        autoencode: true,
        caption: "Example jQuery Grid",
        gridComplete: function (e) {
            console.log('Grid Complete');
            console.log(e);
        },
        loadComplete: function (e) {
            console.log('Load Complete');
            console.log(e);
        }
    });
}
