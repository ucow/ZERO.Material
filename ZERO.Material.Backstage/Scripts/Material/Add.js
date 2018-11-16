//layui.use(['form', 'layedit', 'laydate'], function () {
//    var form = layui.form
//        , layer = layui.layer
//        , layedit = layui.layedit
//        , laydate = layui.laydate;

//    form.on('submit(addForm)',
//        function () {
//            var $ = layui.$;
//            $.ajax({
//                type: 'POST',
//                url: '/Material/Add', // ajax请求路径
//                data: data.field,
//                success: function (result) {
//                    if (result == 'OK') {
//                        layer.msg('添加成功');
//                    } else if (result == 'Error') {
//                        layer.msg('添加失败');
//                    }
//                    return false;//禁止跳转，否则会提交两次，且页面会刷新
//                }
//            });
//        });
//});