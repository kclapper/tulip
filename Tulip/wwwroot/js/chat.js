"use strict";

function resetScroll() {
    const messageList = document.getElementById("messageList");
    if (messageList) {
        messageList.lastElementChild.scrollIntoView();
    }
}

var connection = new signalR.HubConnectionBuilder()
                            .withUrl("/chatHub")
                            .withAutomaticReconnect()
                            .build();
var currentUser;

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
            resetScroll();
            return connection.invoke("GetCurrentUser");
        })
        .then((userName) => currentUser = userName)
        .catch((err) => console.error(err.toString()));

if (document.getElementById("messageEditor")) {
    document.getElementById("messageEditor").addEventListener("submit", (event) => {
        event.preventDefault();

        const recipient = document.getElementById("recipientInput").value;

        const messageBox = document.getElementById("messageInput");
        const message = messageBox.value;
        if (message === "") {
            return;
        }

        connection.invoke("SendMessage", recipient, message)
                .catch(function(err) {
                    console.error(err.toString());
                });

        messageBox.value = "";
        resetScroll();
    });
}

if (document.getElementById("composeEditor")) {
    document.getElementById("composeEditor").addEventListener("submit", (event) => {
        event.preventDefault();

        const recipientBox = document.getElementById("recipientInput");
        const recipient = recipientBox.value;

        const messageBox = document.getElementById("messageInput");
        const message = messageBox.value;
        if (message === "") {
            return;
        }

        connection.invoke("SendMessage", recipient, message)
                .catch(function(err) {
                    console.error(err.toString());
                });

        messageBox.value = "";
        if (!recipientBox.readOnly) {
            recipientBox.value = "";
        }

        const sendRedirect = (recipientId) => {
            window.location.href = `/Chat/Message/${recipientId}`;
            connection.off("SentMessage");
        }

        connection.on("SentMessage", sendRedirect);
    });
}
