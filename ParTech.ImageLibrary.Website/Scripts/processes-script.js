$(function () {
    var completedText;
    var progressText;
    var runningText;

    function updateMonitor(taskId, progress, status) {
        $("#" + taskId).html(progress + ": " + status);
    }

    $("#startgenerateinvoices").click(function (e) {
        completedText = $(this).attr("data-completed");
        progressText = $(this).attr("data-progress");
        runningText = $(this).attr("data-running");
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            alert(runningText);
        }
        else {
            $.post("/Admin/StartGenerateInvoices", {}, function (taskId) {

                // Init monitors
                $("#monitors").html($("<p id='" + taskId + "'/>"));
                updateMonitor(taskId, "Started");
                $("#startgenerateinvoices").addClass("disabled");

                // Periodically update monitors
                var intervalId = setInterval(function () {
                    $.post("/Admin/ProgressGenerateInvoices", { id: taskId }, function (progress) {
                        if (progress >= 100) {
                            updateMonitor(taskId, progressText, completedText);
                            clearInterval(intervalId);
                            $("#startgenerateinvoices").removeClass("disabled");
                        } else {
                            updateMonitor(taskId, progressText, progress + "%");
                        }
                    });
                }, 100);
            });
        }
    });

    $("#startindexrebuild").click(function (e) {
        completedText = $(this).attr("data-completed");
        progressText = $(this).attr("data-progress");
        runningText = $(this).attr("data-running");
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            alert(runningText);
        }
        else {
            $.post("/Admin/StartIndexRebuild", {}, function(taskId) {

                // Init monitors
                $("#monitors").html($("<p id='" + taskId + "'/>"));
                updateMonitor(taskId, "Started");
                $("#startindexrebuild").addClass("disabled");

                // Periodically update monitors
                var intervalId = setInterval(function() {
                    $.post("/Admin/ProgressIndexRebuild", { id: taskId }, function(progress) {
                        if (progress >= 100) {
                            updateMonitor(taskId, progressText, completedText);
                            clearInterval(intervalId);
                            $("#startindexrebuild").removeClass("disabled");
                        } else {
                            updateMonitor(taskId, progressText, progress + "%");
                        }
                    });
                }, 100);
            });
        }
    });
});