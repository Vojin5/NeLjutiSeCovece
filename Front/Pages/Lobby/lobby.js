import { serverUrl } from "../../config.js";

export class Lobby {
    constructor()
    {
        //Containers
        this.playersContainer = document.querySelector(".lobby-container");
        this.buttonsContainer = document.querySelector(".buttons-container");

        //Buttons
        this.joinButton = document.querySelector("#join-button");
        this.createButton = document.querySelector("#create-button");
        this.exitButton = document.querySelector("#exit-button");
        this.setEventListeners();

        //user izgled
        document.getElementById("avatar").src = "data:image/png;base64," + localStorage.getItem("image");
        document.getElementById("username").innerHTML = localStorage.getItem("username");
        document.getElementById("points").innerHTML = localStorage.getItem("elo");

        //player avatars and usernames
        this.p0Avatar = document.getElementById("avatar0");
        this.p0Username = document.getElementById("username0");
        this.p1Avatar = document.getElementById("avatar0");
        this.p1Username = document.getElementById("username0");
        this.p2Avatar = document.getElementById("avatar0");
        this.p2Username = document.getElementById("username0");
        this.p3Avatar = document.getElementById("avatar0");
        this.p3Username = document.getElementById("username0");

        this.connection = null;

        document.addEventListener('DOMContentLoaded', async () => {
            await this.establishConnection();
        });

    }

    setEventListeners()
    {
        this.joinButton.addEventListener("click",async () => {
            this.buttonsContainer.classList.remove("enabled");
            this.buttonsContainer.classList.add("disabled");

            this.playersContainer.classList.remove("disabled");
            this.playersContainer.classList.add("enabled");

            await this.joinLobby();
        });

        this.exitButton.addEventListener("click", async () => {
            this.buttonsContainer.classList.remove("disabled");
            this.buttonsContainer.classList.add("enabled");

            this.playersContainer.classList.remove("enabled");
            this.playersContainer.classList.add("disabled");
            
            await this.leaveLobby();
        });
    }

    async joinLobby() {
        await this.connection.invoke("JoinLobby");
    }

    async leaveLobby() {
        await this.connection.invoke("LeaveLobby");
    }

    async establishConnection() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(serverUrl + "/game")
            .build();

        this.connection.on("UpdateLobby", (players) => this.updateLobby(players));

        await this.connection.start();
        const pInfo = {
            playerId: Number.parseInt(localStorage.getItem("id")),
            avatar: localStorage.getItem("image")
        }
        
        await this.connection.invoke("SendMyInfo", pInfo.playerId, pInfo.avatar);
    }

    updateLobby(players) {
        this.p0Username.innerHTML = "Nesto";
        let pAvatar, pUsername;
        console.log("SVE OK");
        players.forEach((p, index) => {
            pAvatar = document.getElementById("avatar" + index);
            pUsername = document.getElementById("username" + index);
            pAvatar.src = "data:image/png;base64," + p.avatar;
            pUsername.innerHTML = p.username;
        });
    }
}

let lobby = new Lobby();