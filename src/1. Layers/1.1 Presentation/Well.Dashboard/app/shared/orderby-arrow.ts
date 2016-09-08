import {Component, OnInit, EventEmitter, Output} from '@angular/core';


@Component({
    selector: "ow-orderbyarrow",
    templateUrl: 'app/shared/orderby-arrow.html'
})
export class OrderArrowComponent implements OnInit {
    imgAsc: string;
    imgDsc: string;
    imgSrc: string;
    isDesc: boolean;
    @Output() onSortDirectionChanged = new EventEmitter<boolean>();

    ngOnInit(): void {
        this.imgAsc = './Content/Images/triangle_asc.png';
        this.imgDsc = './Content/Images/triangle_desc.png';
        this.imgSrc = this.imgDsc;
        this.isDesc = true;
    }

    setImageSrc() {
        this.isDesc = !this.isDesc;
        this.imgSrc = this.isDesc ? this.imgDsc : this.imgAsc;
        this.onSortDirectionChanged.emit(this.isDesc);

    }

}