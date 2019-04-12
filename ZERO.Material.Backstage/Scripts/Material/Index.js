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
            layui.use(["table", "layer", 'form'], function () {
                var table = layui.table;
                var layer = layui.layer;
                var form = layui.form;
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
                var tableIns = table.render(tableOption);

                // #region 筛选条件
                var typeId = "";
                var companyName = "";
                var materialName = "";
                // #endregion

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
                                    area: ["100%", "100%"],
                                    resize: false,
                                    content: "Add?" + column[0].field + "=" + data[column[0].field],
                                    end: function () {
                                        table.reload("baseTable", tableOption);
                                    }
                                });
                                break;
                            default:
                        }
                    });

                form.on('switch(show)', function (obj) {
                    layer.msg(this.value + ":" + obj.elem.checked);
                    $.ajax({
                        url: "SetMaterialShow",
                        type: "POST",
                        data: { 'id': this.value, 'check': obj.elem.checked },
                        success: function (data) {
                            if (data === "OK") {
                                layer.msg("设置成功");
                            } else {
                                layer.msg("设置失败");
                            }
                        }
                    });
                });

                form.on('select(grand)', function (obj) {
                    var typeId = obj.value;
                    getChildType('#father', typeId);
                    $("#son")[0].innerHTML = "<option value=\"\">请选择</option>";
                });
                form.on('select(father)', function (obj) {
                    var typeId = obj.value;
                    getChildType('#son', typeId);
                });

                form.on('select(son)',
                    function (obj) {
                        typeId = obj.value;
                        reloadTable();
                    });
                form.on('select(company)',
                    function(obj) {
                        companyName = obj.value;
                        reloadTable();
                    });

                $("#materialName").on("input", function (e) {
                    //获取input输入的值
                    materialName = e.delegateTarget.value;
                    reloadTable();
                });

                function reloadTable() {
                    tableIns.reload(
                        {
                            page: {
                                curr: 1
                            },
                            where: {
                                typeId: typeId,
                                companyName: companyName,
                                materialName:materialName
                            }
                        });
                }

                function getChildType(son, value) {
                    $.ajax({
                        url: "GetChildType",
                        type: "POST",
                        data: { "typeId": value },
                        success: function (data) {
                            if (data !== "") {
                                var value = JSON.parse(data);
                                $(son)[0].innerHTML = "<option value=\"\">请选择</option>";
                                var optionContent = "<option value='{0}'>{1}</option>";
                                value.forEach(function (currentValue, index, arr) {
                                    $(son)[0].innerHTML += optionContent.format(currentValue.Material_Type_Id,
                                        currentValue.Material_Type_Name);
                                }, this);
                                form.render();
                            }
                        }
                    });
                }
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
        if (data[i].name === "Is_Show") {
            obj.templet = "#show";
        }
        column.push(obj);
    }
    obj = new Object();
    obj.title = "操作";
    obj.toolbar = "#colToolBar";
    obj.fixed = 'right';
    column.push(obj);
}

