import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Response } from '@angular/http';
import { ToasterService } from 'angular2-toaster/angular2-toaster';
import { IUser, HttpResponse, UserService, AssignModel } from './shared';
import { UserJobs } from './userJobs';
import * as _ from 'lodash';

@Component({
    selector: 'assign-modal',
    templateUrl: 'app/shared/assign-modal.html'
})
export class AssignModal
{
    public isVisible: boolean = false;
    public users: IUser[];
    public userJobs: UserJobs;
    public model: AssignModel;

    public httpResponse: HttpResponse = new HttpResponse();
    @Output() public onAssigned = new EventEmitter();
    public assigned = false;

    constructor(
        private userService: UserService,
        private router: Router,
        private toasterService: ToasterService)
    {
        this.userJobs = new UserJobs();
    }

    public show(model: AssignModel)
    {
        this.model = model;
        this.userService.getUsersForBranch(model.branch.id)
            .subscribe(users =>
            {
                this.users = _.filter(users, current => current.name != model.assigned);
                this.isVisible = true;
            });
    }

    public hide()
    {
        this.isVisible = false;
    }
    public userSelected(userid: number, model: AssignModel): void
    {
        this.userJobs.jobIds = model.jobIds;
        this.userJobs.userId = userid;

        this.userService.assign(this.userJobs)
            .subscribe((res: Response) =>
            {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success)
                {
                    this.toasterService.pop('success', 'Jobs have been assigned', '');
                    this.assigned = true;
                }
                if (this.httpResponse.failure)
                {
                    this.toasterService.pop('error', 'Jobs unassigned', '');
                }
                this.hide();
                this.onAssigned.emit(this.assigned);
            });
    }

    public unassign(model: AssignModel): void
    {
        this.userService.unassign(model.jobIds)
            .subscribe((res: Response) =>
            {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success)
                {
                    this.toasterService.pop('success', 'Jobs have been unassigned', '');
                    this.assigned = true;
                }
                if (this.httpResponse.failure)
                {
                    this.toasterService.pop('error', 'Jobs are still assigned', '');
                }
            });

        this.hide();
        this.onAssigned.emit({ event: event, isAssigned: this.assigned, model: this.model });
    }
}