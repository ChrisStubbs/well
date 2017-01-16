import {Component, OnInit, Input} from '@angular/core';

@Component({
    selector: 'ow-cod',
    templateUrl: 'app/shared/cashOnDelivery.html'
})
export class CodComponent implements OnInit {

    @Input() public isCashOnDelivery: string;
    public isCod: boolean;

    public ngOnInit(): void {
        this.isCod = this.isCashOnDelivery === 'True';
    }

}