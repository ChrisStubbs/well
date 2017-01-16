import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {LogService} from '../logService';

@Injectable()
export class SecurityService {
    public actionDeliveries: string = 'ActionDeliveries';
    public branchSelection: string = 'BranchSelection';
    public landingPage: string = 'LandingPage';
    public userBranchPreferences: string = 'UserBranchPreferences';
    public readOnly: string = 'ReadOnly';

    constructor(private router: Router, private logService: LogService) { }

    public validateUser(permissions: string[], requiredPermission: string): void {
        if (!permissions ||
            (requiredPermission && requiredPermission.length > 0 && permissions.indexOf(requiredPermission) == -1)) {
            this.logService.log('Permissions: ' + permissions);
            this.logService.log('Required permission: \'' + requiredPermission + '\'');
            this.router.navigate(['/unauthorised']);
        }
    }

    public hasPermission(permissions: string[], requiredPermission: string): boolean {
        return permissions.indexOf(requiredPermission) != -1;
    }
}