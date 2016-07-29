import { Component, OnInit, ChangeDetectorRef}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_DIRECTIVES  } from '@angular/router';

import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-app',
    templateUrl: 'applayout',
    providers: [HTTP_PROVIDERS],
    directives: [ROUTER_DIRECTIVES]
})

export class AppComponent {
    
}