import { serverUrl } from "../../config.js";
import { prefix64Encoded } from "../../constants.js";
import { Dice } from ".././Home/Components/dice.js";

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
<<<<<<< HEAD

=======
        //proba dugme - obrisati kasnije
        this.diceButton = document.body.querySelector(".dice");
        
>>>>>>> origin/lobby
        this.setEventListeners();

        //user izgled
        document.getElementById("avatar").src = prefix64Encoded + localStorage.getItem("image");
        document.getElementById("username").innerHTML = localStorage.getItem("username");
        document.getElementById("points").innerHTML = localStorage.getItem("elo");

        //player avatars and usernames
        this.p0Avatar = document.body.querySelector(".avatar0");
        this.p0Username = document.body.querySelector(".username0");
        this.p1Avatar = document.body.querySelector(".avatar1");
        this.p1Username = document.body.querySelector(".username1");
        this.p2Avatar = document.body.querySelector(".avatar2");
        this.p2Username = document.body.querySelector(".username2");
        this.p3Avatar = document.body.querySelector(".avatar3");
        this.p3Username = document.body.querySelector(".username3");
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

            document.body.classList.remove("scrollDown");
            document.body.classList.add("scrollDown");

            await this.connection.invoke("JoinLobby");
        });

        this.exitButton.addEventListener("click", async () => {
            this.buttonsContainer.classList.remove("disabled");
            this.buttonsContainer.classList.add("enabled");

            this.playersContainer.classList.remove("enabled");
            this.playersContainer.classList.add("disabled");
            
            await this.connection.invoke("LeaveLobby");
        });

    }   

    async establishConnection() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(serverUrl + "/game")
            .build();

        this.connection.on("UpdateLobby", (players) => this.updateLobbyScreen(players));
        this.connection.on("GameStart", (gameId) => this.startGameScreen(gameId));
        this.connection.on("NextPlayer", (playerId) => this.nextPlayerScreen(playerId));
        this.connection.on("DiceArrived", (diceNum) => this.diceArrivedScreen(diceNum));

        await this.connection.start();
        const pInfo = {
            playerId: Number.parseInt(localStorage.getItem("id")),
            avatar: localStorage.getItem("image"),
            username: localStorage.getItem("username")
        }
        
        await this.connection.invoke("SendMyInfo", pInfo.playerId, pInfo.avatar, pInfo.username);
    }

    updateLobbyScreen(players) {
        this.p0Avatar.src = "../../Resources/user.jpg";
        this.p0Username.textContent = "Player Name";
        this.p1Avatar.src = "../../Resources/user.jpg";
        this.p1Username.textContent = "Player Name";
        this.p2Avatar.src = "../../Resources/user.jpg";
        this.p2Username.textContent = "Player Name";
        this.p3Avatar.src = "../../Resources/user.jpg";
        this.p3Username.textContent = "Player Name";

        let pAvatar, pUsername;
        this.players = players;
        this.players.forEach((p, index) => {
            pAvatar = document.body.querySelector(".avatar" + index);
            pUsername = document.body.querySelector(".username" + index);
            pAvatar.src = prefix64Encoded + p.avatar;
            pUsername.textContent = p.username;
        });
    }

    startGameScreen(gameId) {
        this.gameId = gameId;

        this.buttonsContainer.classList.remove("enabled");
        this.buttonsContainer.classList.add("disabled");

        this.playersContainer.classList.remove("enabled");
        this.playersContainer.classList.add("disabled");

    }

    nextPlayerScreen(playerId) {
        if (Number.parseInt(localStorage.getItem("id")) == playerId) {
            this.diceButton.removeAttribute("disabled");
        }
        else {
            this.diceButton.setAttribute("disabled", "true");
        }
    }

    diceArrivedScreen(diceNum) {
        console.log(diceNum);
    }
}

let lobby = new Lobby();