// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$("#comment_form").submit(function (e) {
    e.preventDefault();
    submitBtn = $(this).find("[type='submit']");
    presentationId = $(this).find("#PresentationId").val();
    DisableBtn(submitBtn);
    $.ajax({
        type: "POST",
        url: "/Comment/CreateAjax",
        data: $(this).serialize(),
        success: function (result) {
            EnableBtn(submitBtn);
            CommentAjax(presentationId);
        }
    });
});

function CommentAjax(id) {
    commentSection = $("section[name='Comment']").find("div[name='comment-box']");
    commentSection.html("LOADING ...");
    $.ajax({
        url: "/Presentation/LoadComments",
        data: { id: id },
        success: function (result) {
            $(commentSection).html(result);
        }
    });
}

function DisableBtn(btn) {
    pre_text = $(btn).text();
    $(btn).text("Please wait ...");
    $(btn).addClass("disabled");
    $(btn).attr("disabled", "disabled");
    $(btn).attr("pre_text", pre_text);
}

function EnableBtn(btn) {
    pre_text = $(btn).attr("pre_text");
    $(btn).text(pre_text);
    $(btn).removeClass("disabled");
    $(btn).removeAttr("disabled");
}