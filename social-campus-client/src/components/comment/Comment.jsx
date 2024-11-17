import React from "react";
import PropTypes from "prop-types";
import "./Comment.css";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";
import DateTime from "../dateTime/DateTime";

function Comment(props) {
  return (
    <div className="comment-container">
      <div className="comment-info">
        <div className="user-info">
          <ShortProfile
            username={props.username}
            login={props.login}
            profileImage={props.profileImage}
          />
          <DateTime dateTime={props.creationTime} locale="en-US" />
        </div>
        <h2 className="comment-text">{props.text}</h2>
      </div>
      <div className="comment-interactions">
        <InteractionItem
          label={props.likeCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
          itemType="like"
        />
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
};

export default Comment;
