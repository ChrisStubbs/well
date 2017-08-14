import { Component, Output, Input } from '@angular/core';
import {BranchService} from './branchService';

@Component({
    selector: 'ow-branch-name',
    template: '{{branchName}} ({{branchId}})',
    providers: [BranchService]
})
export class BranchNameComponent {
    @Input() public branchId: number;
    @Output() public branchName: string;

    constructor(private branchService: BranchService) {

    }

    public ngOnChanges() {
        if (this.branchId) {
            this.branchService.getById(this.branchId).subscribe((branch): void => {
                this.branchName = branch.name;
            });
        }
    }
}