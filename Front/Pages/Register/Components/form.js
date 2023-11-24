import { serverUrl } from "../../../config.js";
import { usernamePattern, emailPattern, passwordPattern } from "../../../regex.js"
export class Form {
    constructor() {
        this.container = document.body.querySelector(".form-div");
        
        this.username = "";
        this.email = "";
        this.password = "";
    }

    setupRegisterBtn() {
        this.register = this.container.querySelector(".register");
        this.register.addEventListener("click", async () => {
            await this.validateAndRegister();
        });
    }

    setupUsernameInput() {
        this.usernameInp = this.container.querySelector(".username");
        this.usernameInp.addEventListener("change", ev => {
            this.username = ev.target.value;
        })
    }

    setupEmailInput() {
        this.emailInp = this.container.querySelector(".email");
        this.emailInp.addEventListener("change", ev => {
            this.email = ev.target.value;
        });
    }

    setupPasswordInput() {
        this.passwordInp = this.container.querySelector(".password");
        this.passwordInp.addEventListener("change", ev => {
            this.password = ev.target.value;
        });
    }

    async validateAndRegister() {
        if (!this.username.match(usernamePattern)) {
            alert("Username je nevalidnog oblika");
            return;
        }

        if (!this.email.match(emailPattern)) {
            alert("Email je nevalidnog oblika");
            return;
        }

        if (!this.password.match(passwordPattern)) {
            alert("Password je nevalidnog oblika");
            return;
        }
        
        const user = {
            username: this.username,
            email: this.email,
            password: this.password
        };

        const userRegisterFetch = await fetch(serverUrl + "/User/Register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(user)
        })

        if (!userRegisterFetch.ok) {
            alert("Username je zauzet");
            return;
        }

        alert("Uspesna registracija!");

    }

    draw() {
        this.setupRegisterBtn();
        this.setupUsernameInput();
        this.setupEmailInput();
        this.setupPasswordInput();
    }
}