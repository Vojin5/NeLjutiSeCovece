import { prefix64Encoded } from "../../../constants.js";
import { Figure } from "../figure.js";
import { Dice } from "./dice.js";

export class GameTable{

    constructor(gameID, connection, players)
    {
        this.gameID = gameID;
        this.connection = connection;
        this.players = players;
        this.dice = null;
        this.diceEnabled = false;

        this.connection.on("handleMyTurn", async () => await this.handleMyTurn());
        this.connection.on("handleStartDiceAnimation", () => this.handleStartDiceAnimation())
        this.connection.on("handleDiceNumber", (diceNum) => this.handleDiceNumber(diceNum));
        this.connection.on("handlePossibleMoves", async (moves) => await this.handlePossibleMoves(moves));
        this.connection.on("handlePlayerMove", (move) => this.handlePlayerMove(move));

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

        this.lista = new Array(56);
        for(let i = 0;i<56;i++)
        {
            this.lista[i] = null;
        }
    }

    draw() {
        document.body.classList.remove("scrollDown");
        document.body.classList.add("scrollDown");
        this.dice = new Dice();
        this.dice.toggleVisibility();
        this.dice.addClickListener(async () => {
            await this.connection.invoke("DiceThrown", this.gameID);
            this.dice.toggleVisibility();
        });
        
    }

    //bilo je async
    handleMyTurn() {
        // setTimeout(() => {
        //     this.dice.toggleVisibility();    
        // }, 2000);
        this.dice.toggleVisibility();
    }

    handleStartDiceAnimation() {
        this.dice.animateDice();
    }

    handleDiceNumber(diceNum) {
        // setTimeout(() => {
        //     this.dice.stopAnimation(diceNum);
        // }, 2000);
        this.dice.stopAnimation(diceNum);
        console.log("kockica : " + diceNum);
    }

    async handlePossibleMoves(moves) {
        console.log(moves);
        const moveChoice = prompt();
        await this.connection.invoke("MovePlayed", this.gameID, moves[Number.parseInt(moveChoice)]);
    }

    handlePlayerMove(move) {
        // console.log("Igrac je odigrao potez ");
        // console.log(move);
        
        // if(this.lista[move.newPosition] != null)
        // {
        //     const attackedFigure = lista[move.newPosition];
        //     this.putInBase(attackedFigure);
        //     this.lista[move.newPosition] = null;
        // }

        // this.makeMove(move);
        // this.lista[move.newPosition] = new Figure(move.figureId,move.newPosition);
        // this.lista[move.oldPosition] = null;
        console.log(move);
        if (move.actionType == 0)
        {
            let srcStr = "b" + move.oldPosition;
            let dstStr = "p" + move.newPosition;
            let Src = document.body.querySelector("#"+srcStr);
            let Dst = document.body.querySelector("#"+dstStr);
            console.log(Src);
            console.log(Dst);
            this.animateFigure(Src,Dst,1000);
        }
        else if(move.actionType == 1)
        {
            let srcFieldStr = "p" + move.oldPosition1;
            let dstBaseStr = "b" + move.newPosition1;
            let Src = document.body.querySelector("#"+srcFieldStr);
            let Dst = document.body.querySelector("#"+dstBaseStr);
            this.animateFigure(Src,Dst,1000);

            let srcBaseStr = "b" + move.oldPosition2;
            let dstFieldStr = "p" + move.newPosition2;
            Src = document.body.querySelector("#"+srcBaseStr);
            Dst = document.body.querySelector("#"+dstFieldStr);
            this.animateFigure(Src,Dst,1000);
        }
        else if(move.actionType == 2)
        {
            let srcFieldStr = "p" + move.oldPosition;
            let dstFieldStr = "p" + move.newPosition;
            let Src = document.body.querySelector("#"+srcFieldStr);
            let Dst = document.body.querySelector("#"+dstFieldStr);
            this.animateFigure(Src,Dst,1000);
        }
        else if(move.actionType == 3)
        {
            let srcFieldStr = "p" + move.oldPosition1;
            let dstBaseStr = "b" + move.newPosition1;
            let Src = document.body.querySelector("#"+srcFieldStr);
            let Dst = document.body.querySelector("#"+dstBaseStr);
            this.animateFigure(Src,Dst,1000);

            srcFieldStr = "p" + move.oldPosition2;
            let dstFieldStr = "p" + move.newPosition2;
            Src = document.body.querySelector("#"+srcFieldStr);
            Dst = document.body.querySelector("#"+dstFieldStr);
            this.animateFigure(Src,Dst,1000);
        }
    }

    makeMove(move)
    {
        let srcStr;
        if(move.oldPosition == -1)
        {
            srcStr = "b" + move.figureId;
        }
        else{
            srcStr = "p" + move.oldPosition;
        }
        const dstStr = "p" + move.newPosition;
        const Src = document.body.querySelector("#" + srcStr);
        const Dst = document.body.querySelector("#" + dstStr);

        this.animateFigure(Src,Dst,1000);
    }

    putInBase(figure)
    {
        const bstr = "b" + figure.id;
        const pstr = "p" + figure.position;
        const baza = document.body.querySelector("#"+bstr);
        const trPosition = document.body.querySelector("#"+pstr);
        this.animateFigure(trPosition,baza,1000);

        figure.position = -1;
    }

    animateFigure(figureSrc,figureDst,baseTime)
    {
        let tmp = figureSrc.src;
        figureSrc.classList.remove("animate-source");
        figureSrc.classList.add("animate-source");
        figureSrc.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
        figureDst.src = tmp;
        figureDst.classList.remove("animate-destination");
        figureDst.classList.add("animate-destination");
        // setTimeout(() => {
            //figureSrc.classList.remove("animate-source");
        // }, baseTime);
        // setTimeout(() => {
        //     figureSrc.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
        // }, baseTime/4);
        // setTimeout(() => {
        //     figureDst.src = tmp;
        // }, (baseTime/5) * 4);
        // setTimeout(() => {
            
        // }, baseTime/2);
        // setTimeout(() => {
        //     figureDst.classList.remove("animate-destination");
        // }, baseTime + (baseTime/2));

        // let tmp = figureSrc.src;
        // figureSrc.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
        // figureDst.src = tmp;
    }
    
}