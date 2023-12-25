import { Dice } from "./dice.js";

export class GameTable{

    constructor(gameID)
    {
        this.gameID = gameID;
        this.dice = new Dice();
    }

    
}