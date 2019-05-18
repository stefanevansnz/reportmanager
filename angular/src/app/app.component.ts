import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthenticationService } from "./shared/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'reporterbot';

  constructor(              
    private authenticationService: AuthenticationService ) {
  }

  userUpdate: Subscription;

}
