import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl: string = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getMessages(pageNumber: number, pageSize: number, container: string)
  {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.httpClient);
  }

  getMessageThread(userName: string)
  {
    return this.httpClient.get<Message[]>(this.baseUrl + 'messages/thread/' + userName);
  }

  sendMessage(username: string, content: string)
  {
    return this.httpClient.post<Message>(this.baseUrl + 'messages', {receiverUsername: username, content});
  }
}
