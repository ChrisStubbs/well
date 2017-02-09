import {DropDownItem} from './dropDownItem';

export class FilterOption {
    public dropDownItem: DropDownItem;
    public filterText: string;

    constructor(dropDownItem: DropDownItem = DropDownItem.CreateDefaultOption(), filterText = '') {
        this.dropDownItem = dropDownItem;
        this.filterText = filterText;
    }
   
}