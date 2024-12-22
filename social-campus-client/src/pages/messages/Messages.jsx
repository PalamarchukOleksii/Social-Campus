import React from "react";
import MessageBubble from "../../components/messageBubble/MessageBubble";

const messages = [
  {
    id: 1,
    text: "Hello, how are you?",
    sender: "Mike Brown",
    avatar:
      "https://img.freepik.com/free-vector/blond-man-with-eyeglasses-icon-isolated_24911-100831.jpg?ga=GA1.1.435982531.1698268990&semt=ais_hybrid",
    timestamp: "10:45 AM",
    isSender: false,
  },
  {
    id: 2,
    text: "Hello, I`m fine, and you?",
    sender: "You",
    avatar: null,
    timestamp: "10:46 AM",
    isSender: true,
  },
];

function Messages() {
  return (
    <div className="chat">
      <h1>Messages</h1>
      {messages.map((message) => (
        <MessageBubble
          key={message.id}
          text={message.text}
          sender={message.sender}
          avatar={message.avatar}
          isSender={message.isSender}
          timestamp={message.timestamp}
        />
      ))}
    </div>
  );
}

export default Messages;
