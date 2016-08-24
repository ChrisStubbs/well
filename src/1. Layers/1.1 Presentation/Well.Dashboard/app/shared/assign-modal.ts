import {Component} from '@angular/core';
import {IUser} from '../shared/user';
import {ExceptionDelivery} from '../exceptions/exceptionDelivery';
import {Router} from '@angular/router';
import { HTTP_PROVIDERS, Response} from '@angular/http';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {HttpResponse} from '../shared/http-response';
import {UserJob} from '../shared/userJob';
import {ExceptionDeliveryService}  from '../exceptions/exceptionDeliveryService';


@Component({
    selector: 'assign-modal',
    templateUrl: 'app/shared/assign-modal.html',
    providers: [HTTP_PROVIDERS, ExceptionDeliveryService]
})
export class AssignModal {
    public IsVisible: boolean;
    users: IUser[];
    userJob:UserJob;
    delivery: ExceptionDelivery;
    httpResponse: HttpResponse = new HttpResponse();

    constructor(
        private exceptionDeliveryService: ExceptionDeliveryService,
        private router: Router,
        private toasterService: ToasterService) {
        this.userJob = new UserJob();
    }

    show(users: IUser[], delivery: ExceptionDelivery) {

        this.delivery = delivery;
        
        this.users = users;
        this.IsVisible = true;
    }

    hide() {
        this.IsVisible = false;
    }

    userSelected(userid, deliveryid): void {
        this.userJob.jobId = deliveryid;
        this.userJob.userId = userid;
        
        this.exceptionDeliveryService.assign(this.userJob)
            .subscribe((res: Response) => {
                    this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) this.toasterService.pop('success', 'Delivery has been assigned!', '');
                if (this.httpResponse.adamdown) this.toasterService.pop('error', 'Delivery unassigned', '');

            });

       this.hide();

    }
}