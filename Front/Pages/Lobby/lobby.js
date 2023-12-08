
export class Lobby{
    constructor()
    {
        //Containers
        this.playersContainer = document.querySelector(".lobby-container");
        this.buttonsContainer = document.querySelector(".buttons-container");

        //Buttons
        this.joinButton = document.querySelector("#join-button");
        this.createButton = document.querySelector("#create-button");
        this.exitButton = document.querySelector("#exit-button");
        this.setEventListeners()

    }

    setEventListeners()
    {
        this.joinButton.addEventListener("click",() => {
            this.buttonsContainer.classList.remove("enabled");
            this.buttonsContainer.classList.add("disabled");

            this.playersContainer.classList.remove("disabled");
            this.playersContainer.classList.add("enabled");
        });

        this.exitButton.addEventListener("click",() => {
            this.buttonsContainer.classList.remove("disabled");
            this.buttonsContainer.classList.add("enabled");

            this.playersContainer.classList.remove("enabled");
            this.playersContainer.classList.add("disabled");
        });
    }
}

let lobby = new Lobby();