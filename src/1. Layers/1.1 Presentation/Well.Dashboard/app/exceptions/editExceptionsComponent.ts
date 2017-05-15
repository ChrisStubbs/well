import { Component, Input }         from '@angular/core';
import { IObservableAlive }         from '../shared/IObservableAlive';
import { EditExceptionService }     from './editExceptionsService';
import { IEditLineItemException }   from './editLineItemException';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-editExceptions',
    templateUrl: './app/routes/singleRouteComponent.html',
    providers: [EditExceptionService]
})
export class EditExceptionsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<IEditLineItemException>;
    @Input() public ids: Array<number>;

    constructor(private editExceptionService: EditExceptionService) {}

    public ngOnInit()
    {
        this.editExceptionService.get(this.ids)
            .takeWhile(() => this.isAlive)
            .subscribe((values: Array<IEditLineItemException>) => this.source = values);
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public editLine(line: IEditLineItemException): void
    {
        console.log(line);
        //i have to do something here
    }
}