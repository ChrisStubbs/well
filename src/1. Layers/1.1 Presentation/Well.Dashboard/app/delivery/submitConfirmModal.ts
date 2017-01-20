import {Component, EventEmitter, Input, Output} from '@angular/core';
import {SubmitLine} from './model/submitLine';

@Component({
    selector: 'submitConfirmModal',
    templateUrl: './app/delivery/submit-confirm-modal.html'
})
export class SubmitConfirmModal {
    @Input() public isVisible: boolean = false;
    @Input() public heading: string;
    @Input() public submitLines: SubmitLine[] = new Array<SubmitLine>();
    @Output() public confirmed = new EventEmitter();

    public show() {
        this.isVisible = true;
    }
    
    public hide() {
        this.isVisible = false;
    }

    public confirm() {
        this.confirmed.emit({});
        this.isVisible = false;
    }
}