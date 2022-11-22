import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {
  baseUrl = "https://localhost:5001/api/";
  validationErrors: string[] = [];

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
  }

  get404Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/not-found').subscribe(
      {
        next: resp => console.log(resp),
        error: err => console.log(err)
      }
    );
  }

  get400Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/bad-request').subscribe(
      {
        next: resp => console.log(resp),
        error: err => console.log(err)
      }
    );
  }

  get500Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/server-error').subscribe(
      {
        next: resp => console.log(resp),
        error: err => console.log(err)
      }
    );
  }
  get401Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/auth').subscribe(
      {
        next: resp => console.log(resp),
        error: err => console.log(err)
      }
    );
  }
  get400ValidataionError()
  {
    this.httpClient.post(this.baseUrl + 'account/register', {}).subscribe(
      {
        next: resp => console.log(resp),
        error: err => 
        {
          console.log(err);
          this.validationErrors = err;
        }
      }
    );
  }

}
