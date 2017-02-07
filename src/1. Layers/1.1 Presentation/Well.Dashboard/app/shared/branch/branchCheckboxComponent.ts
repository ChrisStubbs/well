import {Component, OnInit, Input} from '@angular/core';
import {Branch} from './branch';
import {BranchService} from './branchService';

@Component({
    selector: 'ow-branch-select',
    templateUrl: './app/shared/branch/branch-select.html'
})
export class BranchCheckboxComponent implements OnInit {
    public branches: Branch[];
    public selectedBranches: Array<Branch> = [];
    public selectAllCheckbox: boolean = false;    
    @Input() public username: string;
    @Input() public seasonalDateId: number;
    @Input() public creditThresholdId: number;
    @Input() public cleanPreferenceId: number;

    constructor(private branchService: BranchService) {}

    public ngOnInit(): void {
        if (this.creditThresholdId) {
            this.branchService.getBranchesWithCreditThreshold(this.creditThresholdId)
                .subscribe(branches => {
                    this.branches = branches;
                    this.branches.forEach(branch => {
                        if (branch.selected) {
                            this.selectedBranches.push(branch)
                        }
                    });

                    if (this.branches.every(x => x.selected)) {
                        this.selectAllCheckbox = true;
                    }
                });

        } else if (this.cleanPreferenceId) {
            this.branchService.getBranchesWithCleanPreference(this.cleanPreferenceId)
                .subscribe(branches => {
                    this.branches = branches;
                    this.branches.forEach(branch => {
                        if (branch.selected) {
                            this.selectedBranches.push(branch)
                        }
                    });

                    if (this.branches.every(x => x.selected)) {
                        this.selectAllCheckbox = true;
                    }
                });

        } else if (this.seasonalDateId) {
            this.branchService.getBranchesWithSeasonalDate(this.seasonalDateId)
                .subscribe(branches => {
                    this.branches = branches;
                    this.branches.forEach(branch => {
                        if (branch.selected) {
                            this.selectedBranches.push(branch)
                        }
                    });

                    if (this.branches.every(x => x.selected)) {
                        this.selectAllCheckbox = true;
                    }
                });
            
        } else {
            this.branchService.getBranches(this.username)
                .subscribe(branches => {
                    this.branches = branches;
                    this.branches.forEach(branch => {
                        if (branch.selected) {
                            this.selectedBranches.push(branch)
                        }
                    });

                    if (this.branches.every(x => x.selected)) {
                        this.selectAllCheckbox = true;
                    }
                });
        }
    }

    public selectAll(selectAllCheckbox): void {
        this.branches.forEach(branch => {
            const index = this.selectedBranches.indexOf(branch, 0);
            if (index > -1) {
                this.selectedBranches.splice(index, 1);
            }
            branch.selected = selectAllCheckbox;

            if (selectAllCheckbox) {
                this.selectedBranches.push(branch);
            } else {
                this.selectedBranches = [];
            }
        });
    }

    public selectBranch(branch): void {
        const index = this.selectedBranches.indexOf(branch, 0);

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