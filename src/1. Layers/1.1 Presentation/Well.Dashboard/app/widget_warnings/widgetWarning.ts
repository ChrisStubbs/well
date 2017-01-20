import {Branch} from '../shared/branch/branch';

export class WidgetWarning {
    public id: number;
    public widgetName: string;
    public warningLevel: number;
    public branches: Branch[];
    public type: string;
} 