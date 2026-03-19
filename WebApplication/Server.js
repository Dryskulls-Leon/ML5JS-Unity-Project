const WebSocket = require("ws");

const wss = new WebSocket.Server({ port: 3000 })

console.log("Server running on ws://localhost:3000");

wss.on("connection", ws => {
    console.log("Client connected!");

    ws.on("message", message => {
        console.log("Recieved", message.toString());
        
        wss.clients.forEach(client => {
            if (client.readyState === WebSocket.OPEN) {
                client.send(message);
            }
        });
    });
});