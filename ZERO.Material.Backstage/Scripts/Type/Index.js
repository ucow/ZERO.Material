layui.config({
    base: '../Content/module/'
}).extend({
    treetable: 'treetable-lay/treetable'
}).use(['layer', 'table', 'treetable'], function () {
    var $ = layui.jquery;
    var table = layui.table;
    var layer = layui.layer;
    var treetable = layui.treetable;

    var renderTable = function () {
        layer.load(2);
        treetable.render({
            treeColIndex: 1,          // treetable新增参数
            treeSpid: -1,             // treetable新增参数
            treeIdName: 'Material_Type_Id',       // treetable新增参数
            treePidName: 'Material_Type_Parent_Id',     // treetable新增参数
            treeDefaultClose: true,   // treetable新增参数
            treeLinkage: true,        // treetable新增参数
            elem: '#baseTable',
            url: 'List',
            toolbar: '#baseTableToolBar',
            cols: [[
                { field: 'Material_Type_Id', title: '类别编号' },
                { field: 'Material_Type_Name', title: '类别名称' },
                { field: 'Material_Type_Remark', title: '备注' },
                { toolbar: '#colToolBar', title: '操作' }
            ]],
            page: false,
            done: function () {
                layer.closeAll('loading');
            }
        });
    };

    renderTable();

    table.on('toolbar(baseTable)',
        function (obj) {
            var checkStatus = table.checkStatus(obj.config.id);
            switch (obj.event) {
                case 'add':
                    layer.msg('添加');
                    break;
                case 'open':
                    treetable.expandAll('#baseTable');
                    break;
                case 'close':
                    treetable.foldAll('#baseTable');
                    break;
            };
        });
});

function AddClick() {
    alert(this.attr('id'));
}

function Expand() {
    treetable.expandAll('#baseTable');
}