import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DropDownItem } from './shared';
@Component({
    selector: 'ow-split-button',
    template: `
        <!-- Split button -->
        <div class="btn-group">
          <button type="button" class="btn btn-success"  
                        [disabled]="disabled" 
                        (click)="optionClicked(selectedOption)">{{selectedOption}}</button>
          <button type="button" class="btn btn-success dropdown-toggle" 
                        data-toggle="dropdown" 
                        aria-haspopup="true" aria-expanded="false" [disabled]="disabled" >
            <span class="caret"></span>
            <span class="sr-only">Toggle Dropdown</span>
          </button>
          <ul class="dropdown-menu">
            <li *ngFor="let option of options" (click)="optionClicked(option)"><a>{{option}}</a></li>
          </ul>
        </div>
    `
})
export class SplitButtonComponent
{
    @Input() public options: string[];
    @Input() public disabled: boolean = false;
    @Output() public onOptionClicked: EventEmitter<string> = new EventEmitter<string>();
    private defaultSeletedOption: string = 'Action';
    private selectedOption: string = this.defaultSeletedOption;

    public optionClicked = (option: string): void =>
    {
        this.selectedOption = option;
        this.onOptionClicked.emit(option);
    }

    public reset(): void {
        this.selectedOption = this.defaultSeletedOption;
    }
}