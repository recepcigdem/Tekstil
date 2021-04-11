function post(url, data, headers, success, error) {

    var token = getCookie('Token');

    $.ajax({
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Token', token);
            xhr.setRequestHeader('ApplicationSecret', '1c8e7a63acc8487a8cef1df61eadd9ce');
            xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
            xhr.setRequestHeader('Accept', '*/*');
            xhr.setRequestHeader('Cache-Control', 'no-cache');
            xhr.setRequestHeader('Accept-Encoding', 'gzip, deflate, br');
            xhr.setRequestHeader('Connection', 'keep-alive');
            xhr.setRequestHeader("Content-type", "application/json");
            $.each(headers, function (key, val) {
                xhr.setRequestHeader(key, val);
            });
        },
        type: "POST",
        url: url,
        processData: false,
        data: data,
        dataType: "JSON",
        success: success,
        error: function (response) {           
            if (response.status == 500) {
                error(response, response.responseJSON.errorMessage);
            }
            else {
                ShowError('Http Hatası', response.status + ' - ' + response.statusText);
                error(response);
            }
        }
    });
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {

    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);

    }
    return null;
}

function deleteCookie(name) {
    setCookie(name, '', 10);
}

function ShowError(title, message) {

    Swal.fire({
        position: 'top-end',
        title: title,
        text: message,
        icon: 'error',
        showCloseButton: true,
        showConfirmButton: true

    });
}

function GridAddBooleanColumn(data) {
    var html = '<div style="text-align:center">';
    if (data == false)
        html += '<i class="icon fa-minus" style="color: red; font-size:26px;" aria-hidden="true"></i>';
    else
        html += '<i class="icon fa-check" style="color: green;font-size:26px;" aria-hidden="true"></i>';
    html += '</div>';
    return html;
}

function NumericColumn(data) {
    var html = '<div class="numericField">' + data + '</div>';
    return html;
}


function InitSubList(divList, data) {
    var entity = JSON.parse(data);
    var line = JSON.parse(entity["dataLine"]);

    var html = '<div class="col-md-11 row" style="font-weight: 400; padding: 0px;">';
    for (visibleIndex = 0; visibleIndex < entity["visibility"].length; visibleIndex++) {
        if (entity["visibility"][visibleIndex] == true && !Object.keys(line)[visibleIndex].endsWith('Id')) {
            html += '<div class="col-md-' + entity["length"][visibleIndex] + '"><span>' + entity["header"][visibleIndex] + '</span></div>';
        }
    }
    html += '</div>';

    for (index = 0; index < entity["data"].length; index++) {

        html += '<div class="panel-heading-kna row">';
        html += '<div class="col-md-11 subListContentContainer" style="padding-right: 0px;" ><div class="col-md-12 row subListRow" style="padding: 0px; margin-right: 0px;">';

        for (visibleIndex = 0; visibleIndex < entity["visibility"].length; visibleIndex++) {
            if (entity["visibility"][visibleIndex] == true) {
                html += SubListAddText(entity["length"][visibleIndex], Object.keys(entity["data"][index])[visibleIndex], Object.values(entity["data"][index])[visibleIndex]);
            }
        }
        html += '</div></div>';
        html += '<div class="col-md-1 margin-auto" style="padding-left: 0px;">';
        html += '<button type="button" class="btn btn-floating btn-danger btn-xsm btn-sm button-subList" onclick="RemoveLineFromSubList(this)"><i class="icon wb-minus" aria-hidden="true"></i></button>'
        html += '</div>'
        html += '</div>';
    }

    var item = $(html).hide();
    divList.append(item);
    item.show('slow');
}
function InitSubListReadOnly(divList, data) {
    var entity = JSON.parse(data);
    var line = JSON.parse(entity["dataLine"]);

    var html = '<div class="col-md-12 row" style="font-weight: 400; padding: 0px;">';
    for (visibleIndex = 0; visibleIndex < entity["visibility"].length; visibleIndex++) {
        if (entity["visibility"][visibleIndex] == true && !Object.keys(line)[visibleIndex].endsWith('Id')) {
            html += '<div class="col-md-' + entity["length"][visibleIndex] + '"><span>' + entity["header"][visibleIndex] + '</span></div>';
        }
    }
    html += '</div>';

    for (index = 0; index < entity["data"].length; index++) {

        html += '<div class="panel-heading-kna row">';
        html += '<div class="col-md-12 subListContentContainer" style="padding-right: 0px;" ><div class="col-md-12 row subListRow" style="padding: 0px; margin-right: 0px;">';

        for (visibleIndex = 0; visibleIndex < entity["visibility"].length; visibleIndex++) {
            if (entity["visibility"][visibleIndex] == true) {
                html += SubListAddText(entity["length"][visibleIndex], Object.keys(entity["data"][index])[visibleIndex], Object.values(entity["data"][index])[visibleIndex]);
            }
        }
        html += '</div></div>';
      
        html += '</div>';
    }

    var item = $(html).hide();
    divList.append(item);
    item.show('slow');
}

function GetSubListData(divList) {

    var retVal = '[';
    var dataContainers = divList.children('.panel-heading-kna');
    for (index = 0; index < dataContainers.length; index++) {
        var jsonString = '{';
        var container = $(dataContainers[index]).children('.subListContentContainer');
        var rowContainer = $(container[0]).children('.subListRow');
        var dataControls = $(rowContainer[0]).children('[class*=" sublist-data-control-"]');
        for (controlIndex = 0; controlIndex < dataControls.length; controlIndex++) {
            var classes = $(dataControls[controlIndex]).attr('class').split(' ');
            for (classIndex = 0; classIndex < classes.length; classIndex++) {
                if (classes[classIndex].indexOf('sublist-data-control-') >= 0) {
                    var fieldKey = classes[classIndex].replace('sublist-data-control-', '');
                    var fieldValue = $(dataControls[controlIndex]).val();
                    if (jsonString != '{')
                        jsonString += ', ';
                    jsonString += '"' + fieldKey + '":"' + fieldValue + '"';
                }
            }
        }
        jsonString += '}';
        if (retVal.indexOf(jsonString) < 0) {
            if (retVal != '[')
                retVal += ', ';
            retVal += jsonString;
        }

    }

    return retVal + ']';
}

function SubListAddLine(divList, data) {

    var entity = JSON.parse(data);

    var html = '<div class="panel-heading-kna row">';
    html += '<div class="col-md-11 subListContentContainer" style="padding-right: 0px;">';
    html += '<div class="col-md-12 row subListRow" style="padding: 0px; margin-right: 0px;">';
    for (visibleIndex = 0; visibleIndex < entity["visibility"].length; visibleIndex++) {
        var line = JSON.parse(entity["dataLine"]);
        if (entity["visibility"][visibleIndex] == true) {
            html += SubListAddText(entity["length"][visibleIndex], Object.keys(line)[visibleIndex], Object.values(line)[visibleIndex]);
        }
    }
    html += '</div></div>';
    html += '<div class="col-md-1 margin-auto" style="padding-left: 0px;">';
    html += '<button type="button" class="btn btn-floating btn-danger btn-xsm btn-sm button-subList" onclick="RemoveLineFromSubList(this)"><i class="icon wb-minus" aria-hidden="true"></i></button>'
    html += '</div>'
    html += '</div>';
    var item = $(html).hide();
    divList.append(item);
    item.show('slow');
}
function RemoveLineFromSubList(button) {
    var parent = $(button).parent();
    while (!parent.hasClass('panel-heading-kna')) {
        parent = parent.parent();
    }
    if (parent) {
        parent.hide('slow', function () { parent.remove(); });
    }
}

function SubListAddText(len, key, val) {
    var html = '';

    if (key == 'Id' || key.endsWith('Id')) {
        if (val == null)
            val = '0';
        html += '<input type="hidden" class="sublist-data sublist-data-control-' + key + '" value="' + val + '" />';
    }
    else {
        html += '<input type="text" class="col-md-' + len + ' form-control sublist-data sublist-data-control-' + key + '" value="' + val + '" style="margin-right: 0px;" />'
    }

    return html;
}

function GetCulture() {

    var url = window.location.href;
    url = url.replace("https://", "").replace("http://", "");
    urlPart = url.split('/');
    var culture = 'en';
    if (urlPart.length > 1)
        culture = urlPart[1];
    if (culture == '' || culture == null || (culture != 'en' && culture != 'tr'))
        culture = 'en';

    return culture;

}
