import {Component, OnInit, Input} from '@angular/core';

@Component({
    selector: "ow-cod",
    templateUrl: 'app/shared/cashOnDelivery.html'
})
export class CodComponent implements OnInit {

    @Input() isCashOnDelivery: string;
    isCod:boolean;

    ngOnInit(): void {
        this.isCod = this.isCashOnDelivery === 'True';
    }

}