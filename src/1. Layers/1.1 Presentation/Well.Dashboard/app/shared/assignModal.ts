import { Component, EventEmitter, Output, Input, OnInit, OnDestroy }    from '@angular/core';
import { Router }                                                       from '@angular/router';
import { Response }                                                     from '@angular/http';
import { ToasterService }                                               from 'angular2-toaster/angular2-toaster';
import { IUser, HttpResponse, UserService, AssignModel }                from './shared';
import { UserJobs }                                                     from './userJobs';
import * as _                                                           from 'lodash';
import {IObservableAlive}                                               from './IObservableAlive';

@Component({
    selector: 'assign-modal',
    templateUrl: 'app/shared/assign-modal.html'
})
export class AssignModal implements IObservableAlive, OnInit, OnDestroy
{
    public isVisible: boolean = false;
    public users: IUser[];
    public userJobs: UserJobs;
    public httpResponse: HttpResponse = new HttpResponse();
    public assigned = false;
    public isAlive: boolean = true;
    @Input() public model: AssignModel;
    @Output() public onAssigned = new EventEmitter();
    constructor(
        private userService: UserService,
        private router: Router,
        private toasterService: ToasterService)
    {
        this.userJobs = new UserJobs();
    }

    public ngOnInit()
    {
        this.userService.getUsersForBranch(this.model.branch.id)
            .takeWhile(() => this.isAlive)
            .subscribe(users =>
            {
                this.users = _.filter(users, current => current.name != this.model.assigned);
            });
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public hide()
    {
        this.isVisible = false;
    }

    public userSelected(userid: number, model: AssignModel): void
    {
        this.userJobs.jobIds = model.jobIds;
        this.userJobs.userId = userid;
        this.model.assignedTo = _.find(this.users, current => current.id = userid).name;
        this.userService.assign(this.userJobs)
            .takeWhile(() => this.isAlive)
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
            .takeWhile(() => this.isAlive)
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