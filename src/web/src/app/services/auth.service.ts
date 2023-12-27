import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserLogin } from '../models/user-login';
import { LocalStorageService } from './local-storage.service';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    private loggedInUser: BehaviorSubject<UserLogin | null> = new BehaviorSubject<UserLogin | null>(null);

    constructor(private router: Router, private http: HttpClient, private localStorageService: LocalStorageService) {
        const user = this.localStorageService.get('user');
        if (user) {
            this.loggedIn.next(true);
            this.loggedInUser.next(user);
        }
    }

    get isLoggedIn(): Observable<boolean> {
        return this.loggedIn.asObservable();
    }

    get loggedUser(): Observable<UserLogin | null> {
        return this.loggedInUser.asObservable();
    }

    public login(userName: string, password: string): void {
        this.http.post(`${environment.expenseApi}/api/login`, { userName, password }).subscribe((res) => {
            this.loggedIn.next(true);
            this.loggedInUser.next(res as UserLogin);
            this.localStorageService.set('user', res);
            this.router.navigate(['/']);
        });
    }

    public logout(): void {
        this.loggedIn.next(false);
        this.loggedInUser.next(null);
        this.localStorageService.remove('user');
        this.router.navigate(['/login']);
    }
}
