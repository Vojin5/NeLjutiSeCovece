
export class HomePage {
    constructor() {
        this.playButton = document.getElementById("play-button");
        this.setClickEvents()
    }

    setClickEvents()
    {
        this.playButton.addEventListener("click", () => {
            this.playButton.classList.add("animate");
            setTimeout(() => {
                this.playButton.classList.remove("animate");
            }, 1000);
        });
    }

}

