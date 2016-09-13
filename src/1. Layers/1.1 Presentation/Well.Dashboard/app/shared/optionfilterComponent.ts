import {Component, OnChanges, Input, Output, EventEmitter} from "@angular/core";
import {FilterOption} from "./filterOption";
import {DropDownItem} from "./dropDownItem";

@Component({
    selector: "ow-optionfilter",
    templateUrl: "app/shared/optionFilter.html"
})
export class OptionFilterComponent implements OnChanges {

    private defaultOption: DropDownItem = new DropDownItem("Option", "");

    @Input() options: DropDownItem[];
    filterText: string;
    inputPlaceholder: string = "";
    selectedOption: DropDownItem = this.defaultOption;
    @Output() filterClicked: EventEmitter<FilterOption> = new EventEmitter<FilterOption>();

    ngOnChanges(): void {
        //console.log("onchange");
    }

    clearFilterText(): void {
        this.filterText = '';
        this.selectedOption = this.defaultOption;
        this.applyFilter();
    }

    applyFilter(): void {
        this.filterClicked.emit(new FilterOption(this.selectedOption, this.filterText));
    }

    setSelectedOption(option: DropDownItem): void {
        this.selectedOption = option;
        if (option.description == "Date") {
            this.inputPlaceholder = "dd/mm/yyyy";
        } else {
            this.inputPlaceholder = "";
        }
    }

    @Input()
    set setKnownOption(option: DropDownItem) {
        if (option) this.setSelectedOption(option);
    }

    @Input()
    set setKnownFilter(filter: string) {
        if (filter) this.filterText = filter;
    }
}