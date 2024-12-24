import React from "react";
import Loading from "../../components/loading/Loading";
import ChatList from "../../components/chatList/ChatList";

function Messages() {
  return (
    <div className="chat">
      <h1>Messages</h1>
      <ChatList/>
    </div>
  );
}

export default Messages;
