import { Component, OnInit}  from '@angular/core';
import { HTTP_PROVIDERS, Response } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {IBranch} from './branch';
import {BranchService} from './branchService';
import {HttpResponse} from '../shared/http-response';

@Component({
    selector: 'ow-branch',
    templateUrl: './app/branch/branch-list.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, BranchService]
})
export class BranchSelectionComponent implements OnInit {
    errorMessage: string;
    branches: IBranch[];
    selectedBranches: Array<IBranch> = [];
    selectAllCheckbox: boolean;
    httpResponse: HttpResponse = new HttpResponse();

    constructor(private branchService: BranchService) {
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
            .subscribe((res: Response) => this.httpResponse = JSON.parse(JSON.stringify(res)));
    }
}