import { serverUrl } from "../../config.js";
import { emailPattern, passwordPattern, usernamePattern } from "../../regex.js";
export class HomePage {
    constructor() {
        //Buttons
        this.playButton = document.getElementById("play-button");
        this.loginButton = document.getElementById("login-button");
        this.registerButton = document.getElementById("register-button");
        this.imageInput = document.body.querySelector(".file-input");

        //Register form
        this.registerUsername = document.getElementById("UsernameRegister");
        this.registerEmail = document.getElementById("EmailRegister");
        this.registerPassword = document.getElementById("PasswordRegister");
        this.registerUsernameLabel = document.getElementById("registerUsernameLabel");
        this.registerPasswordLabel = document.getElementById("registerPasswordLabel");
        this.registerEmailLabel = document.getElementById("registerEmailLabel");

        //Login form
        this.UsernameLogin = document.getElementById("UsernameLogin");
        this.PasswordLogin = document.getElementById("PasswordLogin");

        //data fields
        this.username = "";
        this.password = "";
        this.email = "";
        this.image = "";

        this.usernameLogin = "";
        this.passwordLogin = "";

        this.setClickEvents()
    }

    setClickEvents()
    {
        this.playButton.addEventListener("click", () => {
            this.playButton.classList.add("animate");
            setTimeout(() => {
                this.playButton.classList.remove("animate");
            }, 1000);
            document.body.classList.remove("scrollLogin");
            document.body.classList.add("scrollLogin");
            document.body.classList.add("bod");
        });

        this.loginButton.addEventListener("click", async () => {
            this.loginButton.classList.add("animate");
            setTimeout(() => {
                this.loginButton.classList.remove("animate");
            }, 1000);

            this.usernameLogin = this.UsernameLogin.value;
            this.passwordLogin = this.PasswordLogin.value;

            const userLogin = await fetch(serverUrl + "/User/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({username: this.usernameLogin, password: this.passwordLogin})
            });

            if (userLogin.ok) {
                const user = await userLogin.json();
                localStorage.setItem("id", user.id);
                localStorage.setItem("username", user.username);
                localStorage.setItem("elo", user.elo);
                localStorage.setItem("image", "data:image/png;base64," + user.image);
                console.log(localStorage.getItem("id"));
            }
            else {
                alert("Dodati kasnije errore kada je login neuspesan");
            }
        });

        this.imageInput.addEventListener("change", () => {
            const imagePreview = document.body.querySelector(".image-preview");
            const file = this.imageInput.files[0];
            const reader = new FileReader();
        
            reader.onloadend = () => {
                imagePreview.src = reader.result;
                imagePreview.style = "block";
                this.image = reader.result.toString().replace(/^data:(.*,)?/, '');
            }
        
            if (file) {
                reader.readAsDataURL(file);
            }
            else {
                imagePreview.src = "";
            }
        });

        this.registerUsername.addEventListener('input',() => {
            this.registerUsernameLabel.style.color = "whitesmoke";
        });

        this.registerEmail.addEventListener('input',() => {
            this.registerEmailLabel.style.color = "whitesmoke";
        });

        this.registerPassword.addEventListener('input',() => {
            this.registerPasswordLabel.style.color = "whitesmoke";
        });

        this.registerButton.addEventListener("click",async () => {
            let success = true;
            if(this.registerUsername.value.length == 0 || !this.registerUsername.value.match(usernamePattern))
            {
                this.registerUsernameLabel.style.color = "red";
                success = false;
            }
            if(this.registerEmail.value.length == 0 || !this.registerEmail.value.match(emailPattern))
            {
                this.registerEmailLabel.style.color = "red";
                success = false;
            }
            if(this.registerPassword.value.length == 0 || !this.registerPassword.value.match(passwordPattern))
            {
                this.registerPasswordLabel.style.color = "red";
                success = false;
            }

            if(success)
            {
                this.username = this.registerUsername.value;
                this.password = this.registerPassword.value;
                this.email = this.registerEmail.value;

                const userRegister = await fetch(serverUrl + "/User/register", {
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({username: this.username, password: this.password, email: this.email, ImageBase64Encoded: this.image}),

                });

                if (userRegister.ok) {
                    let anchor = document.createElement("a");
                    anchor.setAttribute('href','#page2');
                    anchor.click();
                }
                else {
                    alert("Dodati kasnije ako je BadRequest()");
                }
            }

        })
    }

}

