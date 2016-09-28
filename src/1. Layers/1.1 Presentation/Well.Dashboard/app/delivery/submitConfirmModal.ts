import {Component, EventEmitter, Input, Output} from '@angular/core';
import {SubmitLine} from './model/submitLine';

@Component({
    selector: 'submitConfirmModal',
    templateUrl: './app/delivery/submit-confirm-modal.html'
})
export class SubmitConfirmModal {
    @Input() isVisible: boolean = false;
    @Input() heading: string;
    @Input() submitLines: SubmitLine[] = new Array<SubmitLine>();
    @Output() confirmed = new EventEmitter();

    show() {
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    confirm() {
        this.confirmed.emit({});
        this.isVisible = false;
    }
}