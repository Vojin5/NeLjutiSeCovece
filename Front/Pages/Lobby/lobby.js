import { serverUrl } from "../../config.js";
import { prefix64Encoded } from "../../constants.js";
import { GameTable } from "../Home/Components/gameTable.js";

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
        document.getElementById("avatar").src = prefix64Encoded + localStorage.getItem("image");
        document.getElementById("username").innerHTML = localStorage.getItem("username");
        document.getElementById("points").innerHTML = localStorage.getItem("points");

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

        this.connection.on("handleUpdateLobby", (players) => this.handleUpdateLobby(players));
        this.connection.on("handleGameStart", (gameId) => this.handleGameStart(gameId));

        await this.connection.start();
        const pInfo = {
            playerId: Number.parseInt(localStorage.getItem("id")),
            avatar: localStorage.getItem("image"),
            username: localStorage.getItem("username")
        }
        
        await this.connection.invoke("SendMyInfo", pInfo.playerId, pInfo.avatar, pInfo.username);
    }

    handleUpdateLobby(players) {
        
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

    handleGameStart(gameId) {
        new GameTable(gameId, this.connection, this.players).draw();
    }
}

let lobby = new Lobby();

let y1 = document.getElementById("-1");
let y2 = document.getElementById("0");
y1.addEventListener("click" , () => {
    let tmp = y1.src;
    y1.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
    y2.src = tmp;
});

y2.addEventListener("click" , () => {
    let tmp = y2.src;
    y2.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
    y1.src = tmp;
})


