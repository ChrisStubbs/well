import { Injectable }                   from '@angular/core';
import { Router }                       from '@angular/router';
import { GlobalSettingsService }        from '../globalSettings';
import { SessionStorageService }        from 'ngx-webstorage';

@Injectable()
export class SecurityService
{
    public static manuallyCompleteBypass: string = 'Manually Complete Or Bypass';
    public static editExceptions: string = 'Edit Exceptions';
    public static allocateUsers: string = 'Allocate Users';
    public static adminPages: string = 'Access Admin Pages';
    public static submitCreditApprovals: string = 'Submit Credit Approvals';
    public static submitMissingGRN: string = 'Submit Missing GRN';

    constructor(private router: Router,
                private storageService: SessionStorageService) { }

    public userHasPermission(permission: string): boolean
    {
        return GlobalSettingsService.getCachedPermissions(this.storageService).includes(permission);
    }

    public validateAccess(permission: string): void
    {
        if (!this.userHasPermission(permission))
        {
            this.router.navigate(['/unauthorised']);
        }
    }
}