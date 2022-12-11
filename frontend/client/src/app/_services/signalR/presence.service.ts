import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { User } from '../../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection; //npm install @microsoft/signalr

  constructor(private toastr: ToastrService) { }

  createHubConnection(user: User)
  {
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
  }

  stopHubConnection()
  {
    this.hubConnection.stop().catch(e => console.log(e));
  }
}
