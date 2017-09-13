import { Injectable }                   from '@angular/core';
import { Response, URLSearchParams }    from '@angular/http';
import { GlobalSettingsService }        from '../globalSettings';
<<<<<<< HEAD
import { HttpErrorService }             from '../httpErrorService';
import { HttpService }                  from '../httpService';
import { Observable }                   from 'rxjs';
import * as _                           from 'lodash';
import { SessionStorageService }        from 'ngx-webstorage'
=======
import { HttpErrorService }             from './httpErrorService';
import { HttpService }                  from './httpService';
import { Observable }                   from 'rxjs';
import * as _                           from 'lodash';
import { SessionStorageService }        from 'ngx-webstorage';
>>>>>>> 9894b25d6c6a6ba235690ef65e4f413d2250d1a7
import {LookupsEnum}                    from './lookupsEnum';
import {ILookupValue}                   from './ILookupValue';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';

@Injectable()
export class LookupService
{
    private key: string = 'lookup_';

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private storageService: SessionStorageService) { }

    public get(lookupName: LookupsEnum): Observable<Array<ILookupValue>>
    {
        const stringLookupName = LookupsEnum[lookupName];
        const lookupKey = this.key + stringLookupName;
        const storedValue = this.storageService.retrieve(lookupKey);

        if (!_.isNil(storedValue))
        {
            return Observable.of([]).map(value => {
                return storedValue;
            });
        }

        const url = this.globalSettingsService.globalSettings.apiUrl + 'Lookup/' + encodeURIComponent(stringLookupName);

        return this.http.get(url)
            .map((response: Response) =>
            {
                const value = response.json();
<<<<<<< HEAD
                let objectToSave = [];
=======
                const objectToSave = [];
>>>>>>> 9894b25d6c6a6ba235690ef65e4f413d2250d1a7

                _.each(_.keys(value), (current: string) => {
                    let newObj: ILookupValue;

                    newObj =
                        {
                            key: current, value: value[current]
                        };
                    objectToSave.push(newObj);
                });

<<<<<<< HEAD
                objectToSave = _.sortBy(objectToSave, 'value');
=======
                //objectToSave = _.sortBy(objectToSave, 'value');
>>>>>>> 9894b25d6c6a6ba235690ef65e4f413d2250d1a7
                this.storageService.store(lookupKey, objectToSave);

                return objectToSave;
            })
            .catch(e => this.httpErrorService.handleError(e));
    }
}
