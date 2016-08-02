import { Component, OnInit, ChangeDetectorRef}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_DIRECTIVES  } from '@angular/router';
import {GlobalSettingsService} from './shared/globalSettings';

import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-app',
    templateUrl: 'home/applayout',
    providers: [HTTP_PROVIDERS, GlobalSettingsService],
    directives: [ROUTER_DIRECTIVES]
})

export class AppComponent {
    version: string = "";
    branches: string = "";

    constructor(private globalSettingsService: GlobalSettingsService) {
        this.globalSettingsService.getVersion().subscribe(version => this.version = version);
        this.globalSettingsService.getBranches().subscribe(branches => this.branches = branches);
    }
}