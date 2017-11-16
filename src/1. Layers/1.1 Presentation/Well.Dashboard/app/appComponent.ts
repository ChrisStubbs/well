import { Component }                from '@angular/core';
import { GlobalSettingsService }    from './shared/globalSettings';
import { BranchService }            from './shared/branch/branchService';
import { SecurityService }          from './shared/services/securityService';
import { IObservableAlive }         from './shared/IObservableAlive';
import 'rxjs/Rx';
import {GlobalSettings} from './shared/globalSettings';

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
    public IsWellDeveloper: boolean;

    private get currentBranchIdDisplay(): string {
        if (this.globalSettingsService.currentBranchId) {
            return `(${ this.globalSettingsService.currentBranchId})`;
        }
        return undefined;
    }
    
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
        this.IsWellDeveloper = securityService.userHasPermission(SecurityService.developer) || false;
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