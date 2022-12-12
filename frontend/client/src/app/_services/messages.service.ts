import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group } from '../_models/group';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl: string = environment.apiUrl;
  hubUrl: string = environment.hubUrl;
  private hubConnection?: HubConnection;

  private messageThreadSource = new BehaviorSubject<Message[]>([])
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private httpClient: HttpClient) { }

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.jwtToken
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.log(err));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: messages => {
          this.messageThreadSource.next([...messages, message]);
        }
      });
    });

    this.hubConnection.on("UpdateGroup", (group: Group) => {
      if(group.connections.some(x => x.username === otherUsername))
      {
        this.messageThread$.pipe(take(1)).subscribe({
          next: messages => {
            messages.forEach(m => {
              if(!m.dateRead) m.dateRead = new Date(Date.now());
            })
            this.messageThreadSource.next([...messages]);
          }
        });
      }
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.httpClient);
  }

  getMessageThread(userName: string) {
    return this.httpClient.get<Message[]>(this.baseUrl + 'messages/thread/' + userName);
  }

  async sendMessage(username: string, content: string) {
    return this.hubConnection?.invoke('SendMessage', { receiverUsername: username, content })
      .catch(err => console.log(err));
  }

  deleteMessage(msgId: number) {
    return this.httpClient.delete(this.baseUrl + `messages/${msgId}`);
  }
}
