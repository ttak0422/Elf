import { getText } from "./Sample.fs";
export { inputText }

function inputText() {
    const text = prompt("input: ");
    getText(text);
}
