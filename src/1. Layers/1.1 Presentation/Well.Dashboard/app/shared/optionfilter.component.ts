import {Component, OnChanges, Input, Output, EventEmitter} from "@angular/core";
import {FilterOption} from "./filterOption";
import {DropDownItem} from "./dropDownItem";

@Component({
    selector: "ow-optionfilter",
    templateUrl: "app/shared/optionfilter.component.html"
})
export class OptionFilterComponent implements OnChanges {

    private defaultOption: DropDownItem = new DropDownItem("Option", "");

    @Input() options: DropDownItem[];
    filterText: string;
    selectedOption: DropDownItem = this.defaultOption;
    @Output() filterClicked: EventEmitter<FilterOption> = new EventEmitter<FilterOption>();

    ngOnChanges(): void {
        console.log("onchange");
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
    }
}