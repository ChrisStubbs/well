import {Component, OnInit, Input} from '@angular/core';

@Component({
    selector: 'ow-account-flags',
    templateUrl: 'app/shared/accountFlagsComponent.html'
})
export class AccountFlagsComponent implements OnInit {

    @Input() public isCashOnDelivery: string;
    @Input() public isProofOfDelivery: boolean; 
    public isCod: boolean;
    public isPod: boolean;

    public ngOnInit(): void {
        this.isCod = this.isCashOnDelivery === 'True';
        this.isPod = this.isProofOfDelivery;
    }

}