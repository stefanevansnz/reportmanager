import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthenticationService } from '../shared/authentication.service';
import { NotificationService } from '../shared/notification.service';
import { Router, ActivatedRoute, Params } from '@angular/router';

declare var jQuery:any;

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {

  @ViewChild('f') slForm: NgForm;
  messageUpdate: Subscription;
  message: any;
  loading: boolean = false;
  signUpEmail: string;  

  email: string;

  constructor(private authenticationService: AuthenticationService,
              private notificationService: NotificationService,
              private router: Router,
              private route: ActivatedRoute) { }

  ngOnInit() {    
    this.messageUpdate = this.notificationService.getMessage()
    .subscribe(
      (message) => {
        this.message = message;
      }
    );

    let self = this;
  }

  ngAfterViewInit() {
    console.log('on after view init');
    setTimeout(_=> this.updateForm());
  } 

  updateForm() {
    if (this.email != undefined) {

      this.slForm.controls['email'].setValue(this.email);

    }
  }


  onSignin(form: NgForm) {
    console.log('sign in');
    this.loading = true;
    let email = form.value.email;
    if (email != undefined) {
      email = email.toLowerCase();
    }      
    this.signUpEmail = email;
    let password = form.value.password;    
    this.authenticationService.signinUser(email, password, null, null, null, null, this);
  } 

  successfulLogin() {
    console.log('successfulLogin');
    this.router.navigateByUrl('/reports'); 
  }


  isLoading() {
    return this.loading;
  }


}
