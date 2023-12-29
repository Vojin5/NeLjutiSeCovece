import { baseUrl, serverUrl } from "../../../config.js";
import { emailPattern, passwordPattern, usernamePattern } from "../../../regex.js";
export class EditProfile
{
    constructor()
    {
        this.usernameInput = document.querySelector("#UsernameRegisterEdit");
        this.passwordInput = document.querySelector("#PasswordRegisterEdit");
        this.emailInput = document.querySelector("#EmailRegisterEdit");

        this.usernameLabel = document.querySelector("#registerUsernameLabelEdit");
        this.passwordLabel = document.querySelector("#registerPasswordLabelEdit");
        this.emailLabel = document.querySelector("#registerEmailLabelEdit");
        this.imageEditButton = document.querySelector(".file-input-edit");

        this.userEdit = {username: "", password: "", email: "", image: ""}

        this.applyButton = document.querySelector("#apply-button");
        this.setListeners();
        this.setInitialData();
    }

    async setInitialData()
    {
        let data = await fetch(serverUrl+"/User/Info/"+localStorage.getItem("id"));
        data = await data.json();
        console.log(data);
        this.usernameInput.value = data.username;
        this.emailInput.value = data.email;
        this.userEdit.image = data.image;

        this.userEdit.username = data.username;
        this.userEdit.email = data.email;
    }

    setListeners()
    {
        this.usernameInput.addEventListener("input",(event) => this.handleUsernameRegisterChange(event));
        this.passwordInput.addEventListener("input",(event) => this.handlePasswordRegisterChange(event));
        this.emailInput.addEventListener("input",(event) => this.handleEmailRegisterChange(event));
        this.imageEditButton.addEventListener("change", () => this.handleImageRegisterChange());


        this.applyButton.addEventListener("click",() => this.handleApplyButtonClick());
    }

    handleUsernameRegisterChange(event) {
        this.userEdit.username = event.target.value;
        this.usernameLabel.style.color = "whitesmoke";
    }

    handlePasswordRegisterChange(event) {
        this.userEdit.password = event.target.value;
        this.passwordLabel.style.color = "whitesmoke";
    }

    handleEmailRegisterChange(event) {
        this.userEdit.email = event.target.value;
        this.emailLabel.style.color = "whitesmoke";
    }

    handleImageRegisterChange() {
        const imagePreview = document.body.querySelector(".image-preview-edit");
        const file = this.imageEditButton.files[0];
        const reader = new FileReader();
       
        reader.onload = () => {
            imagePreview.src = reader.result;
            imagePreview.style = "block";
            this.userEdit.image = reader.result.toString().replace(/^data:(.*,)?/, ''); 
        }
        
        if (file) {
            reader.readAsDataURL(file);
        }
        else {
            imagePreview.src = "";
        }
    }

    async handleApplyButtonClick() {
        if (this.userEdit.username.length == 0 || !this.userEdit.username.match(usernamePattern)) {
            this.usernameLabel.style.color = "red";
            alert("Username : " + this.usernameInput.value + " nije korektno napisan ");
            return;
        }
        if (this.userEdit.password.length == 0 || !this.userEdit.password.match(passwordPattern)) {
            this.passwordLabel.style.color = "red";
            alert("Password nije korektno napisana");
            return;
        }
        if (this.userEdit.email.length == 0 || !this.userEdit.email.match(emailPattern)) {
            this.emailLabel.style.color = "red";
            alert("Email nije korektno napisan");
            return;
        }
        if (this.userEdit.image.length == 0) { 
            //dodati kasnije neki vizuelni error za sliku
            alert("Neophodno je ucitati sliku");
            return;
        }
        console.log(this.userEdit);
        //User/info/{id}
        //User/update/{id} body :user
        const registerRequest = await fetch(serverUrl + "/User/update/"+localStorage.getItem("id"), {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(this.userEdit)
        });

        if (registerRequest.ok) {
            alert("Uspesna izmena profila");
        }
        else {
            alert("Greska u obradi,pokusajte ponovo");
            return;
        }

    }

}