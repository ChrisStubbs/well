import { ElementRef, AfterViewInit, OnDestroy, EventEmitter, Renderer } from '@angular/core';
import { DomHandler } from '../dom/domhandler';
export declare class Dialog implements AfterViewInit, OnDestroy {
    el: ElementRef;
    domHandler: DomHandler;
    renderer: Renderer;
    header: string;
    draggable: boolean;
    resizable: boolean;
    minWidth: number;
    minHeight: number;
    width: any;
    height: any;
    contentStyle: any;
    modal: boolean;
    closeOnEscape: boolean;
    dismissableMask: boolean;
    rtl: boolean;
    closable: boolean;
    responsive: boolean;
    appendTo: any;
    style: any;
    styleClass: string;
    showHeader: boolean;
    headerFacet: any;
    containerViewChild: ElementRef;
    contentViewChild: ElementRef;
    onShow: EventEmitter<any>;
    onHide: EventEmitter<any>;
    visibleChange: EventEmitter<any>;
    _visible: boolean;
    dragging: boolean;
    documentDragListener: Function;
    resizing: boolean;
    documentResizeListener: Function;
    documentResizeEndListener: Function;
    documentResponsiveListener: Function;
    documentEscapeListener: Function;
    maskClickListener: Function;
    lastPageX: number;
    lastPageY: number;
    mask: HTMLDivElement;
    container: HTMLDivElement;
    contentContainer: HTMLDivElement;
    constructor(el: ElementRef, domHandler: DomHandler, renderer: Renderer);
    visible: boolean;
    show(): void;
    hide(): void;
    close(event: Event): void;
    ngAfterViewInit(): void;
    center(): void;
    enableModality(): void;
    disableModality(): void;
    unbindMaskClickListener(): void;
    moveOnTop(): void;
    initDrag(event: any): void;
    onDrag(event: any): void;
    endDrag(event: any): void;
    initResize(event: any): void;
    onResize(event: any): void;
    ngOnDestroy(): void;
}
export declare class DialogModule {
}
