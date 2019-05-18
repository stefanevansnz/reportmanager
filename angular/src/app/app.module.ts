import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpModule } from '@angular/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule } from '@angular/forms';

import { ReportsComponent } from './reports/reports.component';
import { DataStorageService } from './shared/data-storage.service';
import { HtmlProxyService } from './shared/html-proxy.service';

import { AuthenticationService } from './shared/authentication.service';
import { AuthenticationGuardService } from './shared/authentication-guard.service';
import { HeadingComponent } from './heading/heading.component';
import { FooterComponent } from './footer/footer.component';
import { SigninComponent } from './signin/signin.component';
import { SignupComponent } from './signup/signup.component';
import { NotificationService } from './shared/notification.service';
import { SignoutComponent } from './signout/signout.component';
import { PageNotFoundComponent } from './pagenotfound/pagenotfound.component';
import { SplashComponent } from './splash/splash.component';
import { RecoverComponent } from './recover/recover.component';
import { ForgotComponent } from './forgot/forgot.component';
import { ReportComponent } from './report/report.component';
import { SanitizeHtmlPipe } from './report/sanitizehtml.pipe';
@NgModule({
  declarations: [
    AppComponent,
    ReportsComponent,
    HeadingComponent,
    FooterComponent,
    SigninComponent,
    SignupComponent,
    SignoutComponent,
    PageNotFoundComponent,
    SplashComponent,
    RecoverComponent,
    ForgotComponent,
    ReportComponent,
    SanitizeHtmlPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpModule,
    FormsModule, 
    ModalModule     
  ],
  providers: [DataStorageService, HtmlProxyService, AuthenticationService, NotificationService, AuthenticationGuardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
