import { Component, OnInit, ViewChild}  from '@angular/core';
import { Response }                     from '@angular/http';
import {ActivatedRoute}                 from '@angular/router';
import {BranchService}                  from '../shared/branch/branchService';
import {HttpResponse}                   from '../shared/httpResponse';
import {ToasterService}                 from 'angular2-toaster/angular2-toaster';
import {GlobalSettingsService}          from '../shared/globalSettings';
import {SecurityService}                from '../shared/security/securityService';
import {BranchCheckboxComponent}        from '../shared/branch/branchCheckboxComponent';

@Component({
    selector: 'ow-branch',
    templateUrl: './app/branch/branch-list.html'
})
export class BranchSelectionComponent implements OnInit {
    public errorMessage: string;
    public httpResponse: HttpResponse = new HttpResponse();
    public username: string;
    public domain: string;

    constructor(private branchService: BranchService,
                private toasterService: ToasterService,
                private globalSettingsService: GlobalSettingsService,
                private securityService: SecurityService,
                private route: ActivatedRoute) { }

    public ngOnInit(): void {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.branchSelection);
        this.route.params.subscribe(params => {
            this.username = params['name'] === undefined ? '' : params['name']; this.domain = params['domain'];
        });
    }

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;

    public save(): void {
        this.branchService.saveBranches(this.branch.selectedBranches, this.username, this.domain)
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