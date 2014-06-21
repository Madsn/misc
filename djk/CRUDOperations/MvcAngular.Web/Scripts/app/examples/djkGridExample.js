
$(function () {

    $("#dataGrid").jqGrid({
        url: '/api/query',
        datatype: 'json',
        mtype: 'GET',
        colModel: [
            { name: "timestamp", width: 200, label: 'Timestamp' },
            { name: "callerId", width: 200, label: 'Caller ID' },
            { name: "consoleName", width: 100, label: 'Console Name' },
            { name: "employeeName", width: 100, label: 'Employee Name' },
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
