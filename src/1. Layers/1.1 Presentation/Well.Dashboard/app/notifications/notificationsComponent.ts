import { Component, OnInit}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import { Response } from '@angular/http';
import {HttpResponse} from '../shared/httpResponse';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import 'rxjs/Rx';   // Load all features

import {Notification} from './notification';
import {PaginationService } from 'ng2-pagination';
import {NotificationsService} from './notificationsService';


@Component({
    selector: 'ow-notifications',
    templateUrl: './app/notifications/notifications-list.html',
    providers: [PaginationService, NotificationsService]
})
export class NotificationsComponent implements OnInit {
    notifications: Notification[] = new Array<Notification>();
    lastRefresh = Date.now();
    notification: Notification;
    httpResponse: HttpResponse = new HttpResponse();
    rowCount: number = 3;

    constructor(private notificationsService: NotificationsService,
        private toasterService: ToasterService,
        private globalSettingsService: GlobalSettingsService,
        private securityService: SecurityService) { }
        
    getNotifications(): void {
        this.notificationsService.getNotifications()
            .subscribe(notifications => {
                this.notifications = notifications;

                    this.lastRefresh = Date.now();
                },
                error => this.lastRefresh = Date.now());
    }

    ngOnInit() {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.actionDeliveries);
        this.getNotifications();
    }

    archive(notification): void {

        this.notification = notification;
        this.notificationsService.archiveNotification(this.notification.id)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));
                    if (this.httpResponse.success) this.toasterService.pop('success', 'Notification has been archived!', '');
                    if (this.httpResponse.failure) this.toasterService.pop('error', 'Notification could not be archived at this time!', 'Please try again later!');
                    if (this.httpResponse.notAcceptable) this.toasterService.pop('warning', 'Notification id is incorrect, contact support!', '');
            });

    }

    getStyle(notification): string {
        switch (notification.type) {
            case 1:
                return 'bs-callout bs-callout-danger';
            case 2:
                return 'bs-callout bs-callout-warning';
            default:
                return 'bs-callout bs-callout-success';

        }
    }

    getHeading(notification): string {
        switch (notification.type) {
            case 1:
                return 'Credit Failed';
            case 2:
                return 'Warning';
            default:
                return 'Task';

        }
    }

}