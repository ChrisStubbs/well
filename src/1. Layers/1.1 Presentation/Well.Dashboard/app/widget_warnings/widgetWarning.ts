import {Branch} from '../shared/branch/branch';

export class WidgetWarning {
    id: number;
    widgetName: string;
    warningLevel: number;
    branches: Branch[];
    type: string;
} 