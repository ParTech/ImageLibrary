$(function () {
    function updateMonitor(taskId, status) {
        $("#" + taskId).html("Progress of the index rebuild: " + status);
    }

    $("#startindexrebuild").click(function (e) {
        e.preventDefault();
        if ($(this).hasClass('disabled')) {
            alert("An index rebuild is running.");
        }
        else {
            $.post("Admin/StartIndexRebuild", {}, function(taskId) {

                // Init monitors
                $("#monitors").html($("<p id='" + taskId + "'/>"));
                updateMonitor(taskId, "Started");
                $("#startindexrebuild").addClass("disabled");

                // Periodically update monitors
                var intervalId = setInterval(function() {
                    $.post("Admin/ProgressIndexRebuild", { id: taskId }, function(progress) {
                        if (progress >= 100) {
                            updateMonitor(taskId, "Completed");
                            clearInterval(intervalId);
                            $("#startindexrebuild").removeClass("disabled");
                        } else {
                            updateMonitor(taskId, progress + "%");
                        }
                    });
                }, 100);
            });
        }
    });
});