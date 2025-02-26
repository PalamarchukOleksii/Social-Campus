import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import "./Comment.css";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";
import DateTime from "../dateTime/DateTime";
import { IoCreateOutline, IoCreate } from "react-icons/io5";
import login from "../../utils/consts/AuthUserLogin";
import userData from "../../data/userData.json";

function Comment(props) {
  const [isEditHovered, setIsEditHovered] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);
  const [commentText, setCommentText] = useState(props.text);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = () => {
      const user = userData.find((user) => user.login === login);
      setCurrentUser(user);
      setLoading(false);
    };

    fetchData();
  }, []);

  if (loading) {
    return <></>;
  }

  return (
    <div className="comment-container">
      <div className="comment-info">
        <div className="user-info">
          <div className="commenter-info">
            <ShortProfile
              username={props.username}
              login={props.login}
              profileImage={props.profileImage}
            />
            <DateTime dateTime={props.creationTime} locale="en-US" />
          </div>
          {currentUser.login === props.login && (
            <div
              className="edit-comment-icon general-text"
              onMouseEnter={() => setIsEditHovered(true)}
              onMouseLeave={() => setIsEditHovered(false)}
              onClick={() => {}}
            >
              {isEditHovered ? <IoCreate /> : <IoCreateOutline />}
            </div>
          )}
        </div>
        <h2 className="comment-text">{commentText}</h2>
      </div>
      <div className="comment-interactions">
        <InteractionItem
          label={props.likeCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
          itemType="like"
        />
        {!props.hideReplyButton && (
          <InteractionItem
            label={props?.replies?.length || 0}
            icon={InteractionItems.replyIcon}
            itemType="comment"
            onClick={props.onReplyClick}
            hoverIcon={InteractionItems.replyIconActive}
          />
        )}
      </div>
    </div>
  );
}

Comment.propTypes = {
  username: PropTypes.string.isRequired,
  login: PropTypes.string.isRequired,
  profileImage: PropTypes.string.isRequired,
  text: PropTypes.string.isRequired,
  likeCount: PropTypes.number.isRequired,
  creationTime: PropTypes.string.isRequired,
  hideReplyButton: PropTypes.bool,
  onReplyClick: PropTypes.func,
  replies: PropTypes.array,
};

export default Comment;
