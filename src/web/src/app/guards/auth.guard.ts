import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

export const authGuard: CanActivateFn = (): Observable<boolean> => {
    const authService: AuthService = inject(AuthService);
    const router: Router = inject(Router);
    return authService.isLoggedIn.pipe(
        map((isLoggedIn) => {
            if (!isLoggedIn) {
                router.navigate(['/login']);
                return false;
            }
            return true;
        })
    );
};
