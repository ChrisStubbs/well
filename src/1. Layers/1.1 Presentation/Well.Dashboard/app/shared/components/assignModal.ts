import { Component, EventEmitter, Output, Input }   from '@angular/core';
import { Response }                                 from '@angular/http';
import { IUser, HttpResponse, UserService }         from '../shared';
import { UserJobs }                                 from '../userJobs';
import * as _                                       from 'lodash';
import { IObservableAlive }                         from '../IObservableAlive';
import { AssignModalResult }                        from './assignModel';
import { Router }                                   from '@angular/router';
import { ToasterService }                           from 'angular2-toaster';
import { AssignModel }                              from './assignModel';

@Component({
    selector: 'assign-modal',
    templateUrl: 'app/shared/components/accountFlagsComponent.html'
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
                this.allUsers = users;
                this.buildUsersSource();
            });
    }

    private buildUsersSource(): void
    {
        this.users = _.filter(this.allUsers,
            (current: IUser) =>
            {
                return _.isNil(this.model.assigned) || current.name != this.model.assigned;
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

    public userSelected(user: IUser, newModel: AssignModel): void
    {
        this.userJobs.jobIds = newModel.jobIds;
        this.userJobs.userId = user.id;
        newModel.assigned = user.name;
        this.userService.assign(this.userJobs)
            .takeWhile(() => this.isAlive)
            .subscribe((res: Response) => this.handleResponse(res, newModel, user));
    }

    public unassign(newModel: AssignModel): void
    {
        newModel.assigned = undefined;
        this.userService.unassign(newModel.jobIds)
            .takeWhile(() => this.isAlive)
            .subscribe((res: Response) => this.handleResponse(res, newModel, undefined));
    }

    private handleResponse(res: Response, newModel: AssignModel, user: IUser): void
    {
        const result = new AssignModalResult();

        this.httpResponse = JSON.parse(JSON.stringify(res));

        if (this.httpResponse.success)
        {
            this.toasterService.pop('success', 'Jobs have been assigned', '');
            result.assigned = true;
        }
        if (this.httpResponse.failure)
        {
            this.toasterService.pop('error', 'Jobs unassigned', '');
        }
        this.hide();
        result.source = newModel.objectSource;
        result.newUser = user;
        this.model = newModel;

        this.onAssigned.emit(result);
        this.buildUsersSource();
    }
}