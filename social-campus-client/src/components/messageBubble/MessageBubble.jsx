import React from "react";
import PropTypes from "prop-types";
import "./MessageBubble.css"; 

const MessageBubble = ({ text, sender, avatar, timestamp, isSender }) => {
  return (
    <div
      className={`message-bubble-container ${
        isSender ? "message-bubble-sender" : "message-bubble-receiver"
      }`}
    >
      {!isSender && avatar && (
        <img
          src={avatar}
          alt={`${sender}'s avatar`}
          className="message-avatar"
        />
      )}
      <div className="message-content-container">
        {!isSender && <div className="message-sender-name">{sender}</div>}
        <div className="message-text">{text}</div>
        <div className="message-timestamp">{timestamp}</div>
      </div>
    </div>
  );
};

MessageBubble.propTypes = {
  text: PropTypes.string.isRequired,
  sender: PropTypes.string,
  avatar: PropTypes.string,
  timestamp: PropTypes.string.isRequired,
  isSender: PropTypes.bool.isRequired,
};

export default MessageBubble;
