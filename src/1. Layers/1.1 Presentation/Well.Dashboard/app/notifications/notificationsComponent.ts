import { Component, OnInit}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import {SecurityService} from '../shared/security/security-service';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import 'rxjs/Rx';   // Load all features


@Component({
    selector: 'ow-notifications',
    templateUrl: './app/notifications/notifications-list.html'
})
export class NotificationsComponent implements OnInit {

    constructor(private globalSettingsService: GlobalSettingsService, private securityService: SecurityService) {}

    ngOnInit() {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.actionDeliveries);
    }
}