import { Component } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'client';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit()
  {
    this.getUsers();
  }

  getUsers()
  {
    this.http.get("https://localhost:5001/api/Users").subscribe({
      next: response => this.users = response,
      error: error => console.log(error)
    });
  }
}
