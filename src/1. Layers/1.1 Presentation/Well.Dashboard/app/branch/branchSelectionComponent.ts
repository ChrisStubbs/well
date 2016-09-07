﻿import { Component, OnInit}  from '@angular/core';
import { Response } from '@angular/http';
import {ActivatedRoute} from '@angular/router';
import 'rxjs/Rx';   // Load all features
import {Branch} from './branch';
import {BranchService} from './branchService';
import {HttpResponse} from '../shared/http-response';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {GlobalSettingsService} from '../shared/globalSettings';

@Component({
    selector: 'ow-branch',
    templateUrl: './app/branch/branch-list.html'
})
export class BranchSelectionComponent implements OnInit {
    errorMessage: string;
    branches: Branch[];
    usersBranchIds: number[];
    selectedBranches: Array<Branch> = [];
    selectAllCheckbox: boolean = false;
    httpResponse: HttpResponse = new HttpResponse();
    username: string;
    domain: string;

    constructor(private branchService: BranchService,
        private toasterService: ToasterService,
        private globalSettingsService: GlobalSettingsService,
        private route: ActivatedRoute) {
        route.params.subscribe(params => {
            this.username = params['name'] === undefined ? '' : params['name']; this.domain = params['domain'];
        });
    }

    ngOnInit(): void {
        this.branchService.getBranches(this.username)
            .subscribe(branches => {
                this.branches = branches;
                this.branches.forEach(branch => { if(branch.selected) this.selectedBranches.push(branch) });

                if (this.branches.every(x => x.selected)){ this.selectAllCheckbox = true;}
            },
            error => this.errorMessage = <any>error);
    }

    selectAll(selectAllCheckbox): void {
        var selected = !selectAllCheckbox;

        this.branches.forEach(branch => {
            var index = this.selectedBranches.indexOf(branch, 0);
            if (index > -1) {
                this.selectedBranches.splice(index, 1);
            }
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

        if (index > -1 && !branch.selected) {
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
        this.branchService.saveBranches(this.selectedBranches, this.username, this.domain)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) this.toasterService.pop('success', 'Branches have been saved!', '');
                if (this.httpResponse.failure) this.toasterService.pop('error', 'Branches could not be saved at this time!', 'Please try again later!');
                if (this.httpResponse.notAcceptable) this.toasterService.pop('warning', 'Please select at least one branch!', '');
            });
    }
}

