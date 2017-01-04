import { ElementRef, AfterViewInit, AfterViewChecked, OnDestroy, EventEmitter, Renderer } from '@angular/core';
import { DomHandler } from '../dom/domhandler';
export declare class Dialog implements AfterViewInit, AfterViewChecked, OnDestroy {
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
    contentHeight: any;
    modal: boolean;
    closeOnEscape: boolean;
    rtl: boolean;
    closable: boolean;
    responsive: boolean;
    appendTo: any;
    style: any;
    styleClass: string;
    headerFacet: any;
    containerViewChild: ElementRef;
    contentViewChild: ElementRef;
    onBeforeShow: EventEmitter<any>;
    onAfterShow: EventEmitter<any>;
    onBeforeHide: EventEmitter<any>;
    onAfterHide: EventEmitter<any>;
    visibleChange: EventEmitter<any>;
    _visible: boolean;
    dragging: boolean;
    documentDragListener: Function;
    resizing: boolean;
    documentResizeListener: Function;
    documentResizeEndListener: Function;
    documentResponsiveListener: Function;
    documentEscapeListener: Function;
    lastPageX: number;
    lastPageY: number;
    mask: HTMLDivElement;
    shown: boolean;
    container: HTMLDivElement;
    contentContainer: HTMLDivElement;
    positionInitialized: boolean;
    constructor(el: ElementRef, domHandler: DomHandler, renderer: Renderer);
    visible: boolean;
    show(): void;
    ngAfterViewInit(): void;
    ngAfterViewChecked(): void;
    center(): void;
    enableModality(): void;
    disableModality(): void;
    hide(event: any): void;
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
