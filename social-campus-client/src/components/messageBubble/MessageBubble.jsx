import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import "./MessageBubble.css";
import AuthLogin from "../../utils/consts/AuthUserLogin";
import FormatTimestampForMessage from "../../utils/helpers/FormatTimestampForMessage";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";

function MessageBubble(props) {
  const navigate = useNavigate();
  const location = useLocation();

  const handleAvatarClick = () => {
    if (location.pathname !== `/profile/${props.login}`) {
      navigate(`/profile/${props.login}`);
      window.scrollTo(0, 0);
    }
  };

  const scrollToReplyMessage = () => {
    const element = document.getElementById(`message-${props.replyTo.id}`);
    if (element) {
      element.scrollIntoView({ block: "center", behavior: "smooth" });
      element.classList.add("highlight");

      setTimeout(() => {
        element.classList.remove("highlight");
        element.classList.add("fade-out");

        setTimeout(() => {
          element.classList.remove("fade-out");
        }, 1000);
      }, 1000);
    }
  };

  return (
    <div
      className={`message-bubble-container ${
        props.login === AuthLogin
          ? "message-bubble-sender"
          : "message-bubble-receiver"
      }`}
      id={`message-${props.messageId}`}
    >
      <div className="message-bubble-content">
        {props.replyTo && (
          <div className="reply-info" onClick={scrollToReplyMessage}>
            <h4 className="message-sender-name general-text">
              {props.replyTo.sender.username}
            </h4>
            <h4 className="message-text general-text">{props.replyTo.text}</h4>
          </div>
        )}
        <div className="message-info">
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
      <div className="message-interactions">
        <InteractionItem
          label={props.likeCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
          itemType="like"
        />
        <InteractionItem
          label={props?.replies || 0}
          icon={InteractionItems.replyIcon}
          hoverIcon={InteractionItems.replyIconActive}
          itemType="reply"
          onClick={props.handleReplyClick}
        />
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
  likeCount: PropTypes.number.isRequired,
  replies: PropTypes.number,
  handleReplyClick: PropTypes.func,
  replyTo: PropTypes.object,
  messageId: PropTypes.number.isRequired,
};

export default MessageBubble;
