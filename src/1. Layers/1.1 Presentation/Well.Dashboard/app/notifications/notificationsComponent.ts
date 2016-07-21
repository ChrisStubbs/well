import { Component, OnInit}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features


@Component({
    selector: 'ow-notifications',
    templateUrl: './app/notifications/notifications-list.html'
})
export class NotificationsComponent implements OnInit {
   
    ngOnInit() {

    }
}