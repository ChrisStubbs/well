import { Component, Input, Output, EventEmitter }   from "@angular/core";
import { FilterOption }                             from "./filterOption";
import { DropDownItem }                             from "./dropDownItem";
import { NavigateQueryParametersService }           from './NavigateQueryParametersService';
import { IOptionFilter }                            from './IOptionFilter';

@Component({
    selector: "ow-optionfilter",
    templateUrl: "app/shared/optionFilter.html"
})
export class OptionFilterComponent implements IOptionFilter{

    @Input() public options: DropDownItem[];
    public filterText: string;
    inputPlaceholder: string = "";
    public selectedOption: DropDownItem;

    @Output() filterClicked: EventEmitter<FilterOption> = new EventEmitter<FilterOption>();
    public constructor(private navigateQueryParametersService: NavigateQueryParametersService){
        this.ClearFilter();
    }

    public ngAfterContentInit(): void {
        this.navigateQueryParametersService.Navigate(this);
    }

    setSelectedOption = (option: DropDownItem) : void => {
        this.ClearFilter();
        this.SelectedOption = option;
        this.applyFilter();
    }

    private ClearFilter() : void {
        this.filterText = '';
        this.SelectedOption = new DropDownItem("Option", "");
    }

    public clearFilterText(): void {
        this.ClearFilter();
        this.applyFilter();
    }

    public applyFilter(): void {
        this.filterClicked.emit(new FilterOption(this.selectedOption, this.filterText));

        // if (this.filterText != ''){
            const item = {[this.selectedOption.value]: this.filterText};
            this.navigateQueryParametersService.Save(item);
        // }
        // else {
        //     this.navigateQueryParametersService.Save(null);
        // }
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
        if (filter) this.filterText = filter;
    }
}