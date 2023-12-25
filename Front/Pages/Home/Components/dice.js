
export class Dice{
    constructor()
    {
        //Attr
        this.dicePositionMatrix;
        this.diceContainer = document.querySelector(".dice-container");
        this.diceNumber = 1;
        this.interval = null;

        //func
        this.setListeners();
        this.initMatrix();
        this.drawDice(1);
    }
    //Animira kockicu sve dok se ne prosledi funkcija da stane
    animateDice()
    {
        this.interval = setInterval(() => {
            this.randomizeDice();
        }, 50);
    }

    //Zaustavlja animaciju i postavlja konacnu vrednost kocke
    stopAnimation(number)
    {
        clearInterval(this.interval);
        this.diceContainer.removeChild(document.querySelector(".dice"));
        this.drawDice(number);
        this.diceNumber = number;
    }
    //Cisti container i postavlja random kockicu
    randomizeDice()
    {
        this.diceContainer.innerHTML = "";

        const random = Math.floor((Math.random() * 6) + 1);
        this.drawDice(random);
    }

    setListeners()
    {
        
    }

    drawDice(number)
    {
        const dice = document.createElement("div");
    
        dice.classList.add("dice");
    
        for ( const dotPosition of this.dicePositionMatrix[number])
        {
            const dot = document.createElement("div");
            dot.classList.add("dice-dot");
            dot.style.setProperty("--top",dotPosition[0] + "%");
            dot.style.setProperty("--left",dotPosition[1] + "%");
            dice.appendChild(dot);
        }
        this.diceContainer.appendChild(dice);
    }
    
    initMatrix()
    {
        this.dicePositionMatrix = 
        {
            1 : [
                [50,50]
            ],
            2: [
                [20,20],
                [80,80] 
            ],
            3:[
                [20,20],
                [50,50],
                [80,80]
            ],
            4:[
                [20,20],
                [20,80],
                [80,20],
                [80,80]
            ],
            5:[
                [20,20],
                [20,80],
                [50,50],
                [80,20],
                [80,80]
            ],
            6:[
                [20,20],
                [20,80],
                [50,20],
                [50,80],
                [80,20],
                [80,80]
            ]
        };
    }

    addClickListener(func) {
        this.diceContainer.addEventListener("click", func);
    }

}

