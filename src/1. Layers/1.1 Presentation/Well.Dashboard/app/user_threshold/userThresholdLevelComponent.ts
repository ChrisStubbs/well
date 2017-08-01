import { Component, OnInit } from '@angular/core';
import { Response } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { ToasterService } from 'angular2-toaster/angular2-toaster';
import { UserPreferenceService } from '../user_preferences/userPreferenceService';
import { CreditThresholdService } from '../credit_threshold/creditThresholdService';
import { HttpResponse } from '../shared/httpResponse';
import { User } from '../user_preferences/User';
import { GlobalSettingsService } from '../shared/globalSettings';
import { Observable } from 'rxjs/Rx';
import { CreditThreshold } from '../credit_threshold/creditThreshold';
import { IObservableAlive } from '../shared/IObservableAlive';
import * as _ from 'lodash';

@Component({
    selector: 'ow-user-threshold-level',
    templateUrl: './app/user_threshold/user-threshold-level.html'
}
)
export class UserThresholdLevelComponent implements IObservableAlive {
    public isAlive: boolean = true;
    public username: string;
    public httpResponse: HttpResponse = new HttpResponse();
    public user: User;
    private creditThresholds: Array<CreditThreshold> = [];
    private currentThreshold: CreditThreshold = new CreditThreshold();

    constructor(
        private route: ActivatedRoute,
        private creditThresholdService: CreditThresholdService,
        private userPreferenceService: UserPreferenceService,
        private toasterService: ToasterService,
        private globalServiceComponent: GlobalSettingsService) { }

    public ngOnInit(): void
    {
        this.route.params
            .flatMap(params => {
                this.username = params['name'];

                return Observable.forkJoin(
                    this.userPreferenceService.getUser(this.username),
                    this.creditThresholdService.getCreditThresholds()
                );
            })
            .takeWhile(() => this.isAlive)
            .subscribe(result => {
                this.user = result[0] as User;
                this.creditThresholds = result[1] as Array<CreditThreshold>;
                
                this.currentThreshold = _.find(this.creditThresholds,
                    (creditThreshold: CreditThreshold) => creditThreshold.id == this.user.creditThresholdId);
            });
    }

    public ngOnDestroy(): void {
        this.isAlive = false;
    }

    public setSelectedLevel(threshold: CreditThreshold) {
        this.currentThreshold = threshold;
    }

    public save() {
        this.creditThresholdService
            .saveThresholdLevel(this.currentThreshold.id, this.user.identityName).subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Threshold level has been saved.', '');
                    // Force reloading settings. there should be better way to do this
                    this.globalServiceComponent.getSettings();
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Threshold level could not be saved at this time.',
                        'Please try again later!');
                }
                if (this.httpResponse.notAcceptable) {
                    this.toasterService.pop('warning', this.httpResponse.message, '');
                }
            });
    }
}