export enum UpliftAction {
    UpliftCredit = 0,
    CreditNoUplift = 1,
    UpliftNoCredit = 2,
    NoUpliftNoCredit = 3
}

export class UpliftActionHelpers {
    public static getUpliftActionCode(jobType: number, upliftAction: UpliftAction): string {
        if (jobType == 10) {
            switch (upliftAction) {
                case UpliftAction.UpliftCredit:
                    return 'U/C';
                case UpliftAction.CreditNoUplift:
                    return 'NU/C';
                case UpliftAction.UpliftNoCredit:
                    return 'U/NC';
                case UpliftAction.NoUpliftNoCredit:
                    return 'NU/NC';
                default:
                    throw 'Invalid uplift action ' + upliftAction;
            }
        }

        return undefined;
    }
}