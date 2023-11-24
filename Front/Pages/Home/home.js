import { TopNav } from "./Components/topNav.js"

export class HomePage {
    constructor() {
        this.topNav = new TopNav();
    }

    draw() {
        this.topNav.draw();
    }
}