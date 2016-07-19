import { Component, OnInit}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features


@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html'
})
export class ExceptionsComponent implements OnInit {
   
    ngOnInit() {
    }
}