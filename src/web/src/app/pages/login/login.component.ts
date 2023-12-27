import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
    public formLogin!: FormGroup;

    constructor(private formBuilder: FormBuilder, private router: Router, private authService: AuthService) {}

    ngOnInit(): void {
        this.formLogin = this.formBuilder.group({
            userName: new FormControl('', [Validators.required]),
            password: new FormControl('', [Validators.required]),
            passwordRemember: new FormControl(false),
        });
    }

    onSubmit() {
        if (this.formLogin.invalid) {
            alert('Formulário inválido');
            return;
        }

        console.log(this.formLogin.value);
        this.authService.login(this.formLogin.value.userName, this.formLogin.value.password);
    }
}
