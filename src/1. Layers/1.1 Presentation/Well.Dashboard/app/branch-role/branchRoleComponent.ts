import {Component, OnInit}  from '@angular/core';
import {SecurityService} from '../shared/services/securityService';

@Component({
    selector: 'ow-branch-role',
    templateUrl: './app/branch-role/branch-role.html'
})
export class BranchRoleComponent implements OnInit
{
    constructor(private securityService: SecurityService) { }

    public ngOnInit(): void
    {
        this.securityService.validateAccess(SecurityService.adminPages);
    }
}
