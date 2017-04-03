import { DropDownItem }     from "./dropDownItem";
import {FilterOption}       from '../shared/filterOption';

export interface ISort
{
    sortField: string;
    sortDirection: string;
    //onSortDirectionChanged(isDesc: boolean);
}

export interface IPaging
{
    currentPage: number;
}

export interface IOptionFilter extends ISort, IPaging
{
    options: DropDownItem[];
    filterOption: FilterOption;
    selectedOption: DropDownItem;
    selectedFilter: string;
}

export interface INavigationPager {
    SetCurrentPage(pageNumber: number): void;
}