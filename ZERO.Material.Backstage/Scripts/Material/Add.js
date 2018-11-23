function AfterSuccess(data) {
    var index = parent.layer.getFrameIndex(window.name);
    if (data === 'OK') {
        parent.layer.msg("添加/修改成功");
        parent.layer.close(index);
    } else {
        parent.layer.msg("添加/修改失败", { icon: 5 });
        parent.layer.close(index);
    }
}