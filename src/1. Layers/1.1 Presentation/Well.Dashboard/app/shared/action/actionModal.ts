import { Component, EventEmitter, Output, Input } from '@angular/core';
import { IObservableAlive } from '../IObservableAlive';

@Component({
    selector: 'action-modal',
    templateUrl: 'app/shared/action/action-modal.html'
})
export class ActionModal implements IObservableAlive
{
    @Input() public isVisible: boolean = false;
    private actions: string[] = ['Close', 'Credit', 'Re-plan'];
    private sources: string[] = ['Not Defined', 'Input', 'Assembler', 'Checker'];
    private reasons: string[] = ['Not Defined', 'No Credit', 'Damaged Goods', 'Shorts Delivered'];
    public isAlive: boolean = true;

    public ngOnInit()
    {
        console.log('TODO:');
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    private cancel(): void
    {
        this.isVisible = false;
    }

    private save(): void
    {
        console.log('This is where we do the saving');
    }
}