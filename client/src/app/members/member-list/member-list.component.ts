import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../_services/account.service';
import { UserParams } from '../../_models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons'

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  memberService = inject(MembersService);
  private accountService = inject(AccountService);
  userParams = new UserParams(this.accountService.currentUser());
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]

  ngOnInit(): void {
    if (!this.memberService.paginatedResult()) this.loadMember();
  }

  loadMember() {
    this.memberService.getMembers(this.userParams);
  }

  resteFilters() {
    this.userParams = new UserParams(this.accountService.currentUser());
    this.loadMember();
  }

  pageChanged(event: any) {
    if (this.userParams.pageNumber != event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMember();
    }
  }

}
