import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {LogService} from '../logService';

@Injectable()
export class SecurityService {
    actionDeliveries: string = 'ActionDeliveries';
    branchSelection: string = 'BranchSelection';
    landingPage: string = 'LandingPage';
    userBranchPreferences: string = 'UserBranchPreferences';
    readOnly: string = 'ReadOnly';

    constructor(private router: Router,
    private logService: LogService) { }

    validateUser(permissions: string[], requiredPermission: string) : void {
        if (!permissions ||
            (requiredPermission && requiredPermission.length > 0 && permissions.indexOf(requiredPermission) === -1)) {
            this.logService.log("Permissions: " + permissions);
            this.logService.log("Required permission: '" + requiredPermission + "'");
            this.router.navigate(['/unauthorised']);
        }
    }

    hasPermission(permissions: string[], requiredPermission: string): boolean {

        if (permissions.indexOf(requiredPermission) !== -1)
            return true;

        return false;
    }
}