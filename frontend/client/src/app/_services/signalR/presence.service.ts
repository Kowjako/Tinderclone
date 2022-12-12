import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection; //npm install @microsoft/signalr

  // Robimy observable zeby sledzic zmiany
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor(private toastr: ToastrService, private router: Router) { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.jwtToken
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(e => console.log(e));

    this.hubConnection.on('UserIsOnline', username => {
      this.toastr.info(username + ' has connected!');
    });

    this.hubConnection.on('UserIsOffline', username => {
      this.toastr.warning(username + ' has disconnected!');
    });

    this.hubConnection.on('GetOnlineUsers', users => this.onlineUsersSource.next(users));

    this.hubConnection.on('NewMessageReceived', ({ username, knownAs }) => {
      this.toastr.info(knownAs + ' has sent you a new message! Click me to see it')
        .onTap.pipe(take(1)).subscribe({
          next: () => this.router.navigateByUrl('/members/' + username + '?tab=Messages')
        });
    })
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(e => console.log(e));
  }
}
