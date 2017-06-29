import { Component }            from '@angular/core';
import {GlobalSettingsService}  from './shared/globalSettings';
import {BranchService}          from './shared/branch/branchService';
import {SecurityService}        from './shared/security/securityService';
import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-app',
    templateUrl: 'home/applayout'
})
export class AppComponent {
    public version: string = '';
    public branches: string = '';
    public IsWellAdmin: boolean;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private branchService: BranchService,
        private securityService: SecurityService) {
        securityService.validateUser(this.globalSettingsService.globalSettings.permissions, '');
        this.version = this.globalSettingsService.globalSettings.version;
        this.fetchBranches();
        this.branchService.userBranchesChanged$.subscribe(b => this.fetchBranches());

        this.IsWellAdmin = securityService.hasPermission(
            globalSettingsService.globalSettings.permissions, 
            'WellAdmin') || false;
    }

    private fetchBranches() {
        this.globalSettingsService.getBranches().subscribe(branches => {
            this.branches = branches || 'Select Branches';
        });
    }
}