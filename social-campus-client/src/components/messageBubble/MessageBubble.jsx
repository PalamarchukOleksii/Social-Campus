import React from "react";
import PropTypes from "prop-types";
import "./MessageBubble.css";
import AuthLogin from "../../utils/consts/AuthUserLogin";

function MessageBubble(props) {
  return (
    <div
      className={`message-bubble-container ${
        props.login == AuthLogin
          ? "message-bubble-sender"
          : "message-bubble-receiver"
      }`}
    >
      {props.login != AuthLogin && (
        <img
          src={props.profileImage || "/default-profile.png"}
          alt={`${props.username}'s avatar`}
          className="message-avatar"
        />
      )}
      <div className="message-content-container">
        {props.login != AuthLogin && (
          <div className="message-sender-name">{props.username}</div>
        )}
        <div className="message-text">{props.text}</div>
        <div className="message-timestamp">{props.timestamp}</div>
      </div>
    </div>
  );
}

MessageBubble.propTypes = {
  profileImage: PropTypes.string,
  username: PropTypes.string.isRequired,
  login: PropTypes.string.isRequired,
  text: PropTypes.string.isRequired,
  timestamp: PropTypes.string.isRequired,
};

export default MessageBubble;
