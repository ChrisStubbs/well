import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'ow-button-dropdown',
    templateUrl: 'app/shared/buttonDropDownComponent.html'

})
export class  ButtonDropDownComponent
{
    @Input() public options: string[];
    @Input() public disabled: boolean = false;
    @Output() public onOptionClicked: EventEmitter<string> = new EventEmitter<string>();
    private defaultSeletedOption: string = 'Action';
    
    public optionClicked(option: string): void
    {
        this.onOptionClicked.emit(option);
    }

    public reset(): void
    {
        //this.selectedOption = this.defaultSeletedOption;
    }
}