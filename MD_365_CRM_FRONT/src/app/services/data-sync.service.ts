import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DataSyncService {

  dataHolder: any;

  constructor() {
    console.log(this.dataHolder)
  }
}
