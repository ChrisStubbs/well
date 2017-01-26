import {Component, OnInit, EventEmitter, Output}    from '@angular/core';
import { NavigateQueryParametersService }           from './NavigateQueryParametersService';
import { NavigateQueryParameters }                  from './NavigateQueryParameters';

@Component({
    selector: 'ow-orderbyarrow',
    templateUrl: 'app/shared/orderby-arrow.html'
})
export class OrderArrowComponent implements OnInit {
    public imgAsc: string;
    public imgDsc: string;
    public imgSrc: string;
    public isDesc: boolean;
    @Output() public onSortDirectionChanged = new EventEmitter<boolean>();

    public ngOnInit(): void {
        this.imgAsc = './Content/Images/triangle_asc.png';
        this.imgDsc = './Content/Images/triangle_desc.png';
        this.imgSrc = this.imgDsc;
        this.isDesc = true;
    }

    public setImageSrc() {
        this.isDesc = !this.isDesc;
        this.imgSrc = this.isDesc ? this.imgDsc : this.imgAsc;

        const item = new NavigateQueryParameters(undefined, 1, this.isDesc ? 'desc' : 'asc');
        NavigateQueryParametersService.SaveSort(item);

        this.onSortDirectionChanged.emit(this.isDesc);
    }
}