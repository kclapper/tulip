"use strict";

function resetScroll() {
    const messageList = document.getElementById("messageList");
    messageList.lastElementChild.scrollIntoView();
}

var connection = new signalR.HubConnectionBuilder()
                            .withUrl("/chatHub")
                            .build();
var currentUser;

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    const isUserMessage = user === currentUser;

    const newMessage = document.createElement("div");
    newMessage.className = "row no-gutters";

    const spacer = document.createElement("div");
    spacer.className = "col-7";

    const messageRow = document.createElement("div");
    messageRow.className = "col-5";

    const messageCard = document.createElement("div");
    messageCard.className = "card";

    const messageText = document.createElement("div");
    messageText.className = "card-body p-3";
    messageText.textContent = message;

    const messageTimestamp = document.createElement("p")
    messageTimestamp.className = isUserMessage ? "text-muted text-end" : "text-muted text-start";
    messageTimestamp.textContent = `${(new Date()).toLocaleString()}`;

    messageCard.appendChild(messageText);
    messageRow.appendChild(messageCard);
    messageRow.appendChild(messageTimestamp);

    if (isUserMessage) {
        newMessage.appendChild(spacer);
        newMessage.appendChild(messageRow)
    } else {
        newMessage.appendChild(messageRow)
        newMessage.appendChild(spacer);
    }

    const messageList = document.getElementById("messageList");
    messageList.appendChild(newMessage);

    resetScroll();
});

connection
        .start()
        .then(() => {
            document.getElementById("sendButton").disabled = false;
            resetScroll();
            return connection.invoke("GetCurrentUser");
        })
        .then((userName) => currentUser = userName)
        .catch((err) => console.error(err.toString()));

document.getElementById("sendButton").addEventListener("click", function(event) {
    let recipient = document.getElementById("recipientInput").value;
    let message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", recipient, message)
              .catch(function(err) {
                return console.error(err.toString());
              });
    event.preventDefault();
});
