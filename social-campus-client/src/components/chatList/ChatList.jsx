import React from 'react';
import ShortProfile from '../shortProfile/ShortProfile';
import NavItem from '../navItem/NavItem';
import InteractionItem from '../interactionItem/InteractionItem';
import userData from '../../data/userData.json';

const ChatList = ({ onChatSelect }) => {
  return (
    <div className="chat-list">
      {userData.map((chat) => (
        <ChatItem
          key={chat.id}
          chat={chat}
          onClick={() => onChatSelect(chat.id)}
        />
      ))}
    </div>
  );
};

const ChatItem = ({ chat, onClick }) => {
  return (
    <div className="chat-item" onClick={onClick}>
      <ShortProfile
        profileImage={chat.avatar}
        username={chat.name}
        login={chat.login}
      />
      <div className="chat-details">
        <div className="chat-last-message">{chat.lastMessage}</div>
      </div>
      <div className="chat-meta">
        <span className="chat-time">{chat.time}</span>
        {chat.unreadCount > 0 && (
          <InteractionItem
            label={chat.unreadCount}
            icon={() => <span>📩</span>}
            activeIcon={() => <span>✅</span>}
          />
        )}
      </div>
    </div>
  );
};

export default ChatList;

