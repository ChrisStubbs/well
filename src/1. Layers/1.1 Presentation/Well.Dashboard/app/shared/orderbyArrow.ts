import {Component, OnInit, EventEmitter, Output} from '@angular/core';

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
        this.onSortDirectionChanged.emit(this.isDesc);

    }

}