import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DropDownItem } from './shared';
@Component({
    selector: 'ow-split-button',
    template: `
        <!-- Split button -->
        <div class="btn-group">
          <button type="button" class="btn btn-success">Action</button>
          <button type="button" class="btn btn-success dropdown-toggle" 
                        data-toggle="dropdown" 
                        aria-haspopup="true" aria-expanded="false">
            <span class="caret"></span>
            <span class="sr-only">Toggle Dropdown</span>
          </button>
          <ul class="dropdown-menu">
            <li *ngFor="let option of options"><a href="#">{{option}}</a></li>
          </ul>
        </div>
    `
})
export class SplitButtonComponent
{
    @Input() public options: string[];
}