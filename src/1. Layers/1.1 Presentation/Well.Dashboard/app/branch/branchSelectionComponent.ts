import { Component, ViewChild}          from '@angular/core';
import { Response }                     from '@angular/http';
import {ActivatedRoute}                 from '@angular/router';
import {BranchService}                  from '../shared/branch/branchService';
import {HttpResponse}                   from '../shared/models/httpResponse';
import {ToasterService}                 from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent}        from '../shared/branch/branchCheckboxComponent';
import {IObservableAlive}               from '../shared/IObservableAlive';

@Component({
    selector: 'ow-branch',
    templateUrl: './app/branch/branch-list.html'
})
export class BranchSelectionComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public httpResponse: HttpResponse = new HttpResponse();
    public username: string;
    public domain: string;
    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;

    constructor(private branchService: BranchService,
                private toasterService: ToasterService,
                private route: ActivatedRoute) { }

    public ngOnInit(): void
    {
        this.route.params
            .takeWhile(() => this.isAlive)
            .subscribe(params =>
            {
                this.username = params['name'] === undefined ? '' : params['name']; this.domain = params['domain'];
            });
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }
    
    public save(): void {
        this.branchService.saveBranches(this.branch.selectedBranches, this.username, this.domain)
            .takeWhile(() => this.isAlive)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Branches have been saved', '');
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Branches could not be saved at this time',
                        'Please try again later!');
                }
                if (this.httpResponse.notAcceptable) {
                    this.toasterService.pop('warning', 'Please select at least one branch', '');
                }
            });
    }
}