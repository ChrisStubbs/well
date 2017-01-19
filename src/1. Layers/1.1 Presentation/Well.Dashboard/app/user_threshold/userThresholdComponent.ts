import {Component} from '@angular/core';
import {UserPreferenceComponent} from '../user_preferences/userPreferenceComponent';

@Component({
        selector: 'ow-user-threshold',
        templateUrl: './app/user_threshold/user-threshold.html'
    }
)
export class UserThresholdComponent {
    public header: string = 'User Threshold Levels';
}