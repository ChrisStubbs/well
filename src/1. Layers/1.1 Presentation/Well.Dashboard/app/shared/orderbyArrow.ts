import {Component, OnInit, EventEmitter, Output}    from '@angular/core';
import { NavigateQueryParametersService }           from './NavigateQueryParametersService';
@Component({
    selector: 'ow-orderbyarrow',
    templateUrl: 'app/shared/orderby-arrow.html'
})
export class OrderArrowComponent implements OnInit {
    public isDesc: boolean;

    @Output() public onSortDirectionChanged = new EventEmitter<boolean>();

    public ngOnInit(): void {
        this.isDesc = NavigateQueryParametersService.GetCurrentSorting() == 'desc';
    }

    public setImageSrc() {
        this.isDesc = !this.isDesc;
        this.onSortDirectionChanged.emit(this.isDesc);
    }
}