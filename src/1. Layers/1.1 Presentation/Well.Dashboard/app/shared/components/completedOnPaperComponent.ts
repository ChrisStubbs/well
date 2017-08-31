import {Component, Input} from '@angular/core';

@Component({
    selector: 'ow-completed-on-paper',
    templateUrl: 'app/shared/components/completedOnPaperComponent.html'
})
export class CompletedOnPaperComponent {
    @Input() public isCompletedOnPaper: boolean;
}