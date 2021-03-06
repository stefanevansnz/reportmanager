import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthenticationService } from '../shared/authentication.service';
import { NotificationService } from '../shared/notification.service';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-recover',
  templateUrl: './recover.component.html',
  styleUrls: ['./recover.component.css']
})
export class RecoverComponent implements OnInit {

  @ViewChild('f') slForm: NgForm;
  messageUpdate: Subscription;
  message: any;
  loading: boolean = false;
  signUpEmail: string;  

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

  onRecover(form: NgForm) {
    this.loading = true;
    let email = form.value.email;
    if (email == undefined) {
      email = email.toLowerCase();
    }  
    this.signUpEmail = email;  
    this.authenticationService.resendConfirmationCode(email, this);
  } 

  successfulResendConfirmationCode() {
    console.log('successfulResendConfirmationCode');
    this.router.navigateByUrl('/signup/confirm'); 
  }

  successfulSignUp() {
    console.log('successfulSignUp');
    this.router.navigateByUrl('/signup/confirm'); 
  }


  isLoading() {
    return this.loading;
  }  


}
