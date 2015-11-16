function disableSubmit() {
    $("button:submit").click(function() {
        var disableSubmitButton = $("#matchingFilesForm").valid();
        if (disableSubmitButton) $(this).text("Please Wait");
    });
    $("form").on("submit", function() {
        var disableSubmitButton = $("#matchingFilesForm").valid();
        if (disableSubmitButton) $(this).find("button:submit").prop("disabled", true);
    });
}