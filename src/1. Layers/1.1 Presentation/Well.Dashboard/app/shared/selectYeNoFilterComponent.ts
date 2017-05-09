import {Component, EventEmitter, Output, Input}  from '@angular/core';
import 'rxjs/add/operator/takeWhile';

@Component({
    selector: 'ow-selectYesNoFilter',
    template: '' +
    '<select class="form-control" [(ngModel)]="selectedValue" (change)="itemClick($event.target.value)">' +
    '<option *ngFor="let item of values" [value]="item[0]">' +
    '{{item[1]}}' +
    '</option>' +
    '</select>'
})
export class SelectYeNoFilterComponent {
    @Output() public filterClicked: EventEmitter<string> = new EventEmitter<string>();
    @Input() public selectedValue: string;

    public values: Array<Array<string>>;

    constructor()
    {
        let item;

        this.values = [[]];

        this.values[0][0] = '';
        this.values[0][1] = 'All';

        item = [];
        item.push(true);
        item.push('Yes');
        this.values.push(item);

        item = [];
        item.push(false);
        item.push('No');
        this.values.push(item);
    }

    public itemClick(value: any)
    {
        this.filterClicked.emit(value);
        this.selectedValue = value;
    }
}