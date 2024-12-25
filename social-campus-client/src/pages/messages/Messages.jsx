import React, { useState, useEffect } from "react";
import ChatItem from "../../components/chatItem/ChatItem";
import chatData from "../../data/chatsData.json";
import authLogin from "../../utils/consts/AuthUserLogin";
import "./Messages.css";

function Messages() {
  const [groupedChats, setGroupedChats] = useState([]);

  useEffect(() => {
    const privateChats = chatData.filter(
      (chat) =>
        chat.sender.login === authLogin || chat.receiver.login === authLogin
    );

    const sortedChats = privateChats.sort(
      (a, b) => new Date(b.timestamp) - new Date(a.timestamp)
    );

    const grouped = [];
    sortedChats.forEach((chat) => {
      const otherUser =
        chat.sender.login === authLogin ? chat.receiver : chat.sender;
      const conversationId = otherUser.login;

      let existingGroup = grouped.find(
        (group) => group.conversationId === conversationId
      );

      if (!existingGroup) {
        existingGroup = {
          conversationId,
          otherUser,
          lastMessage: chat.text,
          lastMessageTimestamp: chat.timestamp,
          lastMessageSender: chat.sender.username,
        };
        grouped.push(existingGroup);
      } else {
        if (
          new Date(chat.timestamp) >
          new Date(existingGroup.lastMessageTimestamp)
        ) {
          existingGroup.lastMessage = chat.text;
          existingGroup.lastMessageTimestamp = chat.timestamp;
          existingGroup.lastMessageSender = chat.sender.username;
        }
      }
    });

    setGroupedChats(grouped);
  }, [authLogin]);

  return (
    <div className="chat">
      <h1 className="top-text general-text">Private chats</h1>
      <div className="chats-previews">
        {groupedChats.map((group) => (
          <ChatItem
            key={group.conversationId}
            chatUser={{
              avatar: group.otherUser.profileImage,
              username: group.otherUser.username,
              login: group.otherUser.login,
            }}
            lastMessage={group.lastMessage}
            lastMessageTimestamp={group.lastMessageTimestamp}
            lastMessageSender={group.lastMessageSender}
          />
        ))}
      </div>
    </div>
  );
}

export default Messages;
