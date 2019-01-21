layui.use(['form', 'layedit', 'upload'], function () {
    var form = layui.form;
    var layedit = layui.layedit;
    var upload = layui.upload;

    var ieditor = layedit.build('remark', {
        //插入代码设置
        codeConfig: {
            hide: true,  //是否显示编码语言选择框
            default: 'javascript' //hide为true时的默认语言格式
        }
        , tool: [
            'strong' //加粗
            , 'italic' //斜体
            , 'underline' //下划线
            , 'del' //删除线
            , '|' //分割线
            , 'left' //左对齐
            , 'center' //居中对齐
            , 'right' //右对齐
            , 'link' //超链接
            , 'unlink' //清除链接
        ],
        height: '90%'
    });
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

    form.on('select(grand)', function (obj) {
        var typeId = obj.value;
        getChildType('#father', typeId);
        $("#son")[0].innerHTML = "<option value=\"\">请选择</option>";
    });
    form.on('select(father)', function (obj) {
        var typeId = obj.value;
        getChildType('#son', typeId);
    });

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
    form.render('select');
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

String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof args == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        } else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出

                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
};