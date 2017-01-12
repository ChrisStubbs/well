import {Component, Input, Output, EventEmitter} from '@angular/core';
import {FilterOption}                           from './filterOption';
import {DropDownItem}                           from './dropDownItem';

@Component({
    selector: 'ow-optionfilter',
    templateUrl: 'app/shared/optionFilter.html'
})
export class OptionFilterComponent {

    @Input() public options: DropDownItem[];
    public filterText: string;
    public inputPlaceholder: string = '';
    private selectedOption: DropDownItem;

    @Output() public filterClicked: EventEmitter<FilterOption> = new EventEmitter<FilterOption>();
    public constructor() {
        this.filterText = '';
        this.SelectedOption = new DropDownItem('Option', '');
    }

    public setSelectedOption = (option: DropDownItem): void => {
        this.ClearFilter();
        this.SelectedOption = option;
        this.applyFilter();
    }

    private ClearFilter(): void {
        this.filterText = '';
        this.SelectedOption = new DropDownItem('Option', '');
    }

    public clearFilterText(): void {
        this.ClearFilter();
        this.applyFilter();
    }

    public applyFilter(): void {
        this.filterClicked.emit(new FilterOption(this.selectedOption, this.filterText));
    }

    public set SelectedOption(value: DropDownItem) {
        this.selectedOption = value;
    }

    public get SelectedOption(): DropDownItem {
        return this.selectedOption;
    }

    @Input()
    public set setKnownOption(option: DropDownItem) {
        if (option) {
            this.SelectedOption = option
        }
    }

    @Input()
    public set setKnownFilter(filter: string) {
        if (filter) {
            this.filterText = filter;
        }
    }
}