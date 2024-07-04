import { NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  imports: [NgIf, RegisterComponent]
})
export class HomeComponent implements OnInit {
  http = inject(HttpClient)
  registerMode = false;
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(e: any) {
    this.registerMode = e;
  }

  getUsers() {
    this.http.get('https://localhost:5000/api/users').subscribe({
      next: resonse => this.users = resonse,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    })
  }

}
