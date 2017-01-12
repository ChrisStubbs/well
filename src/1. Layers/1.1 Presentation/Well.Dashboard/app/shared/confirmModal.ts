import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
    selector: 'confirmModal',
    templateUrl: 'app/shared/confirmModal.html'
})
export class ConfirmModal {
    @Input() public isVisible: boolean = false;
    @Input() public heading: string;
    @Input() public messageHtml: string;
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