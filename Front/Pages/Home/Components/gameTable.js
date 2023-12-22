import { prefix64Encoded } from "../../../constants.js";
import { Dice } from "./dice.js";

export class GameTable{

    constructor(gameID, connection, players)
    {
        this.gameID = gameID;
        this.connection = connection;
        this.players = players;
        this.dice = null;

        this.connection.on("handleMyTurn", async () => await this.handleMyTurn());
        this.connection.on("handleStartDiceAnimation", () => this.handleStartDiceAnimation())
        this.connection.on("handleDiceNumber", (diceNum) => this.handleDiceNumber(diceNum));

        this.yellowUserAvatar = document.querySelector(".yellow-user-avatar");
        this.yellowUserUsername = document.querySelector(".yellow-user-username");
        this.yellowUserAvatar.src = prefix64Encoded + this.players[0].avatar;
        this.yellowUserUsername.innerHTML = this.players[0].username;

        this.greenUserAvatar = document.querySelector(".green-user-avatar");
        this.greenUserUsername = document.querySelector(".green-user-username");
        this.greenUserAvatar.src = prefix64Encoded + this.players[1].avatar;
        this.greenUserUsername.innerHTML = this.players[1].username;
        
        this.redUserAvatar = document.querySelector(".red-user-avatar");
        this.redUserUsername = document.querySelector(".red-user-username");
        this.redUserAvatar.src = prefix64Encoded + this.players[2].avatar;
        this.redUserUsername.innerHTML = this.players[2].username;

        this.blueUserAvatar = document.querySelector(".blue-user-avatar");
        this.blueUserUsername = document.querySelector(".blue-user-username");
        this.blueUserAvatar.src = prefix64Encoded + this.players[3].avatar;
        this.blueUserUsername.innerHTML = this.players[3].username;
    }

    draw() {
        document.body.classList.remove("scrollDown");
        document.body.classList.add("scrollDown");
        this.dice = new Dice();
        this.dice.addClickListener(async () => {
            await this.connection.invoke("DiceThrown", this.gameID);
        });
        
    }

    async handleMyTurn() {
        console.log("TVOJ RED");
    }

    handleStartDiceAnimation() {
        this.dice.animateDice();
        
    }

    handleDiceNumber(diceNum) {
        setTimeout(() => {
            this.dice.stopAnimation(diceNum);
        }, 2000);
    }

    

    
}