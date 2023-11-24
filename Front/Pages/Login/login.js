import { Form } from "./Components/form.js";

export class LoginPage {
    constructor() {
        this.form = new Form();
    }

    draw() {
        this.form.draw();
    }
}