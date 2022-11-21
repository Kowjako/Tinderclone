import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr'

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accService: AccountService, private toastr: ToastrService) {}

  canActivate(): Observable<boolean> {
    return this.accService.currentUser$.pipe(
      map(user => {
        if(user) return true;
        this.toastr.error('You shall not pass!');
      })
    );
  }
  
}
