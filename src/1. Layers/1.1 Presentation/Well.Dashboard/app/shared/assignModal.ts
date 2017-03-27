import {Component, EventEmitter, Output} from '@angular/core';
import {IUser} from '../shared/iuser';
import {BaseDelivery} from '../shared/baseDelivery';
import {Router} from '@angular/router';
import {Response} from '@angular/http';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {HttpResponse} from '../shared/httpResponse';
import {UserJob} from '../shared/userJob';
import {UserService}  from './userService';

@Component({
    selector: 'assign-modal',
    templateUrl: 'app/shared/assign-modal.html'
})
export class AssignModal {
    public isVisible: boolean = false;
    public users: IUser[];
    public userJob: UserJob;
    public delivery: BaseDelivery;
    public httpResponse: HttpResponse = new HttpResponse();
    @Output() public onAssigned = new EventEmitter();
    public assigned = false;

    constructor(
        private userService: UserService,
        private router: Router,
        private toasterService: ToasterService) {
        this.userJob = new UserJob();
    }

    public show(delivery: BaseDelivery) {
        this.delivery = delivery;

        this.userService.getUsersForBranch(this.delivery.branchId)
            .subscribe(users => {
                this.users = users;
                this.isVisible = true;
            });
    }

    public hide() {
        this.isVisible = false;
    }

    public userSelected(userid, delivery): void {
        this.userJob.jobId = delivery.id;
        this.userJob.userId = userid;
        
        this.userService.assign(this.userJob)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Delivery has been assigned', '');
                    this.assigned = true;
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop('error', 'Delivery unassigned', '');
                }
                this.hide();
                this.onAssigned.emit(this.assigned); 
        });
    }

    public unassign(delivery): void {

        this.userService.unassign(delivery.id)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Delivery has been unassigned', '');
                    this.assigned = true;
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop('error', 'Delivery still assigned', '');
                }
            });

        this.hide();
        this.onAssigned.emit({ event: event, isAssigned: this.assigned, delivery: this.delivery});
    }
}