import React, { useState } from "react";
import PropTypes from "prop-types";
import "./Comment.css";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";
import DateTime from "../dateTime/DateTime";
import { IoCreateOutline, IoCreate } from "react-icons/io5";
import useAuth from "../../hooks/useAuth";

function Comment(props) {
  const { auth } = useAuth();
  const [isEditHovered, setIsEditHovered] = useState(false);

  return (
    <div className="comment-container">
      <div className="comment-info">
        <div className="user-info">
          <div className="commenter-info">
            <ShortProfile userId={props.comment.creatorId.value} />
            <DateTime
              dateTime={props.comment.creationDateTime}
              locale="en-US"
            />
          </div>
          {auth.shortUser?.id.value === props.comment.creatorId.value && (
            <div
              className="edit-comment-icon general-text"
              onMouseEnter={() => setIsEditHovered(true)}
              onMouseLeave={() => setIsEditHovered(false)}
              onClick={props.onEditClick}
            >
              {isEditHovered ? <IoCreate /> : <IoCreateOutline />}
            </div>
          )}
        </div>
        <h2 className="comment-text">{props.comment.description}</h2>
      </div>
      <div className="comment-interactions">
        <InteractionItem
          label={props.comment.userWhoLikedIds?.length || 0}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
          itemType="like"
        />
        <InteractionItem
          label={props.comment.repliesCount}
          icon={InteractionItems.replyIcon}
          itemType="comment"
          onClick={props.onReplyClick}
          hoverIcon={InteractionItems.replyIconActive}
        />
      </div>
    </div>
  );
}

Comment.propTypes = {
  comment: PropTypes.shape({
    id: PropTypes.shape({
      value: PropTypes.string.isRequired,
    }).isRequired,
    description: PropTypes.string.isRequired,
    creationDateTime: PropTypes.string.isRequired,
    creatorId: PropTypes.shape({
      value: PropTypes.string.isRequired,
    }).isRequired,
    publicationId: PropTypes.shape({
      value: PropTypes.string.isRequired,
    }).isRequired,
    userWhoLikedIds: PropTypes.arrayOf(
      PropTypes.shape({
        value: PropTypes.string.isRequired,
      })
    ),
    repliesCount: PropTypes.number.isRequired,
  }).isRequired,
  onReplyClick: PropTypes.func,
  onEditClick: PropTypes.func,
};

export default Comment;
