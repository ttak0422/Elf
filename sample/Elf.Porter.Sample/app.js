import { getText } from "./App.fs";

export function inputText(foo) {
    const text = prompt("input: ");
    getText(text);
}
