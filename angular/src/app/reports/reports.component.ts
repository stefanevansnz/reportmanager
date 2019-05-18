import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Report } from './reports.model';

import { DataStorageService } from '../shared/data-storage.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { AuthenticationService } from '../shared/authentication.service';

declare var jQuery:any;

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {

  @ViewChild('f') slForm: NgForm;

  message;
  reportsLoading = true;
  sharesLoading = true;
  editMode = false;
  sureOfDelete = false;
  reports: Report[] = [];
  editReport: Report;

  private editIndex: number;

  constructor(private authenticationService: AuthenticationService,
              private dataStorageService: DataStorageService, 
              private router: Router, 
              private route: ActivatedRoute) {}

  ngOnInit() {
    let self = this;
    this.route.params
      .subscribe(
        (params: Params) => {
          // something has changed
          let id = params['id'];
          console.log('id = ' + id);          
          this.dataStorageService.getObjectArrayFromServer('reports', id, self, self.successfullLoad);
        }
      );    
  }

  successfullLoad(self) {
    jQuery("#editModal").on('hidden.bs.modal', function (e) {
      console.log('hidding');
      self.slForm.reset();
      self.errorMessage = '';
      self.successMessage = '';
      self.editMode = false;
  
    })
    
  }

  onViewReports(index: number, report: Report) {
    console.log('onViewReports report id ' + report.id);
    this.router.navigate(['/report/' + report.id]);
  }

  onAddObject() {
    this.editMode = false;
    this.slForm.reset();
    this.sureOfDelete = false;  
    this.message = '';      
    jQuery("#editModal").modal("show");
  }  
  
  closeSetModal() { 
    console.log('closeSetModal');
    jQuery("#editModal").modal("hide");
    this.slForm.reset();    
    this.sureOfDelete = false;    
  }

  closeDeleteModal() { 
    console.log('closeDeleteModal');
    jQuery("#editModal").modal("hide");
    this.slForm.reset();    
    this.sureOfDelete = false;    
  }


  onEditObject(index: number, report: Report) {
    //console.log('onEditObject ' + index);
    this.slForm.resetForm();   

    this.editReport = report;
    console.log('this.editReport.id is ' + this.editReport.id);
    this.editIndex = index;
    this.editMode = true;
    this.sureOfDelete = false;    

    this.message = '';

    /*
    if (report.title != undefined) {
      this.slForm.controls['title'].setValue(report.title);
    }
    */

    this.slForm.setValue({
      title: report.title
    });    
  
    jQuery("#editModal").modal("show");
  }  

  onDeleteSure() {
    console.log('onDeleteSure');
    this.sureOfDelete = true;
  }


  onSubmit(form: NgForm) {
    const value = form.value;
    console.log('sureOfDelete is ' + this.sureOfDelete);
    let self = this; 

    if (this.sureOfDelete) {
      console.log('form submitted suredelete is ' + value.suredelete);
      if (value.suredelete == 'DELETE') {
        this.onDelete();
      } else {
        this.message = 'Type DELETE in the field above.';
      }
    } else {
      console.log('form submitted title is ' + value.title);

      let report = new Report(null, null, value.title, null );
  
      if (this.editMode) {
        console.log('edit mode');
        report = new Report( this.editReport.id, null, value.title, null );
  
      }
      this.dataStorageService.setObjectOnServer('reports', 'editReport', report, self, null);            
    }
  
  }


  onDelete() {
    let self = this;
    var user = this.authenticationService.getUser();
    var userid = user.id;
    
    let report = new Report(this.editReport.id, userid, '', '');    
    console.log('report is ' + JSON.stringify(report));
    this.dataStorageService.deleteObjectsOnServer('reports', report, self);              
  }

}
