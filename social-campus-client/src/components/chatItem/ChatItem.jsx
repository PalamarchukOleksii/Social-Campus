import React from "react";
import PropTypes from "prop-types";
import ShortProfile from "../shortProfile/ShortProfile";
import { useNavigate } from "react-router-dom";
import FormatTimestampForMessage from "../../utils/helpers/FormatTimestampForMessage";
import "./ChatItem.css";

function ChatItem(props) {
  const navigate = useNavigate();

  const handleClick = (e) => {
    e.stopPropagation();
    navigate(`/messages/${props.chatUser.login}`);
  };

  return (
    <div className="chat-item-container" onClick={(e) => handleClick(e)}>
      <div className="chat-user-info">
        <ShortProfile
          profileImage={props.chatUser.avatar}
          username={props.chatUser.username}
          login={props.chatUser.login}
          redirectOnClick={false}
        />
      </div>
      <div className="last-message-preview-container">
        <h4 className="last-message-preview-sender-name general-text">
          {props.lastMessageSender}
        </h4>
        <h4 className="last-message-preview-text general-text">
          {props.lastMessage}
        </h4>
        <h5 className="last-message-preview-timestamp not-general-text">
          {FormatTimestampForMessage(props.lastMessageTimestamp)}
        </h5>
      </div>
    </div>
  );
}

ChatItem.propTypes = {
  chatUser: PropTypes.shape({
    avatar: PropTypes.string.isRequired,
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
  }).isRequired,
  lastMessage: PropTypes.string,
  lastMessageTimestamp: PropTypes.string,
  lastMessageSender: PropTypes.string,
};

export default ChatItem;
