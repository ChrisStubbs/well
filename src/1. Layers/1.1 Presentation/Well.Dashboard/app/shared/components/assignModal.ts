import { Component, EventEmitter, Output, Input }   from '@angular/core';
import { Response }                                 from '@angular/http';
import { IUser, HttpResponse, UserService }         from '../shared';
import { UserJobs, AssignJobResult }                from '../models/userJobs';
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
    public filtredUsers: IUser[];
    public userJobs: UserJobs;
    public httpResponse: HttpResponse = new HttpResponse();
    public assigned = false;
    public isAlive: boolean = true;

    @Input() public model: AssignModel;
    @Output() public onAssigned = new EventEmitter();

    private allUsers: IUser[];
    private isReadOnlyUser: boolean;
    private name: string;

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
        this.filterItem('');
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
            .subscribe((result: AssignJobResult) => this.handleResponse(result, newModel, user));
    }

    public unassign(newModel: AssignModel): void
    {
        this.userService.unassign(newModel.jobIds)
            .takeWhile(() => this.isAlive)
            .subscribe((result: AssignJobResult) => this.handleResponse(result, newModel, undefined));
    }

    private handleResponse(res: AssignJobResult, newModel: AssignModel, user: IUser): void
    {
        const result = new AssignModalResult();

        //this.httpResponse = JSON.parse(JSON.stringify(res));

        if (res.success)
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
            this.toasterService.pop('success', res.message, '');
        }
        else {
            this.toasterService.pop('error', res.message, '');
        }
        this.hide();
        result.source = newModel.objectSource;

        this.onAssigned.emit(result);
        this.buildUsersSource();
    }

    private filterItem(name: string): void
    {
        if (_.isEmpty(name))
        {
            this.filtredUsers = this.users;
        }

        this.filtredUsers = _.filter(this.users, (current: IUser) =>
            {
                return _.includes(current.name.toLowerCase(), name.toLowerCase());
            });
    }
}