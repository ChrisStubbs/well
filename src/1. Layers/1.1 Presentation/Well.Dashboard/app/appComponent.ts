import { Component }                from '@angular/core';
import { GlobalSettingsService }    from './shared/globalSettings';
import { BranchService }            from './shared/branch/branchService';
import { SecurityService }          from './shared/security/securityService';
import { IObservableAlive }         from './shared/IObservableAlive';
import 'rxjs/Rx';

@Component({
    selector: 'ow-app',
    templateUrl: 'home/applayout'
})
export class AppComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public version: string = '';
    public branches: string = '';
    public IsWellAdmin: boolean;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private branchService: BranchService,
        private securityService: SecurityService)
    {
        this.version = this.globalSettingsService.globalSettings.version;
        this.fetchBranches();
        this.branchService.userBranchesChanged$
            .takeWhile(() => this.isAlive)
            .subscribe(b => this.fetchBranches());

        this.IsWellAdmin = securityService.userHasPermission(SecurityService.adminPages) || false;
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public ngOnInit(): void
    {
        this.isAlive = true;
    }

    private fetchBranches()
    {
        this.globalSettingsService.getBranches()
            .takeWhile(() => this.isAlive)
            .subscribe(branches => this.branches = branches || 'Select Branches');
    }
}