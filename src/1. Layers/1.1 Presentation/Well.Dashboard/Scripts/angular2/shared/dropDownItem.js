"use strict";
var DropDownItem = (function () {
    function DropDownItem(description, value, requiresServerCall) {
        if (description === void 0) { description = ""; }
        if (value === void 0) { value = ""; }
        if (requiresServerCall === void 0) { requiresServerCall = false; }
        this.description = description;
        this.value = value;
        this.requiresServerCall = requiresServerCall;
    }
    return DropDownItem;
}());
exports.DropDownItem = DropDownItem;
