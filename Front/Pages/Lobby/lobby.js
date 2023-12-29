import { serverUrl } from "../../config.js";
import { prefix64Encoded } from "../../constants.js";
import { EditProfile } from "../Home/Components/editProfile.js";
import { GameTable } from "../Home/Components/gameTable.js";
import { MatchHistory } from "../Home/Components/matchHistory.js";

export class Lobby {
    constructor()
    {
        //Containers
        this.playersContainer = document.querySelector(".lobby-container");
        this.buttonsContainer = document.querySelector(".buttons-container");
        this.matchHistoryContainer = document.querySelector(".match-history-container");
        this.editProfileContainer = document.querySelector(".edit-profile-container");

        //Buttons
        this.joinButton = document.querySelector("#join-button");
        this.createButton = document.querySelector("#create-button");
        this.exitButton = document.querySelector("#exit-button");
        this.matchHistoryButton = document.querySelector("#match-history-button");
        this.matchHistoryLeaveButton = document.querySelector("#leave-match-history-button");
        this.editProfileButton = document.querySelector("#edit-profile-button");
        this.exitEditButton = document.querySelector("#exit-edit-button");

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

        this.matchHistoryButton.addEventListener("click", async () => {
            this.buttonsContainer.classList.remove("enabled");
            this.buttonsContainer.classList.add("disabled");

            this.matchHistoryContainer.classList.remove("disabled");
            this.matchHistoryContainer.classList.add("enabled");
            let matchHistory = new MatchHistory(this.matchHistoryContainer);
        });

        this.matchHistoryLeaveButton.addEventListener("click",() => {
            this.buttonsContainer.classList.remove("disabled");
            this.buttonsContainer.classList.add("enabled");

            this.matchHistoryContainer.classList.remove("enabled");
            this.matchHistoryContainer.classList.add("disabled");
        });

        this.editProfileButton.addEventListener("click",() => {
            this.buttonsContainer.classList.remove("enabled");
            this.buttonsContainer.classList.add("disabled");

            this.editProfileContainer.classList.remove("disabled");
            this.editProfileContainer.classList.add("enabled");


            let editProfile = new EditProfile();
        });

        this.exitEditButton.addEventListener("click",() => {
            this.buttonsContainer.classList.remove("disabled");
            this.buttonsContainer.classList.add("enabled");

            this.editProfileContainer.classList.remove("enabled");
            this.editProfileContainer.classList.add("disabled");
        })
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



