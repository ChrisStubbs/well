import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IObservableAlive } from '../shared/IObservableAlive';
import { ApprovalsService } from './approvalsService';
import { Approval } from './approval';
import * as _ from 'lodash';

@Component({
    selector: 'ow-approval',
    templateUrl: './app/approvals/approvalsComponent.html',
    providers: [ApprovalsService]
})
export class ApprovalsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<Approval>;
    private gridSource: Array<Approval> = [];

    constructor(
        private approvalsService: ApprovalsService,
        private route: ActivatedRoute) { }

    public ngOnInit(): void
    {
        this.route.params
            .flatMap(data =>
            {
                return this.approvalsService.get();
            }).takeWhile(() => this.isAlive)
            .subscribe((data: Approval[]) =>
            {
                this.source = data;
                this.fillGridSource();
            });
    }

    public fillGridSource(): void
    {
        const values: Array<any> = [];
        _.map(this.source, (x: Approval) =>
        {
            // will do filtering here!
            values.push(x);
        });

        this.gridSource = values;
    }

    public ngOnDestroy(): void 
    {
        this.isAlive = false;
    }

    private clearFilter(): void
    {
        this.fillGridSource();
    }

    private disableSubmitActions(): boolean 
    {
        return true;
    }

    private jobsSubmitted($event): void 
    {
        //
    }

    private getSelectedJobIds(): void
    {
        //
    }
}