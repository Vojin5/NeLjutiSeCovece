import { baseUrl, serverUrl } from "../../../config.js"
export class Form {
    constructor() {
        this.container = document.body.querySelector(".form-div");

        this.username = "";
        this.password = "";
    }

    setupLoginBtn() {
        this.loginBtn = this.container.querySelector(".login");
        this.loginBtn.addEventListener("click", async () => {
            await this.fetchUser();
        });
    }

    setupUsernameInput() {
        this.usernameInp = this.container.querySelector(".username");
        this.usernameInp.addEventListener("change", ev => {
            this.username = ev.target.value;
        });
    }

    setupPasswordInput() {
        this.passwordInp = this.container.querySelector(".password");
        this.passwordInp.addEventListener("change", ev => {
            this.password = ev.target.value;
        });
    }

    async fetchUser() {
        const userFetch = await fetch(serverUrl + "/User/Login?username=" + this.username + "&password=" + this.password, {
            method: "GET"
        });

        if (!userFetch.ok) {
            //za sada alert, posle nesto vise fancy
            alert("Uneti podaci su netacni");
            return;
        }

        const user = await userFetch.json();
        localStorage.setItem("user-login-info", JSON.stringify(user));
        window.open(baseUrl + "/Pages/Home/home.html");
    }
    

    draw() {
        this.setupLoginBtn();
        this.setupUsernameInput();
        this.setupPasswordInput();
    }
}