layui.use(['table', 'layer'], function () {
    var table = layui.table;
    var layer = layui.layer;
    table.render({
        elem: "#baseTable",
        height: 500,
        url: "List",
        toolbar: "#baseTableToolBar",
        defaultToolbar: false,
        cellMinWidth: 50,
        page: true,
        limits: [10, 20, 30, 40],
        text: "未加载到数据",
        cols: [[ //表头
            { field: 'Material_Id', title: '器材编号', width: 80 }
            , { field: 'Material_Name', title: '器材名称', width: 80 }
            , { field: 'Material_Count', title: '器材数量', width: 80, sort: true }
            , { field: 'Material_Remain_Count', title: '剩余数量', width: 177, sort: true }
            , { field: 'Material_Count_Unit', title: '单位', width: 80 }
            , { field: 'Material_Remark', title: '备注', width: 135 }
        ]]
    });

    table.on('toolbar(baseTable)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'Add':
                layer.open({
                    title: "添加器材",
                    type: 2,
                    area: ['400px', '550px'],
                    content: "Add"
                });
                break;
        };
    });
});