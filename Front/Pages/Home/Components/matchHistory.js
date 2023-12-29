import { serverUrl } from "../../../config.js";
import { prefix64Encoded } from "../../../constants.js";

export class MatchHistory{

    constructor(container)
    {
        this.container = container;

        this.draw();
    }

    async draw()
    {
        let data = await fetch (serverUrl + "/MatchHistory/mymatchhistory/" + localStorage.getItem("id"));
        data = await data.json();
        data.forEach(game => {
            let newGame = document.createElement("div");
            newGame.className = "match-history-match";
            game.users.forEach(user => {
                let card = this.drawCard(user.image,user.username,user.points);
                newGame.appendChild(card);
            });
            this.container.appendChild(newGame);
        });
    }

    drawCard(image,name,points)
    {   //Card
        let card = document.createElement("div");
        card.className = "match-history-card";
        //Image
        let imageAvatar = document.createElement("img");
        imageAvatar.className = "match-history-image";
        imageAvatar.src = prefix64Encoded + image;
        //Result
        let result = document.createElement("div");
        result.className = "match-history-result";
        result.innerHTML = "🏆" + points;
        //Username
        let username = document.createElement("div");
        username.className = "match-history-result";
        username.innerHTML = name;

        card.appendChild(imageAvatar);
        card.appendChild(username);
        card.appendChild(result);
        return card
    }
}