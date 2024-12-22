import React from "react";
import PropTypes from "prop-types";
import "./MessageBubble.css";

function MessageBubble(props) {
  return (
    <div
      className={`message-bubble-container ${
        props.isSender ? "message-bubble-sender" : "message-bubble-receiver"
      }`}
    >
      {!props.isSender && props.avatar && (
        <img
          src={props.avatar}
          alt={`${props.sender}'s avatar`}
          className="message-avatar"
        />
      )}
      <div className="message-content-container">
        {!props.isSender && (
          <div className="message-sender-name">{props.sender}</div>
        )}
        <div className="message-text">{props.text}</div>
        <div className="message-timestamp">{props.timestamp}</div>
      </div>
    </div>
  );
}

MessageBubble.propTypes = {
  text: PropTypes.string.isRequired,
  sender: PropTypes.string,
  avatar: PropTypes.string,
  timestamp: PropTypes.string.isRequired,
  isSender: PropTypes.bool.isRequired,
};

export default MessageBubble;
