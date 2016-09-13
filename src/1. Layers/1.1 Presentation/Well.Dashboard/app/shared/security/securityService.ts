import {Injectable} from '@angular/core';
import {Router} from '@angular/router';

@Injectable()
export class SecurityService {
    actionDeliveries: string = 'ActionDeliveries';
    branchSelection: string = 'BranchSelection';
    landingPage: string = 'LandingPage';
    userBranchPreferences: string = 'UserBranchPreferences';

    constructor(private router: Router) { }

    validateUser(permissions: string[], requiredPermission: string) : void {
        if (!permissions ||
            (requiredPermission && requiredPermission.length > 0 && permissions.indexOf(requiredPermission) === -1)) {
            console.log("Permissions: " + permissions);
            console.log("Required permission: '" + requiredPermission + "'");
            this.router.navigate(['/unauthorised']);
        }
    }
}