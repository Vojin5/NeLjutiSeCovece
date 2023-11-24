import { Form } from "./Components/form.js";

export class RegisterPage {
    constructor() {
        this.form = new Form();
    }

    draw() {
        this.form.draw();
    }
}