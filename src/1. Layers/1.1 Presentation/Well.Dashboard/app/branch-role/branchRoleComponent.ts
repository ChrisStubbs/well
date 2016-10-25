import {Component, OnInit, ViewChild}  from '@angular/core';
import {TabsModule} from 'ng2-tabs';
import {SecurityService} from '../shared/security/securityService';
import {GlobalSettingsService} from '../shared/globalSettings';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {SeasonalDatesViewComponent} from '../seasonal_dates/seasonalDatesViewComponent';
import {CreditThresholdViewComponent} from '../credit_threshold/creditThresholdViewComponent';
import {CleanPreferenceComponent} from '../clean_preferences/cleanPreferenceComponent';
import {WidgetWarningsViewComponent} from '../widget_warnings/widgetWarningsViewComponent';

@Component({
    selector: 'ow-branch-role',
    templateUrl: './app/branch-role/branch-role.html'
})
export class BranchRoleComponent implements OnInit {

    roles: string[] = [ 'Customer Service User', 'Customer Service Manager', 'Branch Manager' ];

    constructor(private securityService: SecurityService, private globalSettingsService: GlobalSettingsService) { }

    ngOnInit(): void {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.branchSelection);
    }

    save(): void {
        console.log('save this');
    }
}
