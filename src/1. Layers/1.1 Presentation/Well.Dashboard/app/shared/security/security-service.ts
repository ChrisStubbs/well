import {Injectable} from '@angular/core';
import {Router} from '@angular/router';

@Injectable()
export class SecurityService {
    actionDeliveries: string = 'ActionDeliveries';
    branchSelection: string = 'BranchSelection';
    landingPage: string = 'LandingPage';
    userBranchPreferences: string = 'UserBranchPreferences';

    constructor(private router: Router) { }

    validateUser(permissions: string[], requiredPermission: string) {
        if (permissions && permissions.indexOf(requiredPermission) === -1) {
            this.router.navigate(['/unauthorised']);
        }
    }
}