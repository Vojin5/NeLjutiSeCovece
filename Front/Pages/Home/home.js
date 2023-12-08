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

        this.loginButton.addEventListener("click", () => {
            this.loginButton.classList.add("animate");
            setTimeout(() => {
                this.loginButton.classList.remove("animate");
            }, 1000);
        });

        this.imageInput.addEventListener("change", () => {
            const imagePreview = document.body.querySelector(".image-preview");
            const file = this.imageInput.files[0];
            const reader = new FileReader();
        
            reader.onloadend = () => {
                imagePreview.src = reader.result;
                imagePreview.style = "block";
                //img = reader.result.toString().replace(/^data:(.*,)?/, '');
                //console.log(img);
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

        this.registerButton.addEventListener("click",() => {
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
                let anchor = document.createElement("a");
                anchor.setAttribute('href','#page2');
                anchor.click();
            }

        })
    }

}

