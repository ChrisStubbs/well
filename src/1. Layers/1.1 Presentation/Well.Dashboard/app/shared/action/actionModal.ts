import { Component, EventEmitter, Output, Input } from '@angular/core';
import { IObservableAlive } from '../IObservableAlive';
import { ActionModalModel } from './actionModalModel';

@Component({
    selector: 'action-modal',
    templateUrl: 'app/shared/action/action-modal.html'
})
export class ActionModal implements IObservableAlive
{
    private actions: string[] = ['Close', 'Credit', 'Re-plan'];
    private sources: string[] = ['Not Defined', 'Input', 'Assembler', 'Checker'];
    private reasons: string[] = ['Not Defined', 'No Credit', 'Damaged Goods', 'Shorts Delivered'];
    public model: ActionModalModel = new ActionModalModel();
    public isAlive: boolean = true;

    public ngOnInit()
    {
        console.log('TODO:');
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public show(model: ActionModalModel): void {
        this.model = model;
    }

    private cancel(): void
    {
        console.log('cancel modal');
    }

    private save(): void
    {
        console.log('This is where we do the saving');
    }
}