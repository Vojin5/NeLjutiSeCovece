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
            this.dice.stopBounce();
        });
        
    }

    //bilo je async
    handleMyTurn() {
        // setTimeout(() => {
        //     this.dice.toggleVisibility();    
        // }, 2000);
        this.dice.toggleVisibility();
        this.dice.bounce();
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
        
        // let moveChoice;
        // let elements = [];
        // for (let i = 0; i < moves.length; i++) {
        //     let el;
        //     const j = i;
        //     if (moves[i].oldPosition == -1) {
        //         el = document.getElementById("b" + moves[i].figureId);
        //         el.classList.add("bounce");
        //         el.addEventListener("click", async () => {
        //             if(el.classList.contains("bounce"))
        //             {
        //                 let move = i;
        //                 moveChoice = move;
        //                 console.log(moveChoice);
        //                 await this.connection.invoke("MovePlayed", this.gameID, moves[Number.parseInt(j)]);
        //             }
        //             for(let j = 0;j<elements.length;j++)
        //             {
        //                 elements[j].classList.remove("bounce");
        //             }
        //         },{once:true});
                
        //     }
        //     else {
        //         el = document.getElementById("p" + moves[i].oldPosition);
        //         el.classList.add("bounce");
        //         el.addEventListener("click", async () => {
        //             if(el.classList.contains("bounce"))
        //             {
        //                 let move = i;
        //                 moveChoice = move;
        //                 console.log(moveChoice);
        //                 await this.connection.invoke("MovePlayed", this.gameID, moves[Number.parseInt(j)]);
        //                 console.log(j);
        //             }
        //             for(let j = 0;j<elements.length;j++)
        //             {
        //                 elements[j].classList.remove("bounce");
        //             }
        //         },{once:true});
        //     }
        //     elements.push(el);  
        // }

        

        // for(let i = 0;i<elements.length;i++)
        // {

        //     const clk = () => {

        //         for(let j = 0;j < elements.length;j++)
        //         {
        //             elements
        //         }
        //     }


        // }

        const ev = new Event("remove");

        let elements = [];
        for (let i = 0; i < moves.length; i++) {
            if (moves[i].oldPosition == -1) {
                elements.push(document.getElementById("b" + moves[i].figureId));
                elements[i].classList.add("bounce");
            }
            else {
                elements.push(document.getElementById("p" + moves[i].oldPosition));
                elements[i].classList.add("bounce");
            }
        }

        for (let i = 0; i < elements.length; i++) {
            elements[i].addEventListener("click", async() => {
                for (let j = 0; j < elements.length; j++) {
                    elements[j].classList.remove("bounce");
                    const clonedNode = elements[j].cloneNode(true);
                    elements[j].parentNode.replaceChild(clonedNode, elements[j]);
                }
                await this.connection.invoke("MovePlayed", this.gameID, moves[Number.parseInt(i)]); 
            });
        }
        
    }


    handlePlayerMove(move) {
        
        console.log(move);
        if (move.actionType == 0)
        {
            let srcStr = "b" + move.oldPosition;
            let dstStr = "p" + move.newPosition;
            let Src = document.body.querySelector("#"+srcStr);
            let Dst = document.body.querySelector("#"+dstStr);
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