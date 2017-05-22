import { Component, EventEmitter, Output, Input } from '@angular/core';
import { IObservableAlive } from '../IObservableAlive';
import { LookupService } from '../services/lookupService';

@Component({
    selector: 'action-Modal',
    templateUrl: 'app/shared/action/actionModal.html'
})
export class ActionModal implements IObservableAlive
{

    @Input() public disabled: boolean = false;
    @Output() public onActionClicked: EventEmitter<string> = new EventEmitter<string>();
    @Input() public lineItemActionIds: number[];
    @Input() public actionSummaryData: string = 'This is the Summary info supplied by the consumer of the modal';

    private mAdditionalItemsItem: string[];
    @Input() public set additionalOptions(value: string[])
    {
        this.mAdditionalItemsItem = value;
        this.deliveryActions.push.apply(this.deliveryActions, this.mAdditionalItemsItem);
    };

    public get item(): string[]
    {
        return this.mAdditionalItemsItem;
    }
    
    public isAlive: boolean = true;
    private deliveryActions: string[] = ['Close', 'Credit', 'Replan'];
    private selectedAction: string = 'Action';

    constructor(private lookupService: LookupService) { }

    public ngOnInit()
    {
        //
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public actionClicked(action: string): void
    {
        console.log(action);
        this.selectedAction = action;
        
        this.onActionClicked.emit(action);
    }

}