var column = new Array();

$(function () {
    GetModelInfo();
});

function GetModelInfo() {
    $.ajax({
        type: "post",
        url: "GetModelInfo",
        dataType: "json",
        success: function (data) {
            GetColumns(eval(data));
            layui.use(["table", "layer"], function () {
                var table = layui.table;
                var layer = layui.layer;
                var load = layer.load(1);
                var tableOption = {
                    elem: "#baseTable",
                    //height: 100 %,
                    url: "List",
                    loading: true,
                    toolbar: "#baseTableToolBar",
                    defaultToolbar: false,
                    cellMinWidth: 50,
                    page: true,
                    limits: [10, 20, 30, 40],
                    text: "未加载到数据",
                    cols: [column],
                    done: function (res) {
                        layer.close(load);
                    }
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
                                //                                maxmin: true, //开启最大化最小化按钮
                                area: ["100%", "100%"],
                                content: "Add?" + column[0].field + "=",
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
                                    //                                    maxmin: true, //开启最大化最小化按钮
                                    area: ["100%", "100%"],
                                    content: "Detail?" + column[0].field + "=" + data[column[0].field],
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
                                        url: "Delete?" + column[0].field + "=" + data[column[0].field],
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
                                    //                                    maxmin: true, //开启最大化最小化按钮
                                    area: ["100%", "100%"],
                                    resize: false,
                                    content: "Add?" + column[0].field + "=" + data[column[0].field],
                                    //                                    full: function (layero) {
                                    //                                        layer.iframeAuto(layer.getFrameIndex(window[layero.find('iframe')[0]['name']].name));
                                    //                                    },
                                    //                                    restore: function (layero) {
                                    //                                        //                                        var index = layer.getFrameIndex(window[layero.find('iframe')[0]['name']].frameElement.name);
                                    //                                        //                                        layer.style(index,
                                    //                                        //                                            {
                                    //                                        //                                                height: '500px'
                                    //                                        //                                            });
                                    //                                        window[layero.find('iframe')[0]['name']].frameElement.height = "500px";
                                    //                                    },
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
        }
    });
}

function GetColumns(data) {
    var obj;
    for (var i = 0; i < data.length; i++) {
        obj = new Object();
        obj.field = data[i].name;
        obj.title = data[i].value;
        obj.sort = data[i].isSort;
        column.push(obj);
    }
    obj = new Object();
    obj.title = "操作";
    obj.toolbar = "#colToolBar";
    column.push(obj);
}