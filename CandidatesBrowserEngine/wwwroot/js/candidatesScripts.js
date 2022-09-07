var routeURL = location.protocol + "//" + location.host;


function onCompanyChange(id) {

    var companyId = parseInt(id);
  
        $.ajax({
            url: "/CandidateDetails/Company",
            type: "Get",
            data: { "id": companyId }
        })
            .done(function (partialViewResult) {
                $("#companySection").html(partialViewResult);
                onCompanyLoad();

            });
  

};

function onCompanyEditChange(id, candidateId) {
 
    $.ajax({
        url: routeURL + '/api/CandidateDetails/GetCompanyEditDetailsById/' + id + "/" + candidateId,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {

            if (response.status == 1 && response.dataenum != undefined) {
                onShowModalCompany(response.dataenum, true);

            }
            else {
                $.notify(response.message, "error");
            }

        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });

};

function onProjectEditChange(id, candidateId) {
    $.ajax({
        url: routeURL + '/api/CandidateDetails/GetProjectEditDetailsById/' + id + "/" + candidateId,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {

            if (response.status == 1 && response.dataenum != undefined) {
                onShowModalProject(response.dataenum, true);
            }
            else {
                $.notify(response.message, "error");
            }

        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onCompanyDelete(id) {

    var proceed = confirm('Are you sure to delete this Company?');
    if (proceed == true) {
        $.ajax({
            url: routeURL + '/api/CandidateDetails/DeleteCompany/' + id,
            type: 'POST',
            dataType: 'JSON',
            success: function (response) {

                if (response.status == 1) {

                    location.reload();
                }
                else {
                    $.notify(response.message, "error");
                }

            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });

    }

}

function onProjectDelete(id) {
    var proceed = confirm('Are you sure to delete this Project?');
    if (proceed == true) {
        $.ajax({
            url: routeURL + '/api/CandidateDetails/DeleteProject/' + id,
            type: 'POST',
            dataType: 'JSON',
            success: function (response) {

                if (response.status == 1) {

                    location.reload();
                }
                else {
                    $.notify(response.message, "error");
                }

            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });

    }
}

function onCompanyChangeTemp(tempKey) {

    var companytempKey = tempKey;

    $.ajax({
        url: "/AddCandidate/Company",
        type: "Get",
        data: { "tempKey": companytempKey }
    })
        .done(function (partialViewResult) {
            $("#companySection").html(partialViewResult);
            onCompanyLoad();

        });


};

function onCompanyChangeSave() {

    //var companyId = parseInt($("#companyId").val());
    var id = parseInt($("#companyId").val());
    
    var companyUpdateViewModel = {
        companyId: parseInt($("#companyId").val()),
        dateEndString: $("#dateStart").val(),
        dateStartString: $("#dateEnd").val()

    };


    $.ajax({
        url: '@Url.Action("Company", "CandidateDetails")',
        type: "POST",
        data: $('form').serialize()
    }).responseText;
        

};

function onCandidatePictureChange() {
    var picture = document.getElementById('profileImage');
    picture.src = URL.createObjectURL(event.target.files[0]);
}

function onCandidateEditPictureChange() {

    var picture = document.getElementById('profileEditImage');
    picture.src = URL.createObjectURL(event.target.files[0]);

        var input = document.getElementById('customFile');
        var file = input.files[0];
        var formData = new FormData();
        formData.append("file", file);
    $.ajax(
        {
            url: routeURL + '/api/CandidateDetails/UploadPicture',
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
     
            }
        } 
    );
 
}

function onCandidateDelete(id) {

    var proceed = confirm('Are you sure to delete record?');
    if (proceed == true) {
        $.ajax({
            url: routeURL + '/api/CandidateDetails/DeleteCandidate/' + id,
            type: 'POST',
            dataType: 'JSON',
            success: function (response) {

                if (response.status == 1) {

                    location.reload();
                }
                else {
                    $.notify(response.message, "error");
                }

            },
            error: function (xhr) {
                $.notify(xhr.responseJSON, "error");
            }
        });
    }


}

function onCompanyLoad() {
    var dateEnd = $("#dateEnd");
    if (dateEnd.val() == "" || dateEnd.val()==undefined) {
        document.getElementById('current').checked = true;       
    }
    validationCheck();
    
}



function editCandidate(id) {

    $.ajax({
        url: routeURL + '/api/CandidateDetails/GetCandidateEditDetailsById/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {

            if (response.status == 1 && response.dataenum != undefined) {
                onShowModalCandidate(response.dataenum, true);

            }
            else {
                $.notify(response.message, "error");
            }

        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });

   
}

function onShowModalCandidate(obj, isEventDetail) {
    if (isEventDetail != null) {
        $("#firstName").val(obj.firstName);
        $("#lastName").val(obj.lastName);
        var picture = document.getElementById('profileEditImage');
        picture.src = routeURL + "/images/" + obj.profilePicture;
        $("#email").val(obj.email);
        $("#id").val(obj.id);
        $("#dateOfBirth").val(obj.dateOfBirth.substring(0, 10));
        $("#description").val(obj.description);
  
    }
    else {

    }

   $("#candidateEditInput").modal("show");
}

function onShowModalCompany(obj, isEventDetail) {
    if (isEventDetail != null) {
        $("#id").val(obj.companyViewModel.id);
        $("#candidateId").val(obj.companyViewModel.candidateId);
        $("#dateStart").val(obj.companyViewModel.dateStart.substring(0, 10));
        if (obj.companyViewModel.dateEnd != "" && obj.companyViewModel.dateEnd != null) {
            $("#dateEnd").val(obj.companyViewModel.dateEnd.substring(0, 10));
        }

        $('#companies').empty();
        var companies = document.getElementById('companies');
     
        for (var i = 0; i < obj.companiesList.length; i++) {
            var opt = obj.companiesList[i];

            var el = document.createElement("option");
            el.text = opt.text;
            el.value = opt.value;
            if (obj.companyViewModel.companyId == el.value) {
                el.selected = true;
            }

            companies.add(el);
        }

    }
    else {

    }
    onCompanyLoad();
    $("#companyEditInput").modal("show");
}

function onShowModalProject(obj, isEventDetail) {
    if (isEventDetail != null) {
        $("#id").val(obj.projectViewModel.id);
        $("#candidateId").val(obj.projectViewModel.candidateId);
   

        $('#projects').empty();
        var projects = document.getElementById('projects');

        for (var i = 0; i < obj.projectList.length; i++) {
            var opt = obj.projectList[i];

            var el = document.createElement("option");
            el.text = opt.text;
            el.value = opt.value;
            if (obj.projectViewModel.projectId == el.value) {
                el.selected = true;
            }

            projects.add(el);
        }

    }
    else {

    }

    $("#projectEditInput").modal("show");
}


function onCloseEditCandidateModal() {

    $("#firstName").val('');
    $("#lastName").val('');
    $("#email").val('');
    $("#id").val(0);
    $("#dateOfBirth").val('');
    $("#description").val('');
    $("#candidateEditInput").modal("hide");
}

function onCloseEditCompanyModal() {

    $("#dateStart").val('');
    $("#dateEnd").val('');
    $('#companies').empty();
    document.getElementById('current').checked = false;
    $("#companyEditInput").modal("hide");
}

function onCloseEditProjectModal() {

    $('#projects').empty();
    $("#projectEditInput").modal("hide");
}

function onSaveEditCandidateModal() {
    
    if (checkValidationEditCandidate()) {
        
        var requestData = {
            Id: parseInt($("#id").val()),
            FirstName: $("#firstName").val(),
            LastName: $("#lastName").val(),
            Email: $("#email").val(),
            DateOfBirth: $("#dateOfBirth").val(),
            Description: $("#description").val()
        };
   
        $.ajax({
            url: routeURL + '/api/CandidateDetails/SaveCandidateEditDetails',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status == 1 || response.status == 2) {                  
                    onCloseEditCandidateModal();
                    location.reload();
        
                }
                else {
                   $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
        
    }

}

function onSaveEditCompanyModal() {
    
    var requestData = {
        id: parseInt($("#id").val()),
        candidateId: parseInt($("#candidateId").val()),
        dateStart: $("#dateStart").val(),
        dateEnd: $("#dateEnd").val(),
        companyId: parseInt($("#companies").val())
    };


    $.ajax({
        url: routeURL + '/api/CandidateDetails/SaveCompanyEditDetails',
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (response) {
            if (response.status == 1 || response.status == 2) {

                onCloseEditCompanyModal();
                location.reload();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });

 
}

function onSaveEditProjectModal() {
    var requestData = {
        id: parseInt($("#id").val()),
        candidateId: parseInt($("#candidateId").val()),
        projectId: parseInt($("#projects").val())
    };


    $.ajax({
        url: routeURL + '/api/CandidateDetails/SaveProjectEditDetails',
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (response) {
            if (response.status == 1 || response.status == 2) {

                onCloseEditProjectModal();
                location.reload();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function candidateDetailsRefresh(id) {

    $.ajax({
        url: "/CandidateDetails/Index",
        type: "Get",
        data: { "id": id }
    });
    
}

function checkValidationEditCandidate() {
    var isValid = true;
    if ($("#firstName").val() === undefined || $("#firstName").val() === "") {
        isValid = false;
        $("#firstName").addClass('error');
    }
    else {
        $("#firstName").removeClass('error');
    }

    if ($("#lastName").val() === undefined || $("#lastName").val() === "") {
        isValid = false;
        $("#lastName").addClass('error');
    }
    else {
        $("#lastName").removeClass('error');
    }

    if ($("#email").val() === undefined || $("#email").val() === "") {
        isValid = false;
        $("#email").addClass('error');
    }
    else {
        $("#email").removeClass('error');
    }

    if ($("#dateOfBirth").val() === undefined || $("#dateOfBirth").val() === "") {
        isValid = false;
        $("#dateOfBirth").addClass('error');
    }
    else {
        $("#dateOfBirth").removeClass('error');
    }

    return isValid;
}


function changeCurrent() {

    var current = document.getElementById('current').checked;
    var dateEnd = $("#dateEnd");

    if (current == true) {
        dateEnd.val('');
        dateEnd.attr('disabled', true);
    }
    else {
        dateEnd.removeAttr('disabled');
    }
    validationCheck();
}


function validationCheck() {

    var isValid = true;
    var errors = new Array();
    var current = document.getElementById('current').checked;
    var dateEnd = $("#dateEnd");
    var dateStart = $("#dateStart");
    var acceptChanges = $("#btnSaveEditCompany");

    dateStart.removeClass('error');
    dateEnd.removeClass('error');
    acceptChanges.removeAttr('disabled');

    if (!current && dateEnd.val() == "") {
        dateEnd.addClass('error');
        isValid = false;
    }
  


    if (dateStart.val() == "") {
        dateStart.addClass('error');
        isValid = false;
    }
  

    if (dateStart.val() != "" && dateEnd.val()!="" && !current && dateStart.val() > dateEnd.val() ) {
        dateStart.addClass('error');
        dateEnd.addClass('error');
        isValid = false;
        errors.push("Start date cannot be greater than End date");
    }

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); 
    var yyyy = today.getFullYear();

    today =  yyyy + '-' + mm + '-' + dd;
    if (dateStart.val() != "" && current && dateStart.val() > today) {
        dateStart.addClass('error');
        dateEnd.addClass('error');
        isValid = false;
        errors.push("Start date cannot be greater than current date");
    }

    
    if (!isValid) {
       
        acceptChanges.attr('disabled', true);
        $.notify(errors.join("\ "), "error");
    }


    if (current == true) {
        dateEnd.val('');
        dateEnd.attr('disabled', true);
    }
    else {
        dateEnd.removeAttr('disabled');
    }

   

}




var timeInSecondsAfterSessionOut = 300; // change this to change session time out (in seconds).
var timeThreshold = 150;
var secondTick = 0;
var alreadyStarted = false;

function ResetThisSession() {
    secondTick = 0;
    var spanTimeLeft = $("#spanTimeLeft");
    spanTimeLeft.attr('hidden', true);

}

function StartThisSessionTimer() {

    secondTick++;
    var timeLeft = ((timeInSecondsAfterSessionOut - secondTick) / 60).toFixed(0); // in minutes
    timeLeft = timeInSecondsAfterSessionOut - secondTick; // override, we have 30 secs only
    if (timeLeft < 0) { timeLeft = 0;}
    if (timeLeft <= timeThreshold) {

        var timeLeftInMinutes ="Session will end in: " + ("0" + (parseInt(timeLeft / 60))) + ':' + ("0" + (timeLeft - (parseInt(timeLeft / 60) * 60))).slice(-2) + " due to inactivity";

        var spanTimeLeft = $("#spanTimeLeft");


        spanTimeLeft.removeAttr('hidden');
        spanTimeLeft.html(timeLeftInMinutes);

    }
   

    if (secondTick > timeInSecondsAfterSessionOut) {
        clearTimeout(tick);
        window.location.href = routeURL + '/Account/LogoffTimeout/';
        return;

    }
    tick = setTimeout("StartThisSessionTimer()", 1000);
}
