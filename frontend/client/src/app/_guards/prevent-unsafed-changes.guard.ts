import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate } from '@angular/router';
import { Observable, of } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { Member } from '../_models/member';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsafedChangesGuard implements CanDeactivate<MemberEditComponent> {

  constructor(private confirmSvc: ConfirmService) {}

  canDeactivate(component: MemberEditComponent): Observable<boolean> {
    if(component.editForm?.dirty) {
      return this.confirmSvc.confirm();
    }
    return of(true);
  }
}
