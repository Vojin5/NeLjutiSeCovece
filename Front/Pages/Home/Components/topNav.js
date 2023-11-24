import { baseUrl } from "../../../config.js";

export class TopNav {
    constructor() {
        this.container = document.body.querySelector(".top-nav");
    }

    draw() {
        this.setupRegisterBtn();
        this.setupLoginBtn();
    }

    setupRegisterBtn() {
        this.register = this.container.querySelector(".register");
        this.register.addEventListener("click", () => {
            window.open(baseUrl + "/Pages/Register/register.html");
        });
    }

    setupLoginBtn() {
        this.login = this.container.querySelector(".login");
        this.login.addEventListener("click", () => {
            window.open(baseUrl + "/Pages/Login/login.html");
        });
    }
}