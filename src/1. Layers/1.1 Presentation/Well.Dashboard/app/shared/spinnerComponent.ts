import {Component, Input, OnDestroy} from '@angular/core';

@Component({
    selector: 'ow-spinner',
    templateUrl: 'app/shared/spinner.html'
})
export class SpinnerComponent implements OnDestroy {
    private currentTimeout: number;
    private _isRunning: boolean = false;

    @Input()
    public delay: number = 0;

    @Input()
    public set isRunning(value: boolean) {
        if (!value) {
            this.cancelTimeout();
            this._isRunning = false;
            return;
        }

        if (this.currentTimeout) {
            return;
        }

        this.currentTimeout = window.setTimeout(() => {
            this._isRunning = value;
            this.cancelTimeout();
        }, this.delay);
    }

    private cancelTimeout(): void {
        clearTimeout(this.currentTimeout);
        this.currentTimeout = undefined;
    }

    ngOnDestroy(): any {
        this.cancelTimeout();
    }
}