﻿import { Component, OnInit, ViewChild}  from '@angular/core';
import { GlobalSettingsService} from '../shared/globalSettings';
import { Response } from '@angular/http';
import {HttpResponse} from '../shared/httpResponse';
import { SecurityService} from '../shared/security/securityService';
import { UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import { ToasterService} from 'angular2-toaster/angular2-toaster';
import 'rxjs/Rx';   // Load all features
import * as lodash from 'lodash';

import { Notification} from './notification';
import { PaginationService } from 'ng2-pagination';
import { NotificationsService} from './notificationsService';
import { NotificationModalComponent} from './notificationModalComponent'

@Component({
    selector: 'ow-notifications',
    templateUrl: './app/notifications/notifications-list.html',
    providers: [PaginationService, NotificationsService]
})

export class NotificationsComponent implements OnInit {
    public notifications: Notification[] = new Array<Notification>();
    public lastRefresh = Date.now();
    public notification: Notification;
    public httpResponse: HttpResponse = new HttpResponse();
    public rowCount: number = 3;

    constructor(
        private notificationsService: NotificationsService,
        private toasterService: ToasterService,
        private globalSettingsService: GlobalSettingsService,
        private securityService: SecurityService) { }

    public ngOnInit() {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.getNotifications();
    }
        
    @ViewChild(NotificationModalComponent) public archiveModal: NotificationModalComponent;

    public getNotifications(): void {
        this.notificationsService.getNotifications()
            .subscribe(notifications => {
                this.notifications = notifications;
                this.lastRefresh = Date.now();
            },
            error => this.lastRefresh = Date.now());
    }

    public archive(notification: Notification): void {
        this.archiveModal.show(notification);
    }

    public getStyle(notification): string {
        switch (notification.type) {
            case 1:
                return 'bs-callout bs-callout-danger';
            case 2:
                return 'bs-callout bs-callout-warning';
            default:
                return 'bs-callout bs-callout-success';

        }
    }

    public getHeading(notification): string {
        switch (notification.type) {
            case 1:
                return 'Credit Failed';
            case 2:
                return 'Warning';
            default:
                return 'Task';
        }
    }

    public onArchived(notification: Notification) {
        const isLastNotificationOnPage = this.notifications.length % this.rowCount === 1;
        lodash.remove(this.notifications, notification);

        if (isLastNotificationOnPage) {
            location.reload();
        }       
    }
}