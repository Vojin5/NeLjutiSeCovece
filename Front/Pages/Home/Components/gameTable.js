import { prefix64Encoded } from "../../../constants.js";
import { Figure } from "../figure.js";
import { Dice } from "./dice.js";

export class GameTable{

    constructor(gameID, state, players, connection)
    {
        this.gameID = gameID;
        this.state = state;
        this.connection = connection;
        this.players = players;
        this.dice = null;
        this.diceEnabled = false;

        this.setupUserAvatars();
        this.setupHubHandlers();
        
        this.lista = new Array(56);
        for(let i = 0;i<56;i++)
        {
            this.lista[i] = null;
        }
    }

    // constructor(gameID, state, players, connection) {
    //     this.gameID = gameID;
    //     this.state = state;
    //     this.players = players;
    //     this.connection = connection;
    //     this.dice = null;
    //     this.diceEnabled = false;

    //     this.lista = new Array(56);
    //     for(let i = 0;i<56;i++)
    //     {
    //         this.lista[i] = null;
    //     }

    //     this.setupUserAvatars();
    //     this.setupHubHandlers();
    // }

    setupUserAvatars() {
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

    setupHubHandlers() {
        this.connection.on("handleMyTurn", async () => await this.handleMyTurn());
        this.connection.on("handleStartDiceAnimation", () => this.handleStartDiceAnimation())
        this.connection.on("handleDiceNumber", (diceNum) => this.handleDiceNumber(diceNum));
        this.connection.on("handlePossibleMoves", async (moves) => await this.handlePossibleMoves(moves));
        this.connection.on("handlePlayerMove", (move) => this.handlePlayerMove(move));
        this.connection.on("handleGameOver", (points) => this.handleGameOver(points));
    }

    draw() {
        document.body.classList.remove("scrollDown");
        document.body.classList.add("scrollDown");
        this.dice = new Dice();
        this.dice.toggleVisibility();
        this.dice.addClickListener(async () => {
            //await this.connection.invoke("DiceThrown", this.gameID);
            await this.connection.invoke("DiceThrown");
            this.dice.toggleVisibility();
            this.dice.stopBounce();
        });   
    }

    recreateGameState() {
        //b - baza, p - polje
        console.log(this.state);
        let tmp = null;
        for (let i = 0; i < 4; i++) {
            let yellowPos = this.state.figures.yellow[i];
            let yellowSrc = document.getElementById("b" + i);
            let yellowDst = yellowPos == -1 ? document.getElementById("b" + i) : document.getElementById("p" + yellowPos);

            tmp = yellowSrc.src;
            yellowSrc.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
            yellowDst.src = tmp;
            
            let greenPos = this.state.figures.green[i];
            let greenSrc = document.getElementById("b" + (i + 4));
            let greenDst = greenPos == -1 ? document.getElementById("b" + (i + 4)) : document.getElementById("p" + greenPos);


            tmp = greenSrc.src;
            greenSrc.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
            greenDst.src = tmp;

            let redPos = this.state.figures.red[i];
            let redSrc = document.getElementById("b" + (i + 8));
            let redDst = redPos == -1 ? document.getElementById("b" + (i + 8)) : document.getElementById("p" + redPos);

            tmp = redSrc.src;
            redSrc.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
            redDst.src = tmp;

            let bluePos = this.state.figures.blue[i];
            let blueSrc = document.getElementById("b" + (i + 12));
            let blueDst = bluePos == -1 ? document.getElementById("b" + (i + 12)) : document.getElementById("p" + bluePos);

            tmp = blueSrc.src;
            blueSrc.src = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs=";
            blueDst.src = tmp;
        }

        this.draw();
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
                await this.connection.invoke("MovePlayed", moves[Number.parseInt(i)]); 
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

    handleGameOver(points) {
        //potrebno je ovde sada ovde prikazati svakom igracu broj osvojenih poena
        //i staviti dugme Home, koje ga vraca na pocetni ekran
    }

    handleReCreationOfGameState(state, gameId, players) {
        console.log("RADIII");
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