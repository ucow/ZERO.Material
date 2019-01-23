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
            treeLinkage: true,       // treetable新增参数
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
                case "Add":
                    layer.open({
                        title: "添加器材",
                        type: 2,
                        shadeClose: true,
                        shade: false,
                        //                                maxmin: true, //开启最大化最小化按钮
                        area: ["100%", "100%"],
                        content: "Add?Material_Type_Id=",
                        end: function () {
                            renderTable();
                        }
                    });
                    break;
                case 'open':
                    treetable.expandAll('#baseTable');
                    break;
                case 'close':
                    treetable.foldAll('#baseTable');
                    break;
            };
        });

    table.on("tool(baseTable)",
        function (obj) {
            var data = obj.data;
            switch (obj.event) {
                case "Detail":
                    layer.open({
                        title: "器材详情",
                        type: 2,
                        shadeClose: true,
                        shade: false,
                        //                                    maxmin: true, //开启最大化最小化按钮
                        area: ["100%", "100%"],
                        content: "Detail?Material_Type_Id=" + data["Material_Type_Id"],
                        end: function () {
                            //table.reload("baseTable", tableOption);
                            //layer.msg("校徽");
                        }
                    });
                    break;
                case "Delete":
                    layer.confirm('确认删除？', function (index) {
                        layer.close(index);
                        //"{\"" + column[0].field + "\":\"" + data[column[0].field] + "\"}";
                        $.ajax({
                            type: "post",
                            url: "Delete?Material_Type_Id=" + data["Material_Type_Id"],
                            success: function (data) {
                                if (data === "OK") {
                                    layer.msg("删除成功");
                                    renderTable();
                                } else {
                                    layer.msg("删除失败");
                                }
                            }
                        });
                    });
                    break;
                case "Update":
                    layer.open({
                        title: "更新器材",
                        type: 2,
                        shadeClose: true,
                        shade: false,
                        //                                    maxmin: true, //开启最大化最小化按钮
                        area: ["100%", "100%"],
                        resize: false,
                        content: "Add?Material_Type_Id=" + data["Material_Type_Id"],
                        end: function () {
                            renderTable();
                        }
                    });
                    break;
                default:
            }
        });
});