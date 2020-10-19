import { recognize } from "./Porter.fs";

SpeechRecognition = webkitSpeechRecognition || SpeechRecognition;
let rec = new SpeechRecognition();
rec.interimResults = true;
rec.continuous = true;

rec.onresult = (event) => {
    recognize(event.resilts);
}

function startRecognition() {
    rec.start();
}
function stopRecognition() {
    rec.stop();
}

export { startRecognition, stopRecognition }