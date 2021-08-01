//app.component.ts
import {Component, ViewChild, OnInit, AfterViewInit, Inject, AfterContentInit} from '@angular/core';
import { MatDialog, MatTable } from '@angular/material';
import { DialogBoxComponent } from './dialog-box/dialog-box.component';
import { HttpClient } from '@angular/common/http';
import {MatSort, Sort} from "@angular/material/sort";
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent{
  displayedColumns: string[] = ['name', 'description', 'location', 'rating', 'action'];
  dataSource: any;
  dataSourceForSorting: any;
  filterSelectObj = [];
  filterSelectObjForName = [];
  filterValues = {};
  public hotel: Hotel;
  @ViewChild(MatTable,{static:true}) table: MatTable<any>;
  @ViewChild(MatSort,{static:true}) sort: MatSort;

  constructor(public dialog: MatDialog, public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {

    // Object to create Filter for
    this.filterSelectObj = [
   {
        name: 'Rating',
        columnProp: 'rating',
        options: []
      }
    ];

    this.filterSelectObjForName = [
      {
        name: 'Hotel Name',
        columnProp: 'name',
        options: []
      }
    ];
    this.fetchInitialData();
  }

  applyFilter(filterValue: string) { // filter, event
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  fetchInitialData(){
    this.http.get<Hotel[]>(this.baseUrl + 'api/Hotel').subscribe(result => {
      //this.dataSource = result;
      this.dataSource = new MatTableDataSource(result);

      //create filter for hotelname and rating
      this.dataSource.filterPredicate = this.createFilter();

      //sorting
      this.dataSource.sort = this.sort;
      const sortState: Sort = {active: 'rating', direction: 'desc'};
      this.sort.active = sortState.active;
      this.sort.direction = sortState.direction;
      this.sort.sortChange.emit(sortState);

      this.filterSelectObj.filter((o) => {
        o.options = this.getFilterObject(result, o.columnProp);
      });
    }, error => console.error(error));
  }

  // Called on Filter change
  filterChange(filter, event) {
    //let filterValues = {}
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

  refreshFilters(){
    this.filterSelectObj.filter((o) => {
      o.options = this.getFilterObject(this.dataSource.data, o.columnProp);
    });
  }

  // Reset table filters
  resetFilters() {
    this.filterValues = {}
    this.filterSelectObj.forEach((value, key) => {
      value.modelValue = undefined;
    })
    this.filterSelectObjForName.forEach((value, key) => {
      value.modelValue = undefined;
    })
    this.dataSource.filter = "";
  }

  // Get Uniqu values from columns to build filter
  getFilterObject(fullObj, key) {
    const uniqChk = [];
    fullObj.filter((obj) => {
      if (!uniqChk.includes(obj[key])) {
        uniqChk.push(obj[key]);
      }
      return obj;
    });
    return uniqChk;
  }

  openDialog(action,obj) {
    obj.action = action;
    const dialogRef = this.dialog.open(DialogBoxComponent, {
      width: '300px',
      data:obj
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result.event == 'Add'){
        this.addRowData(result.data);
      }else if(result.event == 'Update'){
        this.updateRowData(result.data);
      }else if(result.event == 'Delete'){
        this.deleteRowData(result.data);
      }
    });
  }

  addRowData(newRow){
    this.dataSource.data.push({
      name:newRow.name,
      rating:newRow.rating,
      description:newRow.description,
      location:newRow.location
    });

    this.dataSource.data = this.dataSource.data.slice();

    this.hotel = {
       id:0,
       name:newRow.name,
       rating:parseInt(newRow.rating),
       description:newRow.description,
       location:newRow.location
    };
    const headers = { 'content-type': 'application/json'}
    const body=JSON.stringify(this.hotel);
    this.http.post<Hotel>(this.baseUrl + 'api/Hotel', body , {'headers':headers}).subscribe(data => {
    })

    this.table.renderRows();
  }
  updateRowData(newRow){
    this.dataSource.data = this.dataSource.data.filter((value,key)=>{
      if(value.id == newRow.id){
          value.name = newRow.name;
          value.rating = newRow.rating;
          value.description= newRow.description;
          value.location = newRow.location;
      }

      this.hotel = {
        id: newRow.id,
        name:newRow.name,
        rating: parseInt(newRow.rating),
        description:newRow.description,
        location:newRow.location
      };
      const headers = { 'content-type': 'application/json'}
      const body=JSON.stringify(this.hotel);
      this.http.put<Hotel[]>(this.baseUrl + 'api/Hotel/' + newRow.id, body , {'headers':headers}).subscribe(data => {
      })

      this.refreshFilters();
      return true;
    });
  }
  deleteRowData(newRow){
    this.dataSource.data = this.dataSource.data.filter((value,key)=>{
       this.http.delete<any>(this.baseUrl + 'api/Hotel/' + newRow.id).subscribe(data => {
       })
      return value.id != newRow.id;
    });
  }
}

interface Hotel {
  id: number;
  name: string;
  rating: number;
  description: string;
  location: string;
}
