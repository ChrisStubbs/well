import { Component, Input, Output, EventEmitter }   from '@angular/core';
import { FilterOption }                             from './filterOption';
import { DropDownItem }                             from './dropDownItem';
import { NavigateQueryParametersService }           from './NavigateQueryParametersService';
import { DictionaryItem, NavigateQueryParameters }  from './NavigateQueryParameters';

@Component({
    selector: 'ow-optionfilter',
    templateUrl: 'app/shared/optionFilter.html'
})
export class OptionFilterComponent {

    @Input() public options: DropDownItem[];
    public filterText: string;
    public inputPlaceholder: string = '';
    public selectedOption: DropDownItem;

    @Output() public filterClicked: EventEmitter<FilterOption> = new EventEmitter<FilterOption>();
    public constructor() {
        this.ClearFilter();
        this.SelectedOption = DropDownItem.CreateDefaultOption();
    }

    public setSelectedOption = (option: DropDownItem): void => {
        if (!option.IsDefaultItem()) {
            this.ClearFilter();
            this.SelectedOption = option;
            this.applyFilter();
        }
    }

    private ClearFilter(): void {
        this.filterText = '';
        this.SelectedOption = DropDownItem.CreateDefaultOption();
    }

    public clearFilterText(): void {
        this.ClearFilter();
        this.applyFilter();
    }

    public applyFilter(): void {
        let dicItem: DictionaryItem;

        if (!this.SelectedOption.IsDefaultItem()) {
            dicItem = new DictionaryItem();
            dicItem[this.selectedOption.value] = this.filterText;
        }

        const item = new NavigateQueryParameters(dicItem , undefined);
        NavigateQueryParametersService.SaveFilter(item);

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