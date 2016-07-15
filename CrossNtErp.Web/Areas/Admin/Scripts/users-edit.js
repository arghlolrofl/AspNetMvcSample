var rowCount = $('#roles-table').find('tr').length;
var indexRegEx = /([0-9]+)/;

function addRoles() {
    var rolesToAdd = [];
    var availableRolesTable = $('#available-roles-table');
    var rows = availableRolesTable.find('tr');

    for (var i = 0; i < rows.length; i++) {
        var checkbox = $(rows[i]).find('.selected-role');

        var isSelected = checkbox.prop('checked');
        if (isSelected)
            rolesToAdd.push(rows[i]);
    }

    for (var i = 0; i < rolesToAdd.length; i++) {
        var role = rolesToAdd[i];
        $(role).remove();
        $('#roles-table').append(role);

        $(role).find('.selected-role').prop('checked', false);

        setIdAndNameAttribute(
            $(role).find('.first-input'),
            rowCount,
            'RoleId'
        );
        setIdAndNameAttribute(
            $(role).find('.last-input'),
            rowCount,
            'Name'
        );

        rowCount++;
    }
}

function removeRoles() {
    var rolesToRemove = [];
    var rolesTable = $('#roles-table');
    var rows = rolesTable.find('tr');

    for (var i = 0; i < rows.length; i++) {
        var checkbox = $(rows[i]).find('.selected-role');

        var isSelected = checkbox.prop('checked');
        if (isSelected)
            rolesToRemove.push(rows[i]);
    }

    for (var i = 0; i < rolesToRemove.length; i++) {
        var role = rolesToRemove[i];
        $(role).remove();
        $('#available-roles-table').append(role);

        $(role).find('.selected-role').prop('checked', false);
        $(role).find('.first-input').removeProp('id').removeProp('name');
        $(role).find('.last-input').removeProp('id').removeProp('name');

        rowCount--;
    }

    sanitizeIndexes();
}

function sanitizeIndexes() {
    var trList = $('#roles-table').find('tr');

    for (rowCount = 0; rowCount < trList.length; rowCount++) {
        var tr = trList[rowCount];

        setIdAndNameAttribute($(tr).find('.first-input'), rowCount, 'RoleId');
        setIdAndNameAttribute($(tr).find('.last-input'), rowCount, 'Name');
    }
}

function setIdAndNameAttribute(element, index, propertyName) {
    $(element).prop('id', 'Roles_' + index + '__' + propertyName)
              .prop('name', 'Roles[' + index + '].' + propertyName);
}