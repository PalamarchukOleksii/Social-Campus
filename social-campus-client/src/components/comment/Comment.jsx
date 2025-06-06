import React, { useState } from "react";
import PropTypes from "prop-types";
import "./Comment.css";
import ShortProfile from "../shortProfile/ShortProfile";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";
import DateTime from "../dateTime/DateTime";
import { IoCreateOutline, IoCreate } from "react-icons/io5";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const REMOVE_COMMENT_LIKE_URL = "/api/commentlikes/remove/";
const ADD_COMMENT_LIKE_URL = "/api/commentlikes/add";

function Comment(props) {
  const { auth } = useAuth();
  const axios = useAxiosPrivate();
  const [isEditHovered, setIsEditHovered] = useState(false);

  const [isLiked, setIsLiked] = useState(
    props.comment.userWhoLikedIds
      ?.map((idObj) => idObj.value)
      .includes(auth.shortUser.id.value)
  );
  const [likeCount, setLikeCount] = useState(
    props.comment.userWhoLikedIds?.length || 0
  );

  const handleLikeToggle = async () => {
    const userId = auth.shortUser?.id?.value;
    const commentId = props.comment.id.value;

    if (!userId || !commentId) return;

    try {
      if (isLiked) {
        await axios.delete(`${REMOVE_COMMENT_LIKE_URL}${commentId}/${userId}`);
        setIsLiked(false);
        setLikeCount((prev) => prev - 1);
      } else {
        await axios.post(ADD_COMMENT_LIKE_URL, {
          userId: { value: userId },
          commentId: { value: commentId },
        });
        setIsLiked(true);
        setLikeCount((prev) => prev + 1);
      }
    } catch (error) {
      console.error("Error toggling comment like:", error);
    }
  };

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
          label={likeCount}
          icon={InteractionItems.likeIcon}
          activeIcon={InteractionItems.activeLikeIcon}
          itemType="like"
          isActive={isLiked}
          onClick={handleLikeToggle}
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
