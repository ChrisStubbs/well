import { Component, OnInit}  from '@angular/core';
import { HTTP_PROVIDERS, Response } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {IBranch} from './branch';
import {BranchService} from './branchService';
import {HttpResponse} from '../shared/http-response';
import {ToasterContainerComponent, ToasterService} from 'angular2-toaster/angular2-toaster';

@Component({
    selector: 'ow-branch',
    templateUrl: './app/branch/branch-list.html',
    directives: [ToasterContainerComponent],
    providers: [HTTP_PROVIDERS, GlobalSettingsService, BranchService, ToasterService]
})
export class BranchSelectionComponent implements OnInit {
    errorMessage: string;
    branches: IBranch[];
    selectedBranches: Array<IBranch> = [];
    selectAllCheckbox: boolean;
    httpResponse: HttpResponse = new HttpResponse();

    constructor(private branchService: BranchService, private toasterService: ToasterService){
    }

    ngOnInit(): void {
        this.branchService.getBranches()
            .subscribe(branches => this.branches = branches,
            error => this.errorMessage = <any>error);
    }

    selectAll(selected): void {
        this.branches.forEach(branch => {
            branch.selected = selected;

            if (selected) {
                this.selectedBranches.push(branch);
            } else {
                this.selectedBranches = [];
            }
        });
    }

    selectBranch(branch): void {
        var index = this.selectedBranches.indexOf(branch, 0);

        if (index > -1 && branch.selected === false) {
            this.selectedBranches.splice(index, 1);
        } else {
            this.selectedBranches.push(branch);
        }

        if (this.selectedBranches.length > 1 && this.selectedBranches.length === this.branches.length) {
            this.selectAllCheckbox = true;
        } else {
            this.selectAllCheckbox = false;
        }
    }
    
    save(): void {
        this.branchService.saveBranches(this.selectedBranches)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) this.toasterService.pop('success', 'Branches have been saved!', '');
                if (this.httpResponse.failure) this.toasterService.pop('error', 'Branches could not be saved at this time!', 'Please try again later!');
                if (this.httpResponse.notAcceptable) this.toasterService.pop('warning', 'Please select at least one branch!', ''); 


            });
    }
}