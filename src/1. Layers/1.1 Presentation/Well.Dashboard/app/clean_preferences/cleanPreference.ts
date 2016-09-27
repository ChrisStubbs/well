import {Branch} from '../shared/branch/branch';

export class CleanPreference {
    id: number;
    days: number;
    branchName: string;
    branches: Branch[];
}