import {Component} from '@angular/core';

@Component({
    selector: 'well-modal',
    templateUrl: 'app/shared/well-modal.html'
})
export class WellModal {
    public IsVisible: boolean;

    show() {
        this.IsVisible = true;
    }

    hide() {
        this.IsVisible = false;
    }
}