import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
    selector: 'confirmModal',
    templateUrl: 'app/shared/confirmModal.html'
})
export class ConfirmModal {
    @Input() isVisible: boolean = false;
    @Input() heading: string;
    @Input() messageHtml: string;
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