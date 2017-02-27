import {Component, OnInit, Input} from '@angular/core';

@Component({
    selector: 'ow-account-flags',
    templateUrl: 'app/shared/accountFlagsComponent.html'
})
export class AccountFlagsComponent {

    @Input() public isCashOnDelivery: boolean;
    @Input() public isProofOfDelivery: boolean; 
}