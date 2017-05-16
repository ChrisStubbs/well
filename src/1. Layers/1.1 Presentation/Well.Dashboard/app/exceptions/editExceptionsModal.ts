import { Component } from '@angular/core';
import { IObservableAlive } from '../shared/IObservableAlive';

@Component({
    selector: 'edit-exceptions-modal',
    templateUrl: './app/exceptions/editExceptionsModal.html'
})
export class EditExceptionsModal implements IObservableAlive
{
    public isAlive: boolean = true;
    private isEditMode: boolean = false;
    private title: string = this.isEditMode ? 'Edit Exceptions' : 'Add Exceptions';
    private actions: string[] = ['Close', 'Credit', 'Re-plan'];
    private sources: string[] = ['Not Defined', 'Input', 'Assembler', 'Checker'];
    private reasons: string[] = ['Not Defined', 'No Credit', 'Damaged Goods', 'Shorts Delivered'];
    private exceptions: string[] = ['Not Defined', 'Short', 'Bypassed', 'Danmage'];

    public ngOnInit()
    {
        console.log('TODO:');
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }
}