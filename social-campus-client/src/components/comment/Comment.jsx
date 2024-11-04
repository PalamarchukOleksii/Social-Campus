import React from "react";
import "./Comment.css";
import ShortProfile from "../shortProfile/ShortProfile";
import userData from "../../data/userData.json";
import InteractionItems from "../../utils/consts/InteractionItems";
import InteractionItem from "../interactionItem/InteractionItem";

function Comment() {
  return (
    <div className="comment-container">
      <div className="comment-left">
        <ShortProfile username={userData.username} login={userData.login} />
        <p className="comment-text">somejsfaejjanrhjn</p>
        <div className="comment-interactions">
          <InteractionItem
            label={12}
            icon={InteractionItems.likeIcon}
            activeIcon={InteractionItems.activeLikeIcon}
          />
        </div>
      </div>
    </div>
  );
}

export default Comment;
