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
                { field: "Company_Id", title: "公司编号" },
                { field: "Company_Name", title: "公司名称" },
                { field: "Company_Person", title: "公司负责人" },
                { field: "Company_Person_Phone", title: "负责人联系方式" },
                { field: "Company_Remark", title: "备注" },
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
                    title: "添加公司",
                    type: 2,
                    shadeClose: true,
                    shade: false,
                    maxmin: true, //开启最大化最小化按钮
                    area: ["400px", "550px"],
                    content: "Add?CompanyId=''",
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
                        title: "公司详情",
                        type: 2,
                        shadeClose: true,
                        shade: false,
                        maxmin: true, //开启最大化最小化按钮
                        area: ["400px", "550px"],
                        content: "Detail?CompanyId=" + data.Company_Id,
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
                            data: { CompanyId: data.Company_Id },
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
                        title: "更新公司",
                        type: 2,
                        shadeClose: true,
                        shade: false,
                        maxmin: true, //开启最大化最小化按钮
                        area: ["400px", "550px"],
                        content: "Add?CompanyId=" + data.Company_Id,
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