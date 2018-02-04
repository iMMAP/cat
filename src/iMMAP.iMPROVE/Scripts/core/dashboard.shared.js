var modalTemplates = {
    textPrompt:
    '<div class="form-group">' +
    '    <label for="inputLabel">${0}</label>' +
    '    <textarea class="form-control" id="${1}" placeholder="${2}" rows="3"></textarea>' +
    '</div>'
};

//args={notification,url,absoluteUrl,data,onSuccess,onError,method,json}
function ajaxCall(args) {
    var notifyToken = newCode('xxxx-xxxxxxxx');
    //if (args.notification && theme_addNotification)
    //    theme_addNotification(args.notification, notifyToken);

    var urlQsIndex = args.url.indexOf('?');
    if (urlQsIndex > 0)
        url = args.url.substring(0, urlQsIndex).toLowerCase() + args.url.substring(urlQsIndex);
    else
        url = args.url.toLowerCase();

    $.ajax({
        cache: false,
        url: url,
        data: args.json ? JSON.stringify(args.data) : args.data,
        type: args.method ? args.method : 'post',
        contentType: args.json ? 'application/json; charset=utf-8' : 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data) {
                //if (data.error && data.errorType == 4) {
                if (data === "_timeout_")
                    logOff();
                else {
                    if (args.onSuccess)
                        args.onSuccess(data);
                }
            }

            //if (args.notification && theme_removeNotification)
            //    theme_removeNotification(notifyToken);
        },
        error: function (error) {
            showAlert('Error', 'iMPROVE has encountered an error.. Try refresh the dashboard page!');
            if (args.onError)
                args.onError(data);

            //if (args.notification && theme_removeNotification)
            //    theme_removeNotification(notifyToken);

            //logoff if it's permission error
        }
    });
}

function newCode(pattern) {
    return pattern.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : r & 0x3 | 0x8;
        return v.toString(16);
    });
}

function newGuid() {
    return newCode('xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx');
}

function ajaxMessage(data) {
    if (data.error) {
        showAlert("Error", data.message);
    }
    else {
        showAlert(data.title, data.message);
    }
}

function showAlert(title, message) {
    var modal = $('#generic-modal').clone(true);
    modal.find('.modal-title').text(title);
    modal.find('.modal-body>p').text(message);
    modal.find('.btn-cancel').remove();
    modal.on('hidden.bs.modal', function (e) { $(this).remove(); });
    modal.on('shown.bs.modal', function () {
        modal.find('.btn-ok').focus();
    });
    modal.modal('show');
}

function showModal(templateName, options, okayHandler, cancelHandler) {
    var template = modalTemplates[templateName];
    if (typeof options.parameters === typeof [] && options.parameters.length > 0) {
        for (var p in options.parameters) {
            template = template.replace('${' + p + '}', options.parameters[p]);
        }
    }
    var modal = $('#generic-modal').clone(true);
    modal.find('.modal-title').text(options.title);
    modal.find('.modal-body>p').replaceWith(template);
    modal.on('hidden.bs.modal', (e) => $(this).remove());
    modal.on('shown.bs.modal', () => {
        modal.find('.btn-ok').focus().on('click', () => okayHandler(modal.find('.modal-body')));
        modal.find('.btn-cancel').on('click', cancelHandler);
    });
    modal.modal('show');
}

function showSpin(target, message) {
    var spin = $('#generic-psin').clone(true);
    spin.removeAttr("id").show();
    if (message)
        spin.find('.spin-message').text(message);
    if (target)
        target.prepend(spin);
}

function removeSpin(target) {
    target.find('.sk-circle').remove();
}

function showConfirm(title, message, okHandler, cancelHandler) {
    var modal = $('#generic-modal').clone(true);
    modal.find('.modal-title').text(title);
    modal.find('.modal-text').text(message);
    modal.find('.btn-ok').on('click', function () {
        if (typeof okHandler !== 'undefined')
            okHandler();
    });
    modal.find('.btn-cancel').on('click', function () {
        if (typeof cancelHandler !== 'undefined')
            cancelHandler();
    });
    modal.on('hidden.bs.modal', function (e) { $(this).remove(); });
    modal.on('shown.bs.modal', function () {
        modal.find('.btn-cancel').focus();
    });
    modal.modal('show');
}

function logOff() {
    location = '/account/logoff';
}

function uploadFile(options, success, progress, error) {
    if (!options && typeof error !== 'undefined')
        error();

    var oData = new FormData();

    oData.append('file', options.file);
    oData.append('dir', options.dir);

    if (options.prefix)
        oData.append('prefix', options.prefix);
    if (options.code)
        oData.append('code', options.code);
    if (options.overwrite)
        oData.append('overwrite', options.overwrite);
    if (options.resource)
        oData.append('resource', options.resource);
    if (options.tag)
        oData.append('tag', options.tag);

    var oReq = new XMLHttpRequest();
    var url = '/manage/uploadfile';
    oReq.open('post', url, true);
    oReq.onload = function (oEvent) {
        var result = JSON.parse(oReq.response);
        if (oReq.status === 200 && typeof success !== 'undefined') {
            if (result && typeof success === 'function') {
                options.result = result;
                success(options);
            }
        }
        else if (typeof error === 'function')
            error(oEvent);
    };
    oReq.onprogress = function (oEvent) {
        if (typeof progress === 'function' && oEvent.lengthComputable)
            progress(oEvent.loaded);
    };
    oReq.onerror = function (oEvent) {
        if (typeof error === 'function')
            error(oEvent);
    };
    oReq.send(oData);
}