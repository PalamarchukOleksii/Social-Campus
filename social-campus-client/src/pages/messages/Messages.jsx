import React from "react";
import MessageBubble from "../../components/messageBubble/MessageBubble";
import messages from "../../data/chatsData.json";

function Messages() {
  return (
    <div className="chat">
      <h1>Messages</h1>
      {messages.map((message) => (
        <MessageBubble
          key={message.id}
          profileImage={message.sender.profileImage}
          username={message.sender.username}
          login={message.sender.login}
          text={message.text}
          timestamp={message.timestamp}
        />
      ))}
    </div>
  );
}

export default Messages;
