import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { AuthService } from './services/auth.service';
import { UserLogin } from './models/user-login';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
    public title = 'smart-expense-management-web';
    public isLoggedIn$: Observable<boolean>;
    public loggedUser$: Observable<UserLogin | null>;

    constructor(private modalService: NgbModal, private authService: AuthService) {
        this.isLoggedIn$ = this.authService.isLoggedIn;
        this.loggedUser$ = this.authService.loggedUser;
    }

    ngOnInit(): void {}

    public open(modal: any): void {
        this.modalService.open(modal);
    }

    public logout(): void {
        this.authService.logout();
    }
}
