"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/cafeHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage0", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `Time: ${Date.now()} | ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var drinkType = document.querySelector('input[name="drink"]:checked').value;

    if (drinkType == null)
        var drinkType = 0;

    connection.invoke("CreateOrder", Number(drinkType)).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});