import {DropDownItem} from './dropDownItem';

export class FilterOption {
    public dropDownItem: DropDownItem;
    public filterText: string;

    constructor(dropDownItem: DropDownItem = new DropDownItem(), filterText = '') {
        this.dropDownItem = dropDownItem;
        this.filterText = filterText;
    }
   
}