import { HttpErrorService }         from './../shared/services/httpErrorService';
import { HttpService }              from './../shared/services/httpService';
import { GlobalSettingsService }    from './../shared/globalSettings';
import { BranchService }            from './../shared/branch/branchService';
import { Component }                from '@angular/core';
import { IObservableAlive }         from './../shared/IObservableAlive';
import * as moment                  from 'moment';
import { ToasterService }           from 'angular2-toaster';

@Component({
    selector: 'ow-closeExceptions',
    templateUrl: './app/exceptions/CloseExceptionsForBranchComponent.html',
    providers: [BranchService, HttpService] 
})
export class CloseExceptionsForBranchComponent implements IObservableAlive
{
    public isAlive: boolean = true;

    private branches: Array<[string, string]>;
    private selectedBranch: string;
    private selectedDateFrom: string;
    private selectedDateTo: string;

    constructor(
        private branchService: BranchService,
        private globalSettingsService: GlobalSettingsService,
        private http: HttpService,
        private toasterService: ToasterService,
        private httpErrorService: HttpErrorService) {}

    public ngOnInit(): void
    {
        this.fillBranches();
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    private fillBranches(): void
    {
        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.isAlive)
            .subscribe(branches => {
                this.branches = <any>branches;
            });
    }

    private closeActions(): void
    {
        const body = {
            branchId: +this.selectedBranch,
            from: moment.utc(this.selectedDateFrom).toDate(),
            to: moment.utc(this.selectedDateTo).toDate()
        };
        const url = this.globalSettingsService.globalSettings.apiUrl + 'CloseExceptionsForBranch';

        this.http.post(url, body)
        .takeWhile(() => this.isAlive)
        .catch(e => this.httpErrorService.handleError(e))
        .subscribe(resp => this.toasterService.pop('success', 'done', ''));
    }
}