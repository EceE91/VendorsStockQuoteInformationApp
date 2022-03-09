//app.component.ts
import {Component, ViewChild, OnInit, AfterViewInit, Inject, AfterContentInit, Injectable} from '@angular/core';
import { MatDialog, MatTable } from '@angular/material';
import { DialogBoxComponent } from './dialog-box/dialog-box.component';
import { HttpClient, HttpParams } from '@angular/common/http';
import {MatSort, Sort} from "@angular/material/sort";
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent{
  displayedColumns: string[] = ['name', 'symbol', 'exchange', 'action'];
  dataSource: any;
  dataSourceForSorting: any;
  filterSelectObjForName = [];
  filterValues = {};
  public vendor: Vendor;
  @ViewChild(MatTable,{static:true}) table: MatTable<any>;
  @ViewChild(MatSort,{static:true}) sort: MatSort;

  constructor(public dialog: MatDialog, public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {
    // Object to create Filter for name and symbol
    this.filterSelectObjForName = [
      {
        name: 'by name',
        columnProp: 'name',
        options: []
      },
      {
         name: 'by symbol',
         columnProp: 'symbol',
         options: []
      }
    ];
    this.fetchInitialData();
  }

  // GET all vendors
  fetchInitialData(){
    this.http.get<Vendor[]>(this.baseUrl + 'api/Vendor').subscribe(result => {
      this.dataSource = new MatTableDataSource(result);

      //create filter for name and symbol
      this.dataSource.filterPredicate = this.createFilter();
    }, error => console.error(error));
  }

  // Called on Filter change
  filterChange(filter, event) {
    this.filterValues[filter.columnProp] = event.target.value.trim().toLowerCase()
    this.dataSource.filter = JSON.stringify(this.filterValues)
  }

  // Custom filter method fot Angular Material Datatable
  createFilter() {
    let filterFunction = function (data: any, filter: string): boolean {
      let searchTerms = JSON.parse(filter);
      let isFilterSet = false;
      for (const col in searchTerms) {
        if (searchTerms[col].toString() !== '') {
          isFilterSet = true;
        } else {
          delete searchTerms[col];
        }
      }

      console.log(searchTerms);

      let nameSearch = () => {
        let found = false;
        if (isFilterSet) {
          for (const col in searchTerms) {
            searchTerms[col].trim().toLowerCase().split(' ').forEach(word => {
              if (data[col].toString().toLowerCase().indexOf(word) != -1 && isFilterSet) {
                found = true
              }
            });
          }
          return found
        } else {
          return true;
        }
      }
      return nameSearch()
    }
    return filterFunction
  }

  // Reset table filters
  resetFilters() {
    this.filterValues = {}

    this.filterSelectObjForName.forEach((value, key) => {
      value.modelValue = undefined;
    })
    this.dataSource.filter = "";
  }

  // GET StockQuote information by symbol
  openDialog(action,obj) {
   obj.action = action;
   this.http.get<StockQuote[]>(this.baseUrl + 'api/StockQuote/' + obj.symbol).subscribe(result => {
       const dialogRef = this.dialog.open(DialogBoxComponent, {
             width: '500px',
             data:result
           });
   }, error => console.error(error));
  }
}

interface Vendor {
  name: string;
  symbol: string;
  exchange: string;
}

interface StockQuote {
  name: string;
  symbol: string;
  lastprice: number;
  change: number;
  changepercent: number;
  timestamp: string;
  msdate: number;
  marketcap: number;
  volume: number;
  changeytd: number;
  changepercentytd: number;
  high: number;
  low: number;
  open: number;
}
