import {Component, OnInit}  from '@angular/core';
import {SecurityService} from '../shared/security/securityService';
import {GlobalSettingsService} from '../shared/globalSettings';

@Component({
    selector: 'ow-branch-role',
    templateUrl: './app/branch-role/branch-role.html'
})
export class BranchRoleComponent implements OnInit {

    public roles: string[] = [ 'Customer Service User', 'Customer Service Manager', 'Branch Manager' ];

    constructor(private securityService: SecurityService, private globalSettingsService: GlobalSettingsService) { }

    public ngOnInit(): void {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.branchSelection);
    }

    public save(): void {
        console.log('save this');
    }
}
