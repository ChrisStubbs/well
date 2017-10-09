import { Component, EventEmitter, Output, Input }   from '@angular/core';
import { Response }                                 from '@angular/http';
import { IUser, HttpResponse, UserService }         from '../shared';
import { UserJobs }                                 from '../models/userJobs';
import * as _                                       from 'lodash';
import { IObservableAlive }                         from '../IObservableAlive';
import { AssignModalResult }                        from './assignModel';
import { ToasterService }                           from 'angular2-toaster';
import { AssignModel }                              from './assignModel';
import { SecurityService }                          from '../services/securityService';

@Component({
    selector: 'assign-modal',
    templateUrl: 'app/shared/components/assign-modal.html'
})
export class AssignModal implements IObservableAlive
{
    public isVisible: boolean = false;
    public users: IUser[];
    public userJobs: UserJobs;
    public httpResponse: HttpResponse = new HttpResponse();
    public assigned = false;
    public isAlive: boolean = true;

    @Input() public model: AssignModel;
    @Output() public onAssigned = new EventEmitter();

    private allUsers: IUser[];
    private isReadOnlyUser: boolean;

    constructor(
        private securityService: SecurityService,
        private userService: UserService,
        private toasterService: ToasterService)
    {
        this.userJobs = new UserJobs();
    }

    private buildUsersSource(): void
    {
        this.users = _.filter(this.allUsers,
            (current: IUser) =>
            {
                return _.isNil(this.model.assigned) || current.name != this.model.assigned;
            });
    }

    public ngOnInit()
    {
        this.isReadOnlyUser = !this.securityService.userHasPermission(SecurityService.allocateUsers);
        this.isAlive = true;
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public hide()
    {
        this.isVisible = false;
    }

    public show() {
        this.userService.getUsers()
            .takeWhile(() => this.isAlive)
            .subscribe(users => {
                this.allUsers = users;
                this.buildUsersSource();
            });
        this.isVisible = true;

    }

    public userSelected(user: IUser, newModel: AssignModel): void
    {
        this.userJobs.jobIds = newModel.jobIds;
        this.userJobs.userId = user.id;
        this.userJobs.allocatePendingApprovalJobs = newModel.allocatePendingApprovalJobs;
        this.userService.assign(this.userJobs)
            .takeWhile(() => this.isAlive)
            .subscribe((res) => this.handleResponse(res, newModel, user));
    }

    public unassign(newModel: AssignModel): void
    {
        this.userService.unassign(newModel.jobIds)
            .takeWhile(() => this.isAlive)
            .subscribe((res: Response) => this.handleResponse(res, newModel, undefined));
    }

    private handleResponse(res: any, newModel: AssignModel, user: IUser): void
    {
        const result = new AssignModalResult();

        this.httpResponse = JSON.parse(JSON.stringify(res));

        if (this.httpResponse.success)
        {
            if (user) {
                newModel.assigned = user.name;
                result.newUser = user;
            } else {
                newModel.assigned = undefined;
                result.newUser = undefined;
            }

            result.assigned = true;
            this.model = newModel;
            this.toasterService.pop('success', 'Jobs have been assigned', '');
        }
        if (this.httpResponse.failure)
        {
            this.toasterService.pop('error', 'Jobs unassigned', '');
        }
        this.hide();
        result.source = newModel.objectSource;

        this.onAssigned.emit(result);
        this.buildUsersSource();
    }
}