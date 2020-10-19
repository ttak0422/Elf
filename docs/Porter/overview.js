import { getText } from "./Overview.fs";
export { inputText }

function inputText() {
    const text = prompt("input: ");
    getText(text);
}
