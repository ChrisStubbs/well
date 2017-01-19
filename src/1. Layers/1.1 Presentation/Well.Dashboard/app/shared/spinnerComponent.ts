import {Component, Input, OnDestroy} from '@angular/core';

@Component({
    selector: 'ow-spinner',
    templateUrl: 'app/shared/spinner.html'
})
export class SpinnerComponent implements OnDestroy {
    private currentTimeout: number;
    private isItRunning: boolean = false;

    @Input()
    public delay: number = 0;

    @Input()
    public set isRunning(value: boolean) {
        if (!value) {
            this.cancelTimeout();
            this.isItRunning = false;
            return;
        }

        if (this.currentTimeout) {
            return;
        }

        this.currentTimeout = window.setTimeout(() => {
            this.isItRunning = value;
            this.cancelTimeout();
        }, this.delay);
    }

    private cancelTimeout(): void {
        clearTimeout(this.currentTimeout);
        this.currentTimeout = undefined;
    }

    public ngOnDestroy(): void {
        this.cancelTimeout();
    }
}