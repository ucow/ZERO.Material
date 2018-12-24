layui.use(['form', 'layedit', 'upload'], function () {
    var form = layui.form;
    form.on('select(hc_select)', function (data) {   //选择移交单位 赋值给input框
        $("#HandoverCompany").val(data.value);
        $("#hc_select").next().find("dl").css({ "display": "none" });
        form.render();
    });

    window.search = function () {
        var value = $("#HandoverCompany").val();
        $("#hc_select").val(value);
        form.render();
        $("#hc_select").next().find("dl").css({ "display": "block" });
        var dl = $("#hc_select").next().find("dl").children();
        var j = -1;
        for (var i = 0; i < dl.length; i++) {
            if (dl[i].innerHTML.indexOf(value) <= -1) {
                dl[i].style.display = "none";
                j++;
            }
            if (j == dl.length - 1) {
                $("#hc_select").next().find("dl").css({ "display": "none" });
            }
        }
    }
});

function AfterSuccess(data) {
    var index = parent.layer.getFrameIndex(window.name);
    if (data.indexOf("失败") !== -1) {
        parent.layer.msg(data, { icon: 5 });
    } else {
        parent.layer.msg(data, { icon: 1 });
    }

    parent.layer.close(index);
}