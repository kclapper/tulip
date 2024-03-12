"use strict";

var connection = new signalR.HubConnectionBuilder()
                            .withUrl("/chatHub")
                            .build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    let newMessage = document.createElement("li");
    newMessage.textContent = `[${(new Date()).toLocaleString()}] ${user}: ${message}`;

    let messageList = document.getElementById("messagesList");
    let lastMessage = messageList.firstChild;

    if (lastMessage == null) {
        messageList.appendChild(newMessage);
    } else {
        messageList.insertBefore(newMessage, lastMessage);
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function(event) {
    let recipient = document.getElementById("recipientInput").value;
    let message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", recipient, message)
              .catch(function(err) {
                return console.error(err.toString());
              });
    event.preventDefault();
});
