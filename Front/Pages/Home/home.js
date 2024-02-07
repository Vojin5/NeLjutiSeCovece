import { baseUrl, serverUrl } from "../../config.js";
import { emailPattern, passwordPattern, usernamePattern } from "../../regex.js";
export class HomePage {
    constructor() {
        //Buttons
        this.playButton = document.getElementById("play-button");
        this.loginButton = document.getElementById("login-button");
        this.registerButton = document.getElementById("register-button");

        //Register form
        this.usernameRegisterInput = document.getElementById("UsernameRegister");
        this.passwordRegisterInput = document.getElementById("PasswordRegister");
        this.emailRegisterInput = document.getElementById("EmailRegister");
        this.usernameRegisterLabel = document.getElementById("registerUsernameLabel");
        this.passwordRegisterLabel = document.getElementById("registerPasswordLabel");
        this.emailRegisterLabel = document.getElementById("registerEmailLabel");
        this.imageRegisterInput = document.body.querySelector(".file-input");

        //Login form
        this.usernameLoginInput = document.getElementById("UsernameLogin");
        this.passwordLoginInput = document.getElementById("PasswordLogin");

        //data fields
        this.userRegister = {username: "", password: "", email: "", imageBase64Encoded: ""}
        this.userLogin = {username: "", password: ""}

        this.setClickEvents()
    }

    setClickEvents()
    {
        this.playButton.addEventListener("click", () => this.handlePlayButtonClick());

        this.loginButton.addEventListener("click", async () => await this.handleLoginButtonClick());
        this.registerButton.addEventListener("click", () => this.handleRegisterButtonClick());

        this.usernameLoginInput.addEventListener("input", (event) => this.handleUsernameLoginChange(event));
        this.passwordLoginInput.addEventListener("input", (event) => this.handlePasswordLoginChange(event));

        this.usernameRegisterInput.addEventListener("input", (event) => this.handleUsernameRegisterChange(event));
        this.passwordRegisterInput.addEventListener("input", (event) => this.handlePasswordRegisterChange(event));
        this.emailRegisterInput.addEventListener("input", (event) => this.handleEmailRegisterChange(event));
        this.imageRegisterInput.addEventListener("change", () => this.handleImageRegisterChange());
        
    }

    handlePlayButtonClick() {
        this.playButton.classList.add("animate");

        setTimeout(() => {
            this.playButton.remove("animate");
        }, 1000);

        document.body.classList.remove("scrollLogin");
        document.body.classList.add("scrollLogin");
        document.body.classList.add("bod");
    }

    async handleLoginButtonClick() {
        this.loginButton.classList.add("animate");
        setTimeout(() => {
            this.loginButton.classList.remove("animate");
        }, 1000);

        const loginRequest = await fetch(serverUrl + "/User/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.userLogin)
        });

        if (loginRequest.ok) {
            const loginData = await loginRequest.json();
            
            localStorage.setItem("id", loginData.id);
            localStorage.setItem("username", loginData.username);
            localStorage.setItem("points", loginData.elo);
            localStorage.setItem("image", loginData.image);

            window.open(baseUrl + '/Front/Pages/Lobby/lobby.html');
        }
        else {
            alert("Doslo je do greske,pokusajte ponovo");
        }
    }

    async handleRegisterButtonClick() {
        if (this.userRegister.username.length == 0 || !this.userRegister.username.match(usernamePattern)) {
            this.usernameRegisterLabel.style.color = "red";
            return;
        }
        if (this.userRegister.password.length == 0 || !this.userRegister.password.match(passwordPattern)) {
            this.passwordRegisterLabel.style.color = "red";
            return;
        }
        if (this.userRegister.email.length == 0 || !this.userRegister.email.match(emailPattern)) {
            this.emailRegisterLabel.style.color = "red";
            return;
        }
        if (this.userRegister.imageBase64Encoded.length == 0) {
            //dodati kasnije neki vizuelni error za sliku
            alert("Neophodno je dodati sliku");
            return;
        }

        const registerRequest = await fetch(serverUrl + "/User/register", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.userRegister)
        });

        if (registerRequest.ok) {
            const anchor = document.createElement("a");
            anchor.setAttribute('href','#page2');
            anchor.click();
        }
        else {
            alert("Username je zauzet,pokusajte ponovo");
            return;
        }

    }

    handleUsernameLoginChange(event) {
        this.userLogin.username = event.target.value;
    }

    handlePasswordLoginChange(event) {
        this.userLogin.password = event.target.value;
    }

    handleUsernameRegisterChange(event) {
        this.userRegister.username = event.target.value;
        this.usernameRegisterLabel.style.color = "whitesmoke";
    }

    handlePasswordRegisterChange(event) {
        this.userRegister.password = event.target.value;
        this.passwordRegisterLabel.style.color = "whitesmoke";
    }

    handleEmailRegisterChange(event) {
        this.userRegister.email = event.target.value;
        this.emailRegisterLabel.style.color = "whitesmoke";
    }

    handleImageRegisterChange() {
        const imagePreview = document.body.querySelector(".image-preview");
        const file = this.imageRegisterInput.files[0];
        const reader = new FileReader();
       
        reader.onload = () => {
            imagePreview.src = reader.result;
            imagePreview.style = "block";
            this.userRegister.imageBase64Encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
        }
        
        if (file) {
            reader.readAsDataURL(file);
        }
        else {
            imagePreview.src = "";
        }
    }

}

const homePage = new HomePage();

