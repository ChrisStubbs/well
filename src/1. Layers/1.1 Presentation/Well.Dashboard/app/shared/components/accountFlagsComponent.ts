import {Component, Input} from '@angular/core';

@Component({
    selector: 'ow-account-flags',
    templateUrl: 'app/shared/components/accountFlagsComponent.html'
})
export class AccountFlagsComponent {

    @Input() public isCashOnDelivery: boolean;
    @Input() public isProofOfDelivery: boolean; 
}