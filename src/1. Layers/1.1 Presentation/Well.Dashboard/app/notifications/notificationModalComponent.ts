import {Component, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {HttpResponse} from '../shared/httpResponse';
import {NotificationsService} from './notificationsService';
import {Notification} from './notification';

@Component({
    selector: 'ow-notifications-modal',
    templateUrl: './app/notifications/notification-archive-modal.html'
})
export class NotificationModalComponent {
    isVisible: boolean = false;
    notification: Notification;
    httpResponse: HttpResponse = new HttpResponse();
    @Output() onArchived = new EventEmitter<Notification>();

    constructor(private notificationsService: NotificationsService, private toasterService: ToasterService) { }

    show(notification: Notification) {
        this.notification = notification;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    yes() {
        this.notificationsService.archiveNotification(this.notification.id)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));
                if (this.httpResponse.success) this.toasterService.pop('success', 'Notification has been archived!', '');
                if (this.httpResponse.failure) this.toasterService.pop('error', 'Notification could not be archived at this time!', 'Please try again later!');
                if (this.httpResponse.notAcceptable) this.toasterService.pop('warning', 'Notification id is incorrect, contact support!', '');

                this.isVisible = false;

                this.onArchived.emit(this.notification);
            });
    }
}