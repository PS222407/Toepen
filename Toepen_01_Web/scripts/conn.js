const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5273/chatHubApi", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();

document.getElementById("sendButton").disabled = true;

// Connect to SignalR hub
connection.start().then(() => {
    console.log("SignalR Connected");
    document.getElementById("sendButton").disabled = false;
}).catch((error) => {
    console.error("SignalR Connection Error: " + error);
    return console.error(err.toString());
});

// Join room
document.getElementById("joinRoom").addEventListener("click", JoinRoom);
function JoinRoom() {
    var userName = document.getElementById("userInput").value;
    var roomCode = document.getElementById("roomCode").value;
    connection.invoke("JoinRoom", { userName, roomCode })
}

// Receive message
connection.on("ReceiveMessage", function (userName = null, message = null, game = null) {
    if (message == null) {
        console.log(JSON.stringify(JSON.parse(game), undefined, 4))
    } else {
        var li = document.createElement("li");
        document.getElementById("messagesList").appendChild(li);
        li.textContent = `${message}`;
    }
});

// Send message
document.getElementById("sendButton").addEventListener("click", SendMessage);
function SendMessage (event) {
    var userName = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", userName, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

// Start game
document.getElementById("startGameButton").addEventListener("click", StartGame);
function StartGame (event) {
    connection.invoke("StartGame").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

// avoid putting browser tab to sleep
var lockResolver;
if (navigator && navigator.locks && navigator.locks.request) {
    const promise = new Promise((res) => {
        lockResolver = res;
    });

    navigator.locks.request('unique_lock_name', { mode: "shared" }, () => {
        return promise;
    });
}