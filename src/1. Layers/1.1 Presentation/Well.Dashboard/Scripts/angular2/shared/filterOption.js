"use strict";
var dropDownItem_1 = require("./dropDownItem");
var FilterOption = (function () {
    function FilterOption(dropDownItem, filterText) {
        if (dropDownItem === void 0) { dropDownItem = new dropDownItem_1.DropDownItem(); }
        if (filterText === void 0) { filterText = ""; }
        this.dropDownItem = dropDownItem;
        this.filterText = filterText;
    }
    return FilterOption;
}());
exports.FilterOption = FilterOption;
