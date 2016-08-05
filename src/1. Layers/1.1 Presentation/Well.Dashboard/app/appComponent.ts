import { Component, OnInit, ChangeDetectorRef}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_DIRECTIVES  } from '@angular/router';
import {GlobalSettingsService} from './shared/globalSettings';
import {BranchService} from './branch/branchService';
import {RefreshService} from './shared/refreshService';
import {HttpErrorService} from './shared/httpErrorService';
import {ToasterContainerComponent, ToasterService} from 'angular2-toaster/angular2-toaster';
import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-app',
    templateUrl: 'home/applayout',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, BranchService, RefreshService, ToasterService, HttpErrorService],
    directives: [ROUTER_DIRECTIVES, ToasterContainerComponent]
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