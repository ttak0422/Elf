import { getText } from "./App.fs";

export function inputText(foo) {
    alert(foo.A);
    const text = prompt("input: ");
    getText(text);
}
