import { Component, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { ToasterService } from 'angular2-toaster/angular2-toaster';
import { UserPreferenceService } from '../user_preferences/userPreferenceService';
import { CreditThresholdService } from '../credit_threshold/creditThresholdService';
import { HttpResponse } from '../shared/httpResponse';
import {User} from '../user_preferences/User';

@Component({
    selector: 'ow-user-threshold-level',
    templateUrl: './app/user_threshold/user-threshold-level.html'
}
)
export class UserThresholdLevelComponent implements OnInit
{
    public username: string;
    public thresholdLevel: string = 'Level';
    public httpResponse: HttpResponse = new HttpResponse();
    public user: User;

    constructor(
        private route: ActivatedRoute,
        private creditThresholdService: CreditThresholdService,
        private userPreferenceService: UserPreferenceService,
        private toasterService: ToasterService) { }

    public ngOnInit(): void
    {
        this.route.params.subscribe(params =>
        {
            this.username = params['name'];
            this.userPreferenceService.getUser(this.username).subscribe(
                u =>
                {
                    this.user = u;
                    console.log(this.user);
                    if (this.user.thresholdLevelId != 0) {
                        this.thresholdLevel = 'Level ' + this.user.thresholdLevelId;
                    }
                });
        }); 
    }

    public setSelectedLevel(level)
    {
        this.thresholdLevel = level;
    }

    public save()
    {
        this.creditThresholdService
            .saveThresholdLevel(this.thresholdLevel, this.username).subscribe((res: Response) =>
            {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success)
                {
                    this.toasterService.pop('success', 'Threshold level has been saved.', '');
                }
                if (this.httpResponse.failure)
                {
                    this.toasterService.pop(
                        'error',
                        'Threshold level could not be saved at this time.',
                        'Please try again later!');
                }
                if (this.httpResponse.notAcceptable)
                {
                    this.toasterService.pop('warning', this.httpResponse.message, '');
                }
            });
    }
}