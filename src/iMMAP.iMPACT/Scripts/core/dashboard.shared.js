//args={notification,url,absoluteUrl,data,onSuccess,onError,method,json}
function ajaxCall(args) {
    var notifyToken = newCode('xxxx-xxxxxxxx');
    //if (args.notification && theme_addNotification)
    //    theme_addNotification(args.notification, notifyToken);
    var url = getAbsoluteUrl(args.url, !args.absoluteUrl);

    var urlQsIndex = url.indexOf('?');
    if (urlQsIndex > 0)
        url = url.substring(0, urlQsIndex).toLowerCase() + url.substring(urlQsIndex);
    else
        url = url.toLowerCase();

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

                if (args.onSuccess)
                    args.onSuccess(data);
            }

            //if (args.notification && theme_removeNotification)
            //    theme_removeNotification(notifyToken);
        },
        error: function (error) {
            var title = 'Error';
            var message = 'iMPACT has encountered an error.. Try refresh the dashboard page!';
            showAlert(title, message);
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

function showAlert(title, message) {
    var modal = $('#generic-modal').clone(true);
    modal.find('.modal-title').text(title);
    modal.find('.modal-text').html(message);
    modal.find('.btn-cancel').remove();
    modal.on('hidden.bs.modal', function (e) { $(this).remove(); });
    modal.on('shown.bs.modal', function () {
        modal.find('.btn-ok').focus();
    });
    modal.modal('show');
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