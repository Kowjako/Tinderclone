import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();
  user: User | undefined;
  userParams: UserParams | undefined;

  constructor(private httpClient: HttpClient, private accService: AccountService) { 
    this.accService.currentUser$.pipe(take(1)).subscribe({
      next: resp => {
        if(resp)
        {
          this.userParams = new UserParams(resp);
          this.user = resp;
        }
      }
    })
  }

  getUserParams()
  {
    return this.userParams;
  }

  setUserParams(params: UserParams)
  {
    this.userParams = params;
  }

  resetUserParams()
  {
    if(this.user)
    {
      this.userParams = new UserParams(this.user);
      return this.userParams
    }
    return;
  }

  getMembers(userParams: UserParams)
  {
    /* cachowanie - kluczem jest get query */
    console.log(userParams);
    const response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response) return of(response);

    console.log('checker')
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.httpClient).pipe(
      map(result => {
        this.memberCache.set(Object.values(userParams).join('-'), result);
        return result;
      })
    );
  }

  getMember(username: string)
  {
    const member = [...this.memberCache.values()].reduce((arr, elem) => arr.concat(elem.result), [])
                                                 .find((member: Member) => member.userName === username);
    if(member) return of(member);

    return this.httpClient.get<Member>(this.baseUrl + `users/${username}`);
  }

  updateMember(member: Member)
  {
    return this.httpClient.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member}
      })
    );
  }

  setMainPhoto(photoId: number)
  {
    return this.httpClient.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number)
  {
    return this.httpClient.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string)
  {
    return this.httpClient.post(this.baseUrl + 'likes/' + username, {});
  }

  getLikes(predicate: string, pageNumber: number, pageSize: number)
  {
    let params = getPaginationHeaders(pageNumber, pageSize);

    params = params.append('predicate', predicate);

    return getPaginatedResult<Member[]>(this.baseUrl + 'likes', params, this.httpClient);
  }
}
