$(function () {
    var completedText;
    var progressText;
    var runningText;
    var startedText;

    function updateMonitor(taskId, progress, status) {
        $("#" + taskId).html(progress + ": " + status);
    }

    $("#startgenerateinvoices").click(function (e) {
        completedText = $(this).attr("data-completed");
        progressText = $(this).attr("data-progress");
        runningText = $(this).attr("data-running");
        startedText = $(this).attr("data-started");
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            alert(runningText);
        }
        else {
            $.post("/Admin/StartGenerateInvoices", {}, function (taskId) {

                // Init monitors
                $("#monitors").html($("<p id='" + taskId + "'/>"));
                updateMonitor(taskId, progressText, startedText);
                $("#startgenerateinvoices").addClass("disabled");

                // Periodically update monitors
                var intervalId = setInterval(function () {
                    $.post("/Admin/ProgressGenerateInvoices", { id: taskId }, function (progress) {
                        if (progress >= 100) {
                            updateMonitor(taskId, progressText, completedText);
                            clearInterval(intervalId);
                            location.reload();
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
        startedText = $(this).attr("data-started");
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            alert(runningText);
        }
        else {
            $.post("/Admin/StartIndexRebuild", {}, function(taskId) {

                // Init monitors
                $("#monitors").html($("<p id='" + taskId + "'/>"));
                updateMonitor(taskId, progressText, startedText);
                $("#startindexrebuild").addClass("disabled");

                // Periodically update monitors
                var intervalId = setInterval(function() {
                    $.post("/Admin/ProgressIndexRebuild", { id: taskId }, function(progress) {
                        if (progress >= 100) {
                            updateMonitor(taskId, progressText, completedText);
                            clearInterval(intervalId);
                            location.reload();
                        } else {
                            updateMonitor(taskId, progressText, progress + "%");
                        }
                    });
                }, 100);
            });
        }
    });
});