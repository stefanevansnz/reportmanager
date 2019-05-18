import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptions, Response } from "@angular/http";
import { environment } from '../../environments/environment';
import { AuthenticationService } from "./authentication.service";

@Injectable()
export class HtmlProxyService {

    constructor(private http: Http) { }           

 
    public getHtmlFromServer(url: string, component: any, postCallback) {
        let self = this;

        console.log('Calling url: ' + url);
        self.http.get(url).subscribe(
        (success: Response) => {   
            let result = success.text();
            postCallback(component, result);
        },
        (error: Response) => {
            console.log('error' + JSON.stringify(error));
            component.errorMessage = error.json().message;            
        })

    }

    

}    