import { tick } from "./Receiving.fs";

setInterval(() => {
    tick(new Date());
}, 1000);