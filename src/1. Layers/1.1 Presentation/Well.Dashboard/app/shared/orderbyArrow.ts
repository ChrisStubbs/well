import {Component, OnInit, EventEmitter, Output}    from '@angular/core';

@Component({
    selector: 'ow-orderbyarrow',
    templateUrl: 'app/shared/orderby-arrow.html'
})
export class OrderArrowComponent implements OnInit {
    public isDesc: boolean = true;
    @Output() public onSortDirectionChanged = new EventEmitter<boolean>();

    public ngOnInit(): void {
        this.isDesc = true;
    }

    public setImageSrc() {
        this.isDesc = !this.isDesc;
        this.onSortDirectionChanged.emit(this.isDesc);
    }
}