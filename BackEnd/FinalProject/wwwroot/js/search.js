$(document).on('keyup', '#header-search', function () {

    let searchValue = $(this).val().trim();
    let searchList = $("#searchList");
    console.log(searchList);
    $("#searchList li").slice(0).remove();
    $.ajax({
        url: "/common/search?search=" + searchValue,
        method: "get",
        success: function (res) {
            searchList.append(res);
        }
    })
})