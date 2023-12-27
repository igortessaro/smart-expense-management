import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class LocalStorageService {
    private localStorage: Storage;

    constructor() {
        this.localStorage = window.localStorage;
    }

    set(key: string, value: any): boolean {
        if (!this.localStorage) {
            return false;
        }

        this.localStorage.setItem(key, JSON.stringify(value));
        return true;
    }

    get(key: string): any {
        if (!this.localStorage) {
            return null;
        }

        const item = this.localStorage.getItem(key);
        return item != null ? JSON.parse(item): null;
    }

    remove(key: string): boolean {
        if (!this.localStorage) {
            return false;
        }

        this.localStorage.removeItem(key);
        return true;
    }

    clear(): boolean {
        if (!this.localStorage) {
            return false;
        }

        this.localStorage.clear();
        return true;
    }
}
