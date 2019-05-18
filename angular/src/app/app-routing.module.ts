import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReportsComponent } from "./reports/reports.component";
import { ReportComponent } from "./report/report.component";
import { AuthenticationGuardService as AuthenticationGuard } from "./shared/authentication-guard.service";
import { SplashComponent } from "./splash/splash.component";
import { SignupComponent } from "./signup/signup.component";
import { SigninComponent } from "./signin/signin.component";
import { RecoverComponent } from "./recover/recover.component";
import { ForgotComponent } from "./forgot/forgot.component";
import { SignoutComponent } from "./signout/signout.component";
import { PageNotFoundComponent } from "./pagenotfound/pagenotfound.component";

const appRoutes: Routes = [
  { path: 'reports', component: ReportsComponent, canActivate: [AuthenticationGuard] },
  { path: 'report/:id', component: ReportComponent, canActivate: [AuthenticationGuard] },
  { path: '', component: SplashComponent },  
  { path: 'signup', component: SignupComponent },    
  { path: 'signup/:email', component: SignupComponent },    
  { path: 'signup/:email/:code', component: SignupComponent },      
  { path: 'signin', component: SigninComponent },    
  { path: 'signin/:email', component: SigninComponent },
  { path: 'signout', component: SignoutComponent, canActivate: [AuthenticationGuard] },   
  { path: 'recover', component: RecoverComponent }, 
  { path: 'forgot', component: ForgotComponent },                
  { path: '**', component: PageNotFoundComponent}      
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
