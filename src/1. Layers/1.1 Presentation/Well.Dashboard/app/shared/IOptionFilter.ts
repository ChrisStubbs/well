import { DropDownItem } from "./dropDownItem";

export interface IOptionFilter{
    options: DropDownItem[];
    selectedOption: DropDownItem;
    filterText: string;
    applyFilter();
}