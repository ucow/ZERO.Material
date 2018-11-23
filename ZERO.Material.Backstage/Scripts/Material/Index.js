layui.use(["table", "layer"], function () {
    var table = layui.table;
    var layer = layui.layer;
    var tableOption = {
        elem: "#baseTable",
        //height: 100 %,
        url: "List",
        toolbar: "#baseTableToolBar",
        defaultToolbar: false,
        cellMinWidth: 50,
        page: true,
        limits: [10, 20, 30, 40],
        text: "未加载到数据",
        cols: [
            [//表头
                { field: "Material_Id", title: "器材编号" },
                { field: "Material_Name", title: "器材名称" },
                { field: "Material_Count", title: "器材数量", sort: true },
                { field: "Material_Remain_Count", title: "剩余数量", sort: true },
                { field: "Material_Count_Unit", title: "单位" },
                { field: "Material_Remark", title: "备注" },
                { toolbar: "#colToolBar", align: "center", title: "操作" }
            ]
        ]
    };
    table.render(tableOption);

    table.on("toolbar(baseTable)", function (obj) {
        //var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case "Add":
                layer.open({
                    title: "添加器材",
                    type: 2,
                    shadeClose: true,
                    shade: false,
                    maxmin: true, //开启最大化最小化按钮
                    area: ["400px", "550px"],
                    content: "Add?materialId=''",
                    end: function () {
                        table.reload("baseTable", tableOption);
                        //layer.msg("校徽");
                    }
                });
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
                        maxmin: true, //开启最大化最小化按钮
                        area: ["400px", "550px"],
                        content: "Detail?materialId=" + data.Material_Id,
                        end: function () {
                            //table.reload("baseTable", tableOption);
                            //layer.msg("校徽");
                        }
                    });
                    break;
                case "Delete":
                    layer.confirm('确认删除？', function (index) {
                        layer.close(index);
                        $.ajax({
                            data: { materialId: data.Material_Id },
                            type: "post",
                            url: "Delete",
                            success: function (data) {
                                if (data === "OK") {
                                    layer.msg("删除成功");
                                    table.reload("baseTable", tableOption);
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
                        maxmin: true, //开启最大化最小化按钮
                        area: ["400px", "550px"],
                        content: "Add?materialId=" + data.Material_Id,
                        end: function () {
                            table.reload("baseTable", tableOption);
                            //layer.msg("校徽");
                        }
                    });
                    break;
                default:
            }
        });
});