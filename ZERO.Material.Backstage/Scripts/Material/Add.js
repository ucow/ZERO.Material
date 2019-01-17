layui.use(['form', 'layedit', 'upload'], function () {
    var form = layui.form;
    var layedit = layui.layedit;
    var upload = layui.upload;
    layedit.build('remark'); //建立编辑器
    form.on('select(hc_select)', function (data) {   //选择移交单位 赋值给input框
        $("#HandoverCompany").val(data.value);
        $("#hc_select").next().find("dl").css({ "display": "none" });
        form.render();
    });
    //    var index = parent.layer.getFrameIndex(window.name);
    //    parent.layer.iframeAuto(index);

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

    var uploadInst = upload.render({
        elem: '#test1', //绑定元素
        accept: 'images',
        url: 'UploadImage',//上传接口
        acceptMime: "image/*",
        before: function (obj) {
            //预读本地文件示例，不支持ie8
            obj.preview(function (index, file, result) {
                $('#demo1').attr('src', result); //图片链接（base64）
            });
        },
        done: function (res, index, upload) {
            if (res.code === 0) {
                $('#imageBytes').val(res.data);
            }
        },
        error: function () {
            //演示失败状态，并实现重传
            var demoText = $('#demoText');
            demoText.html(
                '<span style="color: #FF5722;">上传失败</span> <a class="layui-btn layui-btn-xs demo-reload">重试</a>');
            demoText.find('.demo-reload').on('click',
                function () {
                    uploadInst.upload();
                });
        }
    });
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