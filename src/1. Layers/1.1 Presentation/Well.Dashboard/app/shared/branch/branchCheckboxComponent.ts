import {Component, OnInit, Input} from '@angular/core';
import {Branch} from './branch';
import {BranchService} from './branchService';

@Component({
    selector: 'ow-branch-select',
    templateUrl: './app/shared/branch/branch-select.html'
})
export class BranchCheckboxComponent implements OnInit{
    branches: Branch[];
    selectedBranches: Array<Branch> = [];
    selectAllCheckbox: boolean = false;    
    @Input() username: string;
    @Input() seasonalDateId: number;
    @Input() creditThresholdId: number;

    constructor(private branchService: BranchService) {}

    ngOnInit(): void {
        if (this.creditThresholdId) {
            this.branchService.getBranchesWithCreditThreshold(this.creditThresholdId)
                .subscribe(branches => {
                    this.branches = branches;
                    this.branches.forEach(branch => { if (branch.selected) this.selectedBranches.push(branch) });

                    if (this.branches.every(x => x.selected)) { this.selectAllCheckbox = true; }
                });

        } else if (this.seasonalDateId) {
            this.branchService.getBranchesWithSeasonalDate(this.seasonalDateId)
                .subscribe(branches => {
                    this.branches = branches;
                    this.branches.forEach(branch => { if (branch.selected) this.selectedBranches.push(branch) });

                    if (this.branches.every(x => x.selected)) { this.selectAllCheckbox = true; }
                });
            
        } else {
            this.branchService.getBranches(this.username)
                .subscribe(branches => {
                    this.branches = branches;
                    this.branches.forEach(branch => { if (branch.selected) this.selectedBranches.push(branch) });

                    if (this.branches.every(x => x.selected)) { this.selectAllCheckbox = true; }
                });
        }
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
}