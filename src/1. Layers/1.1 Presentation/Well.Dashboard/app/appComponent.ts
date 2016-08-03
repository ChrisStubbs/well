import { Component, OnInit, ChangeDetectorRef}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_DIRECTIVES  } from '@angular/router';
import {GlobalSettingsService} from './shared/globalSettings';
import {BranchService} from './branch/branchService';

import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-app',
    templateUrl: 'home/applayout',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, BranchService],
    directives: [ROUTER_DIRECTIVES]
})

export class AppComponent {
    version: string = "";
    branches: string = "";

    constructor(private globalSettingsService: GlobalSettingsService, private branchService: BranchService) {
        this.globalSettingsService.getVersion().subscribe(version => this.version = version);
        this.fetchBranches();
        this.branchService.userBranchesChanged$.subscribe(b => this.fetchBranches());
    }
    
    private fetchBranches() {
        this.globalSettingsService.getBranches().subscribe(branches => this.branches = branches);
    }
}