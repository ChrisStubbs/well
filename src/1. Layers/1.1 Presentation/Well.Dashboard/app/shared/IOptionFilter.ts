import { DropDownItem }     from "./dropDownItem";
import {FilterOption}       from '../shared/filterOption';

export interface IOptionFilter{
    options: DropDownItem[];
    filterOption: FilterOption;
    selectedOption: DropDownItem;
    selectedFilter: string;
    currentPage: number;
}