import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import "./MessageBubble.css";
import AuthLogin from "../../utils/consts/AuthUserLogin";
import FormatTimestampForMessage from "../../utils/helpers/FormatTimestampForMessage";

function MessageBubble(props) {
  const navigate = useNavigate();
  const location = useLocation();

  const handleAvatarClick = () => {
    if (location.pathname !== `/profile/${props.login}`) {
      navigate(`/profile/${props.login}`);
      window.scrollTo(0, 0);
    }
  };

  return (
    <div
      className={`message-bubble-container ${
        props.login === AuthLogin
          ? "message-bubble-sender"
          : "message-bubble-receiver"
      }`}
    >
      <div className="message-bubble-content">
        {props.login !== AuthLogin && (
          <img
            src={props.profileImage || "/default-profile.png"}
            alt={`${props.username}'s avatar`}
            className="message-avatar"
            onClick={handleAvatarClick}
          />
        )}
        <div className="message-content-container">
          {props.login !== AuthLogin && (
            <h4 className="message-sender-name general-text">
              {props.username}
            </h4>
          )}
          <h4 className="message-text general-text">{props.text}</h4>
          <h5 className="message-timestamp not-general-text">
            {FormatTimestampForMessage(props.timestamp)}
          </h5>
        </div>
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
